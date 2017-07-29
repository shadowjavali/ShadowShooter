﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : LevelObject 
{
    public float speed;

    private Vector3 shootDirection;

    AO_Timer _timer = null;


    public override void J_Start () 
    {
        float __x =  Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.z);
        float __y =  Mathf.Sin(Mathf.Deg2Rad *  transform.eulerAngles.z);
        shootDirection = new Vector3(__x, __y, 0f);

        _timer = new AO_Timer(2, delegate ()
         {
             Despawn();
         });
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            collision.transform.GetComponent<Enemy>().Despawn();
            if (_timer != null)
                _timer.Cancel();
            Despawn();
        }
    }

    public override void J_Update()
    {
       
        Vector3 __newPosition = Time.deltaTime * speed * shootDirection;
        transform.position += __newPosition;
    }
}
