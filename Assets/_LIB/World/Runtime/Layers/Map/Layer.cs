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
          -worldSO.initialOffset.x,
          -1 + worldSO.initialOffset.z,
          0
      );

      // Set the scale depending on the zoom
      tilemap.transform.localScale = new Vector3(
          worldSO.tileRealSize.x / 256f,
          worldSO.tileRealSize.z / 256f,
      1f);
    }
  }
}
