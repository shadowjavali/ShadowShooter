using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Deployment.Internal;

public class Generator : MonoBehaviour 
{

	[SerializeField] private GameObject _lightSource;
	[SerializeField] private float _speed = 10;
	[SerializeField] private float _durationFade = 1;

	[SerializeField] private SpriteRenderer[] _spriteRenderer; 

	AO_Tween _tween = null;
	// Use this for initialization
	void Start () 
	{
		
		LightUp ();

	}
	private void LightUp()
	{
		_tween = new AO_FloatTween (0.25f, 0.75f, _durationFade, AO_Tween.TWEEN_MODE.LINEAR, delegate (float p_value) 
		{
			for(int i = 0; i < _spriteRenderer.Length; i++)
			{
				_spriteRenderer[i].color = new Color(_spriteRenderer[i].color.r, _spriteRenderer[i].color.g, _spriteRenderer[i].color.b, p_value);
			}
		});
				
		_tween.onFinish = LightDown;
	}
	private void LightDown()
	{
		_tween = new AO_FloatTween (0.75f, 0.25f, _durationFade, AO_Tween.TWEEN_MODE.LINEAR, delegate (float p_value) 
		{
			for(int i = 0; i < _spriteRenderer.Length; i++)
			{
				_spriteRenderer[i].color = new Color(_spriteRenderer[i].color.r, _spriteRenderer[i].color.g, _spriteRenderer[i].color.b, p_value);
			}
		});
		_tween.onFinish = LightUp;
	}

	// Update is called once per frame
	void Update () 
	{
		_lightSource.transform.Rotate (0,0,_speed * Time.deltaTime);
	}
}
