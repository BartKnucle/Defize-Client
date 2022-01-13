using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.World
{
  [Serializable]
  public class Tile
  {
    public Vector2Int mapPosition;
    public Vector2Int gridPosition;
    public WorldSO worldSO;
    public LayerSO layerSO;
    public Double[] gpsBoundaries;
    public Tile(WorldSO worldSO, LayerSO layerSO)
    {
      this.worldSO = worldSO;
      this.layerSO = layerSO;
      this.mapPosition = worldSO.mapPosition.Value;
      this.gridPosition = worldSO.gridPosition.Value;
      gpsBoundaries = CaclulateGpsBoundaries();
    }

    /// <summary>
    /// Calculate the GPS boundaries of the tile
    /// </summary>
    /// <returns>A Double[4] containing [StartLatitude, StartLongitude, EndLatitude, EndLongitude]</returns>
    public Double[] CaclulateGpsBoundaries()
    {
        double latitude = Utils.tileZ2lat(worldSO.zoom.Value, mapPosition.y);
        double longitude = Utils.tileX2long(worldSO.zoom.Value, mapPosition.x);
        double nextLatitude = Utils.tileZ2lat(worldSO.zoom.Value, mapPosition.y + 1);
        double nextLongitude = Utils.tileX2long(worldSO.zoom.Value, mapPosition.x + 1);

        Double[] boundaries = new Double[4];
        
        boundaries[0] = nextLatitude;
        boundaries[1] = longitude;
        boundaries[2] = latitude;
        boundaries[3] = nextLongitude;

        return boundaries;
    }
  }    
}
