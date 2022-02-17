using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Trees
{
  public class Tree : MonoBehaviour
  {
    public int generations = 3;
    private void Start() {
      CreateTrunk();
    }

    public void CreateTrunk()
    {
      GameObject rootGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
      rootGo.name = "root";
      rootGo.transform.localScale = Vector3.one * 0.1f;
      rootGo.transform.parent = this.transform;
      Branch trunk = rootGo.AddComponent<Branch>();
      trunk.tree = this;
    }

    public void CreateMultipleTrunks()
    {
      int rootCount = Random.Range(1, 10);
      for (int i = 0; i < rootCount; i++)
      {
        CreateTrunk();
      }
    }
  }
}
