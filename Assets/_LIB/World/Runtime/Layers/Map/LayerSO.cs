using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;

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

    public override World.Layer CreateManager(GameObject go, Manager world)
    {
      Layer layerComponent = go.AddComponent<Layer>();
      layerComponent.layerSO = this;
      layerComponent.world = world;
      return layerComponent;
    }

    public override Tile AddTile(Manager world, FunkySheep.World.Layer layer, Vector2Int gridPosition, Vector2Int mapPosition)
    {
      Tile tile = new Tile(world, layer, gridPosition, mapPosition);
      string url = InterpolatedUrl(tile);
      Tilemap tilemap = layer.GetComponent<Tilemap>();

      Vector3Int tileMapPosition = new Vector3Int(tile.gridPosition.x, tile.gridPosition.y, 0);

      layer.StartCoroutine(DownLoad(url, (tileDate) => {
        tilemap.SetTile(tileMapPosition, tileDate);
        tilemap.RefreshTile(tileMapPosition);
      }));
      return tile;
    }

    /// <summary>
    /// Interpolate the url inserting the coordinates and zoom values
    /// </summary>
    /// <param name="zoom">The zoom value</param>
    /// <param name="position">The coordinates</param>
    /// <returns>The interpolated Url</returns>
    public string InterpolatedUrl(Tile tile)
    {
        string [] parameters = new string[3];
        string [] parametersNames = new string[3];

        parameters[0] = tile.world.worldSO.zoom.Value.ToString();
        parametersNames[0] = "zoom";
        
        parameters[1] = tile.mapPosition.x.ToString();
        parametersNames[1] = "position.x";
        
        parameters[2] = tile.mapPosition.y.ToString();
        parametersNames[2] = "position.y";

        return url.Interpolate(parameters, parametersNames);
    }

    /// <summary>
    /// Download the tile
    /// </summary>
    public IEnumerator DownLoad(string url, Action<UnityEngine.Tilemaps.Tile> callback) {
        string hash = FunkySheep.Crypto.Hash(url);
        Texture2D texture;
        texture = new Texture2D(256, 256);
        
        if (File.Exists(path + hash + url.Substring(url.Length - 4)))
        {
            byte[] fileData;
            fileData = File.ReadAllBytes(path + hash + url.Substring(url.Length - 4));
            texture.LoadImage(fileData);
        }

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if(request.result != UnityWebRequest.Result.Success) 
        {
            Debug.Log(request.error);
        }                
        else
        {
            texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            SetTile(texture);
            File.WriteAllBytes(path + hash + url.Substring(url.Length - 4), request.downloadHandler.data);
        }

        UnityEngine.Tilemaps.Tile tileData = SetTile(texture);
        callback(tileData);
        yield break;
    }

    /// <summary>
    /// Set the tile object
    /// </summary>
    /// <param name="texture">The texture to set on the tile sprite</param>
    public UnityEngine.Tilemaps.Tile SetTile(Texture2D texture)
    {
      UnityEngine.Tilemaps.Tile tileData;
      texture.wrapMode = TextureWrapMode.Clamp;
      texture.filterMode = FilterMode.Point;
      tileData = ScriptableObject.CreateInstance<UnityEngine.Tilemaps.Tile>();
      tileData.sprite = Sprite.Create((Texture2D) texture, new Rect(0.0f, 0.0f, texture.width, texture.height), Vector2.zero, 1);
      return tileData;
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