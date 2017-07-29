using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawningAreaManager : MonoBehaviour
{
    public PoolManager.GameArea area;
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
            _spawners[i].J_Start();
        }
    }

    public virtual void J_Update()
    {
        for (int i = 0; i < _spawners.Length; i++)
        {
            _spawners[i].J_Update();
        }
    }

    public bool J_Spawn(PoolManager.AssetType p_type)
    {
        List<Spawner> __spawnersOfType = new List<Spawner>();

        for (int i=0; i< _spawners.Length; i++)
        {
            if (_spawners[i].type == p_type)
            {
                __spawnersOfType.Add(_spawners[i]);
            }
        }

        if (__spawnersOfType.Count > 0)
        {
            int __maxQueue = 0;
            int __index = UnityEngine.Random.Range(0, __spawnersOfType.Count - 1);
            int __firstRandomizedIndex = __index;

            int __emergencyExit = 0;

            while (__spawnersOfType[__index].Spawn(__maxQueue) == false)
            {
                if (__spawnersOfType.Count == 1)
                {
                    __maxQueue++;
                }
                else
                {
                    if (__index == __firstRandomizedIndex - 1)
                    {
                        __maxQueue++;
                        __index = __firstRandomizedIndex;
                    }
                    if (__index < __spawnersOfType.Count - 1)
                    {
                        __index++;
                    }
                    else if ((__index == __spawnersOfType.Count - 1) && (__firstRandomizedIndex == 0))
                    {
                        __maxQueue++;
                        __index = __firstRandomizedIndex;
                    }
                    else
                    {
                        __index = 0;
                    }
                }                             

                __emergencyExit++;
                if (__emergencyExit > 100)
                {
                    Debug.LogError("Something is wrong: MaxQueve = "  + __maxQueue + " / __index = " + __index);
                    break;                

                }
                   
            }          


            return true;
        }

        return false;
    }

    public bool J_Spawn(PoolManager.AssetType p_type, int p_spawnerIndex)
    {
        if (_spawners[p_spawnerIndex].type == p_type)
        {
            _spawners[p_spawnerIndex].Spawn(1000);
            return true;
        }

        return false;
    }

    private GameObject Spawn(PoolManager.AssetType p_type, Vector2 p_position)
    {
        return onSpawn(p_type, p_position, transform);
    }

}
