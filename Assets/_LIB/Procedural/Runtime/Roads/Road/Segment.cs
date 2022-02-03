using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.OSM;

namespace FunkySheep.Procedural.Roads
{
  public class Segment
  {
    public long id;
    public int index;
    public List<Node> nodes = new List<Node>();

    public Segment(long id, int index)
    {
      this.id = id;
      this.index = index;
    }
  }
}
