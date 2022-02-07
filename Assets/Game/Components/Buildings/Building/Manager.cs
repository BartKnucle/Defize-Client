using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.Procedural.Buildings;

namespace Game.Building
{
    [RequireComponent(typeof(Floor))]
    public class Manager : MonoBehaviour
    {
        FunkySheep.Procedural.Buildings.Building building ;
        Floor floor;
        public Material material;
        // Start is called before the first frame update
        void Start()
        {
            this.floor = GetComponent<Floor>();
            this.floor.material = material;
            this.building = floor.building;
            this.floor.Create();
        }

        private void OnCollisionEnter(Collision other) {
            GetComponent<MeshRenderer>().material.color = Color.green;
            GameObject door = new GameObject();
            door.transform.parent = transform;
        }

        private void OnCollisionExit(Collision other) {
            GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }    
}
