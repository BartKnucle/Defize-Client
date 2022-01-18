using UnityEngine;
using UnityEngine.Tilemaps;

namespace FunkySheep.World.Buildings
{
  public class Layer : FunkySheep.World.Layer
  {
    public UnityEngine.Terrain terrain;
    private void Start() {
      terrain = world.GetComponentInChildren<UnityEngine.Terrain>();
    }
  }
}
