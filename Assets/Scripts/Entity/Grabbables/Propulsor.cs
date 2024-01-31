using System;
using Entity.Player;
using UnityEngine;

namespace Entity.Grabbables
{
    public class Propulsor : Grabbable
    {

        private CircleCollider2D _collider2D;
        
        private void OnEnable()
        {
        }

        public override void Start()
        {
            base.Start();
            this.RigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            this._collider2D = this.GetComponent<CircleCollider2D>();
            this._collider2D.isTrigger = true;
            this.GetAttractor().DontAttract(Frog.TheFrog.GetAttractor());

        }

        public override Vector2 GrabScaleFactor()
        {
            return new Vector2(1, 1);
        }

        public override void ResetScale()
        {
        }

        public override void PlayerGrab(Frog player)
        {
            player.GetTongue().Comeback();
            player.GetRigidbody().AddForce(player.GetRigidbody().velocity.normalized * (player.jumpStrength));
            //player.actions--;
        }
        
    }
}