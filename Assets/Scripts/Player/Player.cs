using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class Player : MonoBehaviour, IUpdate
{
    [Header("<color=orange>Animation</color>")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _xAxisName = "xAxis";
    [SerializeField] private string _zAxisName = "zAxis";
    [SerializeField] private string _onJumpName = "onJump";
    [SerializeField] private string _onAttackName = "onAttack";
    [SerializeField] private string _onAreaAttackName = "onAreaAttack";
    [SerializeField] private string _isMovingName = "isMoving";
    [SerializeField] private string _isGroundedName = "isGrounded";
    [SerializeField] private string _onDanceName = "onDance";

    [Header("<color=orange>Audio</color>")]
    [SerializeField] private AudioClip[] _stepClips;
    [SerializeField] private AudioClip[] _attackClips;

    [Header("<color=orange>Inputs</color>")]
    [SerializeField] private KeyCode _attackKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode _areaAttackKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode _danceKey = KeyCode.F;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;

    [Header("<color=orange>Physics</color>")]
    [SerializeField] private Transform _attackOrigin;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private LayerMask _attackMask;
    [SerializeField] private float _areaAttackRadius = 3f;
    [SerializeField] private float _groundRange = .5f;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _wallRange = 1f;
    [SerializeField] private LayerMask _wallMask;

    [Header("<color=orange>Values</color>")]
    [Tooltip("Indicates how much damage an entity takes when it's hit by the Player.")]
    [SerializeField] private int _dmg = 20;
    [Tooltip("Modifies jumping strength. Higher values make the character jump farther.")]
    [SerializeField] private float _jumpForce = 5f;
    [Tooltip("Modifies character's movement speed.")]
    [SerializeField] private float _speed = 5f;

    private bool _isDancing = false;
    private float _xAxis, _zAxis;
    private Vector3 _dir = new(), _transformOffset = new(), _dirOffset = new(), _dirCheck = new();

    private AudioSource _source;
    private Rigidbody _rb;

    private Ray _attackRay, _groundRay, _wallRay;
    private RaycastHit _attackHit;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _rb.angularDrag = 1f;

        _source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        UpdateManager.Instance.Add(this);

        if (!_animator)
        {
            _animator = GetComponentInChildren<Animator>();
        }
    }

    public void ArtificalUpdate()
    {
        if (_isDancing) return;

        _xAxis = Input.GetAxis("Horizontal");
        _animator.SetFloat(_xAxisName, _xAxis);
        _zAxis = Input.GetAxis("Vertical");
        _animator.SetFloat(_zAxisName, _zAxis);

        _animator.SetBool(_isMovingName, (_xAxis != 0 || _zAxis != 0));
        _animator.SetBool(_isGroundedName, IsGrounded());

        if (Input.GetKeyDown(_jumpKey) && IsGrounded())
        {
            _animator.SetTrigger(_onJumpName);
        }

        if (Input.GetKeyDown(_danceKey))
        {
            _animator.SetTrigger(_onDanceName);
        }

        if (Input.GetKeyDown(_attackKey))
        {
            _animator.SetTrigger(_onAttackName);
        }
        else if (Input.GetKeyDown(_areaAttackKey))
        {
            _animator.SetTrigger(_onAreaAttackName);
        }
    }

    public void ArtificalFixedUpdate()
    {
        if (_isDancing) return;

        if ((_xAxis != 0 || _zAxis != 0) && !IsBlocked(_xAxis, _zAxis))
        {
            Movement(_xAxis, _zAxis);
        }
    }

    public void ArtificalLateUpdate() { }

    public void Jump()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    public void Attack()
    {
        _attackRay = new Ray(_attackOrigin.position, transform.forward);

        if(Physics.Raycast(_attackRay, out _attackHit, _attackRange, _attackMask))
        {
            if(_attackHit.collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(_dmg);
            }
        }
    }

    public void AreaAttack()
    {
        Collider[] objs = Physics.OverlapSphere(transform.position, _areaAttackRadius, _attackMask);

        foreach(Collider obj in objs)
        {
            if(obj.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(_dmg * 2);
            }
        }
    }

    public void PlayStepClip()
    {
        if (_source.isPlaying)
        {
            _source.Stop();
        }

        _source.clip = _stepClips[Random.Range(0, _stepClips.Length)];

        _source.Play();
    }

    public void PlayAttackClip(int index)
    {
        if (_source.isPlaying)
        {
            _source.Stop();
        }

        _source.clip = _attackClips[index];

        _source.Play();
    }

    private void Movement(float xAxis, float zAxis)
    {
        _dir = (transform.right * xAxis + transform.forward * zAxis).normalized;

        _rb.MovePosition(transform.position + _dir * _speed * Time.fixedDeltaTime);
    }

    private bool IsBlocked(float xAxis, float zAxis)
    {
        _dirOffset = new Vector3(transform.position.x,
                                       transform.position.y + .1f,
                                       transform.position.z);

        _dirCheck = (transform.right * xAxis + transform.forward * zAxis);

        _wallRay = new Ray(_dirOffset, _dirCheck);

        return Physics.Raycast(_wallRay, _wallRange, _wallMask);
    }

    private bool IsGrounded()
    {
        _transformOffset = new Vector3(transform.position.x,
                                       transform.position.y + _groundRange / 4,
                                       transform.position.z);

        _groundRay = new Ray(_transformOffset, -transform.up);

        return Physics.Raycast(_groundRay, _groundRange, _groundMask);
    }

    public void SetDanceState(int state)
    {
        switch (state)
        {
            case 0:
                _isDancing = false;
                break;
            case 1:
                _isDancing = true;
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_groundRay);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_wallRay);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(_attackRay);
        Gizmos.DrawWireSphere(transform.position, _areaAttackRadius);
    }    
}
