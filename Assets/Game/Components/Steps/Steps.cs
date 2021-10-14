using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using FunkySheep.Network;
using FunkySheep.Variables;

public class Steps : MonoBehaviour
{
    public StringVariable hunt_id_field;
    public StringVariable hunt_id;
    public StringVariable step_name;
    public DoubleVariable latitude;
    public DoubleVariable longitude;
    public DoubleVariable altitude;

    public Service service;
    public UIDocument UI;

    public void Awake()
    {
      Button BtnCreate = UI.rootVisualElement.Q<Button>("Create");
      BtnCreate.RegisterCallback<ClickEvent>(this._create);

      Button BtnShow = UI.rootVisualElement.Q<Button>("Show");
      BtnShow.RegisterCallback<ClickEvent>(this._list);
    }

    private void _create(ClickEvent evt)
    {
      hunt_id.Value = hunt_id_field.Value;
      step_name.Value = UI.rootVisualElement.Q<TextField>("Name").text;
 
      if (UI.rootVisualElement.Q<TextField>("Latitude").text != "0") {
        latitude.Value = float.Parse(UI.rootVisualElement.Q<TextField>("Latitude").text);
      } else {
        latitude.Value = User.Instance.GetComponent<GPS>().latitude.Value;
      }

      if (UI.rootVisualElement.Q<TextField>("Longitude").text != "0") {
        longitude.Value = float.Parse(UI.rootVisualElement.Q<TextField>("Longitude").text);
      } else {
        longitude.Value = User.Instance.GetComponent<GPS>().longitude.Value;
      }

      if (UI.rootVisualElement.Q<TextField>("Altitude").text != "0") {
        altitude.Value = float.Parse(UI.rootVisualElement.Q<TextField>("Altitude").text);
      } else {
        altitude.Value = User.Instance.GetComponent<GPS>().altitude.Value;
      }

      service.CreateRecords();
    }

    private void _list(ClickEvent evt)
    {
      service.FindRecords();
    }

    public void show() {
      SimpleJSON.JSONArray steps = service.lastRawMsg["data"]["data"].AsArray;

      for (int i = 0; i < steps.Count; i++)
        {
          GameObject step = GameObject.CreatePrimitive(PrimitiveType.Capsule);
          step.name = steps[i]["name"].ToString();
          step.transform.parent = this.transform;
          double latitude = steps[i]["latitude"];
          double longitude = steps[i]["longitude"];
          double altitude = steps[i]["altitude"];

          step.transform.localPosition = User.Instance.GetComponent<GPS>().relativeCartesianPosition(latitude, longitude, altitude);
        }
    }
}
