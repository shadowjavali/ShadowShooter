using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : LevelObject
{
    protected bool _playerInside;
    protected Player _playerScript;

    public override void J_Start(params object[] p_args)
    {
        base.J_Start(p_args);

        _playerInside = false;

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

    public override void J_Update()
    {
        base.J_Update();

        if (_playerInside)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_playerScript.LoadCrate(this))
                    Despawn();
            }
        }
    }
}
