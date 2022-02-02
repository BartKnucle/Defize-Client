using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.OSM;

namespace FunkySheep.Procedural.Roads
{
  public class Segment: MonoBehaviour
  {
    public Way way;
    public List<Node> nodes = new List<Node>();    
  }
}
