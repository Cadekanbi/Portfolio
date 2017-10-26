using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour {

    // Variables
    public GameObject manager;

    public GameObject smallAsteroid;

    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    public Sprite sprite4;

    public GameObject speedBoost;
    public GameObject plusLife;
    public GameObject gattlingGun;
    public GameObject pulseBeam;

    private double timer;
    private int hitPoints = 2;

    private Vector2 direction;
    public float speed = 0.02f;

    public float speedTimer;

    // Initialization
    void Start() {

        RandomSprite();

        if (this.gameObject.tag == "Asteroid")
        {
            hitPoints = manager.GetComponent<StatManager>().asteroidHitPoints * 2;
        }

        // Get default speed
        speed = manager.GetComponent<StatManager>().defaultAsteroidSpeed;
        // Get flash time
        timer = manager.GetComponent<StatManager>().damageFlashTime;
    }

    // Update is called once per frame
    void Update() {

        speed += Time.deltaTime * 0.002f;

        // Move "up"
        transform.position += transform.right * (speed);

        if (hitPoints <= 0)
        {
            // Spawn Random PowerUp
            int num = Random.Range(0, 100);
            if (num >= 0 && num < 6)
            {
                Instantiate(speedBoost, transform.position, Quaternion.Euler(0, 0, 0));
            }
            else if (num >= 6 && num < 10)
            {
                Instantiate(plusLife, transform.position, Quaternion.Euler(0, 0, 0));
            }
            else if (num >= 10 && num < 18)
            {
                Instantiate(pulseBeam, transform.position, Quaternion.Euler(0, 0, 0));
            }
            else if (num >= 18 && num < 28)
            {
                Instantiate(gattlingGun, transform.position, Quaternion.Euler(0, 0, 0));
            } 

            SelfDestruct();
        }

        timer-= Time.deltaTime;

        if (timer < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "KillBounds")
        {
            Destroy(gameObject);
        }

        if (col.gameObject.tag == "Bullet")
        {
            DamageFlash();
            hitPoints--;
        }
        if (col.gameObject.tag == "PulseBullet")
        {
            DamageFlash();
            hitPoints-= 4;
        }
    }

    void SelfDestruct()
    {
        // Increase speed at with level
        speedTimer += Time.deltaTime;
        if (manager.GetComponent<StatManager>().currentLevel == 2)
        {
            speed = 0.05f;
        }
        else if (manager.GetComponent<StatManager>().currentLevel == 3)
        {
            speed = 0.08f;
        }

        // Checks to see what type of asteroid it is and acts based on that
        if (this.gameObject.tag == "Asteroid")
        {
            Vector3 pos = gameObject.transform.position;
            // Spawn 3 smaller asteroids with random positions and different directions
            for (int i = -1; i <= 1; i++)
            {
                Instantiate(smallAsteroid, new Vector3(pos.x + (Random.Range(-0.75f, 0.75f)), pos.y + (i / 3f), 0), Quaternion.RotateTowards(transform.rotation, new Quaternion(0, 0, 10f * i, 0), 1f));
            }

            GameObject.Find("SceneManager").SendMessage("AddScore", 10);
            Destroy(gameObject);
        }

        if (this.gameObject.tag == "SmallAsteroid")
        {
            GameObject.Find("SceneManager").SendMessage("AddScore", 10);
            Destroy(gameObject);
        }
    }

    void DamageFlash()
    {
        timer = manager.GetComponent<StatManager>().damageFlashTime;
        gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
    }

    void AddPoints(int num)
    {
        manager.GetComponent<StatManager>().AddScore(num);
    }

    void RandomSprite()
    {
        // Random Asteroid Shape
        float num = Random.Range(0, 4);
        if (num < 1)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite1;
        }
        else if (num < 2)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite2;
        }
        else if (num < 3)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite3;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite4;
        }
    }
}
