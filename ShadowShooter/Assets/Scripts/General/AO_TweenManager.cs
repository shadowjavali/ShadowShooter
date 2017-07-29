using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AO_TweenManager : MonoBehaviour
{

	public static AO_TweenManager instance;
	private List<AO_Tween> _tweens= new List<AO_Tween>();
	private List<AO_Tween> _tweensToRemove = new List<AO_Tween>();

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
		RemoveTweens();
		for (int i=0; i<_tweens.Count; i++)
		{
			_tweens[i].AO_Update();
		}		
	}
	public void AO_AddTween(AO_Tween p_tween)
	{
		_tweens.Add(p_tween);
	}
	public void AO_RemoveTween(AO_Tween p_tween)
	{
		if (!_tweensToRemove.Contains(p_tween))
		{
			_tweensToRemove.Add(p_tween);
		}
	}
	private void RemoveTweens()
	{
		for (int i=0; i<_tweensToRemove.Count; i++)
		{
			_tweens.Remove(_tweensToRemove[i]);
		}
		_tweensToRemove.Clear();
	}
}
