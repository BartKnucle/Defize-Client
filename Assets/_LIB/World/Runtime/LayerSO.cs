using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.World
{
  public abstract class LayerSO : ScriptableObject
  {
    public virtual void CreateManager(GameObject go, Manager world)
    {
      Layer layerComponent = go.AddComponent<Layer>();
      layerComponent.layerSO = this;
      layerComponent.worldSO = world.worldSO;
    }
    public virtual Tile AddTile(Manager world, Layer layer) {
      return new Tile(world, layer);
    }
  }
}
