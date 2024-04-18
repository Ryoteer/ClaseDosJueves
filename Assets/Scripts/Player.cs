using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("<color=orange>Animation</color>")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _xAxisName = "xAxis";
    [SerializeField] private string _zAxisName = "zAxis";
    [SerializeField] private string _onJumpName = "onJump";
    [SerializeField] private string _onLandName = "onLand";
    [SerializeField] private string _onAttackName = "onAttack";
    [SerializeField] private string _isMovingName = "isMoving";

    [Header("Inputs")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _attackKey = KeyCode.Mouse0;

    [Header("Values")]
    [Tooltip("Modifies jumping strength. Higher values make the character jump farther.")]
    [SerializeField] private float _jumpForce = 5f;
    [Tooltip("Modifies character's movement speed.")]
    [SerializeField] private float _speed = 5f;

    private float _xAxis, _zAxis;
    private Vector3 _dir = new Vector3(0f, 0f, 0f);

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _rb.angularDrag = 1f;
    }

    private void Start()
    {
        if (!_animator)
        {
            _animator = GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        _xAxis = Input.GetAxis("Horizontal");
        _animator.SetFloat(_xAxisName, _xAxis);
        _zAxis = Input.GetAxis("Vertical");
        _animator.SetFloat(_zAxisName, _zAxis);

        _animator.SetBool(_isMovingName, (_xAxis != 0 || _zAxis != 0));

        if (Input.GetKeyDown(_jumpKey))
        {
            _animator.SetTrigger(_onJumpName);
        }

        if (Input.GetKeyDown(_attackKey))
        {
            _animator.SetTrigger(_onAttackName);
        }
    }

    private void FixedUpdate()
    {
        if(_xAxis != 0 || _zAxis != 0)
        {
            Movement(_xAxis, _zAxis);
        }
    }

    public void Jump()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    public void Attack()
    {
        print($"Usted se tiene que arrepentir de lo que dijo.");
    }

    private void Movement(float xAxis, float zAxis)
    {
        _dir = (transform.right * xAxis + transform.forward * zAxis).normalized;

        _rb.MovePosition(transform.position + _dir * _speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _animator.SetTrigger(_onLandName);
    }
}
