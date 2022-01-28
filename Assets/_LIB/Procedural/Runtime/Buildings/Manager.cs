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
          bool addWay = true;
          float? lowPoint = null;
          float? hightPoint = null;
          List<Vector3> nodePositions = new List<Vector3>();
          List<Vector2Int> nodeGridPositions = new List<Vector2Int>();
          List<Vector2> insideCellPositions = new List<Vector2>();

          foreach (Node node in way.nodes)
          {
            Vector3 nodePosition = new Vector3(
              (float)FunkySheep.GPS.Utils.lonToX(node.longitude) - FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value.x,
              0,
              (float)FunkySheep.GPS.Utils.latToY(node.latitude) - FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value.z
            );

            Vector2Int nodeGridPosition = new Vector2Int(
              Mathf.FloorToInt((nodePosition.x - initialDisplacement.Value.x) / tile.terrainData.size.x),
              Mathf.FloorToInt((nodePosition.z - initialDisplacement.Value.y) / tile.terrainData.size.z)
            );
            nodeGridPositions.Add(nodeGridPosition);
            float distance = (Vector2.Distance(
              new Vector2((so as SO).drawPosition.Value.x, (so as SO).drawPosition.Value.z),
              new Vector2(nodePosition.x, nodePosition.z)));

            if (distance > (so as SO).drawDistance || nodeGridPosition != tile.gridPosition)
            {
              addWay = false;
              break;
            } else {
              Vector2 insideCellPosition = new Vector2(
              (nodePosition.x - initialDisplacement.Value.x - (nodeGridPosition.x * tile.terrainData.size.x)) / tile.terrainData.size.x,
              (nodePosition.z - initialDisplacement.Value.y - (nodeGridPosition.y * tile.terrainData.size.z)) / tile.terrainData.size.z
              );
              insideCellPositions.Add(insideCellPosition);

              nodePosition.y = tile.terrainData.GetInterpolatedHeight(insideCellPosition.x, insideCellPosition.y);
              nodePositions.Add(nodePosition);

              if (nodePosition.y < lowPoint || lowPoint == null)
              {
                lowPoint = nodePosition.y;
              }
              if (nodePosition.y > hightPoint || hightPoint == null)
              {
                hightPoint = nodePosition.y;
              }
            }
          }

          if (addWay == true)
          {
            Buildings.Building building = new Building(way.id.ToString());
            building.points = nodePositions;
            building.lowPoint = lowPoint.Value;
            building.hightPoint = hightPoint.Value;

            (so as SO).AddBuilding(this, way, building);
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
