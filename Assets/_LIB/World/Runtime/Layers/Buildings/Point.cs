using System;
using UnityEngine;
namespace FunkySheep.World.Buildings
{
    [Serializable]
    public class Point
    {
        public double latitude;
        public double longitude;

        public Vector2 position;

       public Point(double latitude, double longitude, Vector3 initialMercatorPosition)
       {
           this.latitude = latitude;
           this.longitude = longitude;

           position.x = (float)FunkySheep.GPS.Utils.lonToX(longitude) - initialMercatorPosition.x;
           position.y = (float)FunkySheep.GPS.Utils.latToY(latitude) - initialMercatorPosition.z;
       }
    }
}