using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
  public class Controller : MonoBehaviour
  {
    public Animator animator;
    private CharacterController _controller;
    private Rigidbody _rb;
    public float speed = 20;
    public float rotateSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
      _controller = GetComponent<CharacterController>();
      //_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
      // Rotate around y - axis
      transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);

      // Move forward / backward
      float curSpeed = speed * Input.GetAxis("Vertical");
      if (_controller.isGrounded == true)
      {
        animator.SetFloat("Speed", curSpeed);
      } else {
        animator.SetFloat("Speed", 0f);
      }
      _controller.SimpleMove(transform.forward * curSpeed);
      Debug.Log(_controller.isGrounded);
    }

    private void FixedUpdate() {
      if (Input.GetKey("space"))
      {
        _controller.Move(Vector3.up / 2 + transform.forward * 0.3f);  
      }
      HandleAnimations();
    }

    private void HandleAnimations()
    {
        if (!_controller.isGrounded)
        {
            animator.SetBool("isGrounded", false);

            //Set the animator velocity equal to 1 * the vertical direction in which the player is moving 
            animator.SetFloat("velocityY", 1 * Mathf.Sign(_controller.velocity.y));
        }

        if (_controller.isGrounded)
        {
            animator.SetBool("isGrounded", true);
            animator.SetFloat("velocityY", 0);
        }
    }
  }
}
