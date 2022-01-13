using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.World
{
  public abstract class LayerSO : ScriptableObject
  {
    public abstract Layer CreateManager();
    public abstract void AddTile(Layer layer, Tile tile);
  }
}
