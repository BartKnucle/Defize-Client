using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.World.Roads
{
  public class Point : FunkySheep.World.OSM.Point
  {
    public bool inter = false;
    public bool liked = false;
  
    public Point(double latitude, double longitude, Vector3 initialMercatorPosition) : base(latitude, longitude, initialMercatorPosition)
    {

    }
  }
}