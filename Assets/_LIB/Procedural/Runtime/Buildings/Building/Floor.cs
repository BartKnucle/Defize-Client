using UnityEngine;
using FunkySheep.OSM;

namespace FunkySheep.Procedural.Buildings.Building
{
  public class Floor : MonoBehaviour
  {
    public void AddNode(Vector3 position)
    {
      GameObject node = GameObject.CreatePrimitive(PrimitiveType.Cube);
      node.transform.position = position;
      node.transform.parent = this.transform;
    }
  }  
}
