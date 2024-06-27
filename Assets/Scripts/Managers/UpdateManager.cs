using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    #region Singleton
    public static UpdateManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private List<IUpdate> _updateableObjs = new();

    public void Add(IUpdate obj)
    {
        _updateableObjs.Add(obj);
    }

    public void Remove(IUpdate obj)
    {
        if (_updateableObjs.Contains(obj))
        {
            _updateableObjs.Remove(obj);
        }
    }

    public void Clear()
    {
        _updateableObjs.Clear();
    }

    private void Update()
    {
        foreach(IUpdate obj in _updateableObjs)
        {
            obj.ArtificalUpdate();
        }
    }

    private void FixedUpdate()
    {
        foreach (IUpdate obj in _updateableObjs)
        {
            obj.ArtificalFixedUpdate();
        }
    }

    private void LateUpdate()
    {
        foreach (IUpdate obj in _updateableObjs)
        {
            obj.ArtificalLateUpdate();
        }
    }
}
