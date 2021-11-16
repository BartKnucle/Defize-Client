using UnityEngine;
using UnityEngine.XR.ARFoundation;
using FunkySheep.Network;
using FunkySheep.Events;

[RequireComponent(typeof(Camera))]
public class Player : MonoBehaviour
{
    public Service service;
    public ARSessionOrigin origin;
    public FunkySheep.Types.Vector3 position;
    public FunkySheep.Types.Double calculatedLatitude;
    public FunkySheep.Types.Double calculatedLongitude;
    public GameEvent onPlayerMove;
    Vector3 _lastPosition;

    private void Start() {
        position.Value = transform.localPosition;
        _lastPosition = position.Value;
    }

    void Update()
    {
        position.Value = transform.localPosition;
        var calculatedGPS = GPS.fromVirtual(GPS.Instance.latitude.Value, GPS.Instance.longitude.Value, position.Value);
        calculatedLatitude.Value = calculatedGPS.latitude;
        calculatedLongitude.Value = calculatedGPS.longitude;

        float distance = Vector3.Distance(transform.localPosition, _lastPosition);

        if (distance >= 0.5) {
            _lastPosition = transform.localPosition;
            onPlayerMove.Raise();
            //service.CreateRecords();
        }
    }
}
