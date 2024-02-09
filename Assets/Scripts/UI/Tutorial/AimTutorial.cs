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
            return this.frog.IsAiming() ? "Maintenant, vises le cercle 'Jouer', et relâches." : "Pour viser, maintiens espace!";
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