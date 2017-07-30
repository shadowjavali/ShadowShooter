using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelObject : MonoBehaviour
{
    [SerializeField] protected PoolManager.AssetType _assetType;
    public Action<PoolManager.AssetType,GameObject> onDespawn;
    public Func<PoolManager.AssetType, Vector2, Transform, GameObject> onSpawnChild;
    public Func<PoolManager.AssetType, Vector2, GameObject> onSpawnFreeObject;

    protected bool _active = false;

    protected List<LevelObject> _childObjects = new List<LevelObject>();

    public virtual void J_Start(params object[] p_args)
    {
        _active = true;
    }

    public virtual void J_Update()
    {
        for (int i = 0; i < _childObjects.Count; i++)
        {
            _childObjects[i].J_Update();
        }
    }

    public virtual void Despawn()
    {
        _active = false;
        if (onDespawn != null)
        {
            onDespawn(_assetType, gameObject);
        }
    }

}
