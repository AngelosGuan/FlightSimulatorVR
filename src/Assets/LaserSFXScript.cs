using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSFXScript : MonoBehaviour {

    public GameObject laser;
    public AudioSource laserOn;
    public AudioSource laserHum;

    private bool playHum;

	// Use this for initialization
	void Start () {
        playHum = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Right Trigger") == 1)
        {
            if (laser.activeSelf && !playHum && !laserHum.isPlaying)
            {
                laserOn.Play();
                playHum = true;
            }
            if (laser.activeSelf && playHum && !laserOn.isPlaying)
            {
                laserHum.Play();
                playHum = false;
            }
        } else
        {
            laserHum.Stop();
            playHum = false;
        }

    }
}
