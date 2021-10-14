using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using FunkySheep.Variables;

public class GameUiMgmt : GenericSingletonClass<GameUiMgmt>
{
    public UIDocument document;
    public void Start() {
        //  this.document = this.GetComponent<UIDocument>();

        if (Debug.isDebugBuild)
        {

        }
    }
}
