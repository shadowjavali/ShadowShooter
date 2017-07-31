using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretArea : Spawner
{
    private bool _playerInside;
    protected Player _playerScript;

    private Turret _turret = null;

    public override void J_Start(SpawningAreaManager p_grid)
    {
        base.J_Start(p_grid);
        _empty = true;
        Debug.Log("Yeah");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            _playerInside = true;
            _playerScript = collision.transform.GetComponent<Player>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            _playerInside = false;
            _playerScript = null;
        }
    }

    public override void OnTriggerStay2D(Collider2D collision)
    {
       
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
                Debug.Log("Click " + _empty + "/" + _playerInside + "/" + _playerScript.HoldingTurretCrate());
                if (_empty && _playerInside && _playerScript.HoldingTurretCrate())
                {
                    Debug.Log("Turret");

                    _playerScript.ReleaseCrate();

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

}
