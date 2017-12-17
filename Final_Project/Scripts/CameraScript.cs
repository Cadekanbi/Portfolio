using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraScript : MonoBehaviour
{

    public Camera fpsCam;
    public Camera cam1;
    public Camera cam2;

    List<Camera> cameras;

    // Use this for initialization
    void Start()
    {
        cameras = new List<Camera>();

        fpsCam.enabled = true;
        cam1.enabled = false;
        cam2.enabled = false;

        cameras.Add(fpsCam);
        cameras.Add(cam1);
        cameras.Add(cam2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CycleCamera();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("World");
        }
    }

    void CycleCamera()
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            if (cameras[i].enabled == true)
            {
                if (i == cameras.Count - 1)
                {
                    cameras[i].gameObject.GetComponent<Camera>().enabled = false;
                    cameras[0].gameObject.GetComponent<Camera>().enabled = true;
                    return;
                }
                else
                {
                    Debug.Log("Camera Switch");
                    cameras[i].gameObject.GetComponent<Camera>().enabled = false;
                    cameras[i + 1].gameObject.GetComponent<Camera>().enabled = true;
                    return;
                }
            }
        }
    }
}
