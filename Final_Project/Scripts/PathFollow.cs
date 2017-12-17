using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : MonoBehaviour
{

    // Variables
    public Camera pathWatcherRef;

    public GameObject pathFollowerPrefab;
    public GameObject[] followers;
    public Vector3[] pathNodes;

    public int numOfFollowers = 5;

    public float nodeSwitchDistance = 4f;

    // Properties


    // Initialization
    void Start()
    {
        // Path Nodes
        pathNodes = new Vector3[7];
        pathNodes[0] = new Vector3(119, 5, 31);
        pathNodes[1] = new Vector3(114, 5, 55);
        pathNodes[2] = new Vector3(91, 5, 97);
        pathNodes[3] = new Vector3(76, 5, 119);
        pathNodes[4] = new Vector3(83, 5, 143);
        pathNodes[5] = new Vector3(108, 5, 175);
        pathNodes[6] = new Vector3(119, 5, 206);


        // Create the Flow Fielders
        followers = new GameObject[numOfFollowers];
        for (int i = 0; i < numOfFollowers; i++)
        {

            Vector3 rand = new Vector3(Random.Range(110, 117), 0, Random.Range(0, 5));
            GameObject newFollower = Instantiate(pathFollowerPrefab, rand, transform.rotation);
            newFollower.GetComponent<PathFollowerScript>().PathNode = 0;
            followers[i] = newFollower;
        }

        pathWatcherRef.GetComponent<CameraFollow>().target = followers[0].transform;
    }

    // Update
    void Update()
    {


    }

    // Fixed Update
    void FixedUpdate()
    {
        // Give the camera something to look at


        // Loop through every path follower and give them some guidance
        foreach (GameObject follower in followers)
        {
            // Temp vars to shorten code
            Transform followerTransf = follower.GetComponent<Transform>();
            PathFollowerScript script = follower.GetComponent<PathFollowerScript>();
            Vector3 nodeOfInterest = pathNodes[follower.GetComponent<PathFollowerScript>().pathNode];
            Vector3 displacement = nodeOfInterest - followerTransf.position;
            int val = script.pathNode;

            // If your are seeking the last node and are looped to the other side of the terrain, seek the first node
            if (val == pathNodes.Length - 1)
            {
                if (displacement.magnitude >= 175)
                {
                    follower.GetComponent<PathFollowerScript>().PathNode = 0;
                }
            }
            // Do the normal displacement check the other nodes
            else
            {
                if (displacement.magnitude <= nodeSwitchDistance)
                {
                    follower.GetComponent<PathFollowerScript>().PathNode++;
                }
            }


            // Tell them to seek their nodes
            follower.GetComponent<PathFollowerScript>().Seek(nodeOfInterest);
            // And keep their distance from eachother
            foreach (GameObject otherFollower in followers)
            {
                if (!follower.Equals(otherFollower))
                {
                    Vector3 newDisp = (otherFollower.transform.position - followerTransf.position) * -1;
                    if (newDisp.magnitude < 2.5f)
                    {
                        follower.GetComponent<PathFollowerScript>().ApplyForce(newDisp);
                    }
                }
            }
        }
    }
}
