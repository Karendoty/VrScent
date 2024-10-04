using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Random = UnityEngine.Random;
using System.Diagnostics;

/*
 -- Attach this to an empty game object (we just named it Game Manager) --

This is the main functionality of our simulation that controls where the player
and goal spawns, the helper ui elements (arrow and pop-up), rounds, and ending
scenario. 
 */

public class RoundSystem : MonoBehaviour
{
    [Header("Goal Objects")]
    [Tooltip("Prefab should have GoalObjectDetection component")]
    [SerializeField] GameObject objectToFindPrefab;
    public GameObject objectToFind; //this later gets filled by objectToFindPrefab
    [Tooltip("Locations where the goal object will spawn")]
    [SerializeField] private Transform[] objSpawnLocations;
    [Tooltip("Location names where each point is at. ")]
    [SerializeField] private String[] objSpawnNames;
    private String currentObjSpawnName;
    private Transform initialObjectSpawn;
    private bool switchObjLocation = false;

    [Header("UI")]
    public TMP_Text PopupUI;
    public TMP_Text BoardUI;
    public UserFollowUI userFollowUI;
    public PointToTarget helperArrow;

    [Header("UI Timer")]
    public float uiTimer;
    [Tooltip("How long it takes for the UI to pop up the first round")]
    [SerializeField] private float maxUITime;
    public float uiTimerDuration;
    [Tooltip("How long it takes for the UI to turn off")]
    [SerializeField] private float maxUITimeUp;
    public float arrowTimer;
    [Tooltip("How long it takes for the arrow to pop up")]
    [SerializeField] private float maxArrowTime;
    private bool isUITimerGoing;
    private bool isUIUp;
    private bool isArrowTimerGoing;


    [Header("Rounds")]
    [Tooltip("Only use even whole numbers numbers")]
    [SerializeField] private int maxRounds = 6;
    private int currentRound;

    [Header("Player Timer")]
    public TimeTracker timeTracker;

    private GameObject player;
    //private Camera playerCamera;
    [SerializeField] private FadeToBlack fade;

    [Header("Player Spawning")]
    public Transform[] playerSpawnPoints;
    private List<Transform> availableSpawnPoints = new List<Transform>();
    private Transform previousSpawnPoint;
    public Transform originalSpawnPoint;

    private bool isGameEnded;
    public LineRenderController lineRenderController;
    private List<TimeSpan> wrongTimes = new List<TimeSpan>();
    private WrongWayTimer wrongWay = new WrongWayTimer();
    private float distance;
    private float lastDistance;

    [Header("Player Tracking")]
    public SessionTrackingManager sessionTrackingManager;

    void Start()
    {
        timeTracker = GetComponent<TimeTracker>();
        player = GameObject.FindWithTag("Player");
        //playerCamera = Camera.main;

        //Populate the temporary list with available spawn points
        availableSpawnPoints.AddRange(playerSpawnPoints);

        objectToFind = Instantiate(objectToFindPrefab);
        objectToFind.name = objectToFindPrefab.name;
        MoveObject();

        isUITimerGoing = true;
        isArrowTimerGoing = true;
        uiTimer = 120;
        uiTimerDuration = maxUITimeUp;
        arrowTimer = 180f; //for the first round we want it to be longer because they are exploring the area

        StartNewRound();
    }

    private void Update()
    {
        //After timer ends, the UI will pop up.
        if (isUITimerGoing)
        {
            //Debug.Log("UI");
            if (uiTimer > 0)
            {
                uiTimer -= Time.deltaTime;
            }
            else if (uiTimer <= 0)
            {
                userFollowUI.gameObject.SetActive(true);
                isUITimerGoing = false;
                isUIUp = true;
            }
        }

        if (isUIUp)
        {
            if (uiTimerDuration > 0)
            {
                uiTimerDuration -= Time.deltaTime;
            }
            else if (uiTimerDuration <= 0)
            {
                userFollowUI.gameObject.SetActive(false);
                uiTimerDuration = maxUITimeUp;
                timeTracker.startTimer();
                isUIUp = false;
            }
        }

        if (isArrowTimerGoing)
        {
            //Debug.Log("Arrow");
            if (arrowTimer > 0)
            {
                arrowTimer -= Time.deltaTime;
                //Debug.Log(arrowTimer);
            }
            else if (arrowTimer <= 0)
            {
                helperArrow.gameObject.SetActive(true);
                isArrowTimerGoing = false;
            }
        }


        string initialSpawnName = initialObjectSpawn.name;
        switch (initialSpawnName)
        {
            //objSpawnLocations[0]
            case "Point 1":
                CheckDistance(objSpawnLocations[2]);

                break;
            //objSpawnLocations[1]
            case "Point 2":
                //Debug.Log("Going to Point 4");
                CheckDistance(objSpawnLocations[3]);

                break;
            //objSpawnLocations[2]
            case "Point 3":
                //Debug.Log("Going to Point 1");
                CheckDistance(objSpawnLocations[4]);

                break;
            //objSpawnLocations[3]
            case "Point 4":
                //Debug.Log("Going to Point 2");
                CheckDistance(objSpawnLocations[1]);

                break;
        }

    }

    private void CheckDistance(Transform location)
    {
        distance = Vector3.Distance(location.position, transform.position);
        if (lastDistance == 0)
        {
            lastDistance = distance;
            return;
        }
        if (distance > lastDistance)
        {
            wrongWay.WrongWay();
            lastDistance = distance;
        }
        else
        {
            wrongWay.RightWay();
            lastDistance = distance;
        }
    }


    private void ResetTimer()
    {
        wrongTimes.Add(wrongWay.GetTimeSpan());
    }

    public void StartNewRound()
    {
        if (currentRound < maxRounds)
        {
            currentRound++;
            //Debug.Log("Round " + currentRound);
            //Debug.Log("Find the " + objectToFind.name);

            if (currentRound > 1)
            {
                lineRenderController.RoundDone();
                timeTracker.stopTimer();
                RelocatePlayer();

                ResetUI();

                ResetTimer();
            }

            //After halfway through, move object again.
            if (currentRound == (maxRounds / 2) + 1)
            {
                switchObjLocation = true;
                MoveObject();
                ResetTimer();
            }

            PopupUI.text = "Find the " + objectToFind.name + " by the " + currentObjSpawnName;
        }
        else
        {
            EndSimulation();
            ResetTimer();
            lineRenderController.RoundDone();
        }
    }

    private void EndSimulation()
    {
        UnityEngine.Debug.Log("Ending The simulation");

        //ADDED CALL to SAVE THE TRACKING DATA
       // sessionTrackingManager.SaveSession();

        timeTracker.stopTimer();

        isGameEnded = true;
        fade.fadeDuration = 2;
        fade.FadeOut();

        //Debug.Log("Ending Game...");
        timeTracker.saveAllData();
        timeTracker.Export();
        timeTracker.ShowEnding();
        objectToFind.GetComponent<SphereCollider>().enabled = false;

        //BoardUI.text = "Thank you for playing! <br> You may now take off the headset.";
        BoardUI.gameObject.SetActive(false);

        RelocatePlayer();
    }

    //Move player to a random location in the map
    private void RelocatePlayer()
    {
        if (!isGameEnded)
        {
            StartCoroutine(MovePlayer());
            lineRenderController.MarkEndLocation();
        }
        else
        {
            StartCoroutine(EndingGame());
        }
    }

    private IEnumerator MovePlayer()
    {
        helperArrow.gameObject.SetActive(false);
        userFollowUI.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        //fade out
        fade.fadeDuration = 2; //increase fade time
        fade.FadeOut();

        yield return new WaitForSeconds(3f);

        RandomSpawnPoint();

        yield return new WaitForSeconds(2f);

        //fade in
        fade.FadeIn();
        fade.fadeDuration = 1; //resets back to default



        //Enables UI Popup timer
        isUITimerGoing = true;
        isArrowTimerGoing = true;
        uiTimer = 5f;
    }

    private void RandomSpawnPoint()
    {
        //Debug.Log("Called");

        int randomIndex = Random.Range(0, availableSpawnPoints.Count);

        Transform selectedSpawnPoint = availableSpawnPoints[randomIndex];
        //Debug.Log(selectedSpawnPoint.name);

        if (previousSpawnPoint != null && !availableSpawnPoints.Contains(previousSpawnPoint))
        {
            availableSpawnPoints.Add(previousSpawnPoint);
        }

        previousSpawnPoint = selectedSpawnPoint;
        availableSpawnPoints.RemoveAt(randomIndex);

        player.transform.position = selectedSpawnPoint.position;
        player.transform.rotation = selectedSpawnPoint.rotation;

        userFollowUI.gameObject.SetActive(false);
    }

    private IEnumerator EndingGame()
    {
        yield return new WaitForSeconds(2f);

        player.transform.position = originalSpawnPoint.position;
        player.transform.rotation = originalSpawnPoint.rotation;

        fade.FadeIn();

        ResetUI();
    }

    private void MoveObject()
    {
        if (switchObjLocation)
        {
            string initialSpawnName = initialObjectSpawn.name;
            switch (initialSpawnName)
            {
                //objSpawnLocations[0]
                case "Point 1":
                    //Debug.Log("Going to Point 3");
                    objectToFind.transform.position = objSpawnLocations[2].position;
                    objectToFind.transform.rotation = objSpawnLocations[2].rotation;
                    currentObjSpawnName = objSpawnNames[2];
                    break;
                //objSpawnLocations[1]
                case "Point 2":
                    //Debug.Log("Going to Point 4");
                    objectToFind.transform.position = objSpawnLocations[3].position;
                    objectToFind.transform.rotation = objSpawnLocations[3].rotation;
                    currentObjSpawnName = objSpawnNames[3];
                    break;
                //objSpawnLocations[2]
                case "Point 3":
                    //Debug.Log("Going to Point 1");
                    objectToFind.transform.position = objSpawnLocations[0].position;
                    objectToFind.transform.rotation = objSpawnLocations[0].rotation;
                    currentObjSpawnName = objSpawnNames[0];
                    break;
                //objSpawnLocations[3]
                case "Point 4":
                    //Debug.Log("Going to Point 2");
                    objectToFind.transform.position = objSpawnLocations[1].position;
                    objectToFind.transform.rotation = objSpawnLocations[1].rotation;
                    currentObjSpawnName = objSpawnNames[1];
                    break;
            }

        }
        else
        {
            int randomIndex = Random.Range(0, objSpawnLocations.Length);

            Transform selectedTransform = objSpawnLocations[randomIndex];

            initialObjectSpawn = selectedTransform;

            //Debug.Log(selectedTransform.name);

            objectToFind.transform.position = selectedTransform.position;
            objectToFind.transform.rotation = selectedTransform.rotation;
            currentObjSpawnName = objSpawnNames[randomIndex];
        }

    }

    private void ResetUI()
    {
        isUITimerGoing = false;
        isUIUp = false;

        uiTimer = maxUITime;
        uiTimerDuration = maxUITimeUp;

        helperArrow.gameObject.SetActive(false);
        isArrowTimerGoing = false;
        arrowTimer = maxArrowTime;
    }
}