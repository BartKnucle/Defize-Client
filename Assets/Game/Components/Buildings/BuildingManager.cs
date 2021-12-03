using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public Walls walls;
    public Roof roof;
    public Building building;
   
    /// <summary>
    /// Create the building
    /// </summary>
    public void Create(Building building)
    {
        this.building = building;
        this.name = building.Id.ToString();
        walls.Create(this.building);
        roof.Create(this.building);
    }
}
