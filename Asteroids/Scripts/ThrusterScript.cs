using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterScript : MonoBehaviour {

    // Vars
    public GameObject sceneManager;
    public GameObject player;
    public ParticleSystem system;

    float colorVal = 0;
    float sign = 1;

    void Start()
    {
        system = gameObject.GetComponent<ParticleSystem>();
    }

	// Update is called once per frame
	void Update () {

        if (player.GetComponent<ShipPhysics>().maxVel > sceneManager.GetComponent<StatManager>().defaultMaxVel)
        {
            system.startSize = 1.1f;
            system.emissionRate = 10;

            colorVal += sign * 0.2f * Time.deltaTime;

            if (colorVal >= 1)
            {
                sign = -1;
            }
            else if (colorVal <= 0)
            {
                sign = 1;
            }

            Debug.Log(colorVal);


            system.startColor = Color.HSVToRGB(colorVal, colorVal * 10, 100);
        }
        else
        {
            system.startSize = 1f;
            system.emissionRate = 5;
            system.startColor = Color.white;
        }
    }
}
