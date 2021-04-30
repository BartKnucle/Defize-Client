using UnityEngine;
using FunkySheep.Network;
using FunkySheep.Network.Variables;


namespace FunkySheep.Network {
  [AddComponentMenu("FunkySheep/Network/Components/Transform Send")]
  [RequireComponent(typeof(Transform))]
  public class TransformSend : MonoBehaviour
  {
    public Request request;
    public NetFloatVariable xposition;
    public NetFloatVariable yposition;
    public NetFloatVariable zposition;
    public int frequency = 1000;
    private float _lastSentDelta = 0;

    void Update() {
      _lastSentDelta += UnityEngine.Time.deltaTime;

      if (_lastSentDelta * 1000 > frequency) {
        xposition.Value = transform.position.x;
        yposition.Value = transform.position.y;
        zposition.Value = transform.position.z;

        request.execute();
        _lastSentDelta = 0;
      }
    }
  }
}