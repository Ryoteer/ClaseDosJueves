using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class Platform : MonoBehaviour
{
    [Header("<color=purple>AI</color>")]
    [SerializeField] private NavMeshSurface _surface;
    [SerializeField] private NavMeshModifier _modifier;

    [Header("<color=purple>Interaction</color>")]
    [SerializeField] private float _fadeTime = 3f;
    [SerializeField] private float _intermission = 2f;
    [SerializeField] private float _respawnTime = 3f;

    private bool _isActive;

    private Collider _collider;
    private Material _material;

    private Color _ogColor;

    private void Awake()
    {
        if (!_modifier)
        {
            _modifier = GetComponent<NavMeshModifier>();
        }

        _collider = GetComponent<Collider>();
        _material = GetComponent<Renderer>().material;

        _ogColor = _material.color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isActive)
        {
            StartCoroutine(PlatformFade());
        }
    }

    private IEnumerator PlatformFade()
    {
        _isActive = !_isActive;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / _fadeTime;
            _material.color = new Color(_ogColor.r, _ogColor.g, _ogColor.b, Mathf.Lerp(1f, 0f, t));
            yield return null;
        }

        _material.color = new Color(_ogColor.r, _ogColor.g, _ogColor.b, 0f);
        _collider.enabled = false;

        _modifier.enabled = false;
        _surface.BuildNavMesh();

        yield return new WaitForSeconds(_intermission);

        t = 0f;

        while(t < 1f)
        {
            t += Time.deltaTime / _respawnTime;
            _material.color = new Color(_ogColor.r, _ogColor.g, _ogColor.b, Mathf.Lerp(0f, 1f, t));
            yield return null;
        }

        _material.color = new Color(_ogColor.r, _ogColor.g, _ogColor.b, 1f);
        _collider.enabled = true;

        _modifier.enabled = true;
        _surface.BuildNavMesh();

        _isActive = !_isActive;
    }
}
