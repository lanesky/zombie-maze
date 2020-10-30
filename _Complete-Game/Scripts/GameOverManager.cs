using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Completed
{
    public class GameOverManager : MonoBehaviour
    {

        public PlayerHealth playerHealth;
        public float restartDelay = 15f;

        Animator anim;
        float restartTimer;

        void Awake()
        {
            anim = GetComponent<Animator>();
        }


        void Update()
        {
            if (playerHealth.currentHealth <= 0)
            {

                anim.SetTrigger("GameOver");
            

                // .. increment a timer to count up to restarting.
                restartTimer += Time.deltaTime;

                // .. if it reaches the restart delay...
                if (restartTimer >= restartDelay)
                {

                    // .. then reload the currently loaded level.

                    GameManager.instance.resetLevel();
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    // reset score
                    ScoreManager.score = 0;
                }

            }
        }
    }

}


