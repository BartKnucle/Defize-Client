using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.Variables;
using FunkySheep.Events;

public class ColorPicker : MonoBehaviour
{
    public StringVariable color;

    public void setColor()
    {
        Color color = new Color();
        ColorUtility.TryParseHtmlString((string)this.color.Value, out color);
        this.GetComponent<MeshRenderer>().material.color = color;
    }
}
