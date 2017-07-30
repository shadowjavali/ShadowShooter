using System.Collections;
using System.Collections.Generic;
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
    public Image arrow;
    public Slider healthBar;
    public Slider baseEnergyBar;

    void Start()
    {
        _instance = this;
    }

    void Update()
    {
        UpdateArrowRotation();
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
