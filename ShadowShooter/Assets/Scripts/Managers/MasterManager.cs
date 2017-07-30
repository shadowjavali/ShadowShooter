using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterManager : MonoBehaviour {

    [SerializeField] CameraManager _cameramanager;
    [SerializeField] PoolManager _poolManager;
    [SerializeField] AO_TimerManager _timerManager;
    [SerializeField] AO_TweenManager _tweenManager;

    void Start ()
    {
        _poolManager.J_Start();
        _poolManager.J_Spawn(PoolManager.GameArea.CENTRAL_AREA, PoolManager.AssetType.PLAYER);
        _poolManager.J_Spawn(PoolManager.GameArea.CENTRAL_AREA, PoolManager.AssetType.ENEMY_1, 12);

    }    

	void Update ()
    {
        _poolManager.J_Update();
        _timerManager.AO_Update();
        _tweenManager.AO_Update();

    }
}
