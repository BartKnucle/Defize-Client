using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Procedural.Buildings
{
  public class Building
  {
    public string id;
    public List<Vector3> points = new List<Vector3>();
    public float lowPoint = 0;
    public float hightPoint = 0;

    public Building(string id)
    {
      this.id = id;
    }
  }  
}