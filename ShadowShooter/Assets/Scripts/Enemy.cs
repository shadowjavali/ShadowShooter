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
    public float attackRange = 5;

    private Player _playerTarget;
    private Generator _generatorTarget;
    private Transform _target;
    private float _attackCountdownTimer = 0f;
    private bool _canAttack = false;
    private SpawningAreaManager _currentGrid;

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
        //HandleSpriteLookRotation
        float __angleRad = Mathf.Atan2(_playerTarget.transform.position.y - transform.position.y, _playerTarget.transform.position.x - transform.position.x);
        float __angleDeg = (180 / Mathf.PI) * __angleRad;
        transform.eulerAngles = new Vector3(0f, 0f, __angleDeg+90);

        //HandleMovement
        if (Vector3.Distance(transform.position, _playerTarget.transform.position) <= attackRange)
        {
            _target = _playerTarget.transform;
            float __step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _target.position, __step);        
        }
        else
        {
            if (_currentGrid.GetGridPos().x <  0)
            {
                _target = _currentGrid._doorRight.transform;
            }
            else if (_currentGrid.GetGridPos().x > 0)
            {
                _target = _currentGrid._doorLeft.transform;
            }
            else if (_currentGrid.GetGridPos().y < 0)
            {
                _target = _currentGrid._doorDown.transform;
            }
            else if (_currentGrid.GetGridPos().y > 0)
            {
                _target = _currentGrid._doorUp.transform;
            }
            else
            {
                _target = _generatorTarget.transform;
            }
            
            float __step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _target.position, __step);
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
