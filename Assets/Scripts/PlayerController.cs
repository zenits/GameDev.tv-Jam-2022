using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private InputActions _input;
    private Vector2 _moveDirection;
    bool isJumping = false;
    bool isGrounded = false;
    bool isDead = false;
    bool isHit = false;

    Rigidbody2D _rb;
    Animator _animator;

    [SerializeField] LayerMask groundLayers;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float jumpHeight = 1f;
    [SerializeField] float hitForce = 1f;
    [SerializeField] AudioClip jumpSound;
    private void Awake()
    {
        _input = GameManager.Inputs;
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
    }

    private void OnEnable()
    {
        Debug.Log("on enable");
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
    private void OnDisable()
    {
        Debug.Log("on disable");
        _input.Player.Disable();
        _input.Player.Move.started -= OnMove;
        _input.Player.Move.performed -= OnMove;
        _input.Player.Move.canceled -= OnMove;

        _input.Player.Fire.started -= OnFire;
        _input.Player.Fire.performed -= OnFire;
        _input.Player.Fire.canceled -= OnFire;

        _input.Player.Jump.started -= OnJump;
        _input.Player.Jump.performed -= OnJump;
        _input.Player.Jump.canceled -= OnJump;
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
            _rb.velocity = new Vector2(_moveDirection.x * Time.fixedDeltaTime * moveSpeed, _rb.velocity.y);
            _animator.SetBool("Walking", _moveDirection != Vector2.zero);

            // rotate sprite in move direction
            if (_moveDirection.x > 0)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (_moveDirection.x < 0)
                transform.rotation = Quaternion.Euler(0, 180, 0);

        }
    }

    void OnMove(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                _moveDirection = context.ReadValue<Vector2>().normalized;
                break;
            case InputActionPhase.Performed:
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
                break;
            case InputActionPhase.Performed:
                break;
            case InputActionPhase.Canceled:
                break;
            default:
                break;

        }
    }

    void OnJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                if (!this.isJumping && !this.isDead && this.isGrounded)
                {
                    Debug.Log($"OnJump : isJumping={this.isJumping}");
                    Debug.Log("Jump !!!");

                    _rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
                    AudioManager.Instance.PlayOnce(jumpSound, 1);
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

    public void OnDamage(int i)
    {
        Debug.Log("on damaged");
        isHit = true;
        var vel = _rb.velocity;
        vel.x *= -1.8f;
        _rb.velocity = vel;
        _rb.AddForce(Vector2.up * hitForce, ForceMode2D.Impulse);
        _animator.SetBool("Hiting", true);
    }

    public void Die()
    {
        isDead = true;
        _animator.SetBool("Diying", true);
    }


    public void OnHitEnd(int a)
    {
        Debug.Log("OnHitEnd");
        isHit = false;
        _animator.SetBool("Hiting", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & groundLayers.value) > 0)
        {
            isGrounded = true;
            isJumping = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & groundLayers.value) > 0)
        {
            isGrounded = false;
            isJumping = true;
        }
    }

    public void Reset()
    {
        isHit = false;
        isDead = false;
        isJumping = false;
    }
}





