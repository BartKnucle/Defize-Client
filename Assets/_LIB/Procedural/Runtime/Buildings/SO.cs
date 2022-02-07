using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.OSM;

namespace FunkySheep.Procedural.Buildings
{
  [CreateAssetMenu(menuName = "FunkySheep/Procedural/Buildings")]
  public class SO : FunkySheep.SO
  {
    public int drawDistance = 100;
    public FunkySheep.Types.Vector3 drawPosition;
    public FunkySheep.Procedural.Earth.SO EarthSO;
    public GameObject prefab;

    public void AddBuilding(Manager manager, Building building)
    {
      GameObject go = Instantiate(prefab);
      go.name = building.id;
      go.transform.parent = manager.root.transform;
      Floor floor =  go.GetComponent<Buildings.Floor>();
      floor.building = building;
    }
  }
}
