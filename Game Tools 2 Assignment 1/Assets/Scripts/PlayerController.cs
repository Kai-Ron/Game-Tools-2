using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public float airVelocity = 8f;
    public float gravity = 10.0f;
    public float maxVelocityChange = 10.0f;
    public float jumpHeight = 2.0f;
    public float maxFallSpeed = 20.0f;
    public float rotateSpeed = 25f; //Speed the player rotate
    private Vector3 moveDir;
    public GameObject cam;
    public GameObject perspective;
    private Rigidbody rb;

    private float distToGround;

    private float pushForce;
    private Vector3 pushDir;

    public Vector3 checkPoint;
    public bool pov = false;

    void Start()
    {
        // get the distance to ground
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false;

        checkPoint = transform.position;
        // Cursor.visible = false;
    }

    void FixedUpdate()
    {
            if (moveDir.x != 0 || moveDir.z != 0)
            {
                Vector3 targetDir = moveDir; //Direction of the character

                targetDir.y = 0;
                if (targetDir == Vector3.zero)
                    targetDir = transform.forward;
                Quaternion tr = Quaternion.LookRotation(targetDir); //Rotation of the character to where it moves
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, Time.deltaTime * rotateSpeed); //Rotate the character little by little
                transform.rotation = targetRotation;
            }

            if (IsGrounded())
            {
                // Calculate how fast we should be moving
                Vector3 targetVelocity = moveDir;
                targetVelocity *= speed;

                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = rb.velocity;
                if (targetVelocity.magnitude < velocity.magnitude) //If I'm slowing down the character
                {
                    targetVelocity = velocity;
                    rb.velocity /= 1.1f;
                }
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;
                
                    if (Mathf.Abs(rb.velocity.magnitude) < speed * 1.0f)
                        rb.AddForce(velocityChange, ForceMode.VelocityChange);
                // if (Mathf.Abs(rb.velocity.magnitude) < speed * 1.0f)
                // {
                //     rb.AddForce(moveDir * 0.15f, ForceMode.VelocityChange);
                    //Debug.Log(rb.velocity.magnitude);
                // }

                // Jump
                if (IsGrounded() && Input.GetButton("Jump"))
                {
                    rb.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
                }
            }
            else
            {
                    Vector3 targetVelocity = new Vector3(moveDir.x * airVelocity, rb.velocity.y, moveDir.z * airVelocity);
                    Vector3 velocity = rb.velocity;
                    Vector3 velocityChange = (targetVelocity - velocity);
                    velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                    velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                    rb.AddForce(velocityChange, ForceMode.VelocityChange);
                    if (velocity.y < -maxFallSpeed)
                        rb.velocity = new Vector3(velocity.x, -maxFallSpeed, velocity.z);
                // if (Mathf.Abs(rb.velocity.magnitude) < speed * 1.0f)
                // {
                //     rb.AddForce(moveDir * 0.15f, ForceMode.VelocityChange);
                // }
            }
        rb.AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0));
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!pov)
            {
                pov = true;
                perspective.SetActive(true);
                cam.SetActive(false);

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                pov = false;
                cam.SetActive(true);
                perspective.SetActive(false);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        if(pov)
        {
            Vector3 v2 = v * perspective.transform.forward; //Vertical axis to which I want to move with respect to the camera
            Vector3 h2 = h * perspective.transform.right; //Horizontal axis to which I want to move with respect to the camera
            moveDir = (v2 + h2).normalized; //Global position to which I want to move in magnitude 1
        }
        else
        {
            moveDir = new Vector3(-h, 0, -v).normalized; //Global position to which I want to move in magnitude 1
        }
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }
}
