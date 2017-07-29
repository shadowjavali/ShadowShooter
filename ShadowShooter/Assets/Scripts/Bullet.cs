using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : LevelObject 
{
    public float speed = 0.1f;

    private Vector3 shootDirection;

	void Start () 
    {
        float __x = Mathf.Rad2Deg * Mathf.Cos(transform.eulerAngles.z);
        float __y = Mathf.Rad2Deg * Mathf.Sin(transform.eulerAngles.z);
        shootDirection = new Vector3(__x, __y, 0f);
        Destroy(gameObject, 2f);
    }

    void Update()
    {

        Vector3 __newPosition = Time.deltaTime * speed * shootDirection;
        transform.position += __newPosition;
    }
}
