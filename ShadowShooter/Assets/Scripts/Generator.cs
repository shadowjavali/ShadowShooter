using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Deployment.Internal;

public class Generator : MonoBehaviour 
{

	[SerializeField] private GameObject _lightSource;

    [Header("Generator Attributes")]
    [SerializeField] private float _startEnergy = 20;
	[SerializeField] private float _speed = 10;
	[SerializeField] private float _durationFade = 1;

	[SerializeField] private SpriteRenderer[] _spriteRenderer;

    private float _currentEnergy;
    private bool _gameOver = false;

    protected bool _playerInside;
    protected Player _playerScript;

    AO_Tween _tween = null;
	// Use this for initialization
	void Start () 
	{
        _gameOver = false;
		LightUp ();
        _currentEnergy = _startEnergy;
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
        if (_gameOver)
            return;
        UpdateGeneratorAnimation();
        UpdateEnergyCountdown();

        if (_playerInside)
        {
            if (Input.GetKeyDown(KeyCode.E) && _playerScript.HoldingEnergyCrate())
            {
                _playerScript.ReleaseCrate();
                DeliverCrate();
            }
        }
    }

    void UpdateGeneratorAnimation()
    {
        _lightSource.transform.Rotate(0, 0, _speed * Time.deltaTime);
    }

    void UpdateEnergyCountdown()
    {
        _currentEnergy -= Time.deltaTime;
        ScreenCanvas.instance.SetCurrentEnergy(GetCurrentEnergy());
        CheckGameOver();
    }

    public float GetCurrentEnergy()
    {
        return _currentEnergy;
    }

    public void DeliverCrate()
    {       
        _currentEnergy += 30;
        if (_currentEnergy > 100)
            _currentEnergy = 100;
    }

    public void AddEnergy(float p_amount)
    {
        _currentEnergy += p_amount;
        CheckGameOver();
    }

    void CheckGameOver()
    {
        if (_currentEnergy <= 0)
        {
            _currentEnergy = 0;
            _gameOver = true;
            ScreenCanvas.instance.SetCurrentEnergy(GetCurrentEnergy());
            ScreenCanvas.instance.ChangeScreen(1);
        }
    }
}
