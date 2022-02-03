using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Procedural.Earth
{
  public class Tile
  {
    public Vector2 gridPosition;
    public TerrainData terrainData;

    public Tile(Vector2 gridPosition, TerrainData terrainData)
    {
      this.gridPosition = gridPosition;
      this.terrainData = terrainData;
    }
  }

  [AddComponentMenu("FunkySheep/Procedural/Earth")]
  public class Manager : FunkySheep.Manager
  {
    public List<TerrainTile> pendingTiles = new List<TerrainTile>();
    public Queue<TerrainTile> buildTiles = new Queue<TerrainTile>();

    public List<Tile> tiles = new List<Tile>();

    private void Update() {
      if (buildTiles.Count != 0)
      {
        (so as SO).BuildTile(this, buildTiles.Dequeue());
      }
    }

    /// <summary>
    /// Add a normal or height map to the terrain
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="tile"></param>
    public void AddedMapTile(FunkySheep.Maps.Manager manager, FunkySheep.Maps.Tile tile)
    {
      TerrainTile pendingTile = pendingTiles.Find(pendingTile => pendingTile.tilemapPosition == tile.tilemapPosition);
      if (pendingTile == null)
      {
        pendingTile = new TerrainTile(tile.tilemapPosition, tile.mapPosition);
        pendingTiles.Add(pendingTile);
      }

      if (manager.so == (so as SO).heightSO)
      {
        pendingTile.heightTile = tile.data;
      } else if (manager.so == (so as SO).normalSO)
      {
        pendingTile.normalTile = tile.data;
      }

      // If the tile contain all needed data
      if (pendingTile.heightTile != null && pendingTile.normalTile != null)
      {
        buildTiles.Enqueue(pendingTile);
        pendingTiles.Remove(pendingTile);
      }
    }
  }
}
