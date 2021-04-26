using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.Network.Variables;
using FunkySheep.Events;

public class ColorPicker : MonoBehaviour
{
    public NetStringVariable color;

    public void setColor()
    {
        Color color = new Color();
        ColorUtility.TryParseHtmlString(this.color.Value, out color);
        this.GetComponent<MeshRenderer>().material.color = color;
    }
}
