using System;
using Entity.Player;
using TMPro;
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

        private void Start()
        {
            this.TextMeshPro = this.GetComponent<TextMeshProUGUI>();
            
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
            if (this._validationTime > -1 && Time.unscaledTime - this._validationTime > 1)
            {
                this.NextTutorial();
                GameObject.Destroy(this.gameObject);
            }
        }

        public abstract string GetHintText();

        public virtual void ValidateCondition()
        {
            this._validationTime = Time.unscaledTime;
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
        
    }
}