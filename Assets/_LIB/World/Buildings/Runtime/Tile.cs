using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

namespace FunkySheep.World
{
    [AddComponentMenu("FunkySheep/World/Buildings/Tile")]
    public class Tile : MonoBehaviour
    {
        public string id;
        public GameObject buildingPrefab;
        public FunkySheep.Types.Vector3 initialMercatorPosition;
        public string url;
        string filePath;

        //public List<Way> ways = new List<Way>();
        public List<Relation> relations = new List<Relation>();
        public List<Node> nodes = new List<Node>();
        public JSONNode data;

        public void Init(string mapId, string url)
        {
            this.id = mapId;
            this.url = url;

            filePath = Application.persistentDataPath + "/funkysheep/world/buildings/" + id;
        }

            /// <summary>
            /// Load a tile either from the disk or from the internet
            /// </summary>
        public IEnumerator Load(Action Callback)
        {
            if (File.Exists(filePath))
            {
                LoadFromDisk();
                Callback();
                yield break;
            } else {
                yield return DownLoad(Callback);
            }
        }

            /// <summary>
            /// Load tile file from the disk
            /// </summary>
        public void LoadFromDisk()
        {
            data = JSON.Parse(File.ReadAllText(filePath));
        }


            /// <summary>
            /// Download the tile
            /// </summary>
            /// <param name="callback">The callback to be run after download complete</param>
            /// <returns></returns>
        public IEnumerator DownLoad(Action callback) {
                UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();
                if(request.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    data = JSON.Parse(request.downloadHandler.text);
                    File.WriteAllText(filePath, request.downloadHandler.text);
                    callback();
                }
        }

        public void ParseTileData()
        {
            JSONArray elements = data["elements"].AsArray;

            // Add all the nodes first
            for (int i = 0; i < elements.Count; i++)
            {
                if ((string)elements[i]["type"] == "node")
                {
                    Node node = new Node((string)elements[i]["id"]);
                    node.latitude = (double)elements[i]["lat"];
                    node.longitude = (double)elements[i]["lon"];
                    //  Set the node position relative to a mercator value
                    node.position.x = (float)FunkySheep.GPS.Utils.lonToX(node.longitude) - this.initialMercatorPosition.Value.x;
                    node.position.y = (float)FunkySheep.GPS.Utils.latToY(node.latitude) - this.initialMercatorPosition.Value.z;
                    nodes.Add(node);
                }
            }

            for (int i = 0; i < elements.Count; i++)
            {
                if ((string)elements[i]["type"] != "node")
                {
                    switch ((string)elements[i]["type"])
                    {
                        case "way":
                            Way way = new Way((string)elements[i]["id"]);
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
                            //ways.Add(way);
                            break;
                        case "relation":
                            Relation relation = new Relation((string)elements[i]["id"]);
                            relations.Add(relation);
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
    }
}