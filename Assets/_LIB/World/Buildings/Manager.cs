using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.World.Building
{
    [AddComponentMenu("FunkySheep/World/Building/Data")]
    public class Manager : MonoBehaviour
    {
        public FunkySheep.Map.Layer layer;
        public FunkySheep.Types.String url;
        public List<string> OSMNodesTypes;
        public Data data;

        private void Start() {
            foreach (FunkySheep.Map.Tile tile in layer.tiles)
            {
                if (!data.cachedTiles.Contains(tile.id))
                {
                    data.cachedTiles.Add(tile.id);
                    double[] boundaries = tile.GpsBoundaries();
                    string url = InterpolatedUrl(boundaries);
                    Download(url);
                }
            }
        }

        /// <summary>
        /// Download the buildings
        /// </summary>
        public void Download(string url)
        {
            Debug.Log(url);
        }

        /// <summary>
        /// Interpolate the url inserting the boundaries and the types of OSM data to download
        /// </summary>
        /// <param boundaries="zoom">The gps boundaries to download in</param>
        /// <returns>The interpolated Url</returns>
        public string InterpolatedUrl(double[] boundaries)
        {
            string [] parameters = new string[5];
            string [] parametersNames = new string[5];

            parameters[0] = boundaries[0].ToString().Replace(',', '.');
            parametersNames[0] = "startLatitude";

            parameters[1] = boundaries[1].ToString().Replace(',', '.');
            parametersNames[1] = "startLongitude";

            parameters[2] = boundaries[2].ToString().Replace(',', '.');
            parametersNames[2] = "endLatitude";

            parameters[3] = boundaries[3].ToString().Replace(',', '.');
            parametersNames[3] = "endLongitude";

            foreach (string type in OSMNodesTypes)
            {
                parameters[4] += type;
            }
            parametersNames[4] = "types";

            return url.Interpolate(parameters, parametersNames);
        }
    }
}
