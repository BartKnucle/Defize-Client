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
      foreach (LayerSO layer in worldSO.layersSO)
      {
        GameObject go = new GameObject(layer.name);
        Layer layerComponent = go.AddComponent<Layer>();
        layerComponent.layerSO = layer;
        go.transform.parent = this.transform;
      }
    }
    
    public void UpdatePlayerPosition()
    {
      worldSO.UpdatePosition(this);
    }
  }    
}
