using UnityEngine;
using FunkySheep.Variables;
using UnityEngine.UIElements;

public class NetworkUI : MonoBehaviour
{
    public UIDocument networkUI;
    
    public StringVariable Status;

    public void changeUIStatus() {
        this.networkUI.rootVisualElement.Q<Label>("status").text = Status.Value;
    }
}
