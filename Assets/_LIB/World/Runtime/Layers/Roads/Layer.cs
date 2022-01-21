using System.Collections;
using UnityEngine;
using FunkySheep.World.OSM;

namespace FunkySheep.World.Roads
{
  public class Layer : FunkySheep.World.Layer, IDropable
  {
    public UnityEngine.TerrainData terrainData {get; set;}
    public Queue ways = new Queue();
    
    public float GetHeight(Vector2 position)
    {
      return terrainData.GetInterpolatedHeight(position.x, position.y);
    }

    private void Update() {
      if (ways.Count != 0)
      {
        build((Way)ways.Dequeue()); 
      }
    }

    public void build(Way way)
    {

      Layer layer = (Layer)way.tile.layer;
      LayerSO layerSO = (LayerSO)this.layerSO;

      for (int i = 0; i < way.points.Count; i++)
      {
        Vector2 InsideGridRelative =
          (way.points[i].position -
          new Vector2(
            way.tile.world.worldSO.RealWorldPosition(way.tile).x,
            way.tile.world.worldSO.RealWorldPosition(way.tile).z
          )) / way.tile.world.worldSO.tileRealSize.x;

        float height = layer.GetHeight(InsideGridRelative);

        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = way.id.ToString();
        go.transform.parent = way.tile.layer.transform;
        go.transform.position = new Vector3(way.points[i].position.x, height, way.points[i].position.y);
      }
    }
  }
}
