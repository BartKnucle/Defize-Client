using UnityEngine;
using FunkySheep.Network;
using FunkySheep.Variables;


namespace FunkySheep.Network {
  [AddComponentMenu("FunkySheep/Network/Components/Transform Send")]
  [RequireComponent(typeof(Transform))]
  public class TransformSend : MonoBehaviour
  {
    public Service service;
    public FloatVariable xposition;
    public FloatVariable yposition;
    public FloatVariable zposition;
    public int frequency = 1000;
    private float _lastSentDelta = 0;

    void Awake() {
      service.CreateRecords();
    }

    void Update() {
      _lastSentDelta += UnityEngine.Time.deltaTime;

      if (_lastSentDelta * 1000 > frequency) {
        xposition.Value = transform.position.x;
        yposition.Value = transform.position.y;
        zposition.Value = transform.position.z;

        service.PatchRecords();
        _lastSentDelta = 0;
      }
    }
  }
}