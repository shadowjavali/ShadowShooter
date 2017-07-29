using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
=== Acerola Orion Visual Novel Engine ===
--- Matheus Girotto ---
*/

public class AO_TimerManager : MonoBehaviour
{
	public static AO_TimerManager instance;
	private List<AO_Timer> _timers = new List<AO_Timer>();
	private List<AO_Timer> _timersToRemove = new List<AO_Timer>();
	private Action InvokedAction;

	void Awake () 
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this);
		}
	}
	public void AO_Update () 
	{
		RemoveTimers();
		for (int i=0; i<_timers.Count; i++)
		{
			_timers[i].AO_Update();
		}	
	}
	public void AO_AddTimer(AO_Timer p_timer)
	{
		_timers.Add(p_timer);
	}
	public void AO_RemoveTimer(AO_Timer p_timer)
	{
		if (!_timersToRemove.Contains(p_timer))
		{
			_timersToRemove.Add(p_timer);
		}
	}
	private void RemoveTimers()
	{
		for (int i=0; i<_timersToRemove.Count; i++)
		{
			_timers.Remove(_timersToRemove[i]);
		}
		_timersToRemove.Clear();
	}
	public void AO_Invoke(Action p_action,float p_delay)
	{
		InvokedAction = p_action;
		Invoke("InvokeAction",p_delay);
	}
	public void InvokeAction()
	{
		InvokedAction();
	}
}
