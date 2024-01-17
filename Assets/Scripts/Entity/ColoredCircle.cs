using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace entity
{
    [InitializeOnLoad]
    [RequireComponent(typeof(SpriteRenderer))]
    public class ColoredCircle : Grabbable

    {
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            this._spriteRenderer = this.GetComponent<SpriteRenderer>();
        }
        

        public Color GetColor()
        {
            return this._spriteRenderer.color;
        }

        public override Vector2 GrabScaleFactor()
        {
            return new Vector2(0.5F, 0.5F);
        }
        
        public override void PlayerGrab(Player player)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}