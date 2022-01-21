using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.World.OSM;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace FunkySheep.World.Roads
{
  [RequireComponent(typeof(ProBuilderMesh))]
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

      GameObject roadGo = new GameObject(); //GameObject.CreatePrimitive(PrimitiveType.Sphere);
      roadGo.transform.parent = way.tile.layer.transform;
      roadGo.name = way.id.ToString();
      ProBuilderMesh mesh = roadGo.AddComponent<ProBuilderMesh>();

      Vector3 lastPoint = Vector3.zero;

      List<Vector3> nodes = new List<Vector3>();

      for (int i = 0; i < way.points.Count; i++)
      {
        Vector2 InsideGridRelative =
          (way.points[i].position -
          new Vector2(
            way.tile.world.worldSO.RealWorldPosition(way.tile).x,
            way.tile.world.worldSO.RealWorldPosition(way.tile).z
          )) / way.tile.world.worldSO.tileRealSize.x;

        float height = layer.GetHeight(InsideGridRelative);

        GameObject pointGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        pointGo.name = i.ToString();
        pointGo.transform.parent = roadGo.transform;
        pointGo.transform.position = new Vector3(way.points[i].position.x, height, way.points[i].position.y);

        if (i != 0)
        {
          Debug.DrawLine(lastPoint, pointGo.transform.position, Color.red, 600);
          nodes.Insert(i - 1, pointGo.transform.position + Quaternion.Euler(0, 90, 0) * (pointGo.transform.position - lastPoint).normalized * 3);
          nodes.Insert(i, pointGo.transform.position + Quaternion.Euler(0, -90, 0) * (pointGo.transform.position - lastPoint).normalized * 3);
          lastPoint = pointGo.transform.position;
        } else {
          lastPoint = pointGo.transform.position;
        }
      }

      mesh.CreateShapeFromPolygon(nodes, 0.2f, true);
    }
  }
}
