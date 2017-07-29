using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
=== Acerola Orion Visual Novel Engine ===
--- Matheus Girotto ---
*/
public class AO_Timer 
{
	protected float _delay;
	protected float _startTime;
	protected float _currentStepPercentage;
	protected bool _running = false;

	Action _action;
	protected void InsertInManager()
	{
		AO_TimerManager.instance.AO_AddTimer(this);
	}

    public AO_Timer()
    {


    }

	public AO_Timer(float p_delay, Action p_delegateAction )
	{
		_action = p_delegateAction;	
		_delay = p_delay;
		_running = true;
		_startTime = Time.time;
		InsertInManager();
	}

    public void Cancel()
	{
		AO_TimerManager.instance.AO_RemoveTimer(this);
        _action = null;
        _running = false;
	}

	void PreExecuteAction ()
	{
		_action();
		AO_TimerManager.instance.AO_RemoveTimer(this);
	}
	public virtual void AO_Update () 
	{
		if (_running)
		{
			if (Time.time >= _startTime + _delay)
			{
				_action();
				AO_TimerManager.instance.AO_RemoveTimer(this);
				_running = false;
			}
			else
			if(Time.time + Time.maximumDeltaTime >= _startTime + _delay)
			{
				float __difference = -(Time.time - (_startTime + _delay));					
				PreExecuteAction();		
				_running = false;
			}
		}
	}
	
}
