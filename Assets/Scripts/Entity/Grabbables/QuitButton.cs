using Entity.Player;
using UnityEngine;

namespace Entity.Grabbables
{
    public class QuitButton : Grabbable
    {
        public override Vector2 GrabScaleFactor()
        {
            return new Vector2(1, 1);
        }

        public override void ResetScale()
        {
        }

        public override void PlayerGrab(Frog player)
        {
            Application.Quit();
        }
    }
}