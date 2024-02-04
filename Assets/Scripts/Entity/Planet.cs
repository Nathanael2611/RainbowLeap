using System;
using System.Collections.Generic;
using Defs;
using entity;
using Entity;
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
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Light2D))]
public class Planet : Attractor
{

    // Chemin d'accès vers le sprite de la planète.
    public static String planetSprite = "Debug/Circle";

    /**
     * Quelques components justes et nécessaires.
     */
    private CircleCollider2D _circleCollider2D;
    private SpriteRenderer _spriteRenderer;
    private Light2D _light;

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

        if (Frog.TheFrog.playing && this._collideWithPlayer && Frog.TheFrog.GetSimilitude() >= 99)
        {   
            ColoredShockwave shockwave = ColoredShockwave.Create();
            Frog.TheFrog.score += (int)(10 / ((this.size / 2)) * Frog.TheFrog.actions);
            Frog.TheFrog.IncrementActions(5);
            shockwave.ShockWave(this.transform.position, this.size, this._spriteRenderer.color);
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
        ColoredCircle circle = ColoredCircle.Create(this, (uint)(_circleRandom.NextInt()));
            
            circle.GetSpriteRenderer().color = this._circleRandom.NextBool() ? this.Palette.RandomWay(this._circleRandom) : Palette.RanomWayInRandomPalette(this._circleRandom);
            // Ce morceau de code permet de vérifier qu'on ne fasse pas spawn un cercle dans un autre.
            // ON teste 10 fois max des positions, pour en trouver une qui n'entre en collision avec rien.
            for (int security = 0; security < 10; security++)
            {
                float rot = this._circleRandom.NextFloat(0, 365) * Mathf.Deg2Rad;
                Vector2 pos = Helpers.Rotate(
                    new Vector3(0, this._circleRandom.NextFloat(this.size / 2 + 1, this.size + 5), 0),
                    rot);
                
                circle.transform.position = this.transform.position + Helpers.Vec2ToVec3(pos);
                float scale = this._circleRandom.NextFloat(0.6F, 2);
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
        if (col.rigidbody == Frog.TheFrog.GetRigidbody())
        {
            this._collideWithPlayer = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.rigidbody == Frog.TheFrog.GetRigidbody())
        {
            this._collideWithPlayer = false;
        }
    }
    
}
