using UnityEngine;

namespace util
{
    public class PlanetGenerator : MonoBehaviour
    {

        public uint seed;
        public Planet generatedPlanet;
        
        public Planet Generate()
        {
            Planet o = Planet.Create(seed, this);
            this.transform.name = seed + "";
            this.generatedPlanet = o;
            return o;
        }

        public static PlanetGenerator Generator(uint seed)
        {
            GameObject generator = new GameObject();
            PlanetGenerator generatorComponent = generator.AddComponent<PlanetGenerator>();
            generatorComponent.seed = seed;
            return generatorComponent;
        }
        
    }
}