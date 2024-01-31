using System;
using System.Collections.Generic;
using Defs;
using Entity.Player;
using UnityEngine;
using util;
using Random = Unity.Mathematics.Random;

namespace Util
{
    /**
     * Le générateur qui va générer une map aléatoire basée sur une seed.
     */
    public class MapGenerator : MonoBehaviour
    {

        // La seed a utiliser, définie dans l'éditeur, ou pas 
        public uint seed;
        // La liste des planètes déjà générées, accessible dans l'éditeur
        public List<Planet> planets = new();
        public int planetCount = 6;
        public int optimizationRange = 30;

        public float lastVisibilityUpdate = -10F;
        
        private void FixedUpdate()
        {
            if (Time.time - this.lastVisibilityUpdate > 2.5F)
            {
                this.lastVisibilityUpdate = Time.time;
                for (int i = 0; i < this.transform.childCount; i++)
                {
                    Transform child = this.transform.GetChild(i);
                    child.gameObject.SetActive(Vector3.Distance(Frog.TheFrog.transform.position, child.position) < this.optimizationRange);
                }
            }
        }

        /**
         * Dès le début, génères les planètes procéduralement.
         */
        void Start()
        {
            // Register les palettes
            // TODO: changer ça de place, c'pas propre DU TOUT.
            Palette.RegisterPalettes();
            // Définition du random avec la seed.
            Random random = new Random(this.seed);
            // Va stocker la dernière planète générée, utile dans le for ci dessous, pour les mettre à bonne distance
            // en fonction de leur radius.
            PlanetGenerator last = null;
            for (int i = 0; i < this.planetCount; i++)
            {
                PlanetGenerator generator = PlanetGenerator.Generator((uint)(this.seed + i * (this.seed / 2)));
                generator.Generate();
                if (last != null)
                {
                    float prevScaleX = last.generatedPlanet.transform.localScale.x;
                    float nowScaleX = generator.generatedPlanet.transform.localScale.x;
                
                    // Permet de s'assurer qu'on ne fasse pas pop une planète dans une autre.
                    for (int security = 0; security < 10; security++)
                    {
                        float turnAround = random.NextFloat(0, 360) * Mathf.Deg2Rad;
                        Vector2 position = Helpers.Rotate(new Vector3(0, (prevScaleX + nowScaleX) * 1.3F, 0), turnAround);

                    
                        generator.transform.position = last.transform.position +
                                                       Helpers.Vec2ToVec3(position);

                        bool collide = false;
                        foreach (Planet planet in this.planets)
                        {
                            // check de radius, ça revient au même, on parle de cercles parfaits.
                            if (Vector2.Distance(generator.transform.position, planet.transform.position) <
                                planet.GetSize() + generator.generatedPlanet.GetSize() + 1)
                            {
                                collide = true;
                                break;
                            }
                        }

                        if (!collide)
                        {
                            Vector3 pos = Vector3.Lerp(last.generatedPlanet.transform.position, generator.transform.position, 0.5F);
                            GameObject propulsor =
                                GameObject.Instantiate(Caches.Caches.PrefabCache.Get("Prefabs/PropulsorBase"), generator.transform, true);
                            propulsor.transform.position = pos;
                            break;
                        }
                        
                        // Si on n'a pas trouvé de position au bout de 10 essais, alors on supprime la planète, tant pis :(
                        if (security == 9)
                            Destroy(generator);
                    }
                
                }

                last = generator;
                generator.transform.SetParent(this.transform);
                // ajout de la planète à la liste des planètes.
                planets.Add(generator.generatedPlanet);
            }
            // Après avoir généré les planètes, on itère sur ces dernières pour leur générer leurs cercles colorés.
            // C'est important de le faire APRES, pour que les cercles prennent en compte les cercles des autres
            // dans leur méthode d'apparition.
            foreach (Planet planet in planets)
            {
                planet.GenerateCircles(this);
            }
        }

    }
}
