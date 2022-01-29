using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.OSM;

namespace FunkySheep.Procedural.Buildings
{
  [CreateAssetMenu(menuName = "FunkySheep/Procedural/Buildings")]
  public class SO : FunkySheep.SO
  {
    public Material floorMat;
    public int drawDistance = 100;
    public FunkySheep.Types.Vector3 drawPosition;
    public FunkySheep.Procedural.Earth.SO EarthSO;

    public void AddBuilding(Manager manager, Building building)
    {
      GameObject goFloor = new GameObject();
      goFloor.name = building.id;
      goFloor.transform.parent = manager.root.transform;
      Floor floor =  goFloor.AddComponent<Buildings.Floor>();
      floor.material = floorMat;
      floor.Create(building);
    }
  }
}
