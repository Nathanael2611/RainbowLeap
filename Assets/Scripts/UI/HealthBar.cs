using System;
using Entity.Player;
using UnityEngine;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        private RectTransform _rect;

        private float _totalWidth;
        
        private void Awake()
        {
            this._rect = this.GetComponent<RectTransform>();
        }

        private void Start()
        {
            this._totalWidth = this._rect.sizeDelta.x;
        }

        private void Update()
        {
            this._rect.sizeDelta = new Vector2(Frog.TheFrog.actions * this._totalWidth / Frog.TheFrog.maxActions, this._rect.sizeDelta.y);
        }
    }
}