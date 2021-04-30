using UnityEngine;
using FunkySheep.Network.Variables;


namespace FunkySheep.Network {
  [AddComponentMenu("FunkySheep/Network/Components/Transform Send")]
  [RequireComponent(typeof(Transform))]
  public class TransformSend : MonoBehaviour
  {
    public NetFloatVariable xposition;
    public NetFloatVariable yposition;
    public NetFloatVariable zposition;

    void Update() {
      xposition.Value = transform.position.x;
      yposition.Value = transform.position.y;
      zposition.Value = transform.position.z;
    }
  }
}