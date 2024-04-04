using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;

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

    private void Update()
    {
        _xAxis = Input.GetAxis("Horizontal");
        _zAxis = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(_jumpKey))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if(_xAxis != 0 || _zAxis != 0)
        {
            Movement(_xAxis, _zAxis);
        }
    }

    private void Jump()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void Movement(float xAxis, float zAxis)
    {
        _dir = (transform.right * xAxis + transform.forward * zAxis).normalized;

        _rb.MovePosition(transform.position + _dir * _speed * Time.fixedDeltaTime);
    }
}
