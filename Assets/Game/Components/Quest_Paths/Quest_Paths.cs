using UnityEngine;
using UnityEngine.XR.ARFoundation;
using FunkySheep.Network;

[RequireComponent(typeof(Player))]
public class Quest_Paths : MonoBehaviour
{
    Player player;
    public Service service;
    public ARTrackedImageManager imageManager;
    public GameObject img;
    public GameObject pointModel;

    
    private void Start() {
        player = GetComponent<Player>();
        imageManager.trackedImagesChanged += _onImgDetection;
        service.FindRecords();
    }
    
    public void CreatePoint()
    {
        AddPoint(player.position.Value);
        service.CreateRecords();
    }

    /// <summary>
    /// Add the last player postion to the patch
    /// </summary>
    public void AddPoint(Vector3 position) {
        //  if (img) {
            GameObject point = GameObject.Instantiate(pointModel);
            //  point.transform.parent = img.transform;
            point.transform.localPosition = position;
        //  }
    }

    /// <summary>
    /// Fill the path with the last find request
    /// </summary>
    public void fill() {
        SimpleJSON.JSONArray points = service.lastRawMsg["data"]["data"].AsArray;
        for (int i = 0; i < points.Count; i++)
        {
            GameObject point = GameObject.Instantiate(pointModel);
            //Vector3 localPosition = FunkySheep.Types.Vector3.FromString((string)points[i]["position"]);
            Vector3 localPosition = GPS.Instance.relativeCartesianPosition(points[i]["virtual_latitude"], points[i]["virtual_longitude"]);
            AddPoint(localPosition);
        }
    }

    private void _onImgDetection(ARTrackedImagesChangedEventArgs imgEvent) {
        foreach (ARTrackedImage image in imgEvent.added)
        {
            img = image.gameObject;
            fill();
        }
    }

}
