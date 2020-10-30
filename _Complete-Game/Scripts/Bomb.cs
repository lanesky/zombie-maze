using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Completed {

    public class Bomb : MonoBehaviour
    {

        public Explosion explosion;
        //public AudioClip explosionSound;
        private float fuseDuration = 3f;
        private AudioSource myAudio;
        private Rigidbody2D rb;

        private void Awake()
        {
            myAudio = GetComponent<AudioSource>();
            rb = GetComponent<Rigidbody2D>();
        }


        //private void Update()
        //{
        //    Destroy(gameObject, fuseDuration);
        //}

        public void Fuse() {
            //GameManager.instance.playParticleOnSpawnPoint();
            Invoke("explodeBomb", fuseDuration);
            myAudio.PlayDelayed(fuseDuration + 0.5f);
            Destroy(gameObject, fuseDuration + 1.5f);
        }


        private Collider2D findObjectInLineCast(RaycastHit2D[] hits, Vector3 position)
        {
            foreach (RaycastHit2D iter in hits)
            {
                if (iter.collider.transform.position == position)
                    return iter.collider;
            }
            return null;
        }

        private void bombExplodeByDirection(Transform bombTransform, Vector3 direction, float length)
        {
            var startPosition = bombTransform.position;
            var targetPosition = startPosition + direction * length;

            var hits = Physics2D.LinecastAll(startPosition, targetPosition);

            for (var i = 1f; i <= length; i++)
            {
                var nextPosition = startPosition + direction * i;
                var test = findObjectInLineCast(hits, nextPosition);

                if (test != null && test.name.StartsWith("OuterWall", System.StringComparison.Ordinal))
                {
                    break;

                }
                else
                {
                    var gameObj = Instantiate(this.explosion, nextPosition, Quaternion.identity);
                    var explosionComponent = gameObj.GetComponent<Explosion>();
                    switch ((int)i)
                    {
                        case 1:
                            explosionComponent.position = ExplosionPosition.Mid; break;
                        case 2:
                            explosionComponent.position = ExplosionPosition.Tail; break;
                        default:
                            explosionComponent.position = ExplosionPosition.Center; break;
                    }



                    if (direction == Vector3.up)
                    {
                        explosionComponent.direction = ExplosionDirecton.Up;
                    }
                    else if (direction == Vector3.down)
                    {
                        explosionComponent.direction = ExplosionDirecton.Down;
                    }
                    else if (direction == Vector3.left)
                    {
                        explosionComponent.direction = ExplosionDirecton.Left;
                    }
                    else if (direction == Vector3.right)
                    {
                        explosionComponent.direction = ExplosionDirecton.Right;
                    }

                    explosionComponent.Explode();

                    // for general wall , stop after the first explosion
                    if (test != null && test.CompareTag("Wall"))
                    {
                        break;

                    }
                }


            }
        }

        void explodeBomb()
        {

            rb.simulated = false;
            this.StartCoroutine(() =>
                                this.gameObject.transform.localScale = Vector3.zero, 0.5f);


            var length = 2.0f;
            var bombTransform = transform;

            bombExplodeByDirection(bombTransform, Vector3.up, length);
            bombExplodeByDirection(bombTransform, Vector3.down, length);
            bombExplodeByDirection(bombTransform, Vector3.left, length);
            bombExplodeByDirection(bombTransform, Vector3.right, length);

            var gameObj = Instantiate(this.explosion, transform.position, Quaternion.identity);
            var explosionComponent = gameObj.GetComponent<Explosion>();
            explosionComponent.Explode();
        }
    }

}

