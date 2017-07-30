using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : LevelObject
{
    private bool _chambered = true;
    [SerializeField] private Animator _animator;
    [SerializeField]
    private Transform _barrelEnd;

    private void Update()
    {
        J_Update();
    }

    public override void J_Update()
    {
        base.J_Update();
        if (_chambered)
        {
            Shoot();
        }
    }
    private void Shoot()
    {
        _chambered = false;
        _animator.SetTrigger("Shoot");
        if (onSpawnFreeObject != null)
        {
            LevelObject __bullet = onSpawnFreeObject(PoolManager.AssetType.BULLET, _barrelEnd.transform.position).GetComponent<LevelObject>();
            __bullet.transform.eulerAngles = _barrelEnd.eulerAngles;
            __bullet.J_Start();
            __bullet.gameObject.name = "Bullet_Turret";

        }

       
    }

    public enum TurretAnimationEvents
    {
        SHOOTING_CICLE_ENDED
    }

    public void AnimationCalledEvent(TurretAnimationEvents p_event)
    {
        switch(p_event)
        {
            case TurretAnimationEvents.SHOOTING_CICLE_ENDED:
                _chambered = true;
            break;
        }
    }
	
}
