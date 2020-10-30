using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Completed
{
    public class PauseManager : MonoBehaviour
    {

        public AudioMixerSnapshot paused;
        public AudioMixerSnapshot unpaused;
        public Canvas creditCanvas;
        Canvas canvas;

        void Start()
        {
            canvas = GetComponent<Canvas>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //canvas.enabled = !canvas.enabled;
                Pause();
            }
        }

        public void Pause()
        {

            Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
            canvas.enabled = Time.timeScale == 0f;
            Lowpass();

            if (creditCanvas != null && !canvas.enabled) {
                creditCanvas.enabled = false;
            }

        }

        void Lowpass()
        {
            if (Time.timeScale == 0f)
            {
                paused.TransitionTo(.01f);
            }

            else

            {
                unpaused.TransitionTo(.01f);
            }
        }

        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
        }
    }
}
