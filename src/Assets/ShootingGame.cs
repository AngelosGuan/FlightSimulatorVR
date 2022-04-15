using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingGame : MonoBehaviour {

    public GameObject laser;
    public GameObject laserSource;
    public GameObject laserDirection;
    public GameObject[] targets;
    public GameObject scoreText;

    public AudioSource explosionSource;

    public static bool startGame;
    public static bool gameOn;
    private int score;
    private float[] timers;

	// Use this for initialization
	void Start () {
        score = 0;
        timers = new float[targets.Length];
        startGame = false;
        gameOn = false;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 source = laserSource.transform.position;
        Vector3 direction = laserDirection.transform.position - source;

        RaycastHit hitInfo;
        if (startGame)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].SetActive(true);
            }
            gameOn = true;
            startGame = false;
            
        }

        if (gameOn)
        {
            if (laser.activeSelf)
            {
                if (Physics.Raycast(source, direction, out hitInfo))
                {
                    Collider collider = hitInfo.collider;
                    for (int i = 0; i < targets.Length; i++)
                    {
                        if (collider.Equals(targets[i].GetComponent<Collider>()))
                        {
                            targets[i].SetActive(false);
                            explosionSource.transform.position = targets[i].transform.position;
                            explosionSource.Play();
                            score++;
                            scoreText.GetComponent<TextMesh>().text = "Score: " + score;
                            timers[i] = 0;
                        }
                    }
                }
            }


            for (int i = 0; i < timers.Length; i++)
            {
                timers[i] += Time.deltaTime;
                if (timers[i] >= 5)
                {
                    targets[i].SetActive(true);
                }
            }

        }
        
	}
}
