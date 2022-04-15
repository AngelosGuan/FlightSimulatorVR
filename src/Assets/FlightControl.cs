using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FlightControl : MonoBehaviour {

    public GameObject plane;
    public GameObject forward;
    public GameObject speedText;
    public GameObject laser;
    public GameObject scoreText;
    public GameObject Pause;
    
    public AudioSource engineSource;
    public AudioClip engineClip;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;

    public GameObject CancelTutorialText;
    public GameObject step0Text; // roll
    public GameObject step1Text; // pitch
    public GameObject step2Text; // yaw
    public GameObject step4Text; // speed
    public GameObject step3Text; // shoot
    public GameObject target1;
    public GameObject laserSource;
    public GameObject laserDirection;

    public static bool loadTutorial;

    private Transform planeTransform;
    private Transform forwardTransform;
    private Vector3 planeStart;
    private float speed;
    private bool resetControllerHeight;
    private Vector3 controllerHeightOrigin;

    private bool endTutorial;
    private int tutorialCounter;
    private float timer;
    private bool holdbutton;

    public static bool gamePaused;
    public static bool onPause;
    private float lastSpeed;

	// Use this for initialization
	void Start () {
        planeTransform = plane.GetComponent<Transform>();
        forwardTransform = forward.GetComponent<Transform>();
        planeStart = planeTransform.position;
        if (loadTutorial)
        {
            speed = 0;
            CancelTutorialText.SetActive(true);
            speedText.SetActive(false);
            scoreText.SetActive(false);
            endTutorial = false;
            tutorialCounter = 0;
            holdbutton = false;
            Pause.SetActive(false);

        } else
        {
            CancelTutorialText.SetActive(false);
            speed = 1;
            ShootingGame.startGame = true;
        }
        resetControllerHeight = true;
        laser.SetActive(false);
        gamePaused = false;
        onPause = false;
    }
	
	// Update is called once per frame
	void Update () {
        Quaternion controllerRot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        Quaternion orientation = planeTransform.rotation;
        Quaternion newOrientaiton = orientation;

        Vector3 source = laserSource.transform.position;
        Vector3 direction = laserDirection.transform.position - source;

        if (speed > 0 && !engineSource.isPlaying)
        {
            engineSource.Play();
        }
        else if (speed == 0)
        {
            engineSource.Stop();
        }

        if (!loadTutorial && !gamePaused && !onPause && Input.GetButtonDown("Pause"))
        {
            lastSpeed = speed;
            speed = 0;
            ShootingGame.gameOn = false;
            ShootingGame.startGame = false;

            gamePaused = true;
        } else if (!loadTutorial && gamePaused && !onPause)
        {
            // shoot block with laser on plane
            onPause = true;
            SceneManager.LoadScene("PauseRoom", LoadSceneMode.Additive);

        } else if (!loadTutorial && gamePaused && onPause)
        {
            //game Paused, in another scene

        } else if (!loadTutorial && !gamePaused && onPause)
        {
            // resume and set onPause to false
            speed = lastSpeed;
            if (!loadTutorial)
            {
                ShootingGame.startGame = true;
            }
            onPause = false;
        }
        else if (loadTutorial)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                endTutorial = true;
            }
            else if (tutorialCounter == 0)
            {
                step0Text.SetActive(true);
                if (Input.GetAxis("Right Horizontal") == 1)
                {
                    newOrientaiton *= Quaternion.Euler(0, 0, -0.1f);
                    holdbutton = true;
                }
                else if (Input.GetAxis("Right Horizontal") == -1)
                {
                    newOrientaiton *= Quaternion.Euler(0, 0, 0.1f);
                }
                else if (holdbutton == true)
                {
                    tutorialCounter += 1;
                    step0Text.SetActive(false);
                    holdbutton = false;
                }
            } 
            else if (tutorialCounter == 1)
            {
                step1Text.SetActive(true);
                if (Input.GetAxis("Right Vertical") == 1)
                {
                    newOrientaiton *= Quaternion.Euler(-0.1f, 0, 0);
                    holdbutton = true;
                }
                else if (Input.GetAxis("Right Vertical") == -1)
                {
                    newOrientaiton *= Quaternion.Euler(0.1f, 0, 0);
                }
                else if (holdbutton == true)
                {
                    tutorialCounter += 1;
                    step1Text.SetActive(false);
                    holdbutton = false;
                }
            }
            else if (tutorialCounter == 2)
            {
                step2Text.SetActive(true);
                if (Input.GetAxis("Left Horizontal") == -1)
                {
                    newOrientaiton *= Quaternion.Euler(0, -0.1f, 0);
                }
                else if (Input.GetAxis("Left Horizontal") == 1)
                {
                    newOrientaiton *= Quaternion.Euler(0, 0.1f, 0);
                    holdbutton = true;
                }
                else if (holdbutton == true)
                {
                    tutorialCounter += 1;
                    step2Text.SetActive(false);
                    holdbutton = false;
                }
            }
            else if (tutorialCounter == 3)
            {
               
                step3Text.SetActive(true);
                scoreText.SetActive(true);
                target1.SetActive(true);

                RaycastHit hitInfo;

                if (Input.GetAxis("Right Horizontal") == -1)
                {
                    newOrientaiton *= Quaternion.Euler(0, 0, 0.1f);
                }
                if (Input.GetAxis("Right Horizontal") == 1)
                {
                    newOrientaiton *= Quaternion.Euler(0, 0, -0.1f);
                }
                if (Input.GetAxis("Right Vertical") == -1)
                {
                    newOrientaiton *= Quaternion.Euler(0.1f, 0, 0);
                }
                if (Input.GetAxis("Right Vertical") == 1)
                {
                    newOrientaiton *= Quaternion.Euler(-0.1f, 0, 0);
                }
                if (Input.GetAxis("Left Horizontal") == -1)
                {
                    newOrientaiton *= Quaternion.Euler(0, -0.1f, 0);
                }
                if (Input.GetAxis("Left Horizontal") == 1)
                {
                    newOrientaiton *= Quaternion.Euler(0, 0.1f, 0);
                }

                if (Input.GetAxis("Right Trigger") == 1)
                {
                    laser.SetActive(true);
                }
                else
                {
                    laser.SetActive(false);
                }

                if (laser.activeSelf)
                {
                    if (Physics.Raycast(source, direction, out hitInfo))
                    {
                        Collider collider = hitInfo.collider;
                        if (collider.Equals(target1.GetComponent<Collider>()))
                        {
                            target1.SetActive(false);
                            scoreText.GetComponent<TextMesh>().text = "Score: " + 1;
                            tutorialCounter += 1;
                            step3Text.SetActive(false);

                        }
                    }
                }

            }
            else if (tutorialCounter == 4)
            {
                // speed tutorial 
                step4Text.SetActive(true);
                speedText.SetActive(true);
                

                if(speed == 0)
                {
                    speed = 1;
                }

                if (Input.GetAxis("Right Horizontal") == -1)
                {
                    newOrientaiton *= Quaternion.Euler(0, 0, 0.1f);
                }
                if (Input.GetAxis("Right Horizontal") == 1)
                {
                    newOrientaiton *= Quaternion.Euler(0, 0, -0.1f);
                }
                if (Input.GetAxis("Right Vertical") == -1)
                {
                    newOrientaiton *= Quaternion.Euler(0.1f, 0, 0);
                }
                if (Input.GetAxis("Right Vertical") == 1)
                {
                    newOrientaiton *= Quaternion.Euler(-0.1f, 0, 0);
                }
                if (Input.GetAxis("Left Horizontal") == -1)
                {
                    newOrientaiton *= Quaternion.Euler(0, -0.1f, 0);
                }
                if (Input.GetAxis("Left Horizontal") == 1)
                {
                    newOrientaiton *= Quaternion.Euler(0, 0.1f, 0);
                }

                if (Input.GetAxis("Right Trigger") == 1)
                {
                    laser.SetActive(true);
                }
                else
                {
                    laser.SetActive(false);
                }


                if (Input.GetAxis("Left Grip") == 1 && resetControllerHeight)
                {
                    resetControllerHeight = false;
                    controllerHeightOrigin = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
                }
                else if (Input.GetAxis("Left Grip") == 1)
                {
                    Vector3 currentHeight = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
                    Vector3 heightDiff = currentHeight - controllerHeightOrigin;
                    speed += (2.0f * heightDiff.y);
                    if (speed < 0)
                    {
                        speed = 0;
                    }
                    if (speed > 5)
                    {
                        speed = 5;
                    }
                    speedText.GetComponent<TextMesh>().text = "Speed: " + (speed * 30);
                }
                else 
                {
                    if(speed*30 > 30)
                    {
                        tutorialCounter += 1;
                        endTutorial = true;
                      
                    } else
                    {
                        resetControllerHeight = true;
                    }
                }

            }

            if (endTutorial)
            {
                loadTutorial = false;
                speed = 1;
                step0Text.SetActive(false);
                step1Text.SetActive(false);
                step2Text.SetActive(false);
                step3Text.SetActive(false);
                step4Text.SetActive(false);
                speedText.SetActive(true);
                scoreText.SetActive(true);
                CancelTutorialText.SetActive(false);
                ShootingGame.startGame = true;
                speedText.GetComponent<TextMesh>().text = "Speed: " + (speed * 30);
                scoreText.GetComponent<TextMesh>().text = "Score: " + 0;
            }

            Vector3 forwardDirection = forwardTransform.position - planeTransform.position;
            Vector3 newPosition = planeTransform.position + speed * forwardDirection;

            planeTransform.SetPositionAndRotation(newPosition, newOrientaiton);
        }
        else {
            Pause.SetActive(true);
            if (!ShootingGame.gameOn)
            {
                ShootingGame.startGame = true;
            }

            if (speed > 0)
            {
                if (Input.GetAxis("Right Horizontal") == -1)
                {
                    newOrientaiton *= Quaternion.Euler(0, 0, 0.1f);
                }
                if (Input.GetAxis("Right Horizontal") == 1)
                {
                    newOrientaiton *= Quaternion.Euler(0, 0, -0.1f);
                }
                if (Input.GetAxis("Right Vertical") == -1)
                {
                    newOrientaiton *= Quaternion.Euler(0.1f, 0, 0);
                }
                if (Input.GetAxis("Right Vertical") == 1)
                {
                    newOrientaiton *= Quaternion.Euler(-0.1f, 0, 0);
                }
                if (Input.GetAxis("Left Horizontal") == -1)
                {
                    newOrientaiton *= Quaternion.Euler(0, -0.1f, 0);
                }
                if (Input.GetAxis("Left Horizontal") == 1)
                {
                    newOrientaiton *= Quaternion.Euler(0, 0.1f, 0);
                }
            }
            if (Input.GetAxis("Left Grip") == 1 && resetControllerHeight)
            {
                resetControllerHeight = false;
                controllerHeightOrigin = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            }
            else if (Input.GetAxis("Left Grip") == 1)
            {
                Vector3 currentHeight = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
                Vector3 heightDiff = currentHeight - controllerHeightOrigin;
                speed += (2.0f * heightDiff.y);
                resetControllerHeight = true;
                if (speed < 0)
                {
                    speed = 0;
                }
                if (speed > 5)
                {
                    speed = 5;
                }
                speedText.GetComponent<TextMesh>().text = "Speed: " + (speed * 30);
            }
            else
            {
                resetControllerHeight = true;
            }
            if (Input.GetAxis("Right Trigger") == 1)
            {
                laser.SetActive(true);
            }
            else
            {
                laser.SetActive(false);
            }
            if (ShootingGame.gameOn)
            {
                if (speed <= 0.5)
                {
                    plane.GetComponent<Rigidbody>().useGravity = true;
                    speedText.GetComponent<TextMesh>().color = Color.red;
                }
                else
                {
                    plane.GetComponent<Rigidbody>().useGravity = false;
                    plane.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    speedText.GetComponent<TextMesh>().color = Color.white;
                }
            }
            
            Vector3 forwardDirection = forwardTransform.position - planeTransform.position;
            Vector3 newPosition = planeTransform.position + speed * forwardDirection;

            planeTransform.SetPositionAndRotation(newPosition, newOrientaiton);
        }

    }

}
