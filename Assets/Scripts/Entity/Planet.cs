using physic;
using Unity.VisualScripting;
using UnityEngine;

namespace entity
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Planet : Attractor
    {
        private SpriteRenderer _spriteRenderer;
        
        protected override void Start()
        {
            this._spriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        public Color GetPlanetColor()
        {
            return this._spriteRenderer.color;
        }
        
    }
}