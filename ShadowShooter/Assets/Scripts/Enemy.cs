using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LevelObject
{
    [Header("Enemy Attributes")]
    public float maxHealth = 10f;
    public float speed = 1f;
    public float attackRate = 1f;
    public float damage = 1;
    public float attackRange = 0f;

    private Player _playerTarget;
    private Generator _generatorTarget;
    [SerializeField] private Vector2 _target;
    private float _attackCountdownTimer = 0f;
    private bool _canAttack = false;
    private SpawningAreaManager _currentGrid;
    private Vector2 _nextGridAfterDoorPos;

    public enum State
    {
        GOING_TO_DOOR,
        GOING_TO_NEXT_GRID,
        GOING_TO_PLAYER,
        GOING_TO_GENERATOR
    }

    State _currentState;

    public override void J_Start(params object[] p_args)
    {
        _currentGrid = (SpawningAreaManager) p_args[0];

      
        _playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _generatorTarget = GameObject.FindGameObjectWithTag("Generator").GetComponent<Generator>();
    }

    public void SetCurrentGrid(SpawningAreaManager p_grid)
    {
        _currentGrid = p_grid;
    }

    public override void J_Update()
    {


        if ((Vector3.Distance(transform.position, _playerTarget.transform.position) <= attackRange) && (_currentGrid == _playerTarget.GetCurrentGrid()))
        {
            _currentState = State.GOING_TO_PLAYER;
        }
        else
        {
            if (_currentState != State.GOING_TO_NEXT_GRID)
            {
                if ((_currentGrid.GetGridPos().x != 0) || (_currentGrid.GetGridPos().y != 0))
                {
                    _currentState = State.GOING_TO_DOOR;
                }
                else
                {
                    _currentState = State.GOING_TO_GENERATOR;
                }
            }
        }
          
        switch(_currentState)
        {
            case State.GOING_TO_PLAYER:
            {
                float __angleRad = Mathf.Atan2(_playerTarget.transform.position.y - transform.position.y, _playerTarget.transform.position.x - transform.position.x);
                float __angleDeg = (180 / Mathf.PI) * __angleRad;
                transform.eulerAngles = new Vector3(0f, 0f, __angleDeg + 90);

                _target = _playerTarget.transform.position;
                float __step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _target, __step);
            }
            break;
            case State.GOING_TO_DOOR:
            {
                if (_currentGrid.GetGridPos().x < 0)
                {
                    _target = new Vector2(_currentGrid._doorRight.transform.position.x - 1.28f, _currentGrid._doorRight.transform.position.y);
                    _nextGridAfterDoorPos = new Vector2(_currentGrid._doorRight.transform.position.x + 1.28f, _currentGrid._doorRight.transform.position.y);
                }
                else if (_currentGrid.GetGridPos().x > 0)
                {
                    _target = new Vector2(_currentGrid._doorLeft.transform.position.x + 1.28f, _currentGrid._doorLeft.transform.position.y);
                    _nextGridAfterDoorPos = new Vector2(_currentGrid._doorLeft.transform.position.x - 1.28f, _currentGrid._doorLeft.transform.position.y);
                }
                else if (_currentGrid.GetGridPos().y < 0)
                {
                    _target = new Vector2(_currentGrid._doorUp.transform.position.x, _currentGrid._doorUp.transform.position.y - 1.28f);
                     _nextGridAfterDoorPos = new Vector2(_currentGrid._doorUp.transform.position.x,  _currentGrid._doorUp.transform.position.y + 1.28f);
                    }
                else if (_currentGrid.GetGridPos().y > 0)
                {
                    _target = new Vector2(_currentGrid._doorDown.transform.position.x, _currentGrid._doorDown.transform.position.y + 1.28f);
                    _nextGridAfterDoorPos = new Vector2(_currentGrid._doorDown.transform.position.x, _currentGrid._doorDown.transform.position.y - 1.28f);
                }


                if (Vector3.Distance(transform.position, _target) <= 0.1f)
                {
                     _currentState = State.GOING_TO_NEXT_GRID;
                }
                else
                {
                    float __angleRad = Mathf.Atan2(_target.y - transform.position.y, _target.x - transform.position.x);
                    float __angleDeg = (180 / Mathf.PI) * __angleRad;
                    transform.eulerAngles = new Vector3(0f, 0f, __angleDeg + 90);

                    float __step = speed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, _target, __step);
                }
            }
            break;
            case State.GOING_TO_NEXT_GRID:
            {
                 _target = _nextGridAfterDoorPos;              

                if (Vector3.Distance(transform.position, _target) <= 0.1f)
                {
                    _currentState = State.GOING_TO_GENERATOR;
                }
                else
                {
                    float __angleRad = Mathf.Atan2(_target.y - transform.position.y, _target.x - transform.position.x);
                    float __angleDeg = (180 / Mathf.PI) * __angleRad;
                    transform.eulerAngles = new Vector3(0f, 0f, __angleDeg + 90);

                    float __step = speed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, _target, __step);
                }
            }
            break;
            case State.GOING_TO_GENERATOR:
            {
                _target = _generatorTarget.transform.position;

                float __angleRad = Mathf.Atan2(_target.y - transform.position.y, _target.x - transform.position.x);
                float __angleDeg = (180 / Mathf.PI) * __angleRad;
                transform.eulerAngles = new Vector3(0f, 0f, __angleDeg + 90);

                float __step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _target, __step);
            }
            break;

        }

                
               
           
           
        UpdateAttackCountdown();
    }

    void UpdateAttackCountdown()
    {
        _attackCountdownTimer -= Time.deltaTime;
        if (_attackCountdownTimer <= 0 && _canAttack == false)
        {
            _canAttack = true;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //if (_active)
        //{
            if (collision.transform.tag == "Player")
            {
                if (_canAttack == true)
                {
                    _canAttack = false;
                    _attackCountdownTimer = attackRate;
                    collision.gameObject.GetComponent<Player>().InflictDamage(damage);
                }
            }
       //}
    }
}
