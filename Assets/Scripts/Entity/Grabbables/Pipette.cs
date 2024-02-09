using Entity.Player;
using UnityEngine;

namespace Entity.Grabbables
{
    public class Pipette : Grabbable
    {

        public override void Start()
        {
            base.Start();
            this.GetAttractor().DontAttract(Frog.TheFrog().GetAttractor());
        }
        
        public override Vector2 GrabScaleFactor()
        {
            return new Vector2(0.4f, 0.4f);
        }

        public override void ResetScale()
        {
            
        }

        public override void PlayerGrab(Frog player)
        {
            player.ActivateColorSelection();
            player.GetTongue().Comeback();
            GameObject.Destroy(this.gameObject);

        }
    }
}