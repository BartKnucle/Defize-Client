using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using FunkySheep.OSM;

[RequireComponent(typeof(OSMManager))]
public class MapManager : MonoBehaviour
{
    public UIDocument document;
    OSMManager oSMManager;
    // Start is called before the first frame update
    void Start()
    {
        oSMManager = GetComponent<OSMManager>();
    }

    public void Refresh() {
        document.rootVisualElement.Q<VisualElement>("ve-map").style.backgroundImage = new StyleBackground((Texture2D)oSMManager.tiles[0].texture);
    }
}
