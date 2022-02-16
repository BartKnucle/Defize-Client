using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Trees
{
  public class Tree : MonoBehaviour
  {
    private void Start() {
      CreateMultipleTrunks();
    }

    public void CreateTrunk()
    {
      GameObject rootGo = new GameObject();
      rootGo.name = "root";
      rootGo.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
      rootGo.transform.parent = this.transform;
      rootGo.AddComponent<Branch>();
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
