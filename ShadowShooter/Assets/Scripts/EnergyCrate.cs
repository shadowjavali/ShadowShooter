using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnergyType
{
    PURPLE,
    RED,
    GREEN
}

public class EnergyCrate : MonoBehaviour 
{
    public EnergyType type;

    public Sprite purpleSprite;
    public Sprite redSprite;
    public Sprite greenSprite;

    public void Initialize(EnergyType p_type)
    {
        type = p_type;
        if (p_type == EnergyType.PURPLE)
        {
            GetComponent<SpriteRenderer>().sprite = purpleSprite;
        }
        else if (p_type == EnergyType.RED)
        {
            GetComponent<SpriteRenderer>().sprite = redSprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = greenSprite;
        }
    }
}
