using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;


namespace FunkySheep.Map
{
  [AddComponentMenu ("FunkySheep/OSM/Map")]
  public class Manager : FunkySheep.Manager
  {
    public Queue<Tile> tiles = new Queue<Tile>();
    private void Start() {
      (so as SO).AddTile(this);
    }

    private void Update() {
      if (tiles.Count != 0)
      {
        BuildTile(tiles.Dequeue());
      }
    }

    public void BuildTile(Tile tile)
    {
      root.GetComponent<Tilemap>().SetTile(tile.position, tile.data);
      root.GetComponent<Tilemap>().RefreshTile(tile.position);
    }
  }
}
