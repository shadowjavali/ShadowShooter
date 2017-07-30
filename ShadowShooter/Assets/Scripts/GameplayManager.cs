using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : SystemManager
{
    [SerializeField] PoolManager _poolManager;
    private List<SpawningAreaManager> _gameAreaList = new List<SpawningAreaManager>();

    [SerializeField] private GameObject[] _gameAreaPrefabArray;
    private Dictionary<SpawningAreaManager.GameAreaType, GameObject> _gameAreaPrefabDict;



    public override void J_Start()
    {
        InitializeAreasDictionary();
        _poolManager.J_Start();
        InitializeArea(SpawningAreaManager.GameAreaType.CENTRAL_AREA, new Vector2(0,0));
        InitializeArea(SpawningAreaManager.GameAreaType.AREA_TYPE_0, new Vector2(1, 0));

        _poolManager.J_Spawn(_gameAreaList[0], PoolManager.AssetType.PLAYER);

       // _poolManager.J_Spawn(PoolManager.GameArea.CENTRAL_AREA, PoolManager.AssetType.ENEMY_1, 12);
    }

    private void InitializeAreasDictionary()
    {
        _gameAreaPrefabDict = new Dictionary<SpawningAreaManager.GameAreaType, GameObject>();
        for (int i = 0; i < _gameAreaPrefabArray.Length; i++)
        {
            _gameAreaPrefabDict.Add(_gameAreaPrefabArray[i].GetComponent<SpawningAreaManager>().areaType, _gameAreaPrefabArray[i]);
        }
    }

    private void InitializeArea(SpawningAreaManager.GameAreaType p_areaType, Vector2 p_gridPos)
    {
        SpawningAreaManager __newArea = Instantiate(_gameAreaPrefabDict[p_areaType], p_gridPos * 11.52f, Quaternion.identity, transform).GetComponent<SpawningAreaManager>();
      
        __newArea.onSpawn = _poolManager.JFunc_Spawn;
        __newArea.J_Start(p_gridPos);
        _gameAreaList.Add(__newArea);
    }

    public override void J_Update()
    {
        _poolManager.J_Update();
        for (int i = 0; i < _gameAreaList.Count; i++)
        {
            _gameAreaList[i].J_Update();
        }
    }
}
