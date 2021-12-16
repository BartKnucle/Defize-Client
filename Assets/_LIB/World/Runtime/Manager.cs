using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SimpleJSON;

namespace FunkySheep.World
{
    [AddComponentMenu("FunkySheep/World/Buildings/Manager")]
    public class Manager : MonoBehaviour
    {
        public GameObject buildingPrefab;
        public FunkySheep.Types.Vector3 initialMercatorPosition;
        public FunkySheep.Map.Layer layer;
        public FunkySheep.Types.String url;
        public List<Tile> tiles = new List<Tile>();
        public List<Way> ways = new List<Way>();
        public List<Relation> relations = new List<Relation>();
        public List<Node> nodes = new List<Node>();
        string filePath;

        private void Start() {

            filePath = Application.persistentDataPath + "/funkysheep/world/buildings/";

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
                Tile tile = tiles.Find(tile => tile.id == mapTile.id);
                if (tile == null)
                {
                    double[] boundaries = mapTile.GpsBoundaries();
                    string url = InterpolatedUrl(boundaries);

                    tile = new Tile(mapTile.id, url);

                    tiles.Add(tile);
                    StartCoroutine(tile.Load(() => {
                        ParseTileData(tile);
                    }));
                } else {
                    StartCoroutine(tile.Load(() => {
                        ParseTileData(tile);
                    }));
                }
            }
        }

        public void ParseTileData(Tile tile)
        {
            JSONArray elements = tile.data["elements"].AsArray;

            // Add all the nodes first
            for (int i = 0; i < elements.Count; i++)
            {
                if ((string)elements[i]["type"] == "node")
                {
                    Node node = nodes.Find(node => node.id == (string)elements[i]["id"]);
                    if (node == null)
                    {
                        node = new Node((string)elements[i]["id"]);
                        node.tiles.Add(tile);
                        node.latitude = (double)elements[i]["lat"];
                        node.longitude = (double)elements[i]["lon"];
                        //  Set the node position relative to a mercator value
                        node.position.x = (float)FunkySheep.GPS.Utils.lonToX(node.longitude) - this.initialMercatorPosition.Value.x;
                        node.position.y = (float)FunkySheep.GPS.Utils.latToY(node.latitude) - this.initialMercatorPosition.Value.z;
                        nodes.Add(node);
                    }
                }
            }

            for (int i = 0; i < elements.Count; i++)
            {
                if ((string)elements[i]["type"] != "node")
                {
                    switch ((string)elements[i]["type"])
                    {
                        case "way":
                            Way way = ways.Find(way => way.id == (string)elements[i]["id"]);
                            if (way == null)
                            {
                                way = new Way((string)elements[i]["id"]);
                                way.tiles.Add(tile);

                                // Add the node id to the nodes list
                                JSONArray nodesIds = elements[i]["nodes"].AsArray;

                                for (int j = 0; j < nodesIds.Count; j++)
                                {
                                    way.nodes.Add(nodes.Find(node => node.id == (string)nodesIds[j]));
                                }

                                JSONObject tags = elements[i]["tags"].AsObject;
                                foreach (KeyValuePair<string, JSONNode> tag in (JSONObject)tags)
                                {
                                    way.tags.Add(new Tag(tag.Key, tag.Value));
                                }
                                Build(way);
                                ways.Add(way);
                            }
                            break;
                        case "relation":
                            Relation relation = relations.Find(relation => relation.id == (string)elements[i]["id"]);
                            if (relation == null)
                            {
                                relation = new Relation((string)elements[i]["id"]);
                                relation.tiles.Add(tile);
                                relations.Add(relation);
                            }
                            break;
                        case "node":
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Build the 3D Object
        /// </summary>
        /// <param name="way"></param>
        public void Build(Way way)
        {
            if (way.tags.Find(tag => tag.name == "building") != null)
            {
                FunkySheep.Buildings.Building building = ScriptableObject.CreateInstance<FunkySheep.Buildings.Building>();
                building.points = new Vector2[way.nodes.Count];
                building.id = way.id;

                for (int i = 0; i < way.nodes.Count; i++)
                {
                    building.points[i] = way.nodes[i].position;
                }

                GameObject go = Instantiate(buildingPrefab);
                building.Init();
                go.name = building.id;
                go.transform.parent = this.transform;
                go.transform.position = new Vector3(building.position.x, 0, building.position.y);
                go.GetComponent<FunkySheep.Buildings.BuildingManager>().Create(building);
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
            foreach (Tile tile in tiles)
            {
                if (File.Exists(filePath + tile.id))
                {
                    File.Delete(filePath + tile.id);
                }
            }
            tiles.Clear();
            ways.Clear();
            relations.Clear();
            nodes.Clear();
        }
    }
}
