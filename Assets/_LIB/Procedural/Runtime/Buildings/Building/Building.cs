using System;
using System.Linq;
using UnityEngine;
using FunkySheep.OSM;

namespace FunkySheep.Procedural.Buildings
{
  public class Building
  {
    public string id;
    public Vector2[] points;
    public float[] heights;
    public Vector2 center;
    public Vector2 position;
    public float? lowPoint = null;
    public float? hightPoint = null;

    public Building(Way way)
    {
      this.id = way.id.ToString();
      points = new Vector2[way.nodes.Count - 1];
      heights = new float[way.nodes.Count - 1];

      for (int i = 0; i < points.Length; i++)
      {
        points[i].x = (float)FunkySheep.GPS.Utils.lonToX(way.nodes[i].longitude) - FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value.x;
        points[i].y = (float)FunkySheep.GPS.Utils.latToY(way.nodes[i].latitude) - FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value.z;
      }
      
      //this.position = Position();

      //SetFirstPoint();
      // SetClockWise();
    }

    /// <summary>
    /// Calculate the center relative to the points
    /// </summary>
    /// <returns>The center of all points</returns>
    public Vector2 Center()
    {
        // Calculate the center
        Vector2 center = Vector2.zero;
        for (int i = 0; i < points.Length; i++)
        {
            center += points[i];
        }

        center /= points.Length;

        return center;
    }

    /// <summary>
    /// Calculate the building position and set points relatives to it
    /// </summary>
    /// <returns></returns>
    public Vector2 Position()
    {
        position = Center();
        // Set each point relative to the position
        for (int i = 0; i < points.Length; i++)
        {
            points[i] -= position;
        }
        
        return position;
    }

    /// <summary>
    /// Calculate the building area
    /// </summary>
    /// <returns></returns>
    public float Area()
    {
        float area = 0;
        
        for (int i = 0; i < points.Length -1; i++)
        {
            area += Vector2.Distance(points[i], points[i + 1]);
        }

        return area;
    }

    /// <summary>
    /// If the Vector Array is clockwise, return it
    /// </summary>
    /// <returns></returns>
    public void SetClockWise()
    {
        // Skip the last point since it is the same as the first
        int result = FunkySheep.Vectors.IsClockWise(points[1], points[points.Length - 1] , points[0]);
        if (result < 0) {
            Array.Reverse(points);
        }
    }

    /// <summary>
    /// Set the first point of the building (the farest from the center)
    /// </summary>
    public void SetFirstPoint()
    {
        int maxPointIndex = 0;
        Vector2 maxPoint = new Vector2(0, 0);
        for (int i = 0; i < points.Length; i++)
        {
            if (maxPoint.magnitude < points[i].magnitude)
            {
                maxPointIndex = i;
                maxPoint = points[i];
            }
        }

        Vector2[] tempPoints = new Vector2[points.Length];
        Array.Copy(points, tempPoints, points.Length);
        
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = tempPoints[(i + maxPointIndex) %  points.Length];
        }
    }
  }  
}