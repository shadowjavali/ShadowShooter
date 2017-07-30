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

    private Player _target;
    private float _attackCountdownTimer = 0f;
    private bool _canAttack = false;

    public override void J_Start(params object[] p_args)
    {
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public override void J_Update()
    {
        //HandleSpriteLookRotation
        float __angleRad = Mathf.Atan2(_target.transform.position.y - transform.position.y, _target.transform.position.x - transform.position.x);
        float __angleDeg = (180 / Mathf.PI) * __angleRad;
        transform.eulerAngles = new Vector3(0f, 0f, __angleDeg+90);

        //HandleMovement
        float __step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, __step);

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
       // }
    }
}
