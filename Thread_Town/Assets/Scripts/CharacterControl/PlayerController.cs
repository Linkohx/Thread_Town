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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        freeLookCamera = FindObjectOfType<CinemachineFreeLook>();
        cameraTransform = Camera.main.transform;

        // ���� Rigidbody
        rb.freezeRotation = true;

        // ���� Cinemachine ����Ŀ��
        if (freeLookCamera != null)
        {
            freeLookCamera.Follow = transform;
            freeLookCamera.LookAt = transform;
        }
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
        // �������Ѿ�ͨ�� Cinemachine �Զ�����
        // �������ǻ��������������ƶ�����
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // ���������������ƶ�����
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;

        // Ӧ���ƶ��ٶ�
        Vector3 targetVelocity = moveDirection * moveSpeed;

        // ���� Y ���ٶȲ���
        targetVelocity.y = rb.velocity.y;

        // ���վ���ƶ�ƽ̨�ϣ����ƽ̨�ٶ�
        if (currentPlatform != null)
        {
            targetVelocity += platformVelocity;
        }

        rb.velocity = targetVelocity;

        // �ý�ɫ�����ƶ����򣨽����ƶ�ʱ��
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
        }
    }

    void CheckGrounded()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayer);

        // ���µ�ǰƽ̨��Ϣ
        if (isGrounded && hit.collider.CompareTag("MovingPlatform"))
        {
            if (currentPlatform != hit.collider.transform)
            {
                currentPlatform = hit.collider.transform;
                platformVelocity = Vector3.zero;
            }
        }
        else
        {
            currentPlatform = null;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // ����Ƿ�վ���ƶ�ƽ̨�ϲ���ȡƽ̨�ٶ�
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

    // ���ӻ�������
    void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
