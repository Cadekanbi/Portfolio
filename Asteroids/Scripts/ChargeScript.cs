using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeScript : MonoBehaviour {

    public ParticleSystem system;

    float colorVal = 0;
    float sign = 1;

    void Start()
    {
        system = gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        colorVal += sign * 0.2f * Time.deltaTime;

        if (colorVal >= 1)
        {
            sign = -1;
        }
        else if (colorVal <= 0)
        {
            sign = 1;
        }  

        system.startColor = Color.HSVToRGB(colorVal, colorVal * 10, 100);
    }
}
