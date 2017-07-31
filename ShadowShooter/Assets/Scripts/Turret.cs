using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : LevelObject
{
    private bool _chambered = true;
    private bool _loaded;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _barrelEnd;

    [SerializeField] private int _maxAmmo = 3;
    [SerializeField] private float _reloadTime = 1;

    private int _currentAmmo;

    private TurretStates _currentState;

    AO_Timer _reloadTimer;

    private List<Transform> _targets = new List<Transform>();

    public enum TurretStates
    {
        IDLE,
        RELOADING,
        SHOOTING
    }    

    private void Update()
    {
        J_Update();
    }

    public override void J_Start(params object[] p_args)
    {
        base.J_Start(p_args);
        _loaded = true;
        _currentAmmo = _maxAmmo;
        _currentState = TurretStates.IDLE;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.transform.tag);

        if (collision.transform.tag == "Enemy")
        {
            _targets.Add(collision.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            _targets.Remove(collision.transform);
        }
    }

    public override void J_Update()
    {
        base.J_Update();

        switch(_currentState)
        {
            case TurretStates.RELOADING:
            {
                if (_targets.Count > 0)
                {
                    float __angleRad = Mathf.Atan2(_targets[0].transform.position.y - transform.position.y, _targets[0].transform.position.x - transform.position.x);
                    float __angleDeg = (180 / Mathf.PI) * __angleRad;
                    transform.eulerAngles = new Vector3(0f, 0f, __angleDeg + 180);
                }              
            }
            break;
            case TurretStates.IDLE:
            {
                if (_targets.Count > 0)
                {
                    _currentState = TurretStates.SHOOTING;
                }
                else
                {
                    if (_currentAmmo < _maxAmmo)
                    {
                        Reload();
                    }
                }
            }
            break;
            case TurretStates.SHOOTING:
            {

                if (_targets.Count <= 0)
                {
                    _currentState = TurretStates.IDLE;
                    return;
                }

                float __angleRad = Mathf.Atan2(_targets[0].transform.position.y - transform.position.y, _targets[0].transform.position.x - transform.position.x);
                float __angleDeg = (180 / Mathf.PI) * __angleRad;
                transform.eulerAngles = new Vector3(0f, 0f, __angleDeg +180);

                if (_currentAmmo <= 0)
                {
                    Reload();
                }
                else
                {
                    if (_chambered)
                    {
                        Shoot();
                    }
                }
              
            }
            break;
        }

        
    }
    private void Reload()
    {
        _currentState = TurretStates.RELOADING;
        if (_reloadTimer != null)
            _reloadTimer.Cancel();

        _reloadTimer = new AO_Timer(_reloadTime, delegate
        {
            _currentAmmo = _maxAmmo;
            _currentState = TurretStates.IDLE;
        });
    }

    private void Shoot()
    {
        _currentAmmo--;
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
