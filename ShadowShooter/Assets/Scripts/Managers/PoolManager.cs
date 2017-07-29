using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolManager : SystemManager
{

    #region EnumsAndStructs

    public enum AssetType
    {
        PLAYER,
        BULLET
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
    private Dictionary<AssetType, List<GameObject>> _levelObjectPool = new Dictionary<AssetType, List<GameObject>>() ;

    private SpawningAreaManager[] _gameAreaArray;

    public Func<AssetType, Vector2, Transform, GameObject> JFunc_Spawn;

    #region Initialization

    public override void J_Start()
    {
        base.J_Start();

        FillActions();
        SetDictionary();
        InitializeAllAreas();
    }

    private void InitializeAllAreas()
    {
        _gameAreaArray = GetComponentsInChildren<SpawningAreaManager>();
        for (int i=0; i< _gameAreaArray.Length; i++)
        {
            _gameAreaArray[i].onSpawn = JFunc_Spawn;
            _gameAreaArray[i].J_Start();
        }
    }

    private void FillActions()
    {
        JFunc_Spawn += Spawn;
    }
    private void ClearActions()
    {
        JFunc_Spawn -= Spawn;
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

    public bool J_Spawn(GameArea p_area, AssetType p_assetType, int p_quantity = 1)
    {
        for (int i = 0; i < _gameAreaArray.Length; i++ )
        {
            if (_gameAreaArray[i].area == p_area)
            {
                if (_gameAreaArray[i].J_Spawn(p_assetType))
                {
                    p_quantity--;
                }
            }
        }

        if (p_quantity <= 0)
            return true;
        else
            return false;

    }

    private GameObject Spawn(AssetType p_type, Vector2 p_position, Transform p_parent)
    {
        GameObject __objectToRespawn = null;

        List<GameObject> __poolFractionByType = null;

        if (_levelObjectPool.TryGetValue(p_type, out __poolFractionByType))
        {
            if (__poolFractionByType.Count > 0)
            {
                __objectToRespawn = __poolFractionByType[__poolFractionByType.Count-1];
                __objectToRespawn.GetComponent<LevelObject>().onDespawn = Despawn;
                __objectToRespawn.GetComponent<LevelObject>().onSpawnChild = Spawn;
                __objectToRespawn.GetComponent<LevelObject>().J_Start();
                __poolFractionByType.RemoveAt(__poolFractionByType.Count - 1);
            }
        }

        if (__objectToRespawn == null) //didn't find any objects in pool fraction
        {
            __objectToRespawn = Instantiate(assetDict[p_type].gameObject, p_position, Quaternion.identity, p_parent);
            __objectToRespawn.GetComponent<LevelObject>().onDespawn = Despawn;
            __objectToRespawn.GetComponent<LevelObject>().onSpawnChild = Spawn;
            __objectToRespawn.GetComponent<LevelObject>().J_Start();

        }

        return __objectToRespawn;
            
    }

    public void Despawn(AssetType p_type, GameObject p_gameObject)
    {
        p_gameObject.transform.position = Vector3.one * 10000000;

        List<GameObject> __poolFractionByType = null;

        if (_levelObjectPool.TryGetValue(p_type, out __poolFractionByType))
        {
            __poolFractionByType.Add(p_gameObject);
        }
        else
        {
            __poolFractionByType = new List<GameObject>();
            __poolFractionByType.Add(p_gameObject);
            _levelObjectPool.Add(p_type, __poolFractionByType);
        }

    }

    public override void J_Update()
    {
        for (int i = 0; i < _gameAreaArray.Length; i++)
        {
            _gameAreaArray[i].J_Update();
        }
    }


}
