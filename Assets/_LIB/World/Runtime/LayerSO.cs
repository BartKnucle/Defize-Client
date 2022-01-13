using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.World
{
  public abstract class LayerSO : ScriptableObject
  {
    public virtual void CreateManager(GameObject go, WorldSO worldSO)
    {
      Layer layerComponent = go.AddComponent<Layer>();
      layerComponent.layerSO = this;
      layerComponent.worldSO = worldSO;
    }
    public abstract void AddTile(Layer layer, Tile tile);
  }
}
