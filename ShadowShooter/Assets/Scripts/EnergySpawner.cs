using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySpawner : MonoBehaviour 
{
    public GameObject prefabEnergyCrate;
	// Use this for initialization
	void Start () 
    {
        int __randomIndex = Random.Range(0, 10);
        EnergyType __type;
        if (__randomIndex > 8 && __randomIndex <= 10)
        {
            __type = EnergyType.PURPLE;
        }
        else if (__randomIndex > 4 && __randomIndex <= 8)
        {
            __type = EnergyType.RED;
        }
        else
        {
            __type = EnergyType.GREEN;
        }
        EnergyCrate __crate = Instantiate(prefabEnergyCrate, transform.position, new Quaternion(0f, 0f, 0f, 1f)).GetComponent<EnergyCrate>();
        __crate.Initialize(__type);
	}
}
