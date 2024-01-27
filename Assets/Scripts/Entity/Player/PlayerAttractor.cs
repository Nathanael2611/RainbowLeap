using physic;
using UnityEngine;

namespace entity
{
    public class PlayerAttractor : Attractor
    {

        public Player player;

        public override bool OnAttractedBy(Attractor attractor, ref Vector2 force)
        {
            if (player.IsAiming())
                return false;
            return base.OnAttractedBy(attractor, ref force);
        }
    }
}