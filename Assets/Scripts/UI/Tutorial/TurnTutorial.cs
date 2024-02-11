using UnityEngine;
using Util.Caches;

namespace UI.Tutorial
{
    public class TurnTutorial : Tutorial
    {
        public override string GetHintText()
        {
            return "Pour changer de direction, double-cliques sur espace!";
        }


        public override void NextTutorial()
        {
            GameObject tutorialBase = GameObject.Instantiate(this.GetBasePrefab(), this.transform.parent);
            AimTutorial addComponent = tutorialBase.AddComponent<AimTutorial>();
            addComponent.frog = this.frog;
            
        }

        public override void DirectionChange()
        {
            base.DirectionChange();
            this.ValidateCondition();
        }
    }
}