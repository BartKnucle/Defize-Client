using UnityEngine;
using FunkySheep.Network.Variables;


namespace FunkySheep.Network {
  [AddComponentMenu("FunkySheep/Network/Components/Transform Receive")]
  [RequireComponent(typeof(Transform))]
  public class TransformReceive : MonoBehaviour
  {
    public NetFloatVariable xposition;
    public NetFloatVariable yposition;
    public NetFloatVariable zposition;

    void Update() {
      transform.localPosition = new Vector3(xposition.Value, yposition.Value, zposition.Value);
    }
  }
}