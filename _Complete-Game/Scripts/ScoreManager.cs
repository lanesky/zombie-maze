using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Completed
{
    public class ScoreManager : MonoBehaviour
    {
        public static long score;


        Text text;


        void Awake()
        {
            //DontDestroyOnLoad(gameObject);
            text = GetComponent<Text>();

        }


        void Update()
        {
            text.text = "Score: " + score;
        }
    }
}
