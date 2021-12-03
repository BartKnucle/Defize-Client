using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsManager : MonoBehaviour
{
    public GameObject buildingPrefab;
    public List<Building> buildings;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Building building in buildings)
        {
            GameObject currentBuilding = Instantiate(buildingPrefab);
            currentBuilding.transform.parent = this.transform;
            currentBuilding.GetComponent<BuildingManager>().Create(building);
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
