using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : LevelObject 
{
    public float speed;

    private Vector3 shootDirection;

	public override void J_Start () 
    {
        float __x =  Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.z);
        float __y =  Mathf.Sin(Mathf.Deg2Rad *  transform.eulerAngles.z);
        shootDirection = new Vector3(__x, __y, 0f);

        AO_Timer __timer = new AO_Timer(2, delegate ()
         {
             Despawn();
         });
    }

    public override void J_Update()
    {
       
        Vector3 __newPosition = Time.deltaTime * speed * shootDirection;
        transform.position += __newPosition;
    }
}
