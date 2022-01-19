using UnityEngine;
using UnityEngine.Tilemaps;

namespace FunkySheep.World.Terrain
{
  public class Layer : FunkySheep.World.Layer
  {
    public Tilemap heightLayer;
    public Tilemap normalLayer;

    public static float ColorToElevation(Color color)
    {
        // Convert from color channel values in 0.0-1.0 range to elevation in meters:
        // https://mapzen.com/documentation/terrain-tiles/formats/#terrarium
        return (color.r * 256.0f * 256.0f + color.g * 256.0f + color.b) - 32768.0f;
    }
  }
}
