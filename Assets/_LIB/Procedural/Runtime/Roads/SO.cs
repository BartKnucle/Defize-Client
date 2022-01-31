using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.OSM;

namespace FunkySheep.Procedural.Roads
{
  [CreateAssetMenu(menuName = "FunkySheep/Procedural/Roads")]
  public class SO : FunkySheep.SO
  {
    public Material roadMat;
    public FunkySheep.Procedural.Earth.SO EarthSO;

    /*public void AddRoad(Manager manager, Road road)
    {
      GameObject goFloor = new GameObject();
      goFloor.name = road.id;
      goFloor.transform.parent = manager.root.transform;
      Floor floor =  goFloor.AddComponent<Buildings.Floor>();
      floor.material = roadMat;
      floor.Create(building);
    }*/
  }
}
