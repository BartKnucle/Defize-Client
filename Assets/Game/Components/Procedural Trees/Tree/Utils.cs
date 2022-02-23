using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Trees
{
  public static class Utils
  {
    public static Vector3 RandVector3(Vector3 min, Vector3 max)
    {
      Vector3 randVector = new Vector3(
        Random.Range(min.x, max.x),
        Random.Range(min.y, max.y),
        Random.Range(min.z, max.z)
      );

      return randVector;
    }
  }  
}
