using UnityEngine;
using UnityEngine.Tilemaps;

namespace FunkySheep.World.Buildings
{
  public class Layer : FunkySheep.World.Layer, IDropable
  {
    public UnityEngine.TerrainData terrainData {get; set;}
    
    public float GetHeight(Vector2 position)
    {
      return terrainData.GetInterpolatedHeight(position.x, position.y);
    }
  }
}
