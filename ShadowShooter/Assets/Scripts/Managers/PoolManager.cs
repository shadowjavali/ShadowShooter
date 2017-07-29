using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolManager : SystemManager
{

    #region EnumsAndStructs

    public enum AssetType
    {
        PLAYER
    }

    public enum GameArea
    {
        AREA_0,
        AREA_1
    }

    [System.Serializable]
    public struct Asset
    {
        public GameObject prefab;
        public AssetType type;
    }

    #endregion

    [SerializeField] private Asset[] assetArray;

    private Dictionary<AssetType, GameObject> assetDict;
    private Dictionary<AssetType, List<GameObject>> _levelObjectPool;

    private SpawningAreaManager[] _gameAreaArray;

    #region Initialization

    public override void J_Start()
    {
        base.J_Start();
        SetDictionary();
        InitializeAllAreas();
    }

    private void InitializeAllAreas()
    {
        _gameAreaArray = GetComponentsInChildren<SpawningAreaManager>();

    }

    private void SetDictionary()
    {
        assetDict = new Dictionary<AssetType, GameObject>();

        for (int i=0; i< assetArray.Length; i++)
        {
            assetDict.Add(assetArray[i].type, assetArray[i].prefab);
        }
    }

#endregion

    public GameObject Spawn(AssetType p_type, Vector2 p_position, Transform p_parent)
    {
        GameObject __objectToRespawn = null;

        List<GameObject> __poolFractionByType = null;

        if (_levelObjectPool.TryGetValue(p_type, out __poolFractionByType))
        {
            if (__poolFractionByType.Count > 0)
            {
                __objectToRespawn = __poolFractionByType[__poolFractionByType.Count-1];
                __poolFractionByType.RemoveAt(__poolFractionByType.Count - 1);
            }
        }

        if (__objectToRespawn == null) //didn't find any objects in pool fraction
        {
            return Instantiate(assetDict[p_type].gameObject, p_position, Quaternion.identity  , p_parent);
        }
        else
        {
            return __objectToRespawn;
        }     

    }

	
}
