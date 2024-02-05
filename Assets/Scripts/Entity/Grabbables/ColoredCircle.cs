using System;
using Entity.Planets;
using Entity.Player;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;
using util;
using Util;
using Util.Caches;
using Random = Unity.Mathematics.Random;

namespace Entity.Grabbables
{
    
    /**
     * Cette classe représente le cercle coloré qui va être récupéré, et mélangé avec le·la joueur·se.
     */
    [RequireComponent(typeof(Light2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class ColoredCircle : Grabbable
    {
        
        // Emplacement du sprite
        public static String circleSprite = "Debug/Circle";
        
        /**
         * Définitions des components utilisés ici.
         */
        private SpriteRenderer _spriteRenderer;
        private Light2D _light;

        public Planet parentPlanet;
        
        private void Awake()
        {
            this._spriteRenderer = this.GetComponent<SpriteRenderer>();
            this._light = this.GetComponent<Light2D>();
        }

        public override void Start()
        {
            base.Start();
            this.RigidBody.gravityScale = 0;
            this.RigidBody.mass = 0.1f;
            this.RigidBody.constraints = RigidbodyConstraints2D.FreezePosition;
        }
        
        /**
         * Récupère la couleur du SpriteRenderer
         * C'est la couleur qui sera mélangée.
         */
        public Color GetColor()
        {
            return this._spriteRenderer.color;
        }

        /**
         * Récupère le SpriteRenderer.
         * TODO: Sera peut être supprimé, à voir si c'est utile. J'ai pour réflexe de tout mettre en private,
         * TODO: et de faire des getters par sécurité.
         */
        public SpriteRenderer GetSpriteRenderer()
        {
            return this._spriteRenderer;
        }

        /**
         * La taille à laquelle l'objet termine à ras d'être récupéré par le·la joueur·se.
         */
        public override Vector2 GrabScaleFactor()
        {
            return new Vector2(0.5F, 0.5F);
        }
        
        /**
         * Ce qui se passe quand l'objet est récupéré par le·la joueur·se.
         * En gros, on set sa couleur objectif à la couleur de cet objet.
         * Et on détruit cet objet.
         * <param name="player">L'instance du·de la joueur·se qui le récupère.</param>
         */
        public override void PlayerGrab(Frog player)
        {
            player.setObjectiveColor(this.GetColor(), this.GetDensity());
            this.parentPlanet.grabbables.Remove(this);
            GameObject.Destroy(this.gameObject);
            this.parentPlanet.GenerateCircle();
            player.IncrementActions(-1);
        }

        public float GetDensity()
        {
            float scale = this.GetScaleOnGrab().y;
            float f = Mathf.Max(0.1f, Math.Min(0.9f, (scale - 0.6f) * 1f / 1.4F));
            return f;
        }

        /**
         * A chaque frame, on définit la couleur de la lumière à celle de l'objet.
         */
        private void Update()
        {
            this._light.color = this.GetColor();
        }

        public override void ResetScale()
        {
            this.transform.localScale = this.GetScaleOnGrab();
        }

        /**
         * Méthode qui permet de créer un cercle coloré directement depuis le code, utilisé dans la génération
         * procédurale.
         * <param name="futureParent">La planète autour de laquelle l'objet orbite.</param>
         * <param name="seed">La graine utilisée pour la génération aléatoire.</param>
         */
        public static ColoredCircle Create(Planet futureParent, uint seed)
        {
            Random random = new Random(seed);
            
            GameObject circleObj = GameObject.Instantiate(Caches.PrefabCache.Get("Prefabs/ColoredCircleBase"));
            //GameObject circleObj = new GameObject();

            ColoredCircle coloredCircle = circleObj.GetComponent<ColoredCircle>();
            coloredCircle.parentPlanet = futureParent;
            RotationLock rotationLock = circleObj.GetComponent<RotationLock>();
            rotationLock.x = true;
            rotationLock.y = true;

            float size = random.NextFloat(0.25f, 1.4F);
            if (random.NextBool())
                size += random.NextFloat(0, 0.5F);
            
            circleObj.transform.localScale.Set(size, size, size);
            coloredCircle._spriteRenderer.color = Color.HSVToRGB(random.NextFloat(0f, 1), random.NextFloat(0.5F, 1F),
                random.NextFloat(0.5f, 1));
            coloredCircle._spriteRenderer.sprite = Caches.SpriteCache.Get(circleSprite);

            coloredCircle._light.pointLightOuterRadius = 5;
            
            return coloredCircle;
        }
    }
}