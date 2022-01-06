using UnityEngine.UIElements;

public class GPS : FunkySheep.GPS.Manager
{
    public UIDocument UI;
    public VectorImage deactivatedIcon;
    public VectorImage activatedIcon;

    public void Activate()
    {
        this.UI.rootVisualElement.Q<VisualElement>("gps-icon").style.backgroundImage = new StyleBackground(activatedIcon);
    }
    public void Deactivate()
    {
        this.UI.rootVisualElement.Q<VisualElement>("gps-icon").style.backgroundImage = new StyleBackground(deactivatedIcon);
    }
}