using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.OSM;

namespace FunkySheep.Procedural.Buildings
{
  [CreateAssetMenu(menuName = "FunkySheep/Procedural/Buildings")]
  public class SO : FunkySheep.SO
  {
    public void Build(Manager manager, Data data)
    {
      foreach (Way way in data.ways)
      {
        foreach (Node node in way.nodes)
        {
          GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
          go.transform.parent = manager.root.transform;
        } 
      }

      foreach (Relation relation in data.relations)
      {
        foreach (Way way in relation.ways)
        {
          foreach (Node node in way.nodes)
          {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.parent = manager.root.transform;
          } 
        }
      }
    }
  }
}
