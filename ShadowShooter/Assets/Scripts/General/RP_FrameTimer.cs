using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RP_FrameTimer: AO_Timer
{

    public int _frameDelay;
    protected bool _running = false;

    Action _action;
    protected void InsertInManager()
    {
        AO_TimerManager.instance.AO_AddTimer(this);
    }
    public RP_FrameTimer(int p_delay, Action p_delegateAction)
    {
        _action = p_delegateAction;
        _frameDelay = p_delay;
        _running = true;        
        InsertInManager();
    }
   
    public override void AO_Update()
    {
        if (_running)
        {
            if (_frameDelay == 0)
            {
                _action();
                AO_TimerManager.instance.AO_RemoveTimer(this);
                _running = false;
            }
            else
            {
                _frameDelay--;
            }
        }
    }

}
