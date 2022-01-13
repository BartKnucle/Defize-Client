using UnityEngine;
using UnityEngine.Tilemaps;

namespace FunkySheep.World.Map
{
  [RequireComponent(typeof(Tilemap))]
  [RequireComponent(typeof(TilemapRenderer))]
  [RequireComponent(typeof(Grid))]
  public class Layer : FunkySheep.World.Layer
  {
    private void Awake() {
      GetComponent<Grid>().cellSize = new Vector3(256f, 256f, 0f);
      transform.Rotate(new Vector3(90, 0, 0));
    }

    private void Start() {
      Tilemap tilemap = GetComponent<Tilemap>();
      // Set the position inside the tile
      tilemap.tileAnchor = new Vector3(
          -Utils.LongitudeToInsideX(worldSO.zoom.Value, worldSO.longitude.Value),
          -1 + Utils.LatitudeToInsideZ(worldSO.zoom.Value, worldSO.latitude.Value),
          0
      );

      // Set the scale depending on the zoom
      tilemap.transform.localScale = new Vector3(
          (float)Utils.TileSize(worldSO.zoom.Value) / 256f,
          (float)Utils.TileSize(worldSO.zoom.Value) / 256f,
      1f);
    }
  }
}
