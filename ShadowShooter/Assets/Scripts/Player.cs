using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LevelObject
{
    [Header("PlayerAttributes")]
    public float maxHealth = 10;
    public float moveSpeed = 0.75f;
    public float rateOfFire = 1;

    private float _shootCountdownTimer;

    void Update()
    {
    
        HandleInputs();
    }

    void HandleInputs()
    {
        HandleWalkingInputs();
        HandleShootingInputs();
    }

    void HandleWalkingInputs()
    {
        Vector2 __deltaPosition = Vector2.zero;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            __deltaPosition = new Vector2(__deltaPosition.x, moveSpeed);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            __deltaPosition = new Vector2(-moveSpeed, __deltaPosition.y);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            __deltaPosition = new Vector2(__deltaPosition.x, -moveSpeed);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            __deltaPosition = new Vector2(moveSpeed, __deltaPosition.y);
        }
        transform.position += new Vector3(__deltaPosition.x/10, __deltaPosition.y/10, 0);
    }

    void HandleShootingInputs()
    {
        Vector3 __mouseWorldPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 __direction = Input.mousePosition - __mouseWorldPos;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(__direction.y, __direction.x) * Mathf.Rad2Deg));

        bool __canShoot = (_shootCountdownTimer <= 0) ? true : false;
        _shootCountdownTimer -= Time.deltaTime;
        if (Input.GetMouseButton(0) && __canShoot)
        {
            _shootCountdownTimer = rateOfFire;
        }
    }
}
