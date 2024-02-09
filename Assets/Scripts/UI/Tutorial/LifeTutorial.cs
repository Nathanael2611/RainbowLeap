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
        private TypingEffeect _typingEffect;

        private void Start()
        {
            this.SetChildActives(false);
            PressManager.Instance.RegisterListener(this);
            this._typingEffect = null;
        }

        private void Update()
        {
            if (!this._hasBeenShown)
            {
                Frog theFrog = Frog.TheFrog;
                if (theFrog.actions < theFrog.maxActions - 4)
                {
                    this._hasBeenShown = true;
                    this.SetChildActives(true);
                }
            }

        }

        private void OnEnable()
        {
        }

        public void SetChildActives(bool active)
        {
            this._activeChilds = active;
            for (int i = 0; i < this.transform.childCount; i++)
            {
                GameObject o = this.transform.GetChild(i).gameObject;
                TypingEffeect type = o.GetComponentInChildren<TypingEffeect>();
                if (type)
                    this._typingEffect = type;
                if(o != null) 
                    o.SetActive(active);
            }
        }

        public void SimpleClick()
        {
            if (this._typingEffect && !this._typingEffect.IsTyping())
            {
                this.SetChildActives(false);
            }
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