using UnityEngine;
using UnityEngine.Tilemaps;

namespace FunkySheep.Maps
{
  public class Tile
  {
    public Vector3Int tilemapPosition;
    public Vector2Int mapPosition;
    public UnityEngine.Tilemaps.Tile data;
    public Tile(Vector3Int tilemapPosition, Vector2Int mapPosition, UnityEngine.Tilemaps.Tile data)
    {
      this.tilemapPosition = tilemapPosition;
      this.mapPosition = mapPosition;
      this.data = data;
    }
  }


  [CreateAssetMenu(menuName = "FunkySheep/Maps/Map")]
  public class SO : FunkySheep.SO
  {
    public FunkySheep.Types.String urlTemplate;
    public FunkySheep.Types.Int zoom;
    Vector2Int? _initialMapPosition;

    private void OnEnable() {
      _initialMapPosition = null;
    }

    public override void Create(FunkySheep.Manager manager)
    {
      base.Create(manager);
      manager.root.AddComponent<Tilemap>();
      manager.root.AddComponent<TilemapRenderer>();
      manager.root.AddComponent<Grid>();
    }

    public void AddTile(Manager manager, Vector2Int mapPosition)
    {
      if (!_initialMapPosition.HasValue)
      {
        _initialMapPosition = mapPosition;
      }

      // Reverse mercator and grid positions
      Vector2Int gridPosition = (mapPosition -_initialMapPosition.Value) * new Vector2Int(1, -1);
      Vector3Int tileMapPosition = new Vector3Int(gridPosition.x, gridPosition.y, 0);
      string url = InterpolatedUrl(mapPosition);

      DownloadTile(manager, tileMapPosition, mapPosition, url);
    }
    
    public void DownloadTile(Manager manager, Vector3Int tileMapPosition, Vector2Int mapPosition, string url)
    {
      manager.StartCoroutine(FunkySheep.Network.Downloader.Download(url, (fileID, file) => {
        manager.tiles.Enqueue(new Tile(tileMapPosition, mapPosition, SetTile(file)));
      }));
    }

    /// <summary>
    /// Set the tile object
    /// </summary>
    /// <param name="texture">The texture to set on the tile sprite</param>
    public UnityEngine.Tilemaps.Tile SetTile(byte[] textureFile)
    {
      Texture2D texture = new Texture2D(256, 256);
      texture.LoadImage(textureFile);
      UnityEngine.Tilemaps.Tile tileData;
      texture.wrapMode = TextureWrapMode.Clamp;
      texture.filterMode = FilterMode.Point;
      tileData = ScriptableObject.CreateInstance<UnityEngine.Tilemaps.Tile>();
      tileData.sprite = Sprite.Create((Texture2D) texture, new Rect(0.0f, 0.0f, texture.width, texture.height), Vector2.zero, 1);
      return tileData;
    }

    /// <summary>
    /// Interpolate the url inserting the coordinates and zoom values
    /// </summary>
    /// <param name="zoom">The zoom value</param>
    /// <param name="position">The coordinates</param>
    /// <returns>The interpolated Url</returns>
    public string InterpolatedUrl(Vector2Int mapPosition)
    {
      string [] parameters = new string[3];
      string [] parametersNames = new string[3];

      parameters[0] = zoom.Value.ToString();
      parametersNames[0] = "zoom";
      
      parameters[1] =  mapPosition.x.ToString();
      parametersNames[1] = "position.x";
      
      parameters[2] =  mapPosition.y.ToString();
      parametersNames[2] = "position.y";

      return urlTemplate.Interpolate(parameters, parametersNames);
    }
  }
}
