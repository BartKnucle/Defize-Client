using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.Variables;
using FunkySheep.Network;

public class Quest_Paths : MonoBehaviour
{
    public int updateDelay = 1000;
    float _lastUpdate = 0;

    public List<Vector3> points;

    public Camera playerCamera;
    public FloatVariable playerPositionX;
    public FloatVariable playerPositionY;
    public FloatVariable playerPositionZ;
    Vector3 _lastPosition;

    public Service service;

    public bool recording = false;

    
    private void Start() {
        _lastPosition = playerCamera.transform.localPosition;
        playerPositionX.Value = playerCamera.transform.localPosition.x;
        playerPositionY.Value = playerCamera.transform.localPosition.y;
        playerPositionZ.Value = playerCamera.transform.localPosition.z;

        service.FindRecords();
    }
    
    void Update()
    {
        _lastUpdate += Time.deltaTime;
        
        if (_lastUpdate * 1000 > updateDelay && recording && _lastPosition != playerCamera.transform.localPosition) {
            
            _lastUpdate = 0;
            _addPoint(playerCamera.transform.localPosition);
            service.CreateRecords();
            _lastPosition = playerCamera.transform.localPosition;
        }
    }

    /// <summary>
    /// Add the last player postion to the patch
    /// </summary>
    private void _addPoint(Vector3 position) {
        if (_lastPosition != position) {
            points.Add(position);
            playerPositionX.Value = playerCamera.transform.localPosition.x;
            playerPositionY.Value = playerCamera.transform.localPosition.y;
            playerPositionZ.Value = playerCamera.transform.localPosition.z;

            GameObject point = GameObject.CreatePrimitive(PrimitiveType.Cube);
            point.transform.localScale *= 0.05f;
            point.transform.parent = this.transform;
            point.transform.position = position;
            point.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(0, 0, 255);
        }
    }

    /// <summary>
    /// Fill the path with the last find request
    /// </summary>
    public void fill() {
        SimpleJSON.JSONArray points = service.lastRawMsg["data"]["data"].AsArray;
        for (int i = 0; i < points.Count; i++)
        {
            _addPoint(new Vector3(points[i]["positionx"], points[i]["positiony"], points[i]["positionz"]));
        }
    }
}
