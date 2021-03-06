﻿using System;
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
    private CameraManager _cameraManager;

    bool _holdingFirstCrate = false;

    TurretCrate _startingCrate = new TurretCrate();

    [SerializeField] private SpawningAreaManager _currentGrid;

    private float _shootCountdownTimer;
    private float _currentHealth;

    private Crate _crateHolding = null;

    public bool HoldingEnergyCrate()
    {
        if (_crateHolding is EnergyCrate)
        {
            return true;
        }
        return false;
    }

    public bool HoldingTurretCrate()
    {
        if (_crateHolding is TurretCrate)
        {
            return true;
        }
        return false;
    }

    public void ReleaseCrate()
    {
        _crateHolding = null;
        ScreenCanvas.instance.SetCurrentCrate(ScreenCanvas.CrateType.NONE);
        ScreenCanvas.instance.UpdateCratesText();
        _holdingFirstCrate = false;
    }

    public bool LoadCrate(Crate p_crate)
    {
        if (_holdingFirstCrate)
            return false;

        if (_crateHolding == null)
        {
            _crateHolding = p_crate;

            if (_crateHolding is TurretCrate)          
                ScreenCanvas.instance.SetCurrentCrate(ScreenCanvas.CrateType.TURRET);
            else if (_crateHolding is EnergyCrate)
                ScreenCanvas.instance.SetCurrentCrate(ScreenCanvas.CrateType.ENERGY);

            ScreenCanvas.instance.UpdateCratesText();
            return true;
        }

        return false;
    }

    public void SetCurrentGrid(SpawningAreaManager p_grid)
    {
        _currentGrid = p_grid;
    }

    public SpawningAreaManager GetCurrentGrid()
    {
        return _currentGrid;
    }

    public override void J_Start(params object[] p_args)
    {
        base.J_Start();
        _cameraManager = onSpawnFreeObject(PoolManager.AssetType.CAMERAMANAGER, transform.position).GetComponent<CameraManager>();
        _cameraManager.SetPlayerToFollow(transform);
        _currentHealth = maxHealth;
        _currentGrid = (SpawningAreaManager)p_args[0];
        ScreenCanvas.instance.SetHealthBarPercentage(GetHealthPercentage());
        LoadCrate(_startingCrate);
        _holdingFirstCrate = true;
    }

    public override void J_Update()
    {
        base.J_Update();
        HandleInputs();        
        _cameraManager.J_Update();
    }

    public float GetHealthPercentage()
    {
        return _currentHealth / maxHealth;
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
            __deltaPosition += Vector2.up * moveSpeed;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            __deltaPosition += Vector2.left * moveSpeed;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            __deltaPosition += Vector2.down * moveSpeed;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            __deltaPosition += Vector2.right * moveSpeed;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, __deltaPosition, Mathf.Infinity, LayerMask.NameToLayer("LevelObject"));
        Debug.DrawRay(transform.position, __deltaPosition, Color.red);

        transform.localPosition += new Vector3(__deltaPosition.x / 10, __deltaPosition.y / 10, 0);
    }

    void HandleShootingInputs()
    {
        Vector3 __mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float __angleRad = Mathf.Atan2(__mouseWorldPos.y - transform.position.y, __mouseWorldPos.x - transform.position.x);
        float __angleDeg = (180 / Mathf.PI) * __angleRad;

        transform.eulerAngles = new Vector3(0f, 0f, __angleDeg);


        bool __canShoot = (_shootCountdownTimer <= 0) ? true : false;

        _shootCountdownTimer -= Time.deltaTime;

        if (Input.GetMouseButton(0) && __canShoot)
        {
            _shootCountdownTimer = 1 / rateOfFire;
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
            __bullet.gameObject.name = "Bullet_Player";
        }
    }

    public void InflictDamage(float p_damage)
    {
        _currentHealth -= p_damage;
        if (_currentHealth <= 0)
        {               
            ScreenCanvas.instance.ChangeScreen(1);    
        }
        ScreenCanvas.instance.SetHealthBarPercentage(GetHealthPercentage());
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        //if (_active)
        //{
        if (collision.transform.tag == "Generator")
        {
            collision.gameObject.GetComponent<Generator>().DeliverCrates(_dictCratesAmount);
            ScreenCanvas.instance.UpdateCratesText(_dictCratesAmount);
        }

        if (collision.transform.tag == "EnergyCrate")
        {
            _dictCratesAmount[collision.gameObject.GetComponent<EnergyCrate>().type]++;

            Destroy(collision.gameObject);
            ScreenCanvas.instance.UpdateCratesText(_dictCratesAmount);
        }
        //}
        */
    }
}
