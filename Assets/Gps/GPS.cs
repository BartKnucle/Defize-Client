using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using FunkySheep.Variables;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif


public class GPS : MonoBehaviour
{
    public DoubleVariable latitude;
    public DoubleVariable longitude;

    public FloatVariable horizontalAccuracy;
    public DoubleVariable altitude;
    public FloatVariable verticalAccuracy;
    public UIDocument DebugUI;
    private Label _latitude;
    private Label _longitude;
    private Label _altitude;

    GameObject dialog = null;

  public void Awake() {
    this._latitude = DebugUI.rootVisualElement.Q<Label>("latitude");
    this._longitude = DebugUI.rootVisualElement.Q<Label>("longitude");
    this._altitude = DebugUI.rootVisualElement.Q<Label>("altitude");
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
            _latitude.text = Input.location.lastData.latitude.ToString();
            _longitude.text = Input.location.lastData.longitude.ToString();
            _altitude.text = Input.location.lastData.altitude.ToString();
            latitude.Value = Input.location.lastData.latitude;
            longitude.Value = Input.location.lastData.longitude;
            altitude.Value = Input.location.lastData.altitude;
        }
    }

    public Vector3 relativeCartesianPosition(double latitude, double longitude, double altitude) {
        return toCartesian(latitude, longitude, altitude) - toCartesian(this.latitude.Value, this.longitude.Value, this.altitude.Value);
    }

    public Vector3 toCartesian(double latitude, double longitude, double altitude) {
        Vector3 position = new Vector3();
        double R = 6378137 + altitude;

        latitude = latitude * System.Math.PI / 180;
        longitude = longitude * System.Math.PI / 180;

        position.x = (float)(R * System.Math.Cos(latitude) * System.Math.Cos(longitude));
        position.y = (float)(R * System.Math.Sin(latitude));
        position.z = (float)(R * System.Math.Cos(latitude) * System.Math.Sin(longitude));
        
        return position;
    }
}