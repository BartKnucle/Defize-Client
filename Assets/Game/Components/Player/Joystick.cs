using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class Joystick : FixedJoystick
    {
        public Animator animator;
        public CharacterController controller;
        public float speed = 20;
    public float rotateSpeed = 2;
        private void Update() {
            float curSpeed = 0;
            controller.transform.Rotate(0, Horizontal * rotateSpeed, 0);
            curSpeed += speed * Vertical;
            if (Input.touchCount == 2)
            {
                Jump();
            }

            controller.SimpleMove(controller.transform.forward * curSpeed);
            animator.SetFloat("Speed", curSpeed);
        }

        public void Jump()
        {
            animator.SetBool("isGrounded", false);
            controller.Move(Vector3.up * 0.1f + controller.transform.forward * 0.1f);
        }
    }    
}
