using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GhostController : MonoBehaviour
{

    private InputActions _input;
    private Vector2 _moveDirection;
    private bool isJumping = false;
    private bool isInteracting = false;
    private bool groundedPlayer;
    private bool isDead = false;
    private bool isHit = false;

    Rigidbody2D _rb;

    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float jumpHeight = 1f;
    [SerializeField] float hitForce = 1f;

    public UnityEvent onInteract;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = new InputActions();
        _input.Player.Enable();
        _input.Player.Move.started += OnMove;
        _input.Player.Move.performed += OnMove;
        _input.Player.Move.canceled += OnMove;

        _input.Player.Fire.started += OnFire;
        _input.Player.Fire.performed += OnFire;
        _input.Player.Fire.canceled += OnFire;

        _input.Player.Jump.started += OnJump;
        _input.Player.Jump.performed += OnJump;
        _input.Player.Jump.canceled += OnJump;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDead)
        {
            _rb.velocity = new Vector2(0, 0); // freeze player
        }
        else if (!isHit)
        {
            // move in horizontal directions
            _rb.velocity = new Vector2(_moveDirection.x * Time.fixedDeltaTime * moveSpeed,
                                        _moveDirection.y > 0 ? _moveDirection.y * Time.fixedDeltaTime * moveSpeed / 2 : _rb.velocity.y);

            // rotate sprite in move direction
            if (_moveDirection.x > 0)
                transform.rotation = Quaternion.Euler(0, 180, 0);
            else if (_moveDirection.x < 0)
                transform.rotation = Quaternion.Euler(0, 0, 0);


            if (Mathf.Approximately(_rb.velocity.y, 0.0f))
            {
                isJumping = false;
            }
        }
    }


    void OnMove(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                break;
            case InputActionPhase.Performed:
                _moveDirection = context.ReadValue<Vector2>().normalized;
                break;
            case InputActionPhase.Canceled:
                _moveDirection = Vector2.zero;
                break;
            default:
                break;

        }
    }

    void OnFire(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                Debug.Log("Playercontroller.Interact");
                isInteracting = true;
                onInteract.Invoke();
                break;

            case InputActionPhase.Performed:
                break;

            case InputActionPhase.Canceled:
                isInteracting = false;
                break;
            default:
                break;

        }
    }

    void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("OnJump");
        switch (context.phase)
        {
            case InputActionPhase.Started:
                if (!isJumping && !isDead)
                {
                    isJumping = true;
                    _rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
                }
                break;
            case InputActionPhase.Performed:
                break;
            case InputActionPhase.Canceled:
                break;
            default:
                break;

        }
    }

    public void OnDamage()
    {
        Debug.Log("on damaged");
        isHit = true;
        var vel = _rb.velocity;
        vel.x *= -1.8f;
        _rb.velocity = vel;
        _rb.AddForce(Vector2.up * hitForce, ForceMode2D.Impulse);
    }

    public void Die()
    {
        isDead = true;
    }


    public void OnHitEnd(int a)
    {
        Debug.Log("OnHitEnd");
        isHit = false;
    }
    public void Reset()
    {
        isHit = false;
        isDead = false;
        isJumping = false;
    }
}





