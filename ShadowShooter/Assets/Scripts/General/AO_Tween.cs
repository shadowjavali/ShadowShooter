using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AO_Tween  
{
	public enum TWEEN_MODE
	{
		LINEAR,
		QUADRATIC_IN,
		QUADRATIC_OUT,
		CUBIC_IN,
		CUBIC_OUT,
        CUBIC_IN_OUT
	}

	protected float _currentStepPercentage;

	private float _currentTime = 0;
	public float _duration;

	public Action onFinish;
	public Action onCancel;
	public Action onForceFinish;
	public Action onStop;
	
	public bool shouldUpdate = false;


	public void SetupTween(float p_duration, TWEEN_MODE p_mode)
	{

		_mode = p_mode;	
		_duration = p_duration;
		_currentStepPercentage = 0;
		InsertInManager();
		if (p_duration <= 0)
		{
			ForceFinish();
		}
		shouldUpdate = true;
	}

	protected TWEEN_MODE _mode;

	public virtual void AO_Update () 
	{	
		_currentTime += Time.deltaTime;

		if (_currentStepPercentage >= 0.99f)
		{
			Finish();			
		}
		else
		{	

			switch(_mode)
			{
				case TWEEN_MODE.QUADRATIC_IN:
				case TWEEN_MODE.QUADRATIC_OUT:
				{
					_currentStepPercentage = QuadOut(_currentTime,_duration);
		
				}
				break;	
				case TWEEN_MODE.CUBIC_IN:
				case TWEEN_MODE.CUBIC_OUT:
				{
					_currentStepPercentage = CubOut(_currentTime,_duration);
		
				}
				break;	
				case TWEEN_MODE.LINEAR:
				{
					_currentStepPercentage += (1/_duration) *Time.deltaTime;
				}	
				break;
                case TWEEN_MODE.CUBIC_IN_OUT:
                {

                    _currentStepPercentage += 1;
                }
                break;
			}		
			
			
		}
	}
	private float QuadOut(float p_currentTime, float p_duration)
	{
		p_currentTime/=p_duration;

		return -1*p_currentTime*(p_currentTime-2);
	}
	private float CubOut(float p_currentTime, float p_duration)
	{
		p_currentTime/=p_duration;
		p_currentTime--;

		return 1*(p_currentTime*p_currentTime*p_currentTime + 1);
	}
    private float CubInOut(float p_currentTime, float p_duration)
    {
        p_currentTime /= p_duration / 2;

        if (p_currentTime < 1)
            return 1 / 2 * p_currentTime * p_currentTime * p_currentTime;

        p_currentTime -= 2;
        return 1 / 2 * (p_currentTime * p_currentTime * p_currentTime + 2);        

    }

	protected void InsertInManager()
	{
		AO_TweenManager.instance.AO_AddTween(this);
	}
	public virtual void Stop()
	{
		shouldUpdate = false;
		if (onStop != null)
			onStop();
		AO_TweenManager.instance.AO_RemoveTween(this);
	}
	public virtual void ForceFinish()
	{	
		shouldUpdate = false;
		if (onFinish != null)
			onFinish();
		if (onForceFinish != null)
			onForceFinish();		
		AO_TweenManager.instance.AO_RemoveTween(this);
	}
	public virtual void Cancel()
	{
		shouldUpdate = false;
		if (onCancel != null)
			onCancel();
		AO_TweenManager.instance.AO_RemoveTween(this);
	}
	protected virtual void Finish()
	{
		shouldUpdate = false;
		if (onFinish != null)
			onFinish();
		AO_TweenManager.instance.AO_RemoveTween(this);
	}
}

public class AO_FloatTween: AO_Tween
{
	Action<float> _action;
	float _startingPosition;
	float _endPosition;

	public AO_FloatTween(float p_startingPosition, float p_endPosition, float p_duration, TWEEN_MODE p_mode, Action<float> p_delegateAction )
	{
		_action = p_delegateAction;
		_startingPosition = p_startingPosition;
		_endPosition = p_endPosition;
		SetupTween(p_duration,p_mode);		
	}
	public override void AO_Update()
	{
		_action((_startingPosition * (1-_currentStepPercentage)) + (_endPosition * _currentStepPercentage));
		base.AO_Update();			
	}
	public override void Stop ()
	{
		_action( ( _startingPosition * (1-_currentStepPercentage)) + (_endPosition * _currentStepPercentage));
		base.Stop ();		
	}
	public override void ForceFinish ()
	{
		_action( _endPosition);
		base.ForceFinish ();		
	}
	public override void Cancel ()
	{
		_action( _startingPosition);
		base.Cancel ();	
	}
	protected override void Finish ()
	{	
		_action( _endPosition);	
		base.Finish ();
		
	}
}

public class AO_Vec2Tween : AO_Tween
{
	Action<Vector2> _action;
	Vector2 _startingPosition;
	Vector2 _endPosition;

	public AO_Vec2Tween(Vector2 p_startingPosition, Vector2 p_endPosition, float p_duration, TWEEN_MODE p_mode, Action<Vector2> p_delegateAction )
	{
		_action = p_delegateAction;
		_startingPosition = p_startingPosition;
		_endPosition = p_endPosition;
		SetupTween(p_duration,p_mode);		
	}
	public override void AO_Update()
	{
		_action((_startingPosition * (1-_currentStepPercentage)) + (_endPosition * _currentStepPercentage));
		base.AO_Update();		
		
	}
	public override void Stop ()
	{
		_action( ( _startingPosition * (1-_currentStepPercentage)) + (_endPosition * _currentStepPercentage));
		base.Stop ();		
	}
	public override void ForceFinish ()
	{
		_action( _endPosition);
		base.ForceFinish ();		
	}
	public override void Cancel ()
	{
		_action( _startingPosition);
		base.Cancel ();	
	}
	protected override void Finish ()
	{	
		_action( _endPosition);	
		base.Finish ();
		
	}
}

public class AO_ColorTween : AO_Tween
{
	Action<Color> _action;
	Color _startingColor;
	Color _endColor;

	public AO_ColorTween(Color p_startingColor, Color p_endColor, float p_duration, TWEEN_MODE p_mode, Action<Color> p_delegateAction )
	{
		_action = p_delegateAction;
		_startingColor = p_startingColor;
		_endColor = p_endColor;
		SetupTween(p_duration,p_mode);	
	}
	public override void AO_Update()
	{
		_action( ( _startingColor * (1-_currentStepPercentage)) + (_endColor * _currentStepPercentage));
		base.AO_Update();	
	}
	public override void Stop ()
	{
		_action( ( _startingColor * (1-_currentStepPercentage)) + (_endColor * _currentStepPercentage));
		base.Stop ();		
	}
	public override void ForceFinish ()
	{
		_action( _endColor);
		base.ForceFinish ();		
	}
	public override void Cancel ()
	{
		_action( _startingColor);
		base.Cancel ();
	
	}
	protected override void Finish ()
	{	
		_action( _endColor);	
		base.Finish ();
		
	}
}