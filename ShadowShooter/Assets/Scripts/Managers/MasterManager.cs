using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterManager : MonoBehaviour {

    [SerializeField] CameraManager _cameramanager;
    [SerializeField] PoolManager _poolManager;

    void Start ()
    {
        _poolManager.J_Start();
        _poolManager.J_Spawn(PoolManager.GameArea.AREA_0, PoolManager.AssetType.PLAYER);

    }    

	void Update ()
    {
        _poolManager.J_Update();

    }
}
