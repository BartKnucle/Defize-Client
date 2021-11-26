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
        position.Value = _lastPosition = transform.position;
        onPlayerStarted.Raise();
    }

    void Update()
    {
        CalculatePositions();
        float distance = Vector3.Distance(transform.position, _lastPosition);

        if (distance >= 10) {
            onPlayerMove.Raise();
            service.CreateRecords();
            _lastPosition = transform.position;
        }
    }

    public void CalculatePositions() {
        position.Value = transform.position;
        var calculatedGPS = FunkySheep.GPS.Utils.toGeoCoord(position.Value + FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value);
        calculatedLatitude.Value = calculatedGPS.latitude;
        calculatedLongitude.Value = calculatedGPS.longitude;   
    }
}
