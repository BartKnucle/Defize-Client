using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    [RequireComponent(typeof(Game.UI.Status))]
    public class Manager : FunkySheep.Types.SingletonClass<Manager>
    {
        public Game.UI.Status status;
        
        public override void Awake() {
            base.Awake();
            status = GetComponent<Game.UI.Status>();
        }

        private void Start() {
            for (int i = 0; i < 20; i++)
            {
                status.Add("Starting Game " + i.ToString());   
            }
        }
    }   
}
