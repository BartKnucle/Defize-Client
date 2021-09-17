using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    private void Awake() {
        this.enabled = Debug.isDebugBuild;
        Debug.developerConsoleVisible = Debug.isDebugBuild;
    }
}
