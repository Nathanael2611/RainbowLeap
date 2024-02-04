using Entity.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Entity.Grabbables
{
    public class PlayButton : Grabbable
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
            SceneManager.LoadSceneAsync(1);
        }
    }
}