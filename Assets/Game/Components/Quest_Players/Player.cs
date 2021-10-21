using UnityEngine;
using FunkySheep.Network;

public class Player : MonoBehaviour
{
    public Service service;
    public int updatePositionFreqency = 1000;
    private float _lastPositionUpdate = 0;

    // Update is called once per frame
    void Update()
    {
        _lastPositionUpdate += Time.deltaTime;

        if (_lastPositionUpdate >= updatePositionFreqency / 1000) {
            _lastPositionUpdate = 0;
            service.CreateRecords();
        }
    }
}
