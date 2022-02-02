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
    public List<Node> nodes = new List<Node>();
    public void Merge(Data data)
    {
      this.data.Merge(data);
    }

    public void OnPlayerMove() {
      foreach (Way way in data.ways.ToList())
      {
        GameObject segmentGo = new GameObject();
        segmentGo.transform.parent = this.root.transform;
        segmentGo.name = way.id.ToString();
        Segment segment = segmentGo.AddComponent<Segment>();
        segment.way = way;

        Node previousNode = null;

        for (int i = 0; i < segment.way.nodes.Count; i++)
        {
          GameObject nodeGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
          nodeGO.transform.parent = segment.transform;
          Node node = nodeGO.AddComponent<Node>();
          node.transform.position = new Vector3(
            (float)FunkySheep.GPS.Utils.lonToX(segment.way.nodes[i].longitude) - FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value.x,
            0,
            (float)FunkySheep.GPS.Utils.latToY(segment.way.nodes[i].latitude) - FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value.z
          );

          Vector2Int nodeGridPosition = new Vector2Int(
          Mathf.FloorToInt((node.transform.position.x - initialDisplacement.Value.x) / tileSize.Value.x),
          Mathf.FloorToInt((node.transform.position.z - initialDisplacement.Value.y) / tileSize.Value.y)
          );

          Earth.Tile earthTile = earthTiles.Find(tile => tile.gridPosition == nodeGridPosition);

          if (earthTile != null && earthTile.terrainData != null)
          {
            Vector2 insideCellPosition = new Vector2(
            (node.transform.position.x - initialDisplacement.Value.x - (nodeGridPosition.x * tileSize.Value.x)) / tileSize.Value.x,
            (node.transform.position.z - initialDisplacement.Value.y - (nodeGridPosition.y * tileSize.Value.y)) / tileSize.Value.y
            );

            node.transform.position += new Vector3(0, earthTile.terrainData.GetInterpolatedHeight(insideCellPosition.x, insideCellPosition.y), 0);
            if (previousNode != null)
            {
              node.nodes.Add(previousNode);
              previousNode.nodes.Add(node);

              Debug.DrawLine(node.transform.position, previousNode.transform.position, Color.red, 600);
              //Debug.DrawLine(previousNode.transform.position, node.transform.position, Color.green, 600);
            }
            previousNode = node;
          }
        }
        data.ways.Remove(way);
      }
    }

    public void AddedEarthTile(Earth.Manager manager, Earth.Tile tile)
    {
      earthTiles.Add(tile);
    }
  }  
}
