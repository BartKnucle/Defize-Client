using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using FunkySheep.Variables;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif


public class GPS : GenericSingletonClass<GPS>
{
    public DoubleVariable latitude;
    public DoubleVariable longitude;

    public FloatVariable horizontalAccuracy;
    public DoubleVariable altitude;
    public FloatVariable verticalAccuracy;
    public UIDocument UI;
    public VectorImage deactivatedIcon;
    public VectorImage activatedIcon;

    GameObject dialog = null;

  override public void Awake() {
    base.Awake();
  }

    IEnumerator Start()
    {
        #if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            dialog = new GameObject();
            }
        #endif

        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
    }

    private void Update() {
        if (Input.location.isEnabledByUser) {
            this.UI.rootVisualElement.Q<VisualElement>("gps-icon").style.backgroundImage = new StyleBackground(activatedIcon);
            GameMgmt.Instance.UI.rootVisualElement.Q<Label>("latitude").text = Input.location.lastData.latitude.ToString();
            GameMgmt.Instance.UI.rootVisualElement.Q<Label>("longitude").text = Input.location.lastData.longitude.ToString();
            GameMgmt.Instance.UI.rootVisualElement.Q<Label>("altitude").text =  Input.location.lastData.altitude.ToString();

            latitude.Value = Input.location.lastData.latitude;
            longitude.Value = Input.location.lastData.longitude;
            altitude.Value = Input.location.lastData.altitude;
            horizontalAccuracy.Value = Input.location.lastData.horizontalAccuracy;
            verticalAccuracy.Value = Input.location.lastData.verticalAccuracy;
        } else {
            this.UI.rootVisualElement.Q<VisualElement>("gps-icon").style.backgroundImage = new StyleBackground(deactivatedIcon);
        }
    }

    public Vector3 relativeCartesianPosition(double latitude, double longitude, double altitude) {
        var rad = Math.PI / 180;
        var delta = toCartesian(latitude, longitude, altitude) - toCartesian(this.latitude.Value, this.longitude.Value, this.altitude.Value);

        var slat = Math.Sin(latitude * rad);
        var clat = Math.Cos(latitude * rad);
        var slon = Math.Sin(longitude * rad);
        var clon = Math.Cos(longitude * rad);

        var e = -slon * delta.x + clon * delta.y;
        var n = -clon * slat * delta.x -slat * slon * delta.y+ clat*delta.z;
        
        return new Vector3((float)e, (float)(altitude - this.altitude.Value), (float)n);
    }

    public Vector3 toCartesian(double latitude, double longitude, double altitude) {
        Vector3 position = new Vector3();
        double R = 6378137;
        Double E = 0.00669437999014;

        latitude = latitude * System.Math.PI / 180;
        longitude = longitude * System.Math.PI / 180;

        position.x = (float)(R * System.Math.Cos(latitude) * System.Math.Cos(longitude));
        position.y = (float)altitude;
        position.z = (float)(R * System.Math.Cos(latitude) * System.Math.Sin(longitude));
        
        var N = R / Math.Sqrt(1 - E * Math.Pow(Math.Sin(latitude), 2));

        position.x = (float)(N * Math.Cos(latitude) * Math.Cos(longitude));
        position.y = (float)(N * Math.Cos(latitude) * Math.Sin(longitude));
        position.z = (float)((1 - E) * N * Math.Sin(latitude));

        return position;
    }
}