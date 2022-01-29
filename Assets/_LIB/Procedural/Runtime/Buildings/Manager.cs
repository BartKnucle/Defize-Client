using System.Collections.Generic;
using UnityEngine;
using FunkySheep.OSM;

namespace FunkySheep.Procedural.Buildings
{
  [AddComponentMenu("FunkySheep/Procedural/Buildings")]
  public class Manager : FunkySheep.Manager
  {
    public List<Earth.Tile> earthTiles = new List<Earth.Tile>();
    public OSM.Data data = new Data();

    public FunkySheep.Types.Vector2 initialDisplacement;
    public void Merge(Data data)
    {
      this.data.Merge(data);
    }

    private void Update() {
      foreach (Earth.Tile tile in earthTiles)
      {
        List<Way> addedWays = new List<Way>();
        foreach (Way way in data.ways)
        {
          Building building = new Building(way);
          bool addWay = true;
          //List<Vector3> nodePositions = new List<Vector3>();
          List<Vector2Int> nodeGridPositions = new List<Vector2Int>();
          List<Vector2> insideCellPositions = new List<Vector2>();

          for (int i = 0; i < building.points.Length; i++)
          {
            Vector2Int nodeGridPosition = new Vector2Int(
              Mathf.FloorToInt((building.points[i].x - initialDisplacement.Value.x) / tile.terrainData.size.x),
              Mathf.FloorToInt((building.points[i].y - initialDisplacement.Value.y) / tile.terrainData.size.z)
            );
            nodeGridPositions.Add(nodeGridPosition);
            float distance = (Vector2.Distance(
              new Vector2((so as SO).drawPosition.Value.x, (so as SO).drawPosition.Value.z),
              new Vector2(building.points[i].x, building.points[i].y)));

            if (distance > (so as SO).drawDistance || nodeGridPosition != tile.gridPosition)
            {
              addWay = false;
              break;
            } else {
              Vector2 insideCellPosition = new Vector2(
              (building.points[i].x - initialDisplacement.Value.x - (nodeGridPosition.x * tile.terrainData.size.x)) / tile.terrainData.size.x,
              (building.points[i].y - initialDisplacement.Value.y - (nodeGridPosition.y * tile.terrainData.size.z)) / tile.terrainData.size.z
              );
              insideCellPositions.Add(insideCellPosition);

              building.heights[i] = tile.terrainData.GetInterpolatedHeight(insideCellPosition.x, insideCellPosition.y);

              if (building.heights[i] < building.lowPoint || building.lowPoint == null)
              {
                building.lowPoint = building.heights[i];
              }
              if (building.heights[i] > building.hightPoint || building.hightPoint == null)
              {
                building.hightPoint = building.heights[i];
              }
            }
          }

          if (addWay == true)
          {
            (so as SO).AddBuilding(this, building);
            addedWays.Add(way);
          }
        }

        foreach (Way way in addedWays)
        {
          data.ways.Remove(way);
        }
      }
    }

    public void AddedEarthTile(Earth.Manager manager, Earth.Tile tile)
    {
      earthTiles.Add(tile);
    }
  }  
}
