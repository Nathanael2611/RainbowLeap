using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Entity.Planets
{
    public class RainbowPlanet : MonoBehaviour
    {

        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            this._spriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            Color color = Color.HSVToRGB((Mathf.Cos(Time.time * 0.2F) + 1F) / 2F, 1, 1);
            this._spriteRenderer.color = color;
        }
    }
}