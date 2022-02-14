using UnityEngine;
using ProceduralModeling;

namespace Game.Trees
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Manager : FunkySheep.Manager
    {
        private void Start() {
            (so as SO).Create(this);
        }
    }
}
