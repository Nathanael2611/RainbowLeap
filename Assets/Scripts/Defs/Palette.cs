using System.Collections.Generic;
using UnityEngine;
using util;
using Random = Unity.Mathematics.Random;

namespace Defs
{
    
    
    
    
    public class Palette
    {

        public static List<Palette> palettes = new();

        public static void RegisterPalettes()
        {
            Palette.palettes.Clear();
            Palette.palettes.Add(Palette.Create(Helpers.ColorFromHex("#F4D003"))
                .AddWay(Helpers.ColorFromHex("#48A35A"))
                .AddWay(Helpers.ColorFromHex("#0157FB"))
                .Build());
            Palette.palettes.Add(Palette.Create(new Color(255/255F, 0, 204/255F))
                .AddWay(Color.red)
                .AddWay(Color.white)
                .Build());
        }

        public readonly Color destination;
        public readonly List<Color> ways;

        public Palette(Color destination, List<Color> colors)
        {
            this.destination = destination;
            this.ways = colors;
        }

        public static Palette RandomPalette(Random random)
        {
            return palettes[random.NextInt(palettes.Count)];
        }

        public Color RandomWay(Random random)
        {
            return this.ways[random.NextInt(this.ways.Count)];
        }

        public static Builder Create(Color destination)
        {
            return new Builder(destination);
        }

        public class Builder
        {
            private readonly Color _destination;
            private List<Color> _ways = new();

            public Builder(Color destination)
            {
                this._destination = destination;
            }

            public Builder AddWay(Color color)
            {
                this._ways.Add(color);
                return this;
            }

            public Palette Build()
            {
                return new Palette(this._destination, this._ways);
            }

        }

    }
}