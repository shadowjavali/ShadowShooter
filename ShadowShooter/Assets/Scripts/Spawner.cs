using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawner : MonoBehaviour
{
    private List<LevelObject> _levelObjects =  new List<LevelObject>();

    public PoolManager.AssetType type;
    public Func<PoolManager.AssetType, Vector2, GameObject> onSpawn;

    public void Spawn()
    {
        _levelObjects.Add(onSpawn(type, transform.position).GetComponent<LevelObject>());
    }

    public virtual void J_Start()
    {

    }

    public virtual void J_Update()
    {
        for (int i=0; i< _levelObjects.Count;i++)
        {
            _levelObjects[i].J_Update();
        }
    }

}
