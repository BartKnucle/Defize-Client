using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
  public class Controller : MonoBehaviour
  {
      private CharacterController _controller;
      public float speed = 20;
      public float rotateSpeed = 1;
      // Start is called before the first frame update
      void Start()
      {
        _controller = GetComponent<CharacterController>();
      }

      // Update is called once per frame
      void Update()
      {
        // Rotate around y - axis
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);

        // Move forward / backward
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float curSpeed = speed * Input.GetAxis("Vertical");
        _controller.SimpleMove(forward * curSpeed);
      }
  }    
}
