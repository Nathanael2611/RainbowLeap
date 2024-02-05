using UnityEngine;
using Util.Caches;

namespace UI.Tutorial
{
    public class JumpTutorial : Tutorial
    {

        public float jumped = -1;
        
        public override string GetHintText()
        {
            return Time.unscaledTime - this.jumped > 1 && this.jumped > -1? "Encore une fois!" : "Pour sautez, appuyez une fois sur espace!";
        }


        public override void NextTutorial()
        {
            GameObject tutorialBase = GameObject.Instantiate(this.GetBasePrefab(), this.transform.parent);
            TurnTutorial addComponent = tutorialBase.AddComponent<TurnTutorial>();
            addComponent.frog = this.frog;
        }

        public override void Jump()
        {
            base.Jump();
            if (this.jumped > -1)
            {
                this.ValidateCondition();
            }
            else
            {
                this.jumped = Time.unscaledTime;
            }
        }
    }
}