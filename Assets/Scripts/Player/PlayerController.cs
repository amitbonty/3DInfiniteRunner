using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    private int desiredLane = 1;
    [SerializeField]
    private float forwardSpeed;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float laneDistance=4f;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float Gravity = -20;
    [SerializeField]
    private Animator animator;
    private bool IsSliding = false;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted)
        {
            return;
        }
        controller.Move(direction*Time.fixedDeltaTime);
    }
    private void Update() {
        if(!PlayerManager.isGameStarted)
        {
            return;
        }
        if(forwardSpeed < maxSpeed)
        {
            forwardSpeed += 0.1f * Time.deltaTime;
        }
       
        animator.SetBool("IsGameStarted",true);
        direction.z= forwardSpeed;
        animator.SetBool("IsGrounded",controller.isGrounded);
        if(controller.isGrounded)
        {
            direction.y = -1;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump();
            }
        }
        else{
            direction.y += Gravity * Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.DownArrow) && !IsSliding )
        {
            StartCoroutine(Slide());
        }
        
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            desiredLane++;
            if(desiredLane==3)
            {
                desiredLane=2;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            desiredLane--;
            if (desiredLane == -1)
            {
                desiredLane = 0;
            }
        }
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if(desiredLane ==0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if(desiredLane ==2)
        {
            targetPosition += Vector3.right * laneDistance;
        }
        //transform.position = Vector3.Lerp(transform.position,targetPosition,80*Time.deltaTime);
        if(transform.position==targetPosition)
        {
            return;
        }
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized *25 *Time.deltaTime;
        if(moveDir.sqrMagnitude<diff.sqrMagnitude)
        {
            controller.Move(moveDir);
        }
        else
        {
            controller.Move(diff);
        }
        
    }
    private IEnumerator Slide()
    {
        IsSliding =true;
        animator.SetBool("IsSliding", true);
        controller.center = new Vector3(0,-0.5f,0);
        controller.height =1;
        yield return new WaitForSeconds(1.3f);
        controller.center = new Vector3(0,0,0);
        controller.height = 2;
        animator.SetBool("IsSliding",false);
        IsSliding = false;
    }

    private void Jump()
    {
         direction.y = jumpForce;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if(hit.gameObject.CompareTag("Obstacle"))
        {
            PlayerManager.gameOver = true;
        }
    }
}
