using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : SystemManager
{
    [SerializeField] PoolManager _poolManager;
    [SerializeField] ScreenCanvas _screenCanvas;

    private Dictionary<Vector2, SpawningAreaManager> _gameAreaCols = new Dictionary<Vector2, SpawningAreaManager>();

    [SerializeField] private GameObject[] _gameAreaPrefabArray;
    private Dictionary<SpawningAreaManager.GameAreaType, GameObject> _gameAreaPrefabDict;

    private bool _gameOver = false;

    public override void J_Start()
    { 
        InitializeAreasDictionary();
        _poolManager.onSpawnArea = HandleOnSpawnArea;
        _poolManager.J_Start();
        InitializeArea(SpawningAreaManager.GameAreaType.CENTRAL_AREA, new Vector2(0,0), SpawningAreaManager.DoorPosition.NONE);
        
        Player __player = _poolManager.Spawn(PoolManager.AssetType.PLAYER, new Vector2(0, 0)).GetComponent<Player>();
        __player.J_Start( new object[] { _gameAreaCols[new Vector2(0, 0)] } );
        __player.onDespawn = HandleOnPlayerFinish;
        // _poolManager.J_Spawn(PoolManager.GameArea.CENTRAL_AREA, PoolManager.AssetType.ENEMY_1, 12);
    }

    private void HandleOnPlayerFinish(PoolManager.AssetType p_assetType, GameObject p_object)
    {
        _gameOver = true;
    }

    private void InitializeAreasDictionary()
    {
        _gameAreaPrefabDict = new Dictionary<SpawningAreaManager.GameAreaType, GameObject>();
        for (int i = 0; i < _gameAreaPrefabArray.Length; i++)
        {
            _gameAreaPrefabDict.Add(_gameAreaPrefabArray[i].GetComponent<SpawningAreaManager>().areaType, _gameAreaPrefabArray[i]);
        }
    }

    private void HandleOnSpawnArea(SpawningAreaManager.GameAreaType p_areaType, Vector2 p_gridPos, SpawningAreaManager.DoorPosition p_lastDoor)
    {
        int __randomAreaIndex = Random.Range(1, SpawningAreaManager.GameAreaType.GetNames(typeof(SpawningAreaManager.GameAreaType)).Length - 1);
        InitializeArea((SpawningAreaManager.GameAreaType)__randomAreaIndex, p_gridPos, p_lastDoor);
    }

    private void InitializeArea(SpawningAreaManager.GameAreaType p_areaType, Vector2 p_gridPos,SpawningAreaManager.DoorPosition p_lastDoor)
    {
        SpawningAreaManager.DoorPosition __doorToOpen = SpawningAreaManager.DoorPosition.NONE;
        switch (p_lastDoor)
        {
            case SpawningAreaManager.DoorPosition.DOWN:
                __doorToOpen = SpawningAreaManager.DoorPosition.UP;
                break;
            case SpawningAreaManager.DoorPosition.UP:
                __doorToOpen = SpawningAreaManager.DoorPosition.DOWN;
                break;
            case SpawningAreaManager.DoorPosition.LEFT:
                __doorToOpen = SpawningAreaManager.DoorPosition.RIGHT;
                break;
            case SpawningAreaManager.DoorPosition.RIGHT:
                __doorToOpen = SpawningAreaManager.DoorPosition.LEFT;
                break;
        }

        SpawningAreaManager __areaAlreadyOpen = null;
        if (_gameAreaCols.TryGetValue(p_gridPos, out __areaAlreadyOpen))
        {
            __areaAlreadyOpen.J_OpenDoor(__doorToOpen);
            return;
        }

        SpawningAreaManager __newArea = Instantiate(_gameAreaPrefabDict[p_areaType], p_gridPos * 11.52f, Quaternion.identity, transform).GetComponent<SpawningAreaManager>();
        
        __newArea.onSpawn = _poolManager.JFunc_Spawn;
        __newArea.J_Start(p_gridPos, __doorToOpen);

        _gameAreaCols.Add(p_gridPos, __newArea);

    }

    public override void J_Update()
    {
        if (_gameOver == false)
        {
            _poolManager.J_Update();
            foreach (SpawningAreaManager __area in _gameAreaCols.Values)
            {
                __area.J_Update();            
            }        
        }
    }
}
