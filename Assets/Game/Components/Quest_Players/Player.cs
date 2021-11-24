using UnityEngine;
using UnityEngine.XR.ARFoundation;
using FunkySheep.Network;
using FunkySheep.Events;
public class Player : MonoBehaviour
{
    public Service service;
    public ARSessionOrigin origin;
    public FunkySheep.Types.Vector3 position;
    public FunkySheep.Types.Double calculatedLatitude;
    public FunkySheep.Types.Double calculatedLongitude;
    public GameEvent onPlayerStarted;
    public GameEvent onPlayerMove;
    Vector3 _lastPosition;

    private void Start() {
        CalculatePositions();
        _lastPosition = position.Value;
        onPlayerStarted.Raise();
    }

    void Update()
    {
        CalculatePositions();
        float distance = Vector3.Distance(transform.localPosition, _lastPosition);

        if (distance >= 10) {
            _lastPosition = transform.localPosition;
            onPlayerMove.Raise();
            service.CreateRecords();
        }
    }

    public void CalculatePositions() {
        position.Value = transform.localPosition;
        var calculatedGPS = FunkySheep.GPS.Utils.toGeoCoord(position.Value + FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value);
        calculatedLatitude.Value = calculatedGPS.latitude;
        calculatedLongitude.Value = calculatedGPS.longitude;   
    }
}
