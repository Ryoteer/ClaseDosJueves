using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollower : MonoBehaviour, IUpdate
{
    [Header("Components")]
    [Tooltip("Selects which Game Object the camera will track.")]
    [SerializeField] private Transform _target;

    private Vector3 _offset = new Vector3(), _finalCamPos = new Vector3();

    private void Start()
    {
        UpdateManager.Instance.Add(this);

        _offset = transform.position;
    }

    public void ArtificalUpdate() { }

    public void ArtificalFixedUpdate() { }

    public void ArtificalLateUpdate()
    {
        _finalCamPos = _target.position + _offset;

        transform.position = _finalCamPos;
    }
}
