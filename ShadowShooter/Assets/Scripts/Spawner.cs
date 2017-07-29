using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawner : MonoBehaviour
{
    private List<LevelObject> _levelObjects =  new List<LevelObject>();

    [SerializeField] private PoolManager.AssetType _type;
    public Func<PoolManager.AssetType, Vector2, GameObject> onSpawn;

    public void Spawn()
    {
        _levelObjects.Add(onSpawn(_type, transform.position).GetComponent<LevelObject>());
    }

}
