using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IUpdate
{
    [Header("<color=red>AI</color>")]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _target;
    [SerializeField] private float _attackDistance = 1.5f;
    [SerializeField] private float _chaseDistance = 7.5f;
    [SerializeField] private Transform[] _patrolNodes;
    [SerializeField] private float _patrolNodeDist = .5f;

    [Header("<color=red>Values</color>")]
    [SerializeField] private int _maxHP = 100;

    private int _actualHP;

    private Transform _actualDestination = null;

    private void Awake()
    {
        _actualHP = _maxHP;

        if (!_agent)
        {
            _agent = GetComponent<NavMeshAgent>();
        }
    }

    private void Start()
    {
        UpdateManager.Instance.Add(this);

        _actualDestination = GetNode(_actualDestination);

        _agent.SetDestination(_actualDestination.position);
    }

    public void ArtificalUpdate() { }

    public void ArtificalFixedUpdate()
    {
        if ((_target.position - transform.position).sqrMagnitude <= Mathf.Pow(_chaseDistance, 2))
        {
            _agent.SetDestination(_target.position);

            if (((_target.position - transform.position).sqrMagnitude <= Mathf.Pow(_attackDistance, 2)))
            {
                _agent.isStopped = true;

                transform.LookAt(_target.position);

                print($"May thy knife chip and shatter.");
            }
            else if (_agent.isStopped)
            {
                _agent.isStopped = false;
            }
        }
        else if ((_actualDestination.position - transform.position).sqrMagnitude <= Mathf.Pow(_patrolNodeDist, 2))
        {
            _actualDestination = GetNode(_actualDestination);

            _agent.SetDestination(_actualDestination.position);
        }
        else
        {
            _agent.SetDestination(_actualDestination.position);
        }
    }

    public void ArtificalLateUpdate() { }

    public void TakeDamage(int dmg)
    {
        _actualHP -= dmg;

        if(_actualHP <= 0)
        {
            print($"Oh my God, they killed Kenny!");
            Destroy(gameObject);
        }
        else
        {
            print($"<color=red>{name}</color>: Recibí <color=black>{dmg}</color> puntos de daño.");
        }
    }

    private Transform GetNode(Transform actualNode)
    {
        Transform newNode = null;

        do
        {
            newNode = _patrolNodes[Random.Range(0, _patrolNodes.Length)];
        }
        while (newNode == actualNode);

        return newNode;
    }    
}
