using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretArea : Spawner
{
    private bool _playerInside;

    private Turret _turret = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            _playerInside = true;
        }
    }

    public override void J_Update()
    {
        base.J_Update();
        if (_turret != null)
            _turret.J_Update();

        if (_playerInside)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_empty)
                {
                    _turret = onSpawn(type, transform.position).GetComponent<Turret>();                               
                    _turret.J_Start();

                    _turret.onDespawn += delegate (PoolManager.AssetType p_type, GameObject p_object)
                    {
                        _turret = null;
                    };

                    _empty = false;
                   
                }
                
            }
        }
    }

    public override void OnTriggerStay2D(Collider2D collision)
    {
       
    }
}
