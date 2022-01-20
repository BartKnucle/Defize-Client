using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FunkySheep.World.Terrain
{
  [CreateAssetMenu(menuName = "FunkySheep/World/Layers/Terrain")]
  public class LayerSO : FunkySheep.World.LayerSO
  {
    public FunkySheep.World.LayerSO heightLayerSO;
    public FunkySheep.World.LayerSO normalLayerSO;
    public FunkySheep.Types.Float currentHeight;
    public Material material;
    public bool useNormalMap = true;
    public int resolution = 32;
    List<Tile> pendingTiles = new List<Tile>();
    List<Vector2Int> pendingTilesElevations = new List<Vector2Int>();

    public List<World.LayerSO> dropables;

    private void OnEnable() {
      pendingTiles = new List<Tile>();
      pendingTilesElevations = new List<Vector2Int>();
    }

    public override World.Layer CreateManager(GameObject go, Manager world)
    {
      Layer layerComponent = go.AddComponent<Layer>();
      go.transform.position = world.worldSO.initialDisplacement;
      layerComponent.layerSO = this;
      layerComponent.world = world;

      Tilemap.tilemapTileChanged += onTileChanged;

      layerComponent.heightLayer = world.worldSO.AddLayer(heightLayerSO, world).GetComponent<Tilemap>();
      layerComponent.normalLayer = world.worldSO.AddLayer(normalLayerSO, world).GetComponent<Tilemap>();
      return layerComponent;
    }

    // Not used since it is the tile map event that trigger the creation
    public override FunkySheep.World.Tile AddTile(Manager world, FunkySheep.World.Layer layer, Vector2Int gridPosition, Vector2Int mapPosition)
    {
      Tile tile = new Tile(world, layer, gridPosition, mapPosition);
      pendingTiles.Add(tile);
      TryMatch();
      return tile;
    }

    void onTileChanged(Tilemap tilemap, Tilemap.SyncTile[] tiles)
    {
      if (
        tilemap.gameObject.GetComponent<FunkySheep.World.Map.Layer>().layerSO == heightLayerSO ||
        tilemap.gameObject.GetComponent<FunkySheep.World.Map.Layer>().layerSO == normalLayerSO
      )
      {
        AddElevationTiles(tiles);
      }
    }

    void AddElevationTiles(Tilemap.SyncTile[] tiles)
    {
      for (int i = 0; i < tiles.Length; i++)
      {
        pendingTilesElevations.Add(new Vector2Int(tiles[i].position.x, tiles[i].position.y));
      }
      TryMatch();
    }

    void TryMatch()
    {
      foreach (Tile tile in pendingTiles.ToList())
      {
        if (pendingTilesElevations.FindAll(tileElev => tileElev == tile.gridPosition).Count == 2)
        {
          CreateTile(tile);
          pendingTilesElevations.RemoveAll(tileElev => tileElev == tile.gridPosition);
          pendingTiles.Remove(tile);
        }
      }
    }

    void CreateTile(Tile tile)
    {
      GameObject go = new GameObject();
      go.transform.parent = tile.layer.transform;
      TileManager tileManager = go.AddComponent<TileManager>();
      tileManager.tile = tile;
      tileManager.layerSO = this;
     
      tile.heightTexture = tile.layer.GetComponent<Layer>().heightLayer.GetSprite(new Vector3Int(tile.gridPosition.x, tile.gridPosition.y, 0)).texture;
      tile.normalTexture = tile.layer.GetComponent<Layer>().normalLayer.GetSprite(new Vector3Int(tile.gridPosition.x, tile.gridPosition.y, 0)).texture;

      go.name = tile.gridPosition.ToString();

      tileManager.Init();

      foreach (World.LayerSO layerSO in dropables)
      {
        GameObject layerGO = new GameObject(layerSO.name);
        layerGO.transform.localPosition = tile.world.worldSO.RealWorldPosition(tile);
        layerGO.transform.parent = go.transform.parent;
        World.Layer layer = layerSO.CreateManager(layerGO, tile.world);
        IDropable Ilayer = (IDropable)layer;
        Ilayer.terrainData = tileManager.terrainData;
        layerSO.activated = false;
        layerSO.AddTile(tile.world, layerGO.GetComponent<World.Layer>(), tile.gridPosition, tile.mapPosition);
      }
    }
  }
}