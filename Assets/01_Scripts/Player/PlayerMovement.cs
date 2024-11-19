using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 700f;

    public float jumpForce = 5f;
    public bool canJump = true;
    private float lastJumpTime = -Mathf.Infinity;
    public float jumpCooldown = 1f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private Camera mainCamera;

    public bool divertido = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

        if (!divertido) 
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        else 
            rb.freezeRotation = false;
    }

    void Update()
    {
        Rotate();
        Move();

        Jump();
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(groundCheck.position, Vector3.down, 0.2f, groundLayer);
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, mainCamera.transform.localEulerAngles.y, transform.eulerAngles.z);
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * vertical + right * horizontal;

        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);
    }



    private void Jump()
    {
        if (IsGrounded())
            canJump = true;

        if (canJump && Time.time >= lastJumpTime + jumpCooldown && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            lastJumpTime = Time.time;
            canJump = false;
        }
    }
}
