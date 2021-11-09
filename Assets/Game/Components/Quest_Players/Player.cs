using UnityEngine;
using FunkySheep.Network;
using FunkySheep.Variables;

public class Player : MonoBehaviour
{
    public Service service;
    public Camera playerCamera;
    public FloatVariable playerVirtualPositionX;
    public FloatVariable playerVirtualPositionY;
    public FloatVariable playerVirtualPositionZ;
    public DoubleVariable virtualLatitude;
    public DoubleVariable virtualLongitude;
    public int updatePositionFreqency = 1000;
    private float _lastPositionUpdate = 0;

    private void Start() {
        playerVirtualPositionX.Value = playerCamera.transform.localPosition.x;
        playerVirtualPositionY.Value = playerCamera.transform.localPosition.y;
        playerVirtualPositionZ.Value = playerCamera.transform.localPosition.z;
    }

    void Update()
    {
        playerVirtualPositionX.Value = playerCamera.transform.localPosition.x;
        playerVirtualPositionY.Value = playerCamera.transform.localPosition.y;
        playerVirtualPositionZ.Value = playerCamera.transform.localPosition.z;

        var calculatedGPS = GPS.fromVirtual(GPS.Instance.latitude.Value, GPS.Instance.longitude.Value, playerCamera.transform.position);
        virtualLatitude.Value = calculatedGPS.latitude;
        virtualLongitude.Value = calculatedGPS.longitude;

        _lastPositionUpdate += Time.deltaTime;

        if (_lastPositionUpdate >= updatePositionFreqency / 1000) {
            _lastPositionUpdate = 0;
            service.CreateRecords();
        }
    }
}
