using System;
using Entity.Grabbables;
using Entity.Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TestTools;
using Util.Caches;

namespace UI.Tutorial
{
    public abstract class Tutorial : MonoBehaviour
    {

        public Frog frog;

        protected TextMeshProUGUI TextMeshPro;

        private float _validationTime = -1;
        private TypingEffeect _typingEffeect;
        private string _lastText = "";

        private void Start()
        {
            this.TextMeshPro = this.GetComponent<TextMeshProUGUI>();
            this._typingEffeect = this.AddComponent<TypingEffeect>();
        }

        private void OnDestroy()
        {
            if (this.frog.GetTutorial() == this)
            {
                this.frog.SetTutorial(null);
            }
        }

        public virtual void Update()
        {
            this.frog.SetTutorial(this);
            this.TextMeshPro.SetText(this.GetHintText());
            this.TextMeshPro.rectTransform.position = this.frog.transform.position;
            this.TextMeshPro.rectTransform.rotation = this.frog.transform.rotation;
            if (this.IsValidated() && Time.unscaledTime - this._validationTime > 1)
            {
                this.NextTutorial();
                GameObject.Destroy(this.gameObject);
            }

            if (this.TextMeshPro.text != this._lastText)
            {
                Debug.Log("StartTyping");
                this._lastText = this.TextMeshPro.text;
                this._typingEffeect.StartTyping(this._lastText.Length / 20F);
            }
            
            
        }

        public abstract string GetHintText();

        public virtual void ValidateCondition()
        {
            this._validationTime = Time.unscaledTime;
        }

        public bool IsValidated()
        {
            return this._validationTime > -1;
        }
        
        public abstract void NextTutorial();

        public virtual void Jump()
        {
        }

        public virtual void DirectionChange()
        {
        }

        public virtual void AimStart()
        {
        }

        public GameObject GetBasePrefab()
        {
            return
                Caches.PrefabCache.Get("Prefabs/UI/TutorialBase");
        }

        public virtual void PlayerGrab(Grabbable grabbable)
        {
        }
        
    }
}