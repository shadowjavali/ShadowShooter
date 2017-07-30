using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolManager : SystemManager
{

    #region EnumsAndStructs

    public enum AssetType
    {
        PLAYER = 0,
        BULLET = 1,
        ENEMY_1 = 2,
        ENEMY_2 = 3,
        TURRET = 6,
        CAMERAMANAGER = 9,
        WALL_NORMAL_V = 10,
        WALL_NORMAL_H = 11,
        WALL_CORNER = 12,
        DOOR_H = 13,
        DOOR_V = 14
    }
  

    [System.Serializable]
    public struct Asset
    {
        public GameObject prefab;
        public AssetType type;
    }

    #endregion

    [SerializeField] private Asset[] assetArray;

    [SerializeField]
    private Transform _bufferPool;

    private Dictionary<AssetType, GameObject> assetDict;
    private Dictionary<AssetType, List<GameObject>> _levelObjectPool = new Dictionary<AssetType, List<GameObject>>() ;

    public Action<SpawningAreaManager.GameAreaType, Vector2, SpawningAreaManager.DoorPosition> onSpawnArea;

    private List<LevelObject> _childObjects = new List<LevelObject>();

    public Func<AssetType, Vector2, Transform, GameObject> JFunc_Spawn;

    #region Initialization

    public override void J_Start()
    {
        base.J_Start();

        FillActions();
        SetDictionary();
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

    public bool J_Spawn(SpawningAreaManager p_area, AssetType p_assetType, int p_quantity = 1)
    {
        for (int i = 0; i < p_quantity; i++)
        {          
            if (!p_area.J_Spawn(p_assetType))
                return false;                
        }

        return true;
       
            

    }
    private GameObject Spawn(AssetType p_type, Vector2 p_position)
    {
        GameObject __gameObject = Spawn(p_type, p_position, transform.parent);
        _childObjects.Add(__gameObject.GetComponent<LevelObject>());


        __gameObject.GetComponent<LevelObject>().onDespawn += delegate (AssetType p_assetType, GameObject p_gameObject)
        {
            _childObjects.Remove(p_gameObject.GetComponent<LevelObject>());
        };


        return __gameObject;

    }

    private GameObject Spawn(AssetType p_type, Vector2 p_position, Transform p_parent)
    {
        GameObject __objectToRespawn = null;

        List<GameObject> __poolFractionByType = null;

        bool __spawned = false;

        if (_levelObjectPool.TryGetValue(p_type, out __poolFractionByType))
        {
            if (__poolFractionByType.Count > 0)
            {
                __objectToRespawn = __poolFractionByType[__poolFractionByType.Count-1];
                __objectToRespawn.transform.position = p_position;
                __objectToRespawn.transform.SetParent(p_parent);

                __objectToRespawn.GetComponent<LevelObject>().onDespawn = Despawn;
                __objectToRespawn.GetComponent<LevelObject>().onSpawnChild = Spawn;
                __objectToRespawn.GetComponent<LevelObject>().onSpawnFreeObject = Spawn;
                __poolFractionByType.Remove(__poolFractionByType[__poolFractionByType.Count - 1]);

                _levelObjectPool[p_type] = __poolFractionByType;

                if ((p_type == AssetType.DOOR_H) || (p_type == AssetType.DOOR_V))
                {
                    __objectToRespawn.GetComponent<Door>().onSpawnArea = onSpawnArea;
                }

                __spawned = true;
            }
        }

        if (__spawned == false) //didn't find any objects in pool fraction
        {
            __objectToRespawn = Instantiate(assetDict[p_type].gameObject, p_position, Quaternion.identity, p_parent);

            __objectToRespawn.GetComponent<LevelObject>().onDespawn = Despawn;
            __objectToRespawn.GetComponent<LevelObject>().onSpawnChild = Spawn;
            __objectToRespawn.GetComponent<LevelObject>().onSpawnFreeObject = Spawn;
                
            __objectToRespawn.transform.SetAsFirstSibling();

            if ((p_type == AssetType.DOOR_H) || (p_type == AssetType.DOOR_V))
            {
                __objectToRespawn.GetComponent<Door>().onSpawnArea = onSpawnArea;
            }

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
            p_gameObject.name = p_gameObject.name + "_Pool";
        }
        else
        {
            __poolFractionByType = new List<GameObject>();
            __poolFractionByType.Add(p_gameObject);
            p_gameObject.name = p_gameObject.name + "_Pool";
            _levelObjectPool.Add(p_type, __poolFractionByType);
        }

        p_gameObject.transform.SetParent(_bufferPool);
    }

    public override void J_Update()
    {
       
        for (int i = 0; i < _childObjects.Count; i++)
        {
            _childObjects[i].J_Update();
        }
    }
}
