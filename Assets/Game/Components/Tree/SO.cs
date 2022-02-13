using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tree
{
    [CreateAssetMenu(menuName = "FunkySheep/Procedural/Tree/Simple")]
    public class SO : FunkySheep.SO
    {
        public int generations;
        public void Create(Manager manager)
        {
            base.Create(manager);
            Generate(manager);
        }

        public void Generate(Manager manager, Vector3? previousPoint = null)
        {
            if (previousPoint == null)
            {
                previousPoint = Vector3.zero;
            }

            generations = (int)Random.Range(0, 20);

            Vector3 nextPoint = new Vector3(
                Random.Range(0, 5) * Mathf.Exp(1),
                Random.Range(0, 5) * Mathf.Exp(1),
                Random.Range(0, 5) * Mathf.Exp(1)
            );

            Debug.DrawLine(previousPoint.Value, nextPoint, Color.red, 600);
        }
    }   
}