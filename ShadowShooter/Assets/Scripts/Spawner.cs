using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawner : MonoBehaviour
{
    private List<LevelObject> _levelObjects =  new List<LevelObject>();

    [SerializeField] private int _queue = 0;

    public PoolManager.AssetType type;
    public Func<PoolManager.AssetType, Vector2, GameObject> onSpawn;
    RP_FrameTimer _setEmptyTimer = null;


   [SerializeField] private bool _empty = true;

    public bool Spawn(int p_queueMax)
    {
        if (_empty)
        {
            LevelObject __levelObject = onSpawn(type, transform.position).GetComponent<LevelObject>();
            __levelObject.J_Start();
            _levelObjects.Add(__levelObject);

            _empty = false;
            return true;
        }
        else
        {
            if (_queue < p_queueMax)
            {
                _queue++;
                return true;
            }
        }
        return false;
    }

    public virtual void J_Start()
    {
        _empty = true;
    }

    public virtual void J_Update()
    {
        for (int i=0; i< _levelObjects.Count;i++)
        {
            _levelObjects[i].J_Update();
        }

        if (_queue > 0)
        {
            if (Spawn(_queue))
                _queue--;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {      
        _empty = false;
        if (_setEmptyTimer != null)
            _setEmptyTimer.Cancel();

        _setEmptyTimer = new RP_FrameTimer(1, delegate
        {
            _empty = true;
        });
    }

}
