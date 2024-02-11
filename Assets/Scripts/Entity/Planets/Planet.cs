using System;
using System.Collections.Generic;
using System.Net;
using Defs;
using Entity.Grabbables;
using Entity.Player;
using physic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using util;
using Util;
using Util.Caches;
using Random = Unity.Mathematics.Random;

//[ExecuteInEditMode]
namespace Entity.Planets
{
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Light2D))]
    public class Planet : Attractor
    {

        // Chemin d'accès vers le sprite de la planète.
        public static String planetSprite = "Textures/Circle1024";

        /**
     * Quelques components justes et nécessaires.
     */
        private CircleCollider2D _circleCollider2D;
        private SpriteRenderer _spriteRenderer;
        private Light2D _light;
        private AudioSource _audioSource;

        // Seed utilisée pour sa génération.
        private uint _seed;

        // Taille
        [FormerlySerializedAs("_size")] public float size;
    
        // Random
        private Random _random;

        // La palette de la planète.
        public Palette Palette;
    
        // La liste des collectibles qui orbitent autour de la planète.
        public List<Grabbable> grabbables = new();

        private MapGenerator _mapGenerator;
        private Random _circleRandom;

        // True si lea joueur·se·s est en collision avec la planète. 
        private bool _collideWithPlayer;

        private float lastScore = 0;
        
        public override void Awake()
        {
            base.Awake();
            this._light = this.GetComponent<Light2D>();
        }

        /**
     * Initialisation des variables au début
     */
        public override void Start()
        {
            this._spriteRenderer = this.GetComponent<SpriteRenderer>();
            this._circleCollider2D = this.GetComponent<CircleCollider2D>();
            this.autoPlanet = false;
            this.GetRigidBody().constraints = RigidbodyConstraints2D.FreezeAll;
            this.onlyAttractWhenPlanet = true;
            this._audioSource = this.AddComponent<AudioSource>();
        }

        /**
     * Permet de récupérer le collider de la planète.
     */
        public CircleCollider2D GetCircleCollide()
        {
            return this._circleCollider2D;
        }


        /**
     * A chaque frame, définir les options de la lumière.
     */
        private void Update()
        {
            this._light.color = this.GetPlanetColor();
            this._light.pointLightInnerRadius = this.size / 2F;
            this._light.pointLightOuterRadius = this._light.pointLightInnerRadius + 2;

            Frog theFrog = Frog.TheFrog();
            if (theFrog.playing && this._collideWithPlayer && theFrog.GetSimilitude() >= 99 && Time.unscaledTime - this.lastScore > 0.8F)
            {   
                ColoredShockwave shockwave = ColoredShockwave.Create();
                theFrog.score += Mathf.CeilToInt(10 * ((theFrog.actions * 1F / theFrog.maxActions) * 3f));
                theFrog.IncrementActions(3);
                shockwave.ShockWave(this.transform.position, this.size, this._spriteRenderer.color);
                this.lastScore = Time.unscaledTime;
                AudioClip validation = Caches.SoundCache.Get("Sound/PlanetValidation");
                this._audioSource.clip = validation;
                this._audioSource.Play();
                for (int i = 0; i < 10; i++)
                {
                    Palette randomPalette = Palette.RandomPalette(); 
                    if (randomPalette != this.Palette)
                    {
                        this.SetPalette(randomPalette);
            
                        int o = 0;
                        foreach (Grabbable grabbable in this.grabbables)
                        {
                            o++;
                            if (grabbable.GetType() == typeof(ColoredCircle) && this._random.NextBool())
                            {
                                ColoredCircle circle = grabbable as ColoredCircle;
                                if(circle) 
                                    circle.GetSpriteRenderer().color = randomPalette.RandomWay();
                            }
                        }

                        break;
                    }
                }
            }
        }

        /**
     * Récupère la couleur de la planète.
     */
        public Color GetPlanetColor()
        {
            return this._spriteRenderer.color;
        }

        /**
     * Définit la palette de la planète.
     * Ca me sera utile quand les planètes changeront de couleur, mais ça n'est pas encore codé;
     */
        public void SetPalette(Palette palette)
        {
            this.Palette = palette;
            this._spriteRenderer.color = palette.destination;
        }

        /**
     * Génère une planète avec une seed.
     * <param name="seed">La seed de la planète</param>
     * <param name="generator">Le générateur parent.</param>
     * <param name="planetBase">The base planet prefab</param>
     */
        public static Planet Create(uint seed, PlanetGenerator generator)
        {
            GameObject planetObj = GameObject.Instantiate(Caches.PrefabCache.Get("Prefabs/PlanetBase"));
            SpriteRenderer spriteRenderer = planetObj.GetComponent<SpriteRenderer>();
            Rigidbody2D rigidbody2D = planetObj.GetComponent<Rigidbody2D>();
            Planet planet = planetObj.GetComponent<Planet>();
            planet._random = new Random(seed);
            planet.size = planet._random.NextFloat(5, 20);
            planet._seed = seed;
            spriteRenderer.sprite = Caches.SpriteCache.Get(planetSprite);
            planetObj.transform.localScale = new Vector3(planet.size, planet.size, 1);
            planet.Palette = Palette.RandomPalette(planet._random);
            spriteRenderer.color = planet.Palette.destination;
            rigidbody2D.mass = planet.size * 25F;
            planetObj.transform.SetParent(generator.transform);
            planetObj.transform.position = generator.transform.position;
            return planet;
        }

        /**
     * Génères les cercles colorés qui doivent orbiter autour de la planète.
     */
        public void GenerateCircles(MapGenerator mapGenerator)
        {
            this._circleRandom = new Random(this._seed);
            this._mapGenerator = mapGenerator;
            for (int i = 0; i < 1 * this.size; i++)
            {
            
                this.GenerateCircle();
            }
        }

        public void GenerateCircle()
        {
            Grabbable circle;
            if (_random.NextInt(0, 100) > 4)
            {
                circle = ColoredCircle.Create(this, (uint)(this._circleRandom.NextInt()));
                ((ColoredCircle)circle).GetSpriteRenderer().color = this._circleRandom.NextBool() ? this.Palette.RandomWay(this._circleRandom) : Palette.RanomWayInRandomPalette(this._circleRandom);
            }
            else
            {
                circle = GameObject.Instantiate(Caches.PrefabCache.Get("Prefabs/Pipette")).GetComponent<Pipette>();
            }
            // Ce morceau de code permet de vérifier qu'on ne fasse pas spawn un cercle dans un autre.
            // ON teste 10 fois max des positions, pour en trouver une qui n'entre en collision avec rien.
            for (int security = 0; security < 10; security++)
            {
                float rot = this._circleRandom.NextFloat(0, 365) * Mathf.Deg2Rad;
                Vector2 pos = Helpers.Rotate(
                    new Vector3(0, this._circleRandom.NextFloat(this.size / 2 + 1, this.size + 5), 0),
                    rot);
                
                circle.transform.position = this.transform.position + Helpers.Vec2ToVec3(pos);
                float scale = circle.GetType() == typeof(ColoredCircle) ? this._circleRandom.NextFloat(0.6F, 2) : 1F;
                circle.transform.localScale = new Vector3(scale, scale, scale);

                bool collide = false;
                foreach (Planet mapGeneratorPlanet in this._mapGenerator.planets)
                {
                    foreach (Grabbable grabbable in mapGeneratorPlanet.grabbables)
                    {
                        // Check de radius au lieu de faire avec l'engin physique, ça revient au même puisqu'on travaille
                        // avec des cercles.
                        if (grabbable != null &&Vector2.Distance(grabbable.transform.position, circle.transform.position) <
                            circle.transform.localScale.y + grabbable.transform.localScale.y + 1)
                        {
                            collide = true;
                            break;
                        }
                    }

                    if (collide)
                        break;
                }

                if (!collide)
                {
                    circle.transform.SetParent(this.transform.parent);
                    break;
                }

                // Si au bout de la dixième fois, on n'a toujours pas réussi (le break en haut permet de s'en assurer)
                // Bah on dépop l'objet
                if (security == 9)
                    Destroy(circle.gameObject);
            }
            
            // Ajoute le cercle à la liste des collectibles présents.
            this.grabbables.Add(circle);
        }

        /**
     * Récupère la taille de la planète.
     */
        public float GetSize()
        {
            return this.size;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.rigidbody == Frog.TheFrog().GetRigidbody())
            {
                this._collideWithPlayer = true;
                GameObject instantiate = GameObject.Instantiate(Caches.PrefabCache.Get("Prefabs/Particles/PlanetImpact"));
                instantiate.transform.position = col.contacts[0].point;
                ParticleSystem component = instantiate.GetComponent<ParticleSystem>();
                var componentMain = component.main;
                componentMain.startColor = this._spriteRenderer.color;

            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.rigidbody == Frog.TheFrog().GetRigidbody())
            {
                this._collideWithPlayer = false;
            }
        }
    
    }
}
