using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Trees
{
  public class TreeV1 : MonoBehaviour
  {
    public float radius = 1;
    public int resolution = 3;
    public int generations = 5;
    public float density = 0.5f;
    private void Start() {
      CreateTrunk();
    }

    public void CreateTrunk()
    {
      GameObject rootGo = new GameObject("root");
      rootGo.transform.parent = this.transform;
      BranchV1 trunk = rootGo.AddComponent<BranchV1>();
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
