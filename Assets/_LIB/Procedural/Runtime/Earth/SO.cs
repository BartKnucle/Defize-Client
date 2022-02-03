using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FunkySheep.Procedural.Earth
{
  public class TerrainTile
  {
    public Vector3Int tilemapPosition;
    public Vector2Int mapPosition;
    public UnityEngine.Tilemaps.Tile heightTile = null;
    public UnityEngine.Tilemaps.Tile normalTile = null;
    public TerrainTile(Vector3Int tilemapPosition, Vector2Int mapPosition)
    {
      this.tilemapPosition = tilemapPosition;
      this.mapPosition = mapPosition;
    }
  }

  [CreateAssetMenu(menuName = "FunkySheep/Procedural/Earth")]
  public class SO : FunkySheep.SO
  {
    public FunkySheep.Maps.SO heightSO;
    public FunkySheep.Maps.SO normalSO;
    public Vector3 terrainSize;
    public int resolution = 64;
    public Material material;
    public AddedTileEvent addedTileEvent;

    public override void Create(FunkySheep.Manager manager)
    {
      base.Create(manager);
      //Hide the tilemaps
      (Get(manager, heightSO) as FunkySheep.Maps.Manager).root.GetComponent<TilemapRenderer>().enabled = false;
      (Get(manager, normalSO) as FunkySheep.Maps.Manager).root.GetComponent<TilemapRenderer>().enabled = false;
    }

    public void AddTile(Manager manager, Vector2Int mapPosition)
    {
      heightSO.AddTile(Get(manager, heightSO) as FunkySheep.Maps.Manager, mapPosition);
      normalSO.AddTile(Get(manager, normalSO) as FunkySheep.Maps.Manager, mapPosition);
    }

    public Tile BuildTile(Manager manager, TerrainTile tile)
    {
      GameObject go = new GameObject();
      go.name = tile.tilemapPosition.ToString();
      go.transform.parent = manager.root.transform;
      go.transform.localPosition = new Vector3(
        terrainSize.x * tile.tilemapPosition.x,
        0,
        terrainSize.z * tile.tilemapPosition.y
      );

      UnityEngine.Terrain terrain = go.AddComponent<UnityEngine.Terrain>();
      terrain.allowAutoConnect = true;
      terrain.materialTemplate = material;

      UnityEngine.TerrainData terrainData = new TerrainData();
      terrainData.heightmapResolution = resolution;
      terrainData.size = terrainSize;
      
      //terrainData.terrainLayers = terrain.terrainData.terrainLayers;
      terrain.terrainData = terrainData;

      UnityEngine.TerrainCollider terrainCollider = go.AddComponent<UnityEngine.TerrainCollider>();
      terrainCollider.terrainData = terrainData;

      go.AddComponent<TerrainConnector>();

      SetHeights(terrainData, tile);

      Tile earthTile = new Tile(
        new Vector2(
          tile.tilemapPosition.x,
          tile.tilemapPosition.y
        ),
        terrainData
      );

      manager.tiles.Add(earthTile);
      addedTileEvent.Raise(manager, earthTile);
      return earthTile;
    }

    public void SetHeights(UnityEngine.TerrainData terrainData, TerrainTile tile)
    {
      float[,] heights = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];
      for (int x = 0; x < terrainData.heightmapResolution; x++)
      {
          for (int y = 0; y < terrainData.heightmapResolution; y++)
          {
            Color color = tile.heightTile.sprite.texture.GetPixel(
              Convert.ToInt32(tile.heightTile.sprite.texture.height / (terrainData.heightmapResolution - 1) * y),
              Convert.ToInt32(tile.heightTile.sprite.texture.width / (terrainData.heightmapResolution -1) * x)
            );

            // Convert the resulting color value to an elevation in meters.
            float elevation = FunkySheep.Maps.Utils.ColorToElevation(color);

            // Use the tile size in meters at the given zoom level to determine the relative
            // scale of elevation values in the mesh.


            double height = elevation / 9000;
            heights[x, y] = (float)height;
          }
      }
      
      //terrainData.SetHeights(0, 0, heights);
      terrainData.SetHeightsDelayLOD(0, 0, heights);
      terrainData.SyncHeightmap();
    }

    public static float? GetHeight(Vector2 position)
    {
      foreach (Terrain terrain in Terrain.activeTerrains)
      {
        UnityEngine.Bounds bounds = terrain.terrainData.bounds;
        Vector2 terrainMin = new Vector2(
          bounds.min.x + terrain.transform.position.x,
          bounds.min.z + terrain.transform.position.z
        );

        Vector2 terrainMax = new Vector2(
          bounds.max.x + terrain.transform.position.x,
          bounds.max.z + terrain.transform.position.z
        );
        
        if (position.x > terrainMin.x && position.y > terrainMin.y && position.x < terrainMax.x && position.y < terrainMax.y)
        {
          return terrain.terrainData.GetInterpolatedHeight(
            (position.x - terrainMin.x) / (terrainMax.x - terrainMin.x),
            (position.y - terrainMin.y) / (terrainMax.y - terrainMin.y)
          );
        }
      }
      return null;
    }

    public static float? GetHeight(Vector3 position)
    {
      return GetHeight(new Vector2(position.x, position.z));
    }
  }  
}
