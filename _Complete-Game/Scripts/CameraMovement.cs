using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Completed

{
    public class CameraMovement : MonoBehaviour
    {
        private float xMin;
        private float xMax;
        private float yMin;
        private float yMax;
        private Transform player;

        //private void Awake()
        //{

        //    ResetParams();

        //}

        public void ResetParams() {
            float orthographicSize = Camera.main.orthographicSize;
            float aspectRatio = Screen.width * 1.0f / Screen.height;
            float cameraHeight = Camera.main.orthographicSize * 2;
            float cameraWidth = cameraHeight * aspectRatio;
            //Debug.Log("camera size in unit=" + cameraWidth + "," + cameraHeight);

            player = GameObject.FindGameObjectWithTag("Player").transform;

            var columns = (float)(GameManager.instance.tileColumns + 2);
            var rows = (float)(GameManager.instance.tileRows + 2);

            if (cameraWidth >= columns)
            {
                xMin = columns / 2f - 1.5f;
                xMax = xMin;
            }
            else
            {
                xMin = cameraWidth / 2f - 1.5f;
                xMax = columns - 1.5f - cameraWidth / 2f;
            }

            if (cameraHeight >= rows)
            {
                yMin = rows / 2f - 1.5f;
                yMax = yMin;
            }
            else
            {
                yMin = cameraHeight / 2f - 1.5f;
                yMax = rows - 1.5f - cameraHeight / 2f;
            }
        }

        // Update is called once per frame

        void FixedUpdate()
        {

            transform.position = new Vector3(
                Mathf.Clamp(player.position.x,xMin,xMax),
                Mathf.Clamp(player.position.y,yMin,yMax),
                transform.position.z
            );

        }
    }
}
