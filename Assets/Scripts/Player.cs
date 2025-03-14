using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float turnSpeed = 0.1f;
    [SerializeField] private Animator anim;
    private float turnVel;
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3 (horizontal, 0,vertical).normalized;
        
        if (direction.magnitude >= .1f )
        {
            float targetAngle = Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle  = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle,ref turnVel,turnSpeed);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
            anim.SetTrigger("Move");
        }
        else
        {
            anim.SetTrigger("Idle");
        }
    }
}
