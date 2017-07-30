using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawningAreaManager : MonoBehaviour
{
    public enum GameAreaType
    {
        CENTRAL_AREA = 0,
        AREA_TYPE_0,
        AREA_TYPE_2,
        AREA_TYPE_3,
        AREA_TYPE_4

    }

    public enum DoorPosition
    {
        NONE,
        DOWN,
        RIGHT,
        LEFT,
        UP
    }

    private Vector2 _gridPos;

    private Door _doorLeft;
    private Door _doorRight;
    private Door _doorUp;
    private Door _doorDown;


    public GameAreaType areaType;
    public Func<PoolManager.AssetType, Vector2, Transform, GameObject> onSpawn;

    [SerializeField] private Spawner[] _spawners;

    public void J_OpenDoor(DoorPosition p_doorToOpen)
    {
        switch (p_doorToOpen)
        {
            case DoorPosition.DOWN:
                _doorDown.J_OpenDoor();
                break;
            case DoorPosition.UP:
                _doorUp.J_OpenDoor();
                break;
            case DoorPosition.LEFT:
                _doorLeft.J_OpenDoor();
                break;
            case DoorPosition.RIGHT:
                _doorRight.J_OpenDoor();
                break;
        }
    }

    public void J_Start(Vector2 p_gridPos, DoorPosition p_doorToOpen)
    {
        _gridPos = p_gridPos;
        InitializeSpawners();
        SpawnNormalWalls();
        SpawnCornerWalls();
        SpawnDoors(p_doorToOpen);
    }

    private void SpawnCornerWalls()
    {
        GameObject __callCornerLowerLeft = onSpawn(PoolManager.AssetType.WALL_CORNER, new Vector2(transform.position.x, transform.position.y) + new Vector2(-4 * 1.28f, (-3.5f * 1.28f) - 0.64f), transform);
        GameObject __callCornerLowerRight = onSpawn(PoolManager.AssetType.WALL_CORNER, new Vector2(transform.position.x, transform.position.y) + new Vector2(4 * 1.28f, (-3.5f * 1.28f) - 0.64f), transform);
        GameObject __callCornerUpperRight = onSpawn(PoolManager.AssetType.WALL_CORNER, new Vector2(transform.position.x, transform.position.y) + new Vector2((3.5f * 1.28f) + 0.64f, 4 * 1.28f), transform);
        GameObject __callCornerUpperLeft = onSpawn(PoolManager.AssetType.WALL_CORNER, new Vector2(transform.position.x, transform.position.y) + new Vector2((-3.5f * 1.28f) - 0.64f, 4 * 1.28f), transform);

        __callCornerLowerLeft.GetComponent<Wall>().J_Start(new object[] { false, false });
        __callCornerLowerRight.GetComponent<Wall>().J_Start(new object[] { true, false });
        __callCornerUpperRight.GetComponent<Wall>().J_Start(new object[] { true, true });
        __callCornerUpperLeft.GetComponent<Wall>().J_Start(new object[] { false, true });
    }

    private void SpawnNormalWalls()
    {
        for (int row=-3; row < 4; row++)
        {
            if (row != 0)
                onSpawn(PoolManager.AssetType.WALL_NORMAL_H, new Vector2(transform.position.x, transform.position.y) + new Vector2(row * 1.28f, (-3.5f * 1.28f) - 0.64f), transform).GetComponent<Wall>().J_Start(new object[] { false, false });
            if (row != 0)
                onSpawn(PoolManager.AssetType.WALL_NORMAL_H, new Vector2(transform.position.x, transform.position.y) + new Vector2(row * 1.28f, (3.5f * 1.28f) + 0.64f), transform).GetComponent<Wall>().J_Start(new object[] { false, true }); //.flipY = true;
        }
        for (int col = -3; col < 4; col++)
        {
            if (col != 0)
                onSpawn(PoolManager.AssetType.WALL_NORMAL_V, new Vector2(transform.position.x, transform.position.y) + new Vector2((-3.5f * 1.28f ) - 0.64f, col * 1.28f), transform).GetComponent<Wall>().J_Start(new object[] { false, false });
            if (col != 0)
                onSpawn(PoolManager.AssetType.WALL_NORMAL_V, new Vector2(transform.position.x, transform.position.y) + new Vector2((3.5f * 1.28f) + 0.64f, col * 1.28f), transform).GetComponent<Wall>().J_Start(new object[] { true, false });//.flipX = true;
        }
    }

    private void SpawnDoors(DoorPosition p_doorToOpen)
    {

        _doorLeft = onSpawn(PoolManager.AssetType.DOOR_V, new Vector2(transform.position.x, transform.position.y) + new Vector2((-3.5f * 1.28f) - 0.64f, 0), transform).GetComponent<Door>();
        _doorRight = onSpawn(PoolManager.AssetType.DOOR_V, new Vector2(transform.position.x, transform.position.y) + new Vector2((3.5f * 1.28f) + 0.64f, 0), transform).GetComponent<Door>();

        _doorUp = onSpawn(PoolManager.AssetType.DOOR_H, new Vector2(transform.position.x, transform.position.y) + new Vector2(0, (3.5f * 1.28f) + 0.64f), transform).GetComponent<Door>();
        _doorDown = onSpawn(PoolManager.AssetType.DOOR_H, new Vector2(transform.position.x, transform.position.y) + new Vector2(0, (-3.5f * 1.28f) - 0.64f), transform).GetComponent<Door>();

        _doorLeft.J_Start(new object[] { false, false, new Vector2(_gridPos.x-1, _gridPos.y),DoorPosition.LEFT });
        _doorRight.J_Start(new object[] { true, false, new Vector2(_gridPos.x + 1, _gridPos.y), DoorPosition.RIGHT });

        _doorDown.J_Start(new object[] { false, true, new Vector2(_gridPos.x , _gridPos.y-1), DoorPosition.DOWN });
        _doorUp.J_Start(new object[] { false, false, new Vector2(_gridPos.x , _gridPos.y+1), DoorPosition.UP });

        switch (p_doorToOpen)
        {
            case DoorPosition.DOWN:
                _doorDown.J_OpenDoor();
                break;
            case DoorPosition.UP:
                _doorUp.J_OpenDoor();
                break;
            case DoorPosition.LEFT:
                _doorLeft.J_OpenDoor();
                break;
            case DoorPosition.RIGHT:
                _doorRight.J_OpenDoor();
                break;
        }

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
