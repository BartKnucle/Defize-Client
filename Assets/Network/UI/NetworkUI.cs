using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using FunkySheep.Events;
using FunkySheep.Variables;

public class NetworkUI : MonoBehaviour
{
    public UIDocument networkUI;
    
    public StringVariable Status;

    public void changeUIStatus() {
        this.networkUI.rootVisualElement.Q<Label>("status").text = Status.Value;
    }
}
