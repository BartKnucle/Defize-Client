using System;
using System.Linq;
using UnityEngine;
using FunkySheep.OSM;

namespace FunkySheep.Procedural.Roads
{
  public class Road : MonoBehaviour
  {
    public string id;
    public Vector3[] points;
    public Road(Way way)
    {
      /*this.id = way.id.ToString();
      points = new Vector2[way.nodes.Count - 1];
      heights = new float[way.nodes.Count - 1];

      for (int i = 0; i < points.Length; i++)
      {
        points[i].x = (float)FunkySheep.GPS.Utils.lonToX(way.nodes[i].longitude) - FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value.x;
        points[i].y = (float)FunkySheep.GPS.Utils.latToY(way.nodes[i].latitude) - FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value.z;
      }*/
    }
  }  
}