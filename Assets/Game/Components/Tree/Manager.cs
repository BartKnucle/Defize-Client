using UnityEngine;

namespace Game.Tree
{
    public class Manager : FunkySheep.Manager
    {
        private void Start() {
            (so as SO).Create(this);
        }
    }
}
