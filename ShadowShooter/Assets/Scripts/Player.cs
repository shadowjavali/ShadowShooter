using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : LevelObject
{
    public Transform shootStartTransform;

    [Header("PlayerAttributes")]
    public float maxHealth = 10;
    public float moveSpeed = 0.75f;
    public float rateOfFire = 1;

    private Vector3 __lastMousePosition;

    private float _shootCountdownTimer;

    public override void J_Update()
    {
        base.J_Update();  
        HandleInputs();
    }

    void HandleInputs()
    {
        HandleWalkingInputs();
        HandleShootingInputs();
    }

    void HandleWalkingInputs()
    {
        float __x = Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.z);
        float __y = Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.z);

        Vector2 __forwardAxis = new Vector2(__x, __y);

        __x = Mathf.Cos(Mathf.Deg2Rad * (transform.eulerAngles.z + 90));
        __y = Mathf.Sin(Mathf.Deg2Rad * (transform.eulerAngles.z + 90));

        Vector2 __lateralAxis = new Vector2(__x, __y);

        Vector2 __deltaPosition = Vector2.zero;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            __deltaPosition += moveSpeed * __forwardAxis;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            __deltaPosition -= moveSpeed * __lateralAxis;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            __deltaPosition -= moveSpeed * __forwardAxis;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            __deltaPosition += moveSpeed * __lateralAxis;
        }

       

        transform.localPosition += new Vector3(__deltaPosition.x/10, __deltaPosition.y/10, 0);
    }

    void HandleShootingInputs()
    {
        Vector3 __mouseWorldPos = Camera.main.WorldToScreenPoint(Vector3.zero);      

        if (__lastMousePosition != Input.mousePosition)
        {
            __lastMousePosition = Input.mousePosition;
         

        }

        Vector3 __direction = __lastMousePosition - __mouseWorldPos;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(__direction.y, __direction.x) * Mathf.Rad2Deg));


        bool __canShoot = false;
        if (_shootCountdownTimer <= 0)
            __canShoot = true;

        _shootCountdownTimer -= Time.deltaTime;
        if (Input.GetMouseButton(0) && __canShoot)
        {
            _shootCountdownTimer = 1/rateOfFire;
            Shoot();
        }
    }

    void Shoot()
    {
        if (onSpawnFreeObject != null)
        {
            LevelObject __bullet = onSpawnFreeObject(PoolManager.AssetType.BULLET, shootStartTransform.position).GetComponent<LevelObject>();
            __bullet.transform.eulerAngles = shootStartTransform.eulerAngles;
            __bullet.J_Start();
        }
    }
}
