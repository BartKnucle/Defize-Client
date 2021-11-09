using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.Variables;
using UnityEngine.UIElements;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class Compass : MonoBehaviour
{
    public FloatVariable heading;

    public UIDocument UI;
    
    IEnumerator Start()
    {
        Input.compass.enabled = true;
        #if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
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

    private void Update()
    {
        if (Input.location.isEnabledByUser) {
            GameMgmt.Instance.UI.rootVisualElement.Q<Label>("heading").text = Input.compass.magneticHeading.ToString();
            heading.Value = Input.compass.trueHeading;
        }
    }
}
