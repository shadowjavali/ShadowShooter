using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelObject : MonoBehaviour
{
    [SerializeField] protected PoolManager.AssetType _assetType;
    public Action<PoolManager.AssetType,GameObject> onDespawn;
    public Func<PoolManager.AssetType, Vector2, Transform, GameObject> onSpawnChild;

    public virtual void J_Start()
    {
     
    }

    public virtual void J_Update()
    {
       
    }

    public virtual void Despawn()
    {
        if (onDespawn != null)
        {
            onDespawn(_assetType, gameObject);
        }
    }

}
