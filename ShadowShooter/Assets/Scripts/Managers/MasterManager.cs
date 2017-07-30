using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterManager : MonoBehaviour
{

    [SerializeField] GameplayManager _gameplayManager;
    [SerializeField] AO_TimerManager _timerManager;
    [SerializeField] AO_TweenManager _tweenManager;

    void Start ()
    {
        _gameplayManager.J_Start();

    }    

	void Update ()
    {
        _gameplayManager.J_Update();
        _timerManager.AO_Update();
        _tweenManager.AO_Update();

    }
}
