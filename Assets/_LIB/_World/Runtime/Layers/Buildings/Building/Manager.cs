using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FunkySheep.World.Buildings
{
    [AddComponentMenu("FunkySheep/World/Buildings/Building/Manager")]
    public class Manager : MonoBehaviour
    {
        public Walls walls;
        public Roof roof;
        public Floor floor;
        public Building building;
    
        /// <summary>
        /// Create the building
        /// </summary>
        public void Create(Building building)
        {
            this.building = building;
            this.name = building.id;
            walls.Create(this.building);
            roof.Create(this.building);
            floor.Create(this.building);
        }
    }    
}
