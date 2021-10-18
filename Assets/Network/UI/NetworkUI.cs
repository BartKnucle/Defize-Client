using UnityEngine;
using FunkySheep.Variables;
using UnityEngine.UIElements;

public class NetworkUI : MonoBehaviour
{
    public UIDocument UI;  
    public StringVariable Status;
    public VectorImage offlineImg;
    public VectorImage onlineImg;

    public void changeUIStatus() {
        this.UI.rootVisualElement.Q<Label>("network-status").text = Status.Value;

        switch (Status.Value)
        {
            case "Disconnected":
                this.UI.rootVisualElement.Q<VisualElement>("network-icon").style.backgroundImage = new StyleBackground(offlineImg);
                break;
            case "Connected":
                this.UI.rootVisualElement.Q<VisualElement>("network-icon").style.backgroundImage = new StyleBackground(onlineImg);
                break;
            default:
                this.UI.rootVisualElement.Q<VisualElement>("network-icon").style.backgroundImage = new StyleBackground(offlineImg);
                break;
        }
    }
}
