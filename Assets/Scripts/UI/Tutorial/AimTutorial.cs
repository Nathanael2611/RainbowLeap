using Unity.VisualScripting;
using UnityEngine;
using Util.Caches;

namespace UI.Tutorial
{
    public class AimTutorial : Tutorial
    {
        public override string GetHintText()
        {
            if (this.frog.GetTongue())
                return "";
            return this.frog.IsAiming() ? "Maintenant, visez le cercle 'Play', et relachez." : "Pour viser, maintenez espace!";
        }

        public override void Update()
        {
            base.Update();
            
        }


        public override void NextTutorial()
        {
        }

        public override void DirectionChange()
        {
            base.DirectionChange();
        }
    }
}