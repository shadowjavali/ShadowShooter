using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LevelObject
{
    [Header("Enemy Attributes")]
    public float maxHealth = 10f;
    public float speed = 1f;

    private Player _target;

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
    }
}
