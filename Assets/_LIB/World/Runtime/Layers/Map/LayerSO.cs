using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace FunkySheep.World.Map
{
  [CreateAssetMenu(menuName = "FunkySheep/World/Layers/Map")]
  public class LayerSO : FunkySheep.World.LayerSO
  {
    public string cacheRelativePath = "/world/map/";
    string path;
    public FunkySheep.Types.String url;
    private void OnEnable() {
      path = Application.persistentDataPath + cacheRelativePath;
      //Create the cache directory
      if (!Directory.Exists(path))
      {
        Directory.CreateDirectory(path);
      }
    }
    
    public override Layer CreateManager()
    {
      return new Layer();
    }

    public override void AddTile(Layer layer, Tile tile)
    {
    }

    public void ClearCache()
    {
      foreach (string file in Directory.GetFiles(path))
      {
          File.Delete(file);
      }
    }
  }
}