using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using FunkySheep;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif


public class GPS : FunkySheep.Types.SingletonClass<GPS>
{
    public FunkySheep.Types.Double latitude;
    public FunkySheep.Types.Double longitude;

    public FunkySheep.Types.Float horizontalAccuracy;
    public FunkySheep.Types.Double altitude;
    public FunkySheep.Types.Float verticalAccuracy;
    public FunkySheep.Types.Vector3 initialMercatorPosition;
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

        if (initialMercatorPosition.Value == Vector3.zero)
            initialMercatorPosition.Value = MercatorProjection.toVector3(longitude.Value, latitude.Value);
    }

    public Vector3 relativeCartesianPosition(double latitude, double longitude, double altitude) {
        return MercatorProjection.toVector3(longitude, latitude) - MercatorProjection.toVector3(this.longitude.Value, this.latitude.Value);
    }

    /*public Vector3 toCartesian(double latitude, double longitude, double altitude) {
        Vector3 position = new Vector3();
        double R = 6378137;
        Double E = 0.00669437999014;

        latitude = latitude * System.Math.PI / 180;
        longitude = longitude * System.Math.PI / 180;
        
        var N = R / Math.Sqrt(1 - E * Math.Pow(Math.Sin(latitude), 2));

        position.x = (float)(N * Math.Cos(latitude) * Math.Cos(longitude));
        position.y = (float)(N * Math.Cos(latitude) * Math.Sin(longitude));
        position.z = (float)((1 - E) * N * Math.Sin(latitude));

        return position;
    }

    /// <summary>
    /// Calculate new GPS coordinates from GPS references and cartesian coordinates
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="startLatitude"></param>
    /// <param name="startLongitude"></param>
    /// <param name="relativePosition"></param>
    /// <returns></returns>

    public static (double latitude, double longitude) fromVirtual(double startLatitude, double startLongitude, Vector3 relativePosition) {
        double R = 6378137;
        Double E = 0.00669437999014;
        var N = R / Math.Sqrt(1 - E * Math.Pow(Math.Sin(startLatitude), 2));

        double lat = startLatitude + (180 / System.Math.PI) * ( relativePosition.z / 6378137 );
        double lon = startLongitude + (180 / System.Math.PI) *( relativePosition.x / 6378137) / Math.Cos(startLatitude);

        return (lat, lon);
    }*/


    
}