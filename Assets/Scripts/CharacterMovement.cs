using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using System;
using UnityEngine.UI;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Composites;
using UnityEditor.Timeline;
using UnityEngine.InputSystem.HID;
using System.Net.Sockets;
using Unity.VisualScripting;
public class CharacterMovement : MonoBehaviour
{

    private CharacterController rb;

    private PlayerInput playerIn;
    private EventInteractions interact;

    private Vector2 input;
    private Vector3 direction;
    private Vector3 runVector;
    private Vector3 sprintVector;

    [SerializeField] private float speed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float smoothTime;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpInterval;
    [SerializeField] private float gravityController;
    [SerializeField] private float sphereInteractionRadius;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private Collider[] interactionColliders = new Collider[1];
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Collider[] enemyColliders = new Collider[] { };
    [SerializeField] private float sphereAttackRadius;
    [SerializeField] private int damagePoints;
    [SerializeField] private GameObject slashAttack;
    [SerializeField] private float slashDestroyTime;
    [SerializeField] private Vector3 attackOffset;

    private float gravityMult = -9.81f;
    private float velocity;
    private float curVelocity;

    private bool isSprinting;
    private bool canJump = true;

    private int sphereInteract;
    private int sphereAttack;

    private HealthComponent healthComp;

    private Animator animator;


    private void Awake()
    {
        rb = GetComponent<CharacterController>();
        playerIn = new PlayerInput();
        animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnEnable()
    {
        playerIn.Player.Enable();
    }
    private void OnDisable()
    {
        playerIn.Player.Disable();
    }
    void Update()
    {
        GetCalculatedMovement();

        ApplyRotation();
        ApplyMovement();
        ApplyGravity();        

        var enemyCount = GameObject.FindGameObjectsWithTag("Enemy");
        enemyColliders = new Collider[enemyCount.Length];

        GizmoInteractions();
        GizmoAttack();
    }
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Interaction();
        }
    }
    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        direction = new Vector3(input.x, 0, input.y);
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!IsGrounded()) return;

        if (context.started)
        {
            if (IsGrounded() && canJump)
            {
                canJump = false;
                animator.SetBool("isJumping", true);
                velocity += jumpForce;
                StartCoroutine(JumpBreak(jumpInterval));
            }
        }

    }
    public void SprintPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSprinting = true;
        }
    }
    public void SprintReleased(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSprinting = false;
        }
    }
    private bool IsGrounded() => rb.isGrounded;
    private void ApplyGravity()
    {
        if (IsGrounded() && velocity < 0.0f)
        {
            animator.SetBool("isJumping", false);
            velocity = -1.0f;
        }
        else
        {
            velocity += gravityController * gravityMult * Time.deltaTime;
        }
        direction.y = velocity;
        
    }
    private void ApplyRotation()
    {
        if (input.sqrMagnitude == 0) return;

        var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        var smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref curVelocity, smoothTime);

        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
    }
    private void GetCalculatedMovement()
    {
        runVector = speed * Time.deltaTime * direction;
        sprintVector = sprintSpeed * Time.deltaTime * direction;
    }
    private void ApplyMovement()
    {
        if (isSprinting)
        {
                rb.Move(sprintVector);
            if (direction.x != 0 || direction.z != 0)
            {
                animator.SetBool("isSprinting", true);
                animator.SetBool("isRunning", false);
            }
            else if(direction.x == 0 || direction.z == 0)
            {
                animator.SetBool("isSprinting", false);
                animator.SetBool("isRunning", false);
            }      
        }
        else if (!isSprinting)
        {
                rb.Move(runVector);
            if (direction.x != 0 || direction.z != 0)
            {
                animator.SetBool("isSprinting", false);
                animator.SetBool("isRunning", true);
            }
            else if (direction.x == 0 || direction.z == 0)
            {
                animator.SetBool("isSprinting", false);
                animator.SetBool("isRunning", false);
            }
        } 
    }
    public void Interaction()
    {
        for (int i = 0; i < sphereInteract; i++)
        {
            interact = interactionColliders[i].GetComponent<EventInteractions>();
            if (interact != null)
            {
                interact.Interact();
            }
        }
    }
    public void GizmoInteractions()
    {
        sphereInteract = Physics.OverlapSphereNonAlloc(transform.position, sphereInteractionRadius, interactionColliders, interactionLayer);
    }
    public void GizmoAttack()
    {
        sphereAttack = Physics.OverlapSphereNonAlloc(transform.position + new Vector3(0, 1.5f, 0), sphereAttackRadius, enemyColliders, enemyLayer, QueryTriggerInteraction.Ignore);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = sphereInteract > 0 ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position, sphereInteractionRadius);
        
        Gizmos.color = sphereAttack != 0 ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position + new Vector3(0, 1.5f, 0), sphereAttackRadius);

    }
    public void Fire(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (context.started)
        {
                var slashAtt = Instantiate
                (slashAttack,
                new Vector3(transform.position.x + direction.x + attackOffset.x, 
                transform.position.y + attackOffset.y, 
                transform.position.z),
                transform.rotation);
            Destroy(slashAtt, slashDestroyTime); 
            if (sphereAttack != 0)
            {
                foreach (var enem in enemyColliders)
                {
                    if(enem != null)
                    {
                        healthComp = enem.GetComponent<HealthComponent>();
                        healthComp.ApplyDamage(damagePoints);
                    }
                }
            }
        }
    }

    public IEnumerator JumpBreak(float delay)
    {
        yield return new WaitForSeconds(delay);
        canJump = true;
    }
}


