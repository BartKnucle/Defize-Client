using UnityEngine;
using ProceduralModeling;

namespace Game.Trees
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Manager : FunkySheep.Manager
    {
      public float lastUpdate = 0;
      
      private void Start() {
          (so as SO).Create(this);
      }

      private void Update() {
        lastUpdate -= Time.deltaTime;
        if (lastUpdate <= 0)
        {
          lastUpdate = 5;
        }
      }
    }
}
