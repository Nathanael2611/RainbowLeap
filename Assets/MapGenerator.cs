using System;
using System.Collections;
using System.Collections.Generic;
using Defs;
using UnityEngine;
using util;
using Random = Unity.Mathematics.Random;

public class MapGenerator : MonoBehaviour
{

    public uint seed;
    public List<Planet> planets = new();

    void Start()
    {
        Palette.RegisterPalettes();
        Random random = new Random(this.seed);
        PlanetGenerator last = null;
        for (int i = 0; i < 6; i++)
        {
            PlanetGenerator generator = PlanetGenerator.Generator((uint)(this.seed + i * (this.seed / 2)));
            generator.Generate();
            if (last != null)
            {
                float prevScaleX = last.generatedPlanet.transform.localScale.x;
                float nowScaleX = generator.generatedPlanet.transform.localScale.x;
                
                for (int security = 0; security < 10; security++)
                {
                    float turnAround = random.NextFloat(0, 360) * Mathf.Deg2Rad;
                    Vector2 position = Helpers.Rotate(new Vector3(0, (prevScaleX + nowScaleX) * 1.3F, 0), turnAround);

                    
                    generator.transform.position = last.transform.position +
                                                   Helpers.Vec2ToVec3(position);

                    bool collide = false;
                    foreach (Planet planet in this.planets)
                    {
                        if (Vector2.Distance(generator.transform.position, planet.transform.position) <
                            planet.GetSize() + generator.generatedPlanet.GetSize() + 1)
                        {
                            collide = true;
                            break;
                        }
                    }

                    if (!collide)
                        break;
                    if (security == 9)
                        GameObject.Destroy(generator);
                }
                
            }

            last = generator;
            planets.Add(generator.generatedPlanet);
        }
        foreach (Planet planet in planets)
        {
            planet.GenerateCircles(this);
        }
    }

}
