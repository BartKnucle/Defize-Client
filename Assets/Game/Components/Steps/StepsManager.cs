using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.Variables;
using FunkySheep.Network;
using UnityEngine.XR.ARFoundation;

public class StepsManager : MonoBehaviour
{
    public Service service;

    void Start()
    {
        service.FindRecords();
    }

    public void onStepsReceived() {
        switch (service.lastRawMsg["method"].Value)
        {
            case "find":
                SimpleJSON.JSONArray steps = service.lastRawMsg["data"]["data"].AsArray;
                for (int i = 0; i < steps.Count; i++)
                {
                    _addStep(steps[i]);
                }
                break;
            case "patch":
                SimpleJSON.JSONNode step = service.lastRawMsg["data"];
                    _addStep(step);
                break;
            default:
                break;
        }
    }

    private void _addStep(SimpleJSON.JSONNode step) {
        GameObject stepObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        stepObj.transform.localScale *=  0.1f;
        stepObj.name = step["name"].ToString();
        stepObj.transform.parent = this.transform;
        double latitude = step["latitude"];
        double longitude = step["longitude"];
        double altitude = step["altitude"];

        stepObj.GetComponent<MeshRenderer>().sharedMaterial.color = new Color(0, 1, 0, 1);

        stepObj.transform.localPosition = GPS.Instance.GetComponent<GPS>().relativeCartesianPosition(latitude, longitude, altitude);
        //stepObj.AddComponent<ARAnchor>();
    }
}
