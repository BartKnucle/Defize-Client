using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.World.Terrain
{
  public class Tile : FunkySheep.World.Tile 
  {
    public Texture2D heightTexture;
    public Texture2D normalTexture;

    public Tile(Manager world, FunkySheep.World.Layer layer) : base(world, layer)
    {

    }
  }
}