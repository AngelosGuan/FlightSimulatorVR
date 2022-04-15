using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaserScript : MonoBehaviour {

    public GameObject laserPointer;
    public GameObject laserOrigin;
    public GameObject laserDirection;
    public GameObject mainMenu;
    public GameObject tutorial;
    public GameObject startText;
    public GameObject quitText;
    public GameObject resume;

    public GameObject rightHand;
    private Transform rightTransform;

    public AudioSource clickSource;

    private Vector3 laserStart;
    private Collider menuCollider;
    private Collider startCollider;
    private Collider quitCollider;
    private Collider tutorialCollider;
    private Collider resumeCollider;
    private TextMesh menuTextMesh;
    private TextMesh startTextMesh;
    private TextMesh quitTextMesh;
    private TextMesh tutorialTextMesh;
    private TextMesh resumeTextMesh;

    private int counter;

	// Use this for initialization
	void Start () {
        laserStart = laserPointer.transform.position;
        rightTransform = rightHand.GetComponent<Transform>();
        
        
        quitCollider = quitText.GetComponent<Collider>();
        quitTextMesh = quitText.GetComponent<TextMesh>();

        if (startText != null)
        {
            startCollider = startText.GetComponent<Collider>();
            startTextMesh = startText.GetComponent<TextMesh>();
        }

        if (mainMenu != null)
        {
            menuCollider = mainMenu.GetComponent<Collider>();
            menuTextMesh = mainMenu.GetComponent<TextMesh>();
        }

        if (tutorial != null)
        {
            tutorialCollider = tutorial.GetComponent<Collider>();
            tutorialTextMesh = tutorial.GetComponent<TextMesh>();
        }

        if (resume != null)
        {
            resumeCollider = resume.GetComponent<Collider>();
            resumeTextMesh = resume.GetComponent<TextMesh>();
        }
        counter = 0;
	}
	
	// Update is called once per frame
	void Update () {
        Quaternion newLaserRot = rightTransform.rotation * Quaternion.Euler(0, -90, -90);

        laserPointer.transform.SetPositionAndRotation(rightTransform.position, newLaserRot);

        RaycastHit hitInfo;

        Vector3 origin = laserOrigin.transform.position;
        Vector3 direction = laserDirection.transform.position - origin;

        if (Physics.Raycast(origin, direction, out hitInfo))
        {
            //Debug.Log("Collided");

            Collider collider = hitInfo.collider;
            if (startCollider != null && collider.Equals(startCollider))
            {
                //Debug.Log("Start" + counter);
                startTextMesh.color = Color.green;
                if (Input.GetButtonDown("Submit"))
                {
                    clickSource.Play();
                    Debug.Log("Start");
                    FlightControl.loadTutorial = false;
                    SceneManager.LoadScene("CS498HW4");
                }
            }
            else if (collider.Equals(quitCollider))
            {
                //Debug.Log("Quit" + counter);
                quitTextMesh.color = Color.green;
                if (Input.GetButtonDown("Submit"))
                {
                    clickSource.Play();
                    #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                    #else
                        Application.Quit();
                    #endif
                }
            }
            else if (menuCollider != null && collider.Equals(menuCollider))
            {
                menuTextMesh.color = Color.green;
                if (Input.GetButtonDown("Submit"))
                {
                    clickSource.Play();
                    Debug.Log("Menu");
                    SceneManager.LoadScene("MenuRoom");
                }
            }
            else if (tutorialCollider != null && collider.Equals(tutorialCollider))
            {
                tutorialTextMesh.color = Color.green;
                if (Input.GetButtonDown("Submit"))
                {
                    clickSource.Play();
                    Debug.Log("Tutorial");
                    FlightControl.loadTutorial = true;
                    SceneManager.LoadScene("CS498HW4");
                }
            }
            else if (resume != null && collider.Equals(resumeCollider))
            {
                resumeTextMesh.color = Color.green;
                if (Input.GetButtonDown("Submit"))
                {
                    clickSource.Play();
                    Debug.Log("Resume");
                    FlightControl.gamePaused = false;
                    SceneManager.UnloadScene("PauseRoom");
                }
            }
            else
            {
                quitTextMesh.color = Color.white;
                if (startText != null)
                {
                    startTextMesh.color = Color.white;
                }
                if (menuTextMesh != null) {
                    menuTextMesh.color = Color.white;
                }
                if (tutorialTextMesh != null)
                {
                    tutorialTextMesh.color = Color.white;
                }
                if (resumeTextMesh != null)
                {
                    resumeTextMesh.color = Color.white;
                }
            }

            counter++;
        }
    }
}
