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
              Mathf.FloorToInt((nodePosition.z - initialDisplacement.Value.y) / tile.terrainData.size.y)
            );
            nodeGridPositions.Add(nodeGridPosition);

            if (nodeGridPosition != tile.gridPosition)
            {
              addWay = false;
              break;
            } else {
              Vector2 insideCellPosition = new Vector2(
              nodePosition.x - (nodeGridPosition.x * tile.terrainData.size.x),
              nodePosition.z - (nodeGridPosition.y * tile.terrainData.size.y)
              );
              insideCellPositions.Add(insideCellPosition);

              nodePosition.y = tile.terrainData.GetInterpolatedHeight(insideCellPosition.x / tile.terrainData.size.x, insideCellPosition.y / tile.terrainData.size.z);
              nodePositions.Add(nodePosition);
            }
          }

          if (addWay == true)
          {
            (so as SO).AddBuilding(this, way, nodePositions);
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
