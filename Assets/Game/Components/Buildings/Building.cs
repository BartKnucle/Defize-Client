using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Buildings/Building", order = 1)]
public class Building : ScriptableObject
{
    
    public int Id;
    public Vector2[] points;
    public float height;
    public Vector2 center;
    public float area;

    public void OnEnable() {
        this.center = Center();
        this.area = Area();
        //Array.Sort(points, PointsSort);

        // Sort all the point in anticlockwise around the center
        bool sorted = false;
        while(!sorted) {
            sorted = true;
            for (int i = 0; i < points.Length; i++)
            {
                if (PointsSort(points[i], points[(i + 1) % points.Length]) == 1) {
                    //Invert the values
                    Vector2 cached = points[i];
                    points[i] = points[(i + 1) % points.Length];
                    points[(i + 1) % points.Length] = cached;
                    sorted = false;
                }
            }
        }
    }

    /// <summary>
    /// Sort All the points anticlockwise
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns>The anticlockwise integer</returns>
    public int PointsSort(Vector2 first, Vector2 second) {
        return -FunkySheep.Vectors.IsClockWise(first, second, this.center);
    }

    public Vector2 Center()
    {
        Vector2 center = Vector2.zero;
        foreach (Vector2 point in points)
        {
            center += point;
        }

        center /= points.Length;
        return center;
    }

    public float Area()
    {
        float area = 0;
        
        for (int i = 0; i < points.Length; i++)
        {
            area += Vector2.Distance(points[i], points[(i + 1) % points.Length]);
        }

        return area;
    }
}
