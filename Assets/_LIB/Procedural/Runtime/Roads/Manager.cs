using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.OSM;

namespace FunkySheep.Procedural.Roads
{
  [AddComponentMenu("FunkySheep/Procedural/Roads")]
  public class Manager : FunkySheep.Manager
  {
    public List<Earth.Tile> earthTiles = new List<Earth.Tile>();
    public OSM.Data data = new Data();
    public FunkySheep.Types.Vector2 tileSize;
    public FunkySheep.Types.Vector2 initialDisplacement;
    public List<Road> roads = new List<Road>();
    public void Merge(Data data)
    {
      this.data.Merge(data);
      foreach (Way way in data.ways.ToList())
      {
        CreateRoad(way);
      }
    }

    public void CreateRoad(Way way)
    {
      Vector2 minBound = new Vector2(
          (float)FunkySheep.GPS.Utils.lonToX(way.bounds.minLongitude) - FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value.x - (so as SO).drawDistance,
          (float)FunkySheep.GPS.Utils.latToY(way.bounds.minLatitude) - FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value.z - (so as SO).drawDistance
        );

        Vector2 maxBound = new Vector2(
          (float)FunkySheep.GPS.Utils.lonToX(way.bounds.maxLongitude) - FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value.x + (so as SO).drawDistance,
          (float)FunkySheep.GPS.Utils.latToY(way.bounds.maxLatitude) - FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value.z + (so as SO).drawDistance
        );

        Vector2 playerPosition = new Vector2((so as SO).playerPosition.Value.x, (so as SO).playerPosition.Value.z);

        if (playerPosition.x > minBound.x && playerPosition.y > minBound.y && playerPosition.x < maxBound.x && playerPosition.y < maxBound.y)
        {
          if (root.transform.Find(way.id.ToString()) == null)
          {
            GameObject roadGO = new GameObject();
            roadGO.transform.parent = this.root.transform;
            Road road = roadGO.AddComponent<Road>();
            road.way = way;
            road.Create();
          }
          data.ways.Remove(way);
        }
    }

    public void OnPlayerMove() {
      foreach (Way way in data.ways.ToList())
      {
        CreateRoad(way);
      }
      /*foreach (Way way in data.ways.ToList())
      {
        GameObject segmentGo = new GameObject();
        segmentGo.transform.parent = this.root.transform;
        segmentGo.name = way.id.ToString();
        Segment segment = segmentGo.AddComponent<Segment>();
        segment.way = way;

        Node previousNode = null;

        for (int i = 0; i < segment.way.nodes.Count; i++)
        {
          Vector3 nodePosition = new Vector3(
            (float)FunkySheep.GPS.Utils.lonToX(segment.way.nodes[i].longitude) - FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value.x,
            0,
            (float)FunkySheep.GPS.Utils.latToY(segment.way.nodes[i].latitude) - FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value.z
          );

          Vector2Int nodeGridPosition = new Vector2Int(
          Mathf.FloorToInt((nodePosition.x - initialDisplacement.Value.x) / tileSize.Value.x),
          Mathf.FloorToInt((nodePosition.z - initialDisplacement.Value.y) / tileSize.Value.y)
          );

          Vector3 insideCellPosition = new Vector2(
            (nodePosition.x - initialDisplacement.Value.x - (nodeGridPosition.x * tileSize.Value.x)) / tileSize.Value.x,
            (nodePosition.z - initialDisplacement.Value.y - (nodeGridPosition.y * tileSize.Value.y)) / tileSize.Value.y
            );

          Earth.Tile earthTile = earthTiles.Find(tile => tile.gridPosition == nodeGridPosition);

          if (earthTile != null && earthTile.terrainData != null)
          {
            nodePosition.y = earthTile.terrainData.GetInterpolatedHeight(insideCellPosition.x, insideCellPosition.y);

            GameObject nodeGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
            nodeGO.name = segment.way.nodes[i].id.ToString();
            nodeGO.transform.parent = segment.transform;
            Node node = nodeGO.AddComponent<Node>();
            node.material = (so as SO).roadMat;
            node.transform.position = nodePosition;

            node.terrainData = earthTile.terrainData;

            if (previousNode != null)
            {
              node.nodes.Add(previousNode);
              previousNode.nodes.Add(node);

              Debug.DrawLine(node.transform.position, previousNode.transform.position, Color.red, 600);
              //Debug.DrawLine(previousNode.transform.position, node.transform.position, Color.green, 600);
              //previousNode.Create();
              if (i == segment.way.nodes.Count - 1)
              {
                ///node.Create();
              }
            }
            previousNode = node;
          }
        }
        data.ways.Remove(way);
      }*/
    }

    public void AddedEarthTile(Earth.Manager manager, Earth.Tile tile)
    {
      earthTiles.Add(tile);
    }
  }  
}
