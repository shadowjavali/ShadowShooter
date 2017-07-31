using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ScreenCanvas : MonoBehaviour
{
    private static ScreenCanvas _instance;
    public static ScreenCanvas instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject playScreen;
    public GameObject gameOverScreen;

    public Button restartButton;
    public Action onRestartButtonClick;

    public Image arrow;
    public Slider healthBar;
    public Slider baseEnergyBar;

    public int enemiesKilled = 0;
    public int createdFounded = 0;

    private float _playTime = 0;
    private int _screenIndex = 0;

    public Text enemiesText;
    public Text cratesText;
    public Text timeText;

    void Start()
    {
        _instance = this;
        ChangeScreen(0);
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(delegate
        {
            if (onRestartButtonClick != null)
                onRestartButtonClick();
        });
    }

    void Update()
    {
        if (_screenIndex == 0)
        {
            _playTime += Time.deltaTime;
            UpdateArrowRotation();
        }
        else if (_screenIndex == 1)
        {
        
        }
    }

    void ChangeScreen(int p_screenIndex)
    {
        _screenIndex = p_screenIndex;
        if (_screenIndex == 0)
        {
            playScreen.SetActive(true);
            gameOverScreen.SetActive(false);
        }
        else if (_screenIndex == 1)
        {
            playScreen.SetActive(false);
            gameOverScreen.SetActive(false);
            enemiesText.text = "Enemies Killed: " + enemiesKilled;
            cratesText.text = "Crates Found: " + createdFounded;
            timeText.text = "Play Time: " + _playTime + "s";
        }
    }

    void UpdateArrowRotation()
    {
        if (Camera.main == null)
            return;
        Vector3 __currentWorldPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width/2, Screen.height/2));
        float __angleRad = Mathf.Atan2(__currentWorldPos.y, __currentWorldPos.x);
        float __angleDeg = (180 / Mathf.PI) * __angleRad;

        arrow.GetComponent<RectTransform>().eulerAngles = new Vector3(0f, 0f, __angleDeg+180);
    }

    public void SetHealthBarPercentage(float p_percentage)
    {
        p_percentage = Mathf.Clamp01(p_percentage);
        healthBar.value = p_percentage;
    }

    public void SetBaseEnergyBarPercentage(float p_percentage)
    {
        p_percentage = Mathf.Clamp01(p_percentage);
        baseEnergyBar.value = p_percentage;
    }
}
