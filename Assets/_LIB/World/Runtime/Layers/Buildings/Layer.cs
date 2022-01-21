using System.Collections;
using UnityEngine;

namespace FunkySheep.World.Buildings
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
      Building building = new Building();
        building.points = new Vector2[way.points.Count];
        building.id = way.id.ToString();

        Layer layer = (Layer)way.tile.layer;
        LayerSO layerSO = (LayerSO)this.layerSO;

        float terrainTop = 0;
        float terrainBottom = 0;
        for (int i = 0; i < way.points.Count; i++)
        {
          Vector2 InsideGridRelative =
            (way.points[i].position -
            new Vector2(
              way.tile.world.worldSO.RealWorldPosition(way.tile).x,
              way.tile.world.worldSO.RealWorldPosition(way.tile).z
            )) / way.tile.world.worldSO.tileRealSize.x;

          float height = layer.GetHeight(InsideGridRelative);
          building.points[i] = way.points[i].position;

          if (terrainTop < height || i == 0)
          {
            terrainTop = height;
          }

          if (terrainBottom > height || i == 0)
          {
            terrainBottom = height;
          }
        }

        building.terrainBottom = terrainBottom;
        building.terrainTop = terrainTop;

        GameObject go = Instantiate(layerSO.buildingPrefab);
        building.Init();
        go.name = building.id;
        go.transform.parent = way.tile.layer.transform;
        go.transform.position = new Vector3(building.position.x, 0, building.position.y);
        go.GetComponent<Manager>().Create(building);
    }
  }
}
