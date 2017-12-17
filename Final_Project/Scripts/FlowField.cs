using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField : MonoBehaviour {

    // Variables
    public GameObject flowFielderPrefab;
    public GameObject[] fielders;
    public int numOfFielders = 8;

    public float[][] field;

    // Properties
    public float[][] Field {
        get { return field; }
        set { field = value; }
    }

	// Initialization
	void Start () {

        // Flow Field
        field = new float[20][]
        {
            new float[20] {  1, 20, 20,  8,  4,  2,  1,.5f,.4f,.4f,.4f,.2f,.2f,.2f,.2f,.1f,.1f,.1f,.1f,.1f},
            new float[20] {  0,  1, 20,  8,  4,  2,  1,.5f,.4f,.4f,.4f,.2f,.2f,.2f,.2f,.1f,.1f,.1f,.1f,.1f},
            new float[20] {  0,  0,  8,  4,  2,  2,  1,.5f,.4f,.4f,.4f,.2f,.2f,.2f,.2f,.1f,.1f,.1f,.1f,.1f},
            new float[20] {  0,.2f,  2,  8,  4,  4,  1,.5f,.4f,.4f,.4f,.2f,.2f,.2f,.2f,.1f,.1f,.1f,.1f,.1f},
            new float[20] {.2f,.2f,  1,  0,  0, 10,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1},
            new float[20] {.2f,  2,  2,  0,  0,-.1f, 10,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1},
            new float[20] {.2f,  2,  2,  0,  0,-.1f, 10, 10,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1},
            new float[20] {.5f,  2,  2,  0,  0,-.1f, 20, 10, 10,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1},
            new float[20] {.5f,  2,  2,  1,  0,-.1f, 10, 10, 10, 10,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1},
            new float[20] {.5f,  2,  2,  2,  1,  0,  1, 10, 20, 10,  1,  2,  2,  2,  2,  2,  2,  2,  2,  2},
            new float[20] {.5f,  2,  2,  2,  1,  0,  0,  1, 20, 10,  1,  2,  4,  4,  4,  4,  4,  4,  4,  4},
            new float[20] {.5f,  2,.2f,.2f,  1,  1,  0,  0, 10, 10,  1,  0,  4,  6,  6,  6,  6,  6,  6,  6},
            new float[20] {  1,  2,  2,.2f,  1,  1,  1,  0,  0, 20,  1,.4f,  0,  6,  8,  8,  8,  8,  8,  8},
            new float[20] {  1,  1,  2,  2,.2f,.2f,  1,  1,  0,  1,  1,.3f,.2f,  0, 10, 10, 10, 10, 10, 10},
            new float[20] {  5,  1,  1,  2,  1,.2f,  1,  1,  1,  1,  1,.2f,.1f,  0,  1,  1,  1,  1,  1,  1},
            new float[20] {  5,  2,  1,  1,  1,  1,.2f,  1,  1,  1,  1,.2f,.1f,  0,  1,  1,  1,  1,  1,  1},
            new float[20] { 10,  2,  2,  1,  1,  1,.2f,.2f,  1,  1,  1,.2f,.1f,  0,  1,  1,  1,  1,  1,  1},
            new float[20] { 10,  2,  2,  2,  1,  1,.2f,.2f,  1,  1,  1,.2f,.1f,  0,  1,  1,  1,  1,  1,  1},
            new float[20] { 10,  2,  2,  2,  1,  1,  1,.2f,  1,  1,  1,.2f,.1f,  0,  1,  1,  1,  1,  1,  1},
            new float[20] { 10,  2,  2,  2,  1,  1,  1,.2f,  1,  1,  1,.2f,.1f,  0,  1,  1,  1,  1,  1,  1}
        };

        // Create the Flow Fielders
        fielders = new GameObject[numOfFielders];
        for (int i = 0; i < numOfFielders; i++)
        {
            Vector3 rand = new Vector3(Random.Range(0, 10), 0, Random.Range(0, 10));
            GameObject fielder = Instantiate(flowFielderPrefab, rand, transform.rotation);
            fielder.GetComponent<FlowFielderScript>().Field = field;
            fielders[i] = fielder;
        }
	}
	
	// Update
	void Update () {

        // Separation
        foreach (GameObject fielder in fielders)
        {
            foreach (GameObject otherFielder in fielders)
            {

                if (!fielder.Equals(otherFielder))
                {
                    Vector3 disp = (fielder.transform.position - otherFielder.transform.position) * -1;
                    if (disp.magnitude < 2f)
                    {
                        fielder.GetComponent<FlowFielderScript>().ApplyForce(disp);
                    }
                }
            }
        }
	}
}
