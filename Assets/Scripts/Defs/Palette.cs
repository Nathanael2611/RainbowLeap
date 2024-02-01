using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Scrtwpns.Mixbox;
using UnityEditor;
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
    public class Palette : MonoBehaviour, IComparable<Palette>
    {

        // Liste de toutes les palettes enregistrées.
        public static List<Palette> palettes = new();
        
        /**
         * Enregistre les palettes.
         */
        public static void RegisterPalettes()
        {
            //palettes.Clear();
            //palettes.Add(New(Mixbox.Lerp(Helpers.ColorFromHex("#F4D003"), Helpers.ColorFromHex("#0157FB"), 0.5F)).Way(Helpers.ColorFromHex("#F4D003")).Way(Helpers.ColorFromHex("#0157FB")).Build());
            //palettes.Add(New("#D4479B").Way(Color.red).Way(Color.white).Build());
            //palettes.Add(New("#7F3C9D").Way("#0287CA").Way(Color.red).Build());
            //palettes.Add(New("#FB611B").Way("#ED2851").Way("#FFCE18").Build());
            //palettes.Add(New(Color.grey).Way(Color.black).Way(Color.white).Build());
            //palettes.Add(New("#AB755F").Way("#A9D529").Way("#A72D95").Build());
            //palettes.Add(New("#AEB71E").Way("#2A892D").Way("#FEFF01").Way(Color.white).Build());
            //palettes.Add(New("#3D317B").Way("#823A84").Way("#1F4F99").Way(Color.grey).Build());
        }

        private void OnEnable()
        {
            this.destination = new Color(this.destination.r, this.destination.g, this.destination.b, 1);
            List<Color> ways = new();
            foreach (Color color in this.ways)
            {
                ways.Add(new Color(color.r, color.g, color.b, 1F));
            }

            this.ways = ways;
            palettes.Add(this);
            palettes.Sort();
            //Debug.Log("Register palettes");
        }
        
        // La couleur finale de la palette 
        public Color destination;
        // Les couleurs pouvant être utiles pour sa composition.
        public List<Color> ways;

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
        public static Builder New(Color destination)
        {
            return new Builder(destination);
        }

        public static Builder New(string destination)
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

            public Builder Way(Color color)
            {
                this._ways.Add(color);
                return this;
            }

            public Builder Way(string hex)
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

        public int IntValue()
        {
            int total = 0;
            foreach (var color in this.ways)
            {
                total += (int) (color.r + color.g + color.b + color.a) * 256; 
            }

            total += (int)(this.destination.r + this.destination.g + this.destination.b + this.destination.a);
            return total;
        }
        
        public int CompareTo(Palette other)
        {
            return this.IntValue() - other.IntValue();
        }
    }

}