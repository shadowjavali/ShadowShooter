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

    public Image arrow;
    public Slider healthBar;
    public Text energyText;

    public int enemiesKilled = 0;
    public int createdFounded = 0;

    private float _playTime = 0;
    private int _screenIndex = 0;

    public Text enemiesText;
    public Text cratesText;
    public Text timeText;

    private int purpleCratesCount;
    private int redCratesCount;
    private int greenCratesCount;

    public Text purpleText;
    public Text redText;
    public Text greenText;

    void Start()
    {
        _instance = this;
        ChangeScreen(0);
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

    public void ChangeScreen(int p_screenIndex)
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
            gameOverScreen.SetActive(true);
            enemiesText.text = "Enemies Killed: " + enemiesKilled;
            cratesText.text = "Crates Found: " + createdFounded;
            timeText.text = "Play Time: " + _playTime + "s";
        }
    }

    public void UpdateCratesText(Dictionary<EnergyType, int> p_dictCratesAmount)
    {
        if (p_dictCratesAmount.ContainsKey(EnergyType.GREEN))
        {
            greenCratesCount = p_dictCratesAmount[EnergyType.GREEN];
            greenText.text = p_dictCratesAmount[EnergyType.GREEN].ToString();
        }
        else
        {
            greenText.text = "0";
        }

        if (p_dictCratesAmount.ContainsKey(EnergyType.RED))
        {
            redCratesCount = p_dictCratesAmount[EnergyType.RED];
            redText.text = p_dictCratesAmount[EnergyType.RED].ToString();
        }
        else
        {
            redText.text = "0";
        }

        if (p_dictCratesAmount.ContainsKey(EnergyType.PURPLE))
        {
            purpleCratesCount = p_dictCratesAmount[EnergyType.PURPLE];
            purpleText.text = p_dictCratesAmount[EnergyType.PURPLE].ToString();
        }
        else
        {
            purpleText.text = "0";
        }
    }


    void UpdateArrowRotation()
    {
        if (Camera.main == null)
            return;

        if (purpleCratesCount > 0 || greenCratesCount > 0 || redCratesCount > 0)
        {
            arrow.gameObject.SetActive(true);
        }
        else
        {
            arrow.gameObject.SetActive(false);
            return;
        }

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

    public void SetCurrentEnergy(float p_value)
    {
        energyText.text = p_value.ToString();
    }
}
