using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.World
{
  public abstract class LayerSO : ScriptableObject
  {
    public abstract void Create(Layer layer, Tile tile);
  }
}
