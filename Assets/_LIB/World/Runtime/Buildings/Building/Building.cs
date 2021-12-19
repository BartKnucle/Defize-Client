using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.World
{
    [CreateAssetMenu(fileName = "Building", menuName = "FunkySheep/World/Buildings/Building", order = 1)]
    public class Building : ScriptableObject
    {
        
        public string id;
        public Vector2[] points;
        public float height = 5;
        public Vector2 position;
        public Vector2 center = new Vector2(0, 0);
        public float area;
        public int clock;

        public void OnEnable() {
        }
        
        /// <summary>
        /// Initialize the building
        /// </summary>
        public void Init() {
            
            //Remove double
            this.points = this.points.Distinct().ToArray();
            
            this.position = Position();
            this.area = Area();

            SetFirstPoint();
            SetClockWise();
            //Array.Sort(points, (x, y) => (int)Math.Atan2(x.x, x.y));

            //points = points.OrderBy(x => Math.Atan2(x.x, x.y)).ToList();
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
            int result = FunkySheep.Vectors.IsClockWise(points[points.Length - 1], points[0], center);
            clock = result;
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

        /// <summary>
        /// Sort all the point in anticlockwise around the center
        /// </summary>
        /// <returns></returns>
        public void SortAroundCenter()
        {
            bool sorted = false;
            while(!sorted) {
                sorted = true;
                for (int i = 0; i < points.Length; i++)
                {
                    if (PointsSortAroundCenter(points[i], points[(i + 1) % points.Length]) == -1) {
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
        public int PointsSortAroundCenter(Vector2 first, Vector2 second) {
            return -FunkySheep.Vectors.IsClockWise(first, second, this.center);
        }

    }
}