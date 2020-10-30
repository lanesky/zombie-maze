using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Completed
{
    public class PlayerHealth : MonoBehaviour
    {

        public int startingHealth = 100;
        public int currentHealth;
        public Slider healthSlider;
        public Image damageImage;
        public float flashSpeed = 5f;
        public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
        public AudioClip gameOverSound;

        Animator anim;
        //AudioSource playerAudio;
        bool isDead;
        bool damaged;
        BoxCollider2D box2Dcollider;
        Rigidbody2D rb;
        void Awake()
        {
            anim = GetComponent<Animator>();
            //playerAudio = GetComponent<AudioSource>();
            currentHealth = startingHealth;
            box2Dcollider = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (damaged)
            {
                damageImage.color = flashColour;
            }
            else
            {
               damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
            }
            damaged = false;
        }


        public void TakeDamage(int amount)
        {

            anim.SetTrigger("playerHit");
            damaged = true;

            currentHealth -= amount;

            healthSlider.value = currentHealth;

            //playerAudio.Play();

            if (currentHealth <= 0 && !isDead)
            {
                Death();
            }

        }

        public void Death()
        {

            this.isDead = true;
            //Call the PlaySingle function of SoundManager and pass it the gameOverSound as the audio clip to play.
            this.StartCoroutine(()=>{
                anim.SetTrigger("die");
                SoundManager.instance.PlaySingle(gameOverSound);
            }, 0.5f);

            box2Dcollider.enabled = false;
            rb.simulated = false;
            //Stop the background music.
            //SoundManager.instance.musicSource.Stop();

            //Call the GameOver function of GameManager.
            //this.StartCoroutine(() => GameManager.instance.GameOver(), 2f);

        }
    }
}
