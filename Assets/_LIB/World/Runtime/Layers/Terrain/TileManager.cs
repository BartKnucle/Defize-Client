using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.World.Terrain
{
  [RequireComponent(typeof(UnityEngine.Terrain))]
  [RequireComponent(typeof(UnityEngine.TerrainCollider))]
  public class TileManager : MonoBehaviour
  {
    public LayerSO layerSO;
    public Tile tile;

    public UnityEngine.Terrain terrain;
    public UnityEngine.TerrainData terrainData;

    public bool topConnected = false;
    public bool leftConnected = false;

    private void Awake() {
      terrain = GetComponent<UnityEngine.Terrain>();
      terrainData = new TerrainData();
      terrain.allowAutoConnect = true;
      terrain.terrainData = terrainData;
      GetComponent<UnityEngine.TerrainCollider>().terrainData = terrainData;
    }
    public void Init() {
      terrainData.heightmapResolution = layerSO.resolution;
      terrain.materialTemplate = layerSO.material;
      // Set the scale depending on the zoom
      this.transform.localScale = tile.world.worldSO.tileRealSize;

      this.transform.localPosition = tile.world.worldSO.RealWorldPosition(tile) - tile.world.worldSO.initialDisplacement;
      

      terrainData.size = new Vector3(
        tile.world.worldSO.tileRealSize.x,
        9000,
        tile.world.worldSO.tileRealSize.z
      );
      applyHeight();
    }

    private void Update() {
      if (terrain.topNeighbor != null && !topConnected)
      {
        ConnectTop(terrain.topNeighbor);
        
      }

      if (terrain.leftNeighbor != null && !leftConnected)
      {
        ConnectLeft(terrain.leftNeighbor);
      }

      if (tile.gridPosition == tile.world.worldSO.gridPosition.Value)
      {
        LayerSO layerSO = (LayerSO)tile.layer.layerSO;
        layerSO.currentHeight.Value = terrainData.GetInterpolatedHeight(-tile.world.worldSO.currentOffset.x, -tile.world.worldSO.currentOffset.y);
      }
    }
    
    public void applyHeight()
    {
      float[,] heights = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];
      for (int x = 0; x < terrainData.heightmapResolution; x++)
      {
          for (int y = 0; y < terrainData.heightmapResolution; y++)
          {
            Color color = tile.heightTexture.GetPixel(
              Convert.ToInt32(tile.heightTexture.height / (terrainData.heightmapResolution - 1) * y),
              Convert.ToInt32(tile.heightTexture.width / (terrainData.heightmapResolution -1) * x)
            );

            // Convert the resulting color value to an elevation in meters.
            float elevation = Layer.ColorToElevation(color);

            // Use the tile size in meters at the given zoom level to determine the relative
            // scale of elevation values in the mesh.
            double height = elevation / 9000;
            heights[x, y] = (float)height;
          }
      }
      
      terrainData.SetHeights(0, 0, heights);
    }

    void ConnectTop(UnityEngine.Terrain top)
    {
      float[,] heights = terrainData.GetHeights(0, terrainData.heightmapResolution - 1, terrainData.heightmapResolution, 1);
      float[,] heightsTop = top.terrainData.GetHeights(0, 0, terrainData.heightmapResolution, 1);
      float[,] heightsNew = heights;

      for (int y = 0; y < terrainData.heightmapResolution; y++)
      {
        heightsNew[0, y] = (heights[0, y] + heightsTop[0, y]) / 2;
      }

      terrainData.SetHeights(0, top.terrainData.heightmapResolution - 1, heightsNew);
      top.terrainData.SetHeights(0, 0, heightsNew);

      topConnected = true;
    }

    void ConnectLeft(UnityEngine.Terrain top)
    {
      float[,] heights = terrainData.GetHeights(0, 0, 1, terrainData.heightmapResolution);
      float[,] heightsLeft = top.terrainData.GetHeights(terrainData.heightmapResolution - 1, 0, 1, terrainData.heightmapResolution);
      float[,] heightsNew = heights;

      for (int x = 0; x < terrainData.heightmapResolution; x++)
      {
        heightsNew[x, 0] = (heights[x, 0] + heightsLeft[x, 0]) / 2;
      }

      terrainData.SetHeights(0, 0, heightsNew);
      top.terrainData.SetHeights(top.terrainData.heightmapResolution - 1, 0, heightsNew);

      leftConnected = true;
    }
  }
}