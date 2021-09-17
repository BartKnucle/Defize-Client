using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translation : MonoBehaviour
{
    public float speed = 1;
    public Vector3 direction = Vector3.one;
    public float _position = 0;
    public bool _way = true;

    // Update is called once per frame
    void Update()
    {
        if (_position > 1) {
            _way = false;
            _position = 1;
        } else if (_position < -1) {
            _way = true;
            _position = -1;
        }

        if (_way) {
            _position += Time.deltaTime * speed;
        } else {
            _position -= Time.deltaTime * speed;
        }

        transform.localPosition = _position * direction;
    }
}
