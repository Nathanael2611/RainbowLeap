using System.Collections.Generic;
using Scrtwpns.Mixbox;
using UnityEngine;
using util;
using Random = Unity.Mathematics.Random;

namespace Defs
{
    
    /**
     * Cette class va servir a définir des palettes de couleurs pour la génération des planètes, et des cercles colorés
     * qui l'entourent.
     *
     * En gros, chaque Palette contient une couleur, et une liste de couleurs utiles pour la composer lors de mélanges.
     */
    public class Palette
    {

        // Liste de toutes les palettes enregistrées.
        public static List<Palette> palettes = new();

        /**
         * Enregistre les palettes.
         */
        public static void RegisterPalettes()
        {
            Palette.palettes.Clear();
            Palette.palettes.Add(Palette.Create(Mixbox.Lerp(Helpers.ColorFromHex("#F4D003"), Helpers.ColorFromHex("#0157FB"), 0.5F)).AddWay(Helpers.ColorFromHex("#F4D003")).AddWay(Helpers.ColorFromHex("#0157FB")).Build());
            Palette.palettes.Add(Palette.Create("#D4479B").AddWay(Color.red).AddWay(Color.white).Build());
            Palette.palettes.Add(Palette.Create("#7F3C9D").AddWay("#0287CA").AddWay(Color.red).Build());
            Palette.palettes.Add(Palette.Create("#FB611B").AddWay("#ED2851").AddWay("#FFCE18").Build());
            Palette.palettes.Add(Palette.Create(Color.grey).AddWay(Color.black).AddWay(Color.white).Build());
            Palette.palettes.Add(Palette.Create("#AB755F").AddWay("#A9D529").AddWay("#A72D95").Build());
        }

        // La couleur finale de la palette 
        public readonly Color destination;
        // Les couleurs pouvant être utiles pour sa composition.
        public readonly List<Color> ways;

        /**
         * Constructeur.
         * <param name="destination">La couleur de destination</param>
         * <param name="colors">Les couleurs pour la composer</param>
         */
        private Palette(Color destination, List<Color> colors)
        {
            this.destination = destination;
            this.ways = colors;
        }

        /**
         * Choisi une palette aléatoirement dans la liste des palettes enregistrées.
         * <param name="random">L'instance de Random a utiliser. (pratique pour la génération procédurale)</param>
         */
        public static Palette RandomPalette(Random random)
        {
            return palettes[random.NextInt(0, palettes.Count)];
        }

        public static Palette RandomPalette()
        {
            return palettes[UnityEngine.Random.Range(0, palettes.Count)];
        }

        public static Color RanomWayInRandomPalette(Random random)
        {
            return RandomPalette(random).RandomWay(random);
        }
        
        /**
         * Choisi une couleur aléatoirement parmi les couleurs pouvant composer la destination.
         * <param name="random">L'instance de Random a utiliser. (pratique pour la génération procédurale)</param>
         */
        public Color RandomWay(Random random)
        {
            return this.ways[random.NextInt(0, this.ways.Count)];
        }
        
        public Color RandomWay()
        {
            return this.ways[UnityEngine.Random.Range(0, this.ways.Count)];
        }

        /**
         * Crées un Builder de palette pour la couleur souhaitée.
         * <param name="destination">Couleur de destination avec laquelle initialiser le builder.</param>
         */
        public static Builder Create(Color destination)
        {
            return new Builder(destination);
        }

        public static Builder Create(string destination)
        {
            return new Builder(destination);
        }

        /**
         * Cette sous classe va permettre de simplifier la définition de Palette, en permettant de tout faire sur une ligne,
         * Nul besoin de créer une liste, il va le faire pour nous
         */
        public class Builder
        {
            private readonly Color _destination;
            private List<Color> _ways = new();

            public Builder(Color destination)
            {
                this._destination = destination;
            }
            
            public Builder(string hex)
            {
                this._destination = Helpers.ColorFromHex(hex);
            }

            public Builder AddWay(Color color)
            {
                this._ways.Add(color);
                return this;
            }

            public Builder AddWay(string hex)
            {
                this._ways.Add(Helpers.ColorFromHex(hex));
                return this;
            }

            /**
             * Construis la Palette
             */
            public Palette Build()
            {
                return new Palette(this._destination, this._ways);
            }

        }

    }
}