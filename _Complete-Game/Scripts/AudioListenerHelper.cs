using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class AudioListenerHelper : MonoBehaviour {


    public bool soundOn {
        get {
            return !AudioListener.pause;
        } 
        set  {
            AudioListener.pause = !value;
        }
    }
}
