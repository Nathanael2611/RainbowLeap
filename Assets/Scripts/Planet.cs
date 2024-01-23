using System;
using System.Collections;
using System.Collections.Generic;
using Defs;
using entity;
using physic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using util;
using Random = Unity.Mathematics.Random;
using Update = UnityEngine.PlayerLoop.Update;

//[ExecuteInEditMode]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Planet : Attractor
{

    public static String planetSprite = "Debug/Circle";
    
    private SpriteRenderer _spriteRenderer;

    private CircleCollider2D _circleCollider2D;
    private Light2D _light;

    private uint _seed;

    private float _size;

    public Palette Palette;

    public List<Grabbable> grabbables = new();
    //private MeshRenderer _meshRenderer;
    //private ShapeRenderer _shapeRenderer;

    //public Color planetColor;
    //private ShapeRenderer _shapeRenderer;
    //private MeshRenderer _meshRenderer;
    //private CircleCollider2D _collider;

    public override void Start()
     {
         this._spriteRenderer = this.GetComponent<SpriteRenderer>();
         this._circleCollider2D = this.GetComponent<CircleCollider2D>();
         this.autoPlanet = false;
         this.GetRigidBody().constraints = RigidbodyConstraints2D.FreezeAll;
         this.onlyAttractWhenPlanet = true;

         this._light = this.AddComponent<Light2D>();
         
         //this._meshRenderer = this.AddComponent<MeshRenderer>();
         //this.AddComponent<MeshFilter>();
         //this._shapeRenderer = this.AddComponent<ShapeRenderer>();

         //this._shapeRenderer = this.GetComponent<ShapeRenderer>();

         //this._meshRenderer = this.GetComponent<MeshRenderer>();
         //this._collider = this.GetComponent<CircleCollider2D>();
     }

    public CircleCollider2D GetCircleCollide()
    {
        return this._circleCollider2D;
    }


    private void Update()
    {
        //this._meshRenderer.material.color = this.planetColor;
        //this._collider.radius = this._shapeRenderer.polygonRadius;
        //this._spriteRenderer.color = this.planetColor;
        this._light.color = this.GetPlanetColor();
        this._light.pointLightInnerRadius = this._size / 2F;
        this._light.pointLightOuterRadius = this._light.pointLightInnerRadius + 2;
    }

    public Color GetPlanetColor()
    {
        return this._spriteRenderer.color;
    }

    public void SetPalette(Palette palette)
    {
        this.Palette = palette;
        Debug.Log(palette.destination);
        this._spriteRenderer.color = palette.destination;
    }

    
    public static Planet Create(uint seed, PlanetGenerator generator)
    {
        Random random = new Random(seed);

        GameObject planetObj = new GameObject();
        SpriteRenderer spriteRenderer = planetObj.AddComponent<SpriteRenderer>();
        Rigidbody2D rigidbody2D = planetObj.AddComponent<Rigidbody2D>();
        planetObj.AddComponent<Planet>();

        Planet planet = planetObj.GetComponent<Planet>();
        planet._size = random.NextFloat(5, 20);
        planet._seed = seed;
        spriteRenderer.sprite = Caches.SpriteCache.Get(Planet.planetSprite);
        planetObj.transform.localScale = new Vector3(planet._size, planet._size, 1);
        //spriteRenderer.color = Color.HSVToRGB(random.NextFloat(0, 1), random.NextFloat(0.5f, 1), random.NextFloat(0.5F, 1));
        planet.Palette = Defs.Palette.RandomPalette(random);
        spriteRenderer.color = planet.Palette.destination;
        rigidbody2D.mass = planet._size * 25F;
        
        planetObj.transform.SetParent(generator.transform);
        planetObj.transform.position = generator.transform.position;


        

        
        return planet;
    }

    public void GenerateCircles(MapGenerator mapGenerator)
    {
        Random random = new Random(this._seed);
        for (int i = 0; i < 1 * this._size; i++)
        {
            ColoredCircle circle = ColoredCircle.Create(this, (uint)(this._seed + i * this._size));
            
            for (int security = 0; security < 10; security++)
            {
                float rot = random.NextFloat(0, 365) * Mathf.Deg2Rad;
                Vector2 pos = Helpers.Rotate(
                    new Vector3(0, random.NextFloat(this._size, this._size + 5), 0), rot);
                
                circle.transform.position = this.transform.position + Helpers.Vec2ToVec3(pos);

                bool collide = false;
                foreach (Planet mapGeneratorPlanet in mapGenerator.planets)
                {
                    foreach (Grabbable grabbable in mapGeneratorPlanet.grabbables)
                    {
                        if (Vector2.Distance(grabbable.transform.position, circle.transform.position) <
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

                if (security == 9)
                    GameObject.Destroy(circle.gameObject);
            }
            
            this.grabbables.Add(circle);
        }
    }

    public float GetSize()
    {
        return this._size;
    }
}
