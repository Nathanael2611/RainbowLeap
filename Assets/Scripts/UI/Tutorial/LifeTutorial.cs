using System;
using Entity.Player;
using input;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Tutorial
{
    public class LifeTutorial : MonoBehaviour, IInputListener
    {
        private bool _hasBeenShown = false;
        private bool _activeChilds = false;
        
        private void Start()
        {
            this.SetChildActives(false);
            PressManager.Instance.RegisterListener(this);
        }

        private void Update()
        {
            if (!this._hasBeenShown)
            {
                Frog theFrog = Frog.TheFrog;
                if (theFrog.actions < theFrog.maxActions - 3)
                {
                    this._hasBeenShown = true;
                    this.SetChildActives(true);
                }
            }

        }

        public void SetChildActives(bool active)
        {
            this._activeChilds = active;
            for (int i = 0; i < this.transform.childCount; i++)
            {
                GameObject o = this.transform.GetChild(i).gameObject;
                o.SetActive(active);
            }
        }

        public void SimpleClick()
        {
            this.SetChildActives(false);
        }

        public void DoubleClick()
        {
        }

        public void HoldStart()
        {
        }

        public void HoldEnd()
        {
        }
    }
}