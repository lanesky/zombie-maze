using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Completed {

    [System.Serializable]
    public enum ExplosionDirecton {Center, Up, Down, Left, Right}

    [System.Serializable]
    public enum ExplosionPosition {Center, Mid,Tail}

    public class Explosion : MonoBehaviour
    {

        public ExplosionDirecton direction;
        public ExplosionPosition position;
        public float delay = 0.5f;
        public float duration = 1.0f;
        public int damage = 100;

        private Animator animator;
        //private bool needCheckExitAfterDestroy = false;
        private bool needSpawnLotsOfEnemies = false;
        //private SpriteRenderer sRender;

        public void Awake()
        {
            animator = GetComponent<Animator>();
            //sRender = GetComponent<SpriteRenderer>();
            //animator.SetTrigger("center");
        }

        public void Explode()
        {
            Invoke("playAnimation", delay);

            // after explosion is over, destroy gameObject by itself
            Destroy(gameObject, delay + duration);

        }
        private void OnDestroy()
        {
            //if (this.needCheckExitAfterDestroy)
            //{
            //    GameManager.instance.setupExitIfNecessary(transform.position);
            //} else 
            if (this.needSpawnLotsOfEnemies) {
                GameManager.instance.spawnEnemiesByStress();
            }
        }
        private void playAnimation() 
        {

            if (this.direction == ExplosionDirecton.Left) {
                this.transform.Rotate(0f,0f,180f);
            }
            else if (this.direction == ExplosionDirecton.Up)
            {
                this.transform.Rotate(0f, 0f, 90f);
            }
            else if (this.direction == ExplosionDirecton.Down)
            {
                this.transform.Rotate(0f, 0f, -90f);
            }

            switch (this.position)
            {
                case ExplosionPosition.Mid:
                    animator.SetTrigger("mid");
                    break;
                case ExplosionPosition.Tail:
                    animator.SetTrigger("tail");
                    break;
                default:
                    animator.SetTrigger("center");
                    break;
            }

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //Debug.Log(string.Format("hitted!:{0}", other.name));

            if (other.tag == "Enemy")
            {
                var enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                {

                    this.StartCoroutine(() => enemy.Death(), 0.5f);

                }
            }
            else if (other.tag == "Player")
            {
                var playerHealth = other.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    this.StartCoroutine(() =>
                                        playerHealth.TakeDamage(this.damage), 0.1f);
                }
                //GameManager.instance.GameOver();
            }
            else if (other.tag == "Wall")
            {
                this.StartCoroutine(() =>
                {
                    other.GetComponent<Wall>().Death();
                    //Destroy(other.gameObject);
                    GameManager.instance.setupExitIfNecessary(transform.position);
                }, this.delay);

            }
            else if (other.tag == "SpawnPoint" || other.tag == "Exit") 
            {
                this.needSpawnLotsOfEnemies = true;
            }
            else if (other.tag == "OuterWall")
            {
                // do nothing
            } else {
               // Destroy(other.gameObject, this.delay);
            }
        }

    }
}

namespace UnityEngine
{
    public static class MonoBehaviourExtension
    {
        public static Coroutine StartCoroutine(this MonoBehaviour behaviour, System.Action action, float delay)
        {
            return behaviour.StartCoroutine(WaitAndDo(delay, action));
        }

    private static IEnumerator WaitAndDo(float time, System.Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
}
