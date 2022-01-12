using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

namespace FunkySheep.OldWorld.Buildings
{
    [AddComponentMenu("FunkySheep/World/Buildings/Tile")]
    public class Tile : MonoBehaviour
    {
        public string id;
        public GameObject buildingPrefab;
        public FunkySheep.Types.Vector3 initialMercatorPosition;
        public string url;
        string filePath;

        public List<Way> ways = new List<Way>();
        public List<Relation> relations = new List<Relation>();
        JSONArray elements;

        public void Init(string mapId, string url)
        {
            this.id = mapId;
            this.url = url;

            filePath = Application.persistentDataPath + "/world/buildings/" + id;
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
            JSONNode data = JSON.Parse(File.ReadAllText(filePath));
            elements = data["elements"].AsArray;
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
                    JSONNode data = JSON.Parse(request.downloadHandler.text);
                    elements = data["elements"].AsArray;
                    File.WriteAllText(filePath, request.downloadHandler.text);
                    callback();
                }
        }

        /// <summary>
        /// Parse the data from the file
        /// </summary>
        public void ParseTileData()
        {
            // Add all the nodes first
            for (int i = 0; i < elements.Count; i++)
            {
                switch ((string)elements[i]["type"])
                {
                    case "way":
                        AddWay(elements[i]);
                        break;
                    case "relation":
                        AddRelation(elements[i]);
                        break;
                    default:
                        break;
                }
            }
        }

        public Way AddWay(JSONNode wayJSON)
        {
            Way way = new Way(wayJSON["id"]);
            // Add the node id to the nodes list
            JSONArray points = wayJSON["geometry"].AsArray;

            for (int j = 0; j < points.Count; j++)
            {
                way.points.Add(new Point(points[j]["lat"], points[j]["lon"], this.initialMercatorPosition.Value));
            }

            JSONObject tags = wayJSON["tags"].AsObject;

            foreach (KeyValuePair<string, JSONNode> tag in (JSONObject)tags)
            {
                way.tags.Add(new Tag(tag.Key, tag.Value));
            }
            Build(way);
            ways.Add(way);
            
            return way;
        }

        public Relation AddRelation(JSONNode relationJSON)
        {
            Relation relation = new Relation(relationJSON["id"]);

            JSONArray members = relationJSON["members"].AsArray;

            for (int j = 0; j < members.Count; j++)
            {
                Way way = ways.Find(way => way.id == members[j]["ref"]);
                if (way == null) {
                  way = new Way(members[j]["ref"]);
                  JSONArray points = members[j]["geometry"].AsArray;
                  for (int k = 0; k < points.Count; k++)
                  {
                      way.points.Add(new Point(points[k]["lat"], points[k]["lon"], this.initialMercatorPosition.Value));
                  }
                }
                Build(way);
                relation.ways.Add(way);
            }

            JSONObject tags = relationJSON["tags"].AsObject;

            foreach (KeyValuePair<string, JSONNode> tag in (JSONObject)tags)
            {
                relation.tags.Add(new Tag(tag.Key, tag.Value));
            }
            relations.Add(relation);
            return relation;
        }

        /// <summary>
        /// Build the 3D Object
        /// </summary>
        /// <param name="way"></param>
        public void Build(Way way)
        {
            Building building = ScriptableObject.CreateInstance<Building>();
            building.points = new Vector2[way.points.Count];
            building.id = way.id.ToString();

            for (int i = 0; i < way.points.Count; i++)
            {
              building.points[i] = way.points[i].position;
            }

            GameObject go = Instantiate(buildingPrefab);
            building.Init();
            go.name = building.id;
            go.transform.parent = this.transform;
            go.transform.position = new Vector3(building.position.x, 0, building.position.y);
            go.GetComponent<BuildingManager>().Create(building);
        }
    }
}