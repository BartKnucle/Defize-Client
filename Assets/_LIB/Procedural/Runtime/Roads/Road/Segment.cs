using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.OSM;

namespace FunkySheep.Procedural.Roads
{
  public class Segment
  {
    public Road road;
    public List<Node> nodes = new List<Node>();
    public Segment(Road road)
    {
      this.road = road;
    }
  }
}
