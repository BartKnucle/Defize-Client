using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.OSM;
using System.Threading;


namespace FunkySheep.Procedural.Buildings
{
  [AddComponentMenu("FunkySheep/Procedural/Buildings")]
  public class Manager : FunkySheep.Manager
  {
    public List<Earth.Tile> earthTiles = new List<Earth.Tile>();
    public OSM.Data data = new Data();
    public List<Building> buildings = new List<Building>();
    public FunkySheep.Types.Vector2 tileSize;
    public FunkySheep.Types.Vector2 initialDisplacement;
    
    public void Merge(Data data)
    {
      this.data.Merge(data);
      foreach (Way way in data.ways.ToList())
      {
        Building building = new Building(way);
        buildings.Add(building);
        data.ways.Remove(way);
      }
       OnPlayerMove();
    }

    public void OnPlayerMove() {
      List<Building> closeBuildings = buildings.FindAll(building => Vector2.Distance(building.center, new Vector2((so as SO).drawPosition.Value.x, (so as SO).drawPosition.Value.z)) <(so as SO).drawDistance);
      foreach (Building building in closeBuildings.ToList())
      {
        buildings.Remove(building);

        for (int i = 0; i < building.points.Length; i++)
        {
          Vector2Int nodeGridPosition = new Vector2Int(
            Mathf.FloorToInt((building.points[i].x - initialDisplacement.Value.x) / tileSize.Value.x),
            Mathf.FloorToInt((building.points[i].y - initialDisplacement.Value.y) / tileSize.Value.y)
          );

          Earth.Tile earthTile = earthTiles.Find(tile => tile.gridPosition == nodeGridPosition);
          if (earthTiles != null)
          {
            float distance = (Vector2.Distance(
            new Vector2((so as SO).drawPosition.Value.x, (so as SO).drawPosition.Value.z),
            new Vector2(building.points[i].x, building.points[i].y)));

           Vector2 insideCellPosition = new Vector2(
            (building.points[i].x - initialDisplacement.Value.x - (nodeGridPosition.x * tileSize.Value.x)) / tileSize.Value.x,
            (building.points[i].y - initialDisplacement.Value.y - (nodeGridPosition.y * tileSize.Value.y)) / tileSize.Value.y
            );

            building.heights[i] = earthTile.terrainData.GetInterpolatedHeight(insideCellPosition.x, insideCellPosition.y);

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

        (so as SO).AddBuilding(this, building);
      }
    }

    public void AddedEarthTile(Earth.Manager manager, Earth.Tile tile)
    {
      earthTiles.Add(tile);
    }
  }  
}
