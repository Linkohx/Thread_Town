using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float jumpForce = 10f;
    public float mouseSensitivity = 50f;

    [Header("Ground Detection")]
    public LayerMask groundLayer = 1;
    public float groundCheckDistance = 0.1f;

    private Rigidbody rb;
    private CinemachineFreeLook freeLookCamera;
    private Transform cameraTransform;
    private bool isGrounded;
    private Vector3 platformVelocity;
    private Transform currentPlatform;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        freeLookCamera = FindObjectOfType<CinemachineFreeLook>();
        cameraTransform = Camera.main.transform;

        // 设置 Rigidbody
        rb.freezeRotation = true;

        // 设置 Cinemachine 跟随目标
        if (freeLookCamera != null)
        {
            freeLookCamera.Follow = transform;
            freeLookCamera.LookAt = transform;
        }

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMouseLook();
        HandleJump();
    }

    void FixedUpdate()
    {
        HandleMovement();
        CheckGrounded();
    }

    void HandleMouseLook()
    {
        // 鼠标控制已经通过 Cinemachine 自动处理
        // 这里我们基于相机朝向计算移动方向
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 基于相机朝向计算移动方向
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;

        // 应用移动速度
        Vector3 targetVelocity = moveDirection * moveSpeed;

        // 保持 Y 轴速度不变
        targetVelocity.y = rb.velocity.y;

        // 如果站在移动平台上，添加平台速度
        if (currentPlatform != null)
        {
            targetVelocity += platformVelocity;
        }

        rb.velocity = targetVelocity;

        // 让角色朝向移动方向（仅在移动时）
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            animator.SetBool("isJump", true);
        }
    }

    void CheckGrounded()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayer);

        // 更新当前平台信息
        if (isGrounded && hit.collider.CompareTag("MovingPlatform"))
        {
            if (currentPlatform != hit.collider.transform)
            {
                currentPlatform = hit.collider.transform;
                platformVelocity = Vector3.zero;
            }

            animator.SetBool("isJump", false);
        }
        else
        {
            currentPlatform = null;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // 检测是否站在移动平台上并获取平台速度
        if (collision.collider.CompareTag("MovingPlatform") && isGrounded)
        {
            Rigidbody platformRb = collision.collider.GetComponent<Rigidbody>();
            if (platformRb != null)
            {
                platformVelocity = platformRb.velocity;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("MovingPlatform"))
        {
            currentPlatform = null;
            platformVelocity = Vector3.zero;
        }
    }

    // 可视化地面检测
    void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
