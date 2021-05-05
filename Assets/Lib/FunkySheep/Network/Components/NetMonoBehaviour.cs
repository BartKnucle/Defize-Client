using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using FunkySheep.Network;


namespace FunkySheep.Network {
  [AddComponentMenu("FunkySheep/Network/NetMonobehaviour")]
  public class NetMonoBehaviour : MonoBehaviour
  {
    public Service service;
    void Start () {
    }
  }
}