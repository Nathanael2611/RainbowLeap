using System;
using Entity.Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        private RectTransform   _rect;
        private RawImage        _rawImage;

        public float changeDuration = 1;
        
        private int _lastHealth = 0, _healthToLerp = 0;
        private float _changeTime;
        
        private float _totalWidth;
        
        private void Awake()
        {
            this._rect = this.GetComponent<RectTransform>();
            this._rawImage = this.GetComponent<RawImage>();
        }

        private void Start()
        {
            this._totalWidth = this._rect.sizeDelta.x;
        }

        private void Update()
        {

            int healthInt = Frog.TheFrog.actions;
            if (healthInt != this._lastHealth)
            {
                this._healthToLerp = this._lastHealth;
                this._changeTime = Time.unscaledTime;
            }

            this._lastHealth = healthInt;
            float health = Mathf.Lerp(this._healthToLerp, healthInt, Mathf.Max(0, Mathf.Min(this.changeDuration, Time.unscaledTime - this._changeTime)) / this.changeDuration);

            this._rect.sizeDelta = new Vector2((Frog.TheFrog.maxActions - health) * this._totalWidth / Frog.TheFrog.maxActions, this._rect.sizeDelta.y);
            Color color = Color.HSVToRGB((Mathf.Cos(Time.time * 0.2F) + 1F) / 2F, 1, 1);
            this._rawImage.color = color;
        }
        
    }
}