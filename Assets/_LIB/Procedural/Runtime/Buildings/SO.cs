using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.OSM;

namespace FunkySheep.Procedural.Buildings
{
  [CreateAssetMenu(menuName = "FunkySheep/Procedural/Buildings")]
  public class SO : FunkySheep.SO
  {
    public FunkySheep.Procedural.Earth.SO EarthSO;

    public void AddBuilding(Manager manager, Way way, List<Vector3> nodePositions)
    {
      GameObject goFloor = new GameObject();
      goFloor.name = way.id.ToString();
      goFloor.transform.parent = manager.root.transform;
      Building.Floor floor =  goFloor.AddComponent<Buildings.Building.Floor>();

      foreach (Vector3 nodePosition in nodePositions)
      {
        floor.AddNode(nodePosition);
      }
    }
  }
}
