using System;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;
using util;
using Random = Unity.Mathematics.Random;

namespace entity
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ColoredCircle : Grabbable
    {
        public static String circleSprite = "Debug/Circle";
        
        private SpriteRenderer _spriteRenderer;
        private Light2D _light;

        private void Awake()
        {
            this._spriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        public override void Start()
        {
            base.Start();
            this.RigidBody.gravityScale = 0;
            this.RigidBody.mass = 0.1f;
            this.RigidBody.constraints = RigidbodyConstraints2D.FreezePosition;
            this._light = this.GetComponent<Light2D>();
        }


        public Color GetColor()
        {
            return this._spriteRenderer.color;
        }

        public SpriteRenderer GetSpriteRenderer()
        {
            return this._spriteRenderer;
        }

        public override Vector2 GrabScaleFactor()
        {
            return new Vector2(0.5F, 0.5F);
        }
        
        public override void PlayerGrab(Player player)
        {
            player.setObjectiveColor(this.GetColor());
            GameObject.Destroy(this.gameObject);   
        }

        private void Update()
        {
            this._light.color = this.GetColor();
        }


        public static ColoredCircle Create(Planet futureParent, uint seed)
        {
            Random random = new Random(seed);

            GameObject circleObj = new GameObject();

            ColoredCircle coloredCircle = circleObj.AddComponent<ColoredCircle>();
            RotationLock rotationLock = circleObj.AddComponent<RotationLock>();
            rotationLock.x = true;
            rotationLock.y = true;

            float size = random.NextFloat(0.25f, 2f);
            
            circleObj.transform.localScale.Set(size, size, size);

            coloredCircle._spriteRenderer.color = Color.HSVToRGB(random.NextFloat(0f, 1), random.NextFloat(0.5F, 1F),
                random.NextFloat(0.5f, 1));
            coloredCircle._spriteRenderer.color = futureParent.Palette.RandomWay(random);
            coloredCircle._spriteRenderer.sprite = Caches.SpriteCache.Get(circleSprite);

            Light2D light2D = coloredCircle.AddComponent<Light2D>();
            light2D.pointLightOuterRadius = 5;
            
            return coloredCircle;
        }
    }
}