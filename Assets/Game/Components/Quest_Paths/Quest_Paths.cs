using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using FunkySheep.Variables;
using FunkySheep.Network;

public class Quest_Paths : MonoBehaviour
{
    public int updateDelay = 1000;
    float _lastUpdate = 0;

    //  public List<Vector3> points;

    public Camera playerCamera;
    public FloatVariable playerPositionX;
    public FloatVariable playerPositionY;
    public FloatVariable playerPositionZ;
    Vector3 _lastPosition;

    public Service service;

    public bool recording = false;
    public ARTrackedImageManager imageManager;
    public GameObject img;
    public GameObject test;

    
    private void Start() {
        imageManager.trackedImagesChanged += _onImgDetection;
        _lastPosition = playerCamera.transform.localPosition;
        playerPositionX.Value = playerCamera.transform.localPosition.x;
        playerPositionY.Value = playerCamera.transform.localPosition.y;
        playerPositionZ.Value = playerCamera.transform.localPosition.z;

        service.FindRecords();
    }
    
    void Update()
    {
        _lastUpdate += Time.deltaTime;
        float distance = Vector3.Distance(playerCamera.transform.localPosition, _lastPosition);
        
        if (_lastUpdate * 1000 > updateDelay && recording && distance >= 0.5 && img) {
            _lastUpdate = 0;

            GameObject point = GameObject.CreatePrimitive(PrimitiveType.Cube);
            point.transform.position = playerCamera.transform.localPosition + playerCamera.transform.forward;
            point.transform.parent = img.transform;
            
            _addPoint(point);

            service.CreateRecords();
            _lastPosition = playerCamera.transform.localPosition;
        }
    }

    /// <summary>
    /// Add the last player postion to the patch
    /// </summary>
    private void _addPoint(GameObject point) {
        if (img) {
            point.transform.localScale *= 0.05f;
            point.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(0, 0, 255);

            playerPositionX.Value = point.transform.localPosition.x;
            playerPositionY.Value = point.transform.localPosition.y;
            playerPositionZ.Value = point.transform.localPosition.z;
        }
    }

    /// <summary>
    /// Fill the path with the last find request
    /// </summary>
    public void fill() {
        SimpleJSON.JSONArray points = service.lastRawMsg["data"]["data"].AsArray;
        for (int i = 0; i < points.Count; i++)
        {
            GameObject point = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Vector3 localPosition = new Vector3(points[i]["positionx"], points[i]["positiony"], points[i]["positionz"]);
            if (img) {
                point.transform.parent = img.transform;
            }
            point.transform.localPosition = localPosition;

            _addPoint(point);
        }

        if (points.Count == 0) {
            recording = true;
        }
    }

    private void _onImgDetection(ARTrackedImagesChangedEventArgs imgEvent) {
        foreach (ARTrackedImage image in imgEvent.added)
        {
            img = image.gameObject;
            fill();

            test = GameObject.CreatePrimitive(PrimitiveType.Cube);
            test.transform.parent = img.transform;
            test.transform.localScale *= 0.05f;
            test.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(0, 1, 0);
        }
    }
}
