using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;


namespace FunkySheep.Maps
{
  [AddComponentMenu ("FunkySheep/Maps/Map")]
  public class Manager : FunkySheep.Manager
  {
    public Queue<Tile> tiles = new Queue<Tile>();
    public AddedTileEvent addedTileEvent;
    
    private void Update() {
      if (tiles.Count != 0)
      {
        BuildTile(tiles.Dequeue());
      }
    }

    public void BuildTile(Tile tile)
    {
      root.GetComponent<Tilemap>().SetTile(tile.tilemapPosition, tile.data);
      root.GetComponent<Tilemap>().RefreshTile(tile.tilemapPosition);

      if (addedTileEvent != null)
      {
        addedTileEvent.Raise(this, tile);
      }
    }
  }
}
