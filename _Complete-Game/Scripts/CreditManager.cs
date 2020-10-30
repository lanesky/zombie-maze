using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Completed {


    public class CreditManager : MonoBehaviour {


        public Text creditText;
        Canvas canvas;

        void Start()
        {
            canvas = GetComponent<Canvas>();

            string msg;
            //msg = "Graphics:" + System.Environment.NewLine +
            //"\t1. https://opengameart.org/content/zombie-animations Irina Mir (irmirx) cc3" + System.Environment.NewLine +
            //"\t2. https://opengameart.org/content/vampire-animations Irina Mir (irmirx) cc3" + System.Environment.NewLine +
            //"\t3. https://opengameart.org/content/sokoban-100-tiles Kenney.nl cc0" + System.Environment.NewLine +
            //"\t4. bomb https://opengameart.org/content/emotional-explosives cc3" + System.Environment.NewLine +
            //"\t5. explosion https://opengameart.org/content/bomberman-explosion-effect cc0" + System.Environment.NewLine +
            //"\t6. particle :Kenney Vleugels (Kenney.nl) License (Creative Commons Zero, CC0)" + System.Environment.NewLine +
            //"Background music:" + System.Environment.NewLine +
            //"\thttps://assetstore.unity.com/packages/audio/music/absolutely-free-music-4883" + System.Environment.NewLine +
            //"Sound effect:" + System.Environment.NewLine +
            //"\t1. Explosion (Viktor.Hahn@web.de) cc3" + System.Environment.NewLine +
            //"\t2. foottap, enemy death, game over : from unity3d" + System.Environment.NewLine +
            //"GUI:" + System.Environment.NewLine +
            //"\t1. https://assetstore.unity.com/packages/2d/gui/icons/simple-ui-103969";
            // exit https://opengameart.org/content/platformer-art-deluxe Credit "Kenney.nl" or "www.kenney.nl", this is not mandatory.

            msg = "Graphics art:" + System.Environment.NewLine +
                "\t1. Zombie and vampire : Irina Mir (irmirx) " + System.Environment.NewLine +
                "\t2. Bombman, floor tiles, exit sign and a part of particle effect: Kenney Vleugels (Kenney.nl)" + System.Environment.NewLine +
                "\t3. Bomb :SpriteAttack(chris)" + System.Environment.NewLine +
                "\t4. Explosion:Satik64" + System.Environment.NewLine +
                "Background music:" + System.Environment.NewLine +
                "\tVertex Studio" + System.Environment.NewLine +
                "Sound effect:" + System.Environment.NewLine +
                "\t1. Explosion: (Viktor.Hahn@web.de) " + System.Environment.NewLine +
                "\t2. foot tap, enemy death, game over : from unity3d tutorials" + System.Environment.NewLine +
                "GUI:" + System.Environment.NewLine +
                "\t1. Unruly Games and Unity3d tutorials";

            creditText.text = msg;
        }



        public void ShowCredit()
        {
            canvas.enabled = true;



        }

        public void Close() {
            canvas.enabled = false;
        }
    }

}
