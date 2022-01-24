using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.World
{
  [AddComponentMenu("FunkySheep/World/Manager")]
  public class Manager : MonoBehaviour
  {
    public WorldSO worldSO;
    public void Create() {
      worldSO.Create(this);
    }
    
    public void UpdatePlayerPosition()
    {
      worldSO.UpdatePosition(this);
    }
  }    
}
