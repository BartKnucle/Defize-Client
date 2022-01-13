using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.World
{
  [AddComponentMenu("FunkySheep/World/Manager")]
  public class Manager : MonoBehaviour
  {
    public WorldSO worldSO;
    private void Awake() {
      worldSO.Create(this);
    }
    
    public void UpdatePlayerPosition()
    {
      worldSO.UpdatePosition(this);
    }
  }    
}
