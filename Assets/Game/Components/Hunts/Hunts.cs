using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using FunkySheep.Variables;
using FunkySheep.Network;

public class Hunts : MonoBehaviour
{
    public Service service;
    public StringVariable hunt_name;
    public UIDocument UI;
    public void Awake()
    {
      Button Btn = UI.rootVisualElement.Q<Button>("Create");
      Btn.RegisterCallback<ClickEvent>(this._create);
    }

    private void _create(ClickEvent evt)
    {
      hunt_name.Value = UI.rootVisualElement.Q<TextField>("Name").text;
      service.CreateRecords();
    }


}
