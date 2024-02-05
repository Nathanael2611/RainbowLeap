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
            this._rect.sizeDelta = new Vector2((Frog.TheFrog.maxActions - Frog.TheFrog.actions) * this._totalWidth / Frog.TheFrog.maxActions, this._rect.sizeDelta.y);
            Color color = Color.HSVToRGB((Mathf.Cos(Time.time * 0.2F) + 1F) / 2F, 1, 1);
            this._rawImage.color = color;
        }
        
    }
}