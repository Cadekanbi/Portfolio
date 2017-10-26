using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour {

    // Variables
    public GameObject manager;
    public GameObject bulletPrefab;
    public GameObject charge;
    public GameObject pulseBeam;

    private float waitTime = 0.6f;
    private float timer = 0;
    private float chargeCounter = 0;

    public string weapon;

    // Initialization
    void Start()
    {
        //charge.gameObject.SetActive(false);
    }

	// Update
	void FixedUpdate ()
    {
        weapon = manager.GetComponent<StatManager>().currentWeapon;

        charge.transform.position = gameObject.transform.position;
        charge.transform.rotation = gameObject.transform.rotation;

        // Controls time between shots
        timer += Time.deltaTime;

        if (chargeCounter >= 3)
        {
            chargeCounter = 3;
        }

        if (weapon == "Beam")
        {
            charge.gameObject.SetActive(false);
            if (Input.GetKey(KeyCode.Space) && timer > waitTime)
            {
                timer = 0;
                Instantiate(bulletPrefab, transform.position, transform.rotation);
            }
        }
        else if (weapon == "PulseBeam")
        {
            // Charge shots
            if (Input.GetKey(KeyCode.Space) && timer > waitTime / 4)
            {
                chargeCounter += Time.deltaTime;
                Debug.Log("Beam Charge: " + chargeCounter);
                charge.gameObject.SetActive(true);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (chargeCounter >= manager.GetComponent<StatManager>().beamChargeTime)
                {
                    Debug.Log("Beam Fired");
                    Instantiate(pulseBeam, transform.position, transform.rotation);
                }
                chargeCounter = 0;
                charge.gameObject.SetActive(false);
            }
        }
        else if (weapon == "GattlingGun")
        {

            charge.gameObject.SetActive(false);
            if (Input.GetKey(KeyCode.Space) && timer > manager.GetComponent<StatManager>().gatFireSpeed)
            {
                timer = 0;
                Instantiate(bulletPrefab, transform.position, transform.rotation);
            }
        }
	}
}
