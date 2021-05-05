using UnityEngine;
using FunkySheep.Variables;


namespace FunkySheep.Network {
  [AddComponentMenu("FunkySheep/Network/Components/Transform Receive")]
  [RequireComponent(typeof(Transform))]
  public class TransformReceive : MonoBehaviour
  {
    public FloatVariable xposition;
    public FloatVariable yposition;
    public FloatVariable zposition;

    public void onUpdate() {
      transform.localPosition = new Vector3(xposition.Value, yposition.Value, zposition.Value);
    }
  }
}