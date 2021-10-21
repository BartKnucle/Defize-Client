using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using FunkySheep.Variables;
public class GameMgmt : GenericSingletonClass<GameMgmt>
{
    public enum State {
        root
    }
    public State state = State.root;
    public UIDocument UI;

    public void Start() {
        if (Debug.isDebugBuild)
        {
            UI.rootVisualElement.Q<VisualElement>("debug-btn").visible = true;
        } else {
            UI.rootVisualElement.Q<VisualElement>("debug-btn").visible = false;
        }
    }
}
