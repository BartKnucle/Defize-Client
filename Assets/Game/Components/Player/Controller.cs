using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
  public class Controller : MonoBehaviour
  {
    public Animator animator;
    private CharacterController _controller;
    public float speed = 20;
    public float rotateSpeed = 1;
    private Gyroscope gyro;
    // Start is called before the first frame update
    void Start()
    {
      _controller = GetComponent<CharacterController>();
      _controller.minMoveDistance = 0f;
    }

    // Update is called once per frame
    void Update()
    {
      float curSpeed = 0;

      #if UNITY_ANDROID
        Vector2 dir = Vector2.zero;
        dir.x = Input.acceleration.y;
        dir.y = Input.acceleration.x;
        if (dir.sqrMagnitude > 1)
          dir.Normalize();
        transform.Rotate(0, dir.y * rotateSpeed, 0);
        curSpeed += speed * dir.x;
        foreach(Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
              Jump();
            }
        }
      #endif

       #if UNITY_EDITOR
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
        curSpeed += speed * Input.GetAxis("Vertical");
        if (Input.GetKey("space"))
        {
          Jump();
        }
      #endif

      _controller.SimpleMove(transform.forward * curSpeed);
      animator.SetFloat("Speed", curSpeed);
    }

    private void FixedUpdate() {
      if (_controller.isGrounded)
      {
        animator.SetBool("isGrounded", true);
      }
    }

    public void Jump()
    {
      animator.SetBool("isGrounded", false);
      _controller.Move(Vector3.up * 0.1f + transform.forward * 0.1f);
    }
  }
}
