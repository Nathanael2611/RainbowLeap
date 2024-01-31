using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Util.Caches;

namespace Entity
{
    public class ColoredShockwave : MonoBehaviour
    {
        public static String planetSprite = "Debug/Circle";

        private SpriteRenderer _spriteRenderer;

        private Vector3 scaleOnStart = Vector3.zero;
        private Color _color;
        private float _timeStart = -1;

        private void Awake()
        {
            this._spriteRenderer = this.AddComponent<SpriteRenderer>();
            this._spriteRenderer.sprite = Caches.SpriteCache.Get(planetSprite);
            this._spriteRenderer.sortingOrder = 300;
        }

        private void Update()
        {
            if (this._timeStart > 0)
            {
                float progress = Mathf.Min(2, Time.time - this._timeStart) / 2;
                if (progress >= 1)
                {
                    Destroy(this.gameObject);
                }

                this._spriteRenderer.color = new Color(this._color.r, this._color.g, this._color.b, 1 - progress);
                this.transform.localScale = this.scaleOnStart * (1 + progress * 3);
            }
        }

        public void ShockWave(Vector3 position, float scale, Color color)
        {
            this._timeStart = Time.time;
            this.scaleOnStart = new Vector3(scale, scale, 1);
            this._color = color;
            this.transform.position = position;
        }

        public static ColoredShockwave Create()
        {
            GameObject wave = new GameObject();
            ColoredShockwave shockwave = wave.AddComponent<ColoredShockwave>();
            return shockwave;
        }
    }
}