using System;
using UnityEngine;

namespace FunkySheep.Map
{
    public static class Utils
    {
        /// <summary>
        /// Get the X number of the tile relative to Longitude position
        /// </summary>
        /// <returns></returns>
        public static int LongitudeToX(int zoom, double longitude) {
            return (int)(Math.Floor((longitude + 180.0) / 360.0 * (1 << zoom)));
        }

        /// <summary>
        /// Get the Real X number inside the tile
        /// </summary>
        /// <returns></returns>
        public static float LongitudeToInsideX(int zoom, double longitude) {
            return (float)((longitude + 180.0) / 360.0 * (1 << zoom) - LongitudeToX(zoom, longitude));
        }

        /// <summary>
        /// /// Get the Y number of the tile relative to Latitude position
        /// /// !!! The Y position is the reverse of the cartesian one !!!
        /// </summary>
        /// <returns></returns>
        public static int LatitudeToZ(int zoom, double latitude) {
            return (int)Math.Floor((1 - Math.Log(Math.Tan(Mathf.Deg2Rad * latitude) + 1 / Math.Cos(Mathf.Deg2Rad * latitude)) / Math.PI) / 2 * (1 << zoom));
        }

        /// <summary>
        /// /// Get the Real Y number inside of the tile
        /// </summary>
        /// <returns></returns>
        public static float LatitudeToInsideZ(int zoom, double latitude) {
            return (float)((1 - Math.Log(Math.Tan(Mathf.Deg2Rad * latitude) + 1 / Math.Cos(Mathf.Deg2Rad * latitude)) / Math.PI) / 2 * (1 << zoom)) - LatitudeToZ(zoom, latitude);
        }

        /// <summary>
        /// Get the Longitude of the tile relative to X position
        /// </summary>
        /// <returns></returns>
        public static double tileX2long(int zoom, float xPosition)
        {
            return xPosition / (double)(1 << zoom) * 360.0 - 180;
        }

        /// <summary>
        ///  Get the latitude of the tile relative to Y position
        /// </summary>
        /// <returns></returns>
        public static double tileZ2lat(int zoom, float zposition)
        {
            double n = Math.PI - 2.0 * Math.PI * zposition / (double)(1 << zoom);
            return 180.0 / Math.PI * Math.Atan(0.5 * (Math.Exp(n) - Math.Exp(-n)));
        }

        /// <summary>
        /// Calculate size of the OSM tile depending on the zoomValue level.
        /// </summary>
        /// <returns></returns>
        public static double TileSize(int zoom) {
            return 40075016.686 / Math.Pow(2, zoom + 8) * 256;
        }
    }    
}
