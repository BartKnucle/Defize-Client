using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SimpleJSON;

namespace FunkySheep.World
{
    [AddComponentMenu("FunkySheep/World/Buildings/Manager")]
    public class Manager : MonoBehaviour
    {
        public GameObject tilePrefab;
        public FunkySheep.Map.Layer layer;
        public FunkySheep.Types.String url;
        public FunkySheep.Types.Int zoom;
        string filePath;

        private void Start() {

            filePath = Application.persistentDataPath + "/world/buildings/";

            //Create the cache directory
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            Load();
        }

        public void Load()
        {
            foreach (FunkySheep.Map.Tile mapTile in layer.tiles)
            {
                GameObject go = Instantiate(tilePrefab);
                go.transform.parent = this.transform;
                go.name = mapTile.id;
                Tile tile = go.GetComponent<Tile>();
                double[] boundaries = mapTile.GpsBoundaries();
                string url = InterpolatedUrl(boundaries);
                
                tile.Init(mapTile.id, url);

                StartCoroutine(tile.Load(() => {
                    tile.ParseTileData();
                }));
            }
        }

        

        /// <summary>
        /// Interpolate the url inserting the boundaries and the types of OSM data to download
        /// </summary>
        /// <param boundaries="boundaries">The gps boundaries to download in</param>
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

            return url.Interpolate(parameters, parametersNames);
        }

        /// <summary>
        /// Clear the buildings cache
        /// </summary>
        public void ClearCache()
        {
            foreach (string file in Directory.GetFiles(filePath))
            {
                File.Delete(file);
            }
        }
    }
}
