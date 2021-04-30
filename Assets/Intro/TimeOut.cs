using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOut : MonoBehaviour
{
    int delay = 2;
    float time = 0;

    void Update()
    {
      time += Time.deltaTime;

      if (time >= delay) {
        gameObject.SetActive(false);
      }
    }
}
