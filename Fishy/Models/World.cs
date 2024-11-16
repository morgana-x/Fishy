using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Fishy.Models
{
    internal class World
    {
        public List<Vector3> FishSpawns { get; set; }
        public List<Vector3> TrashPoints { get; set; }
        public List<Vector3> ShorelinePoints { get; set; }
        public List<Vector3> HiddenSpots { get; set; }

        public World()
        {
            FishSpawns = [];
            TrashPoints = [];
            ShorelinePoints = [];
            HiddenSpots = [];
        }

        public bool LoadWorld(string filePath)
        {
            string[] contentLines = File.ReadAllText(filePath).Split("\n");
            for (int i = 0; i < contentLines.Length; i++)
            {
                Match match = Regex.Match(contentLines[i], @"groups=\[""(.+)""\]\]");
                if (!match.Success)
                    continue;

                string transformPattern = @"Transform\(.*?,\s*(-?\d+\.?\d*),\s*(-?\d+\.?\d*),\s*(-?\d+\.?\d*)\s*\)";

                Match matchTransform = Regex.Match(contentLines[i + 1], transformPattern);
                if (!matchTransform.Success)
                    continue;

                Vector3 location = new(
                    float.Parse(matchTransform.Groups[1].Value, CultureInfo.InvariantCulture),
                    float.Parse(matchTransform.Groups[2].Value, CultureInfo.InvariantCulture),
                    float.Parse(matchTransform.Groups[3].Value, CultureInfo.InvariantCulture));

                switch (match.Groups[1].Value)
                {
                    case "fish_spawn":
                        FishSpawns.Add(location);
                        break;
                    case "shoreline_point":
                        ShorelinePoints.Add(location);
                        break;
                    case "trash_point":
                        TrashPoints.Add(location);
                        break;
                    case "hidden_spot":
                        HiddenSpots.Add(location);
                        break;
                }
            }
            return true;
        }
    }
}
