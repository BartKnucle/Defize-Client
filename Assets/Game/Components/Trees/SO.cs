using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Trees
{
    [CreateAssetMenu(menuName = "FunkySheep/Procedural/Tree/Simple")]
    public class SO : FunkySheep.SO
    {
      public int seed = 0;
      public Material material;
      public void Create(Manager manager)
      {
        GameObject treeGo = new GameObject();
        treeGo.name = "tree";
        treeGo.transform.parent = manager.root.transform;
        treeGo.AddComponent<Tree>();
      }
    }   
}