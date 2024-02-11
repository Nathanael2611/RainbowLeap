using Entity.Grabbables;
using Unity.VisualScripting;
using UnityEngine;
using Util.Caches;

namespace UI.Tutorial
{
    public class GrabTutorial : Tutorial
    {
        private float _firstGrabTime = 0;
        private float _secondGrabTime = 0;
        
        public override string GetHintText()
        {
            if (this._secondGrabTime > 0)
            {
                float since = Time.unscaledTime - this._secondGrabTime;
                return since < 4.6f ? "Ta couleur se mélange à celle de l'orbe..." : since < 10F ? "Le pourcentage de ressemblance avec la planète change..." : "Parviens aux 100%!";
            }
            if (this._firstGrabTime > 0)
            {
                float since = Time.unscaledTime - this._firstGrabTime;
                return since > 3.5 ? "Essayes encore!" : "Ta couleur change...";
            }
            return "Vises pour attraper une orbe.";
        }

        public override void Update()
        {
            base.Update();

            if (this._secondGrabTime > 0)
            {
                float since = Time.unscaledTime - this._secondGrabTime;
                if(since > 12 && !this.IsValidated())
                    this.ValidateCondition();
            }
        }

        public override void PlayerGrab(Grabbable grabbable)
        {
            base.PlayerGrab(grabbable);
            if (typeof(ColoredCircle) == grabbable.GetType())
            {
                if (this._firstGrabTime == 0)
                {
                    this._firstGrabTime = Time.unscaledTime;
                }
                else if(this._secondGrabTime == 0)
                {
                    this._secondGrabTime = Time.unscaledTime;
                }

                //this.ValidateCondition();
            }
        }


        public override void NextTutorial()
        {
        }

    }
}