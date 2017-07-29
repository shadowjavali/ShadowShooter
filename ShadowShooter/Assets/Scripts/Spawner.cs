using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawner : MonoBehaviour
{
    private PoolManager.GameArea _area;
    [SerializeField] private PoolManager.AssetType _type;
    public Action<PoolManager.AssetType, Vector2> onSpawn;

}
