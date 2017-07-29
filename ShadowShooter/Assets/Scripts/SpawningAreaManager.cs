using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawningAreaManager : MonoBehaviour
{
    private PoolManager.GameArea _area;
    public Func<PoolManager.AssetType, Vector2, Transform, GameObject> onSpawn;

    private Spawner[] _spawners;

    public void J_Start()
    {
        InitializeSpawners();
    }

    private void InitializeSpawners()
    {
        _spawners = GetComponentsInChildren<Spawner>();
        for (int i=0; i< _spawners.Length; i++)
        {
            _spawners[i].onSpawn += Spawn;
        }
    }

    private GameObject Spawn(PoolManager.AssetType p_type, Vector2 p_position)
    {
        return onSpawn(p_type, p_position, transform);
    }

}
