using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionScript : MonoBehaviour {

    public AudioSource explosion;

    private bool gameover;

	// Use this for initialization
	void Start () {
        gameover = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameover && !explosion.isPlaying)
        {
            Debug.Log("Game Over");
            SceneManager.LoadScene("GameOverRoom");
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        explosion.Play();
        gameover = true;
    }
}
