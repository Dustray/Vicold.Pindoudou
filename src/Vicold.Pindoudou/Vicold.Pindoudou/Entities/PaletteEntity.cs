using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vicold.Pindoudou.Entities
{
    public class PaletteEntity
    {
        public Dictionary<string, Vicold.Pindoudou.Entities.Color> Colors { get; set; } = new Dictionary<string, Vicold.Pindoudou.Entities.Color>();
        public List<(string, Vicold.Pindoudou.Entities.Color)> ColorsList { get; set; } = new List<(string, Vicold.Pindoudou.Entities.Color)>();

        public void AddColor(string name, Vicold.Pindoudou.Entities.Color color)
        {
            if (!Colors.ContainsKey(name))
            {
                Colors.Add(name, color);
                ColorsList.Add((name, color));
            }
        }

        public void AddColors(Dictionary<string, Vicold.Pindoudou.Entities.Color> colorDict)
        {
            foreach (var kvp in colorDict)
            {
                AddColor(kvp.Key, kvp.Value);
            }
        }

        public bool RemoveColor(string name)
        {
            if (Colors.Remove(name))
            {
                ColorsList.RemoveAll(item => item.Item1 == name);
                return true;
            }
            return false;
        }

        public void Clear()
        {
            Colors.Clear();
            ColorsList.Clear();
        }

        public Vicold.Pindoudou.Entities.Color GetColor(string name)
        {
            return Colors.TryGetValue(name, out var color) ? color : null;
        }

        public Vicold.Pindoudou.Entities.Color GetColor(int index)
        {
            return index >= 0 && index < ColorsList.Count ? ColorsList[index].Item2 : null;
        }

        public bool ContainsColor(string name)
        {
            return Colors.ContainsKey(name);
        }

        public bool ContainsColor(Vicold.Pindoudou.Entities.Color color)
        {
            return Colors.Values.Any(c => c.Equals(color));
        }

        public Vicold.Pindoudou.Entities.Color FindClosestColor(Vicold.Pindoudou.Entities.Color targetColor)
        {
            if (ColorsList.Count == 0)
            {
                return targetColor;
            }

            Vicold.Pindoudou.Entities.Color closestColor = ColorsList[0].Item2;
            double minDistance = double.MaxValue;

            foreach (var (_, color) in ColorsList)
            {
                double distance = targetColor.DistanceTo(color);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestColor = color;
                }
            }

            return closestColor;
        }

        public int Count => ColorsList.Count;
        public Vicold.Pindoudou.Entities.Color this[int index] => GetColor(index);
        public Vicold.Pindoudou.Entities.Color this[string name] => GetColor(name);
    }
}