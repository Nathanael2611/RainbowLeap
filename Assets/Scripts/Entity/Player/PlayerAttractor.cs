using physic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entity.Player
{
    public class PlayerAttractor : Attractor
    {

        [FormerlySerializedAs("player")] public Frog frog;

        public override bool OnAttractedBy(Attractor attractor, ref Vector2 force)
        {
            if (this.frog.IsAiming() || this.frog.colorSelection)
                return false;
            return base.OnAttractedBy(attractor, ref force);
        }
    }
}