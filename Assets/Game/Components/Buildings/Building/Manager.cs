using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.Procedural.Buildings;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace Game.Building
{
    [RequireComponent(typeof(Floor))]
    public class Manager : MonoBehaviour
    {
        FunkySheep.Procedural.Buildings.Building building ;
        public bool created = false;
        Floor floor;
        GameObject wallsGo;
        public Material material;
        public FunkySheep.Events.GameEventGO onBuildEnter;
        public FunkySheep.Events.GameEventGO onBuildExit;
        // Start is called before the first frame update
        void Start()
        {
            gameObject.layer = 20;
            this.floor = GetComponent<Floor>();
            this.floor.material = material;
            this.building = floor.building;
            this.floor.Create();
        }

        private void OnCollisionEnter(Collision other) {
            if (!created)
            {
              CreateWalls();
              onBuildEnter.Raise(this.gameObject);
            }
        }

        private void OnCollisionExit(Collision other) {
          onBuildExit.Raise(this.gameObject);
          if (!created)
          {
            DestroyWalls();
          }
        }

        public void CreateWalls()
        {
          wallsGo = new GameObject();
          wallsGo.transform.parent = transform;
          wallsGo.transform.position = transform.position;
          Walls.Manager walls = wallsGo.AddComponent<Walls.Manager>();
          walls.material = this.material;
          walls.Create(building);
        }
        public void DestroyWalls()
        {
            Destroy(wallsGo);
        }
    }    
}
