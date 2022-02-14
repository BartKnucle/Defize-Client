using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProceduralModeling;

namespace Game.Trees
{
    [CreateAssetMenu(menuName = "FunkySheep/Procedural/Tree/Simple")]
    public class SO : FunkySheep.SO
    {
        public Material material;
        public int generations;
        public void Create(Manager manager)
        {
            base.Create(manager);
            Generate(manager);
        }

        public void Generate(Manager manager, Vector3? previousPoint = null)
        {
            TreeData data = new TreeData();
            manager.GetComponent<MeshFilter>().mesh = ProceduralTree.Build(
                data,
                6, // generations of a tree
                1.5f, // base height of a tree
                0.15f // base radius of a tree
            );

            manager.GetComponent<MeshRenderer>().material = material;
        }
    }   
}