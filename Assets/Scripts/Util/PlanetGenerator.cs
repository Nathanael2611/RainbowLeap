using UnityEngine;

namespace util
{
    
    /**
     * Code de génération procédural pour générer plusieurs planètes.
     *
     * Il est pour l'instant assez simple, et c'est le début.
     */
    public class PlanetGenerator : MonoBehaviour
    {

        // La seed, définie depuis l'éditeur, ou pas.
        public uint seed;
        // La planète ayant été générée.
        public Planet generatedPlanet;
        
        /**
         * Génère la planète en fonction de la seed donnée.
         */
        public Planet Generate()
        {
            Planet planet = Planet.Create(seed, this);
            this.transform.name = seed + "";
            this.generatedPlanet = planet;
            return planet;
        }

        /**
         * Crée un générateur de planète avec une seed en particulier. Utile pour le faire depuis le code.
         * <param name="seed">La seed voulue.</param>
         */
        public static PlanetGenerator Generator(uint seed)
        {
            GameObject generator = new GameObject();
            PlanetGenerator generatorComponent = generator.AddComponent<PlanetGenerator>();
            generatorComponent.seed = seed;
            return generatorComponent;
        }
        
    }
}