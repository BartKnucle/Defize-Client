using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using FunkySheep.World.OSM;

namespace FunkySheep.World.Roads
{
  [CreateAssetMenu(menuName = "FunkySheep/World/Layers/Roads")]
  public class LayerSO : FunkySheep.World.LayerSO
  {
    public string cacheRelativePath = "/world/buildings/";
    string path;
    public FunkySheep.Types.String url;
    public List<Way> ways = new List<Way>();
    public List<Point> nodes = new List<Point>();
    public FunkySheep.Types.Vector3 initialMercatorPosition;
    public Material material;
    private void OnEnable() {
    ways = new List<Way>();
    nodes = new List<Point>();

      path = Application.persistentDataPath + cacheRelativePath;
      //Create the cache directory
      if (!Directory.Exists(path))
      {
        Directory.CreateDirectory(path);
      }
    }

    public override World.Layer CreateManager(GameObject go, FunkySheep.World.Manager world)
    {
      Layer layerComponent = go.AddComponent<Layer>();
      layerComponent.layerSO = this;
      layerComponent.world = world;
      return layerComponent;
    }

    public override Tile AddTile(FunkySheep.World.Manager world, FunkySheep.World.Layer layer, Vector2Int gridPosition, Vector2Int mapPosition)
    {
      Tile tile = new Tile(world, layer, gridPosition, mapPosition);
      string url = InterpolatedUrl(tile.gpsBoundaries);
      layer.StartCoroutine(Load(url, (data) => {
          ParseTileData(tile, data["elements"].AsArray);
      }));
      return tile;
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
      foreach (string file in Directory.GetFiles(path))
      {
          File.Delete(file);
      }
    }

    /// <summary>
    /// Load a tile either from the disk or from the internet
    /// </summary>
    public IEnumerator Load(string url, Action<JSONNode> Callback)
    {
        string hash = FunkySheep.Crypto.Hash(url);
        if (File.Exists(path + hash))
        {
            LoadFromDisk(path + hash, Callback);
            yield break;
          } else {
            yield return DownLoad(url, path + hash, Callback);
          }
    }

        /// <summary>
        /// Load tile file from the disk
        /// </summary>
    public void LoadFromDisk(string path, Action<JSONNode> Callback)
    {
      JSONNode data = JSON.Parse(File.ReadAllText(path));
      Callback(data);
    }

    /// <summary>
    /// Download the tile
    /// </summary>
    /// <param name="callback">The callback to be run after download complete</param>
    /// <returns></returns>
    public IEnumerator DownLoad(string url, string path, Action<JSONNode> Callback) {
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();
            if(request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                JSONNode data = JSON.Parse(request.downloadHandler.text);
                File.WriteAllText(path, request.downloadHandler.text);
                Callback(data);
            }
    }

    /// <summary>
    /// Parse the data from the file
    /// </summary>
    public void ParseTileData(Tile tile, JSONArray elements)
    {
        // Add all the nodes first
        for (int i = 0; i < elements.Count; i++)
        {
            switch ((string)elements[i]["type"])
            {
                case "way":
                    AddWay(tile, elements[i]);
                    break;
                default:
                    break;
            }
        }
    }

    public Way AddWay(Tile tile, JSONNode wayJSON)
    {
        Way way = new Way(wayJSON["id"], tile);
        // Add the node id to the nodes list
        JSONArray points = wayJSON["geometry"].AsArray;

        bool nextNodeLinked = false;
        for (int j = 0; j < points.Count; j++)
        {
          Point node = nodes.Find(node => (node.latitude == points[j]["lat"] && node.longitude == points[j]["lon"]));
          if (node == null)
          {
            node = new Point(points[j]["lat"], points[j]["lon"], this.initialMercatorPosition.Value);

            if (nextNodeLinked)
            {
              node.liked = true;
              nextNodeLinked = false;
            }

            nodes.Add(node);
          } else {
            node.inter = true;

            if(j == points.Count - 1)
            {
              nodes[nodes.Count - 1].liked = true;
            } else if (j == 0)
            {
              nextNodeLinked = true;
            } else {
              nodes[nodes.Count - 1].liked = true;
              nextNodeLinked = true;
            }
          }
          way.points.Add(node);
        }

        JSONObject tags = wayJSON["tags"].AsObject;

        foreach (KeyValuePair<string, JSONNode> tag in (JSONObject)tags)
        {
          way.tags.Add(new Tag(tag.Key, tag.Value));
        }
        Queue(way);
        
        return way;
    }

    /// <summary>
    /// Queue the 3D Object
    /// </summary>
    /// <param name="way"></param>
    public void Queue(Way way)
    {
        Layer layer = (Layer)way.tile.layer;
        layer.ways.Enqueue(way);
    }
  }
}