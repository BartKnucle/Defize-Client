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

    public FunkySheep.Types.Vector2 initialDisplacement;
    public void Merge(Data data)
    {
      this.data.Merge(data);
    }

    private void Update() {
      foreach (Earth.Tile tile in earthTiles)
      {
        foreach (Way way in data.ways.ToList())
        {
          GameObject roadGO = new GameObject();
          roadGO.transform.parent = root.transform;
          Road road = roadGO.AddComponent<Road>();
          road.name = way.id.ToString();
          road.points = new Vector3[way.nodes.Count - 1];

          for (int i = 0; i < road.points.Length; i++)
          {
            road.points[i].x = (float)FunkySheep.GPS.Utils.lonToX(way.nodes[i].longitude) - FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value.x;
            road.points[i].y = 500;
            road.points[i].z = (float)FunkySheep.GPS.Utils.latToY(way.nodes[i].latitude) - FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value.z;
            if (i != 0)
            {
              Debug.DrawLine(road.points[i], road.points[i - 1], Color.red, 600);
            }
          }

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
