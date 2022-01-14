using UnityEngine;
using UnityEngine.Tilemaps;

namespace FunkySheep.World.Terrain
{
  [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
  public class Layer : FunkySheep.World.Layer
  {
    public Tilemap heightLayer;
    public Tilemap normalLayer;
  }
}
