using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

    public GameObject manager;
    public GameObject life1;
    public GameObject life2;
    public GameObject life3;
    public GameObject life4;

    public string displayScore;
    public int lives;
    public int score;
    public int level;

    // Initialization
    void Start()
    {
        if (gameObject.name == "Lives")
        {
            Vector3 pos = transform.position + new Vector3(1, -0.4f, 0);
            float spacing = 0.7f;
            gameObject.GetComponent<Text>().text = "";

            life1.transform.position = pos;
            life2.transform.position = pos + new Vector3(spacing, 0, 0);
            life3.transform.position = pos + new Vector3(2 * spacing, 0, 0);
            life4.transform.position = pos + new Vector3(3 * spacing, 0, 0);
            Debug.Log("Lives drawn");
        }
    }

	// Update
	void Update ()
    {
        // Just for visual purposes
        score = manager.GetComponent<StatManager>().currentScore;
        level = manager.GetComponent<StatManager>().currentLevel;
        lives = manager.GetComponent<StatManager>().currentLives;

        if (gameObject.name == "Lives")
        {
            // Active lives
            if (lives == 0)
            {
                life1.SetActive(false);
                life2.SetActive(false);
                life3.SetActive(false);
                life4.SetActive(false);
            }
            else if (lives == 1)
            {
                life1.SetActive(true);
                life2.SetActive(false);
                life3.SetActive(false);
                life4.SetActive(false);
            }
            else if (lives == 2)
            {
                life1.SetActive(true);
                life2.SetActive(true);
                life3.SetActive(false);
                life4.SetActive(false);
            }
            else if (lives == 3)
            {
                life1.SetActive(true);
                life2.SetActive(true);
                life3.SetActive(true);
                life4.SetActive(false);
            }
            else if (lives == 4)
            {
                life1.SetActive(true);
                life2.SetActive(true);
                life3.SetActive(true);
                life4.SetActive(true);
            }
        }
        else if (gameObject.name == "Score")
        {
            // Display score
            if (score < 10)
            {
                displayScore = "- 000" + score + " -";
            }
            else if (score < 100)
            {
                displayScore = "- 00" + score + " -";
            }
            else if (score < 1000)
            {
                displayScore = "- 0" + score + " -";
            }
            else {
                displayScore = "- " + score + " -";
            }

            gameObject.GetComponent<Text>().text = displayScore;
        }
        else if (gameObject.name == "Level")
        {
            // Display level
            gameObject.GetComponent<Text>().text = "- Level " + level + " -";
        }
    }
}
