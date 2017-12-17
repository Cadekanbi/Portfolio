using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour {

    // Vars
    public int pulseRate = 1;
    public int rotateRate = 35;
    private float count;
    private int dir = 1;

    // Methods
	// - Initialization
	void Start () {

        count = 7;
	}
	
    // - Fixed Update
	void FixedUpdate () {

        count += Time.fixedDeltaTime * dir * 2;
        if (count >= 10 || count <= 7)
        {
            dir *= -1;
        }
        //Pulse
        gameObject.transform.localScale = new Vector3(1, 1, 1) * (count / 10);
    
        //Rotation
        gameObject.transform.Rotate(Vector3.up, rotateRate * Time.fixedDeltaTime);
	}
}
