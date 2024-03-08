using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Random = UnityEngine.Random;

public class RoundSystem : MonoBehaviour
{
    [Header("Goal Objects")]
    //public GameObject[] objectsToFind;
    [SerializeField] GameObject objectToFindPrefab;
    public GameObject objectToFind;
    [SerializeField] private Transform[] objSpawnLocations;
    private Transform initialObjectSpawn;
    private bool switchObjLocation = false;

    [Header("UI")]
    public TMP_Text PopupUI;
    public TMP_Text BoardUI;
    public UserFollowUI userFollowUI;
    public PointToTarget helperArrow;
    //[SerializeField] private float UIPopupSpeed = 5f;

    private int currentRound;
    [Tooltip("ONLY USE EITHER 4 OR 8!!")]
    [SerializeField] private int maxRounds = 4; //ONLY USE EITHER 4 OR 8!!

    private TimeTracker timeTracker;

    private GameObject player;
    private Camera playerCamera;
    OVRScreenFade playerScreen;

    [Header("Player Spawning")]
    public Transform[] playerSpawnPoints;
    private List<Transform> availableSpawnPoints = new List<Transform>();
    private Transform previousSpawnPoint;

    private bool isGameEnded;

    void Start()
    {
        timeTracker = GetComponent<TimeTracker>();
        player = GameObject.FindWithTag("Player");
        playerCamera = Camera.main;

        //Populate the temporary list with available spawn points
        availableSpawnPoints.AddRange(playerSpawnPoints);

        objectToFind = Instantiate(objectToFindPrefab);
        objectToFind.name = objectToFindPrefab.name;
        MoveObject();

        StartCoroutine(HelperUI(60f)); //maybe we want to do a timer instead?

        StartNewRound();
    }

    public void StartNewRound()
    {
        if (currentRound < maxRounds)
        {
            currentRound++;
            Debug.Log("Round " + currentRound);
            //Debug.Log("Find the " + objectToFind.name);
            PopupUI.text = "Find the " + objectToFind.name;


            if (currentRound > 1)
            {
                timeTracker.stopTimer();
                RelocatePlayer();
            }

            //After halfway through, move object again.
            if (currentRound == (maxRounds / 2) + 1)
            {
                switchObjLocation = true;
                MoveObject();
            }
        }
        else
        {
            EndSimulation();
        }
    }

    private void EndSimulation()
    {
        isGameEnded = true;
        playerScreen.fadeTime = 2;
        playerScreen.FadeOut();

        Debug.Log("Ending Game...");
        timeTracker.Export();
        objectToFind.GetComponent<SphereCollider>().enabled = false;

        BoardUI.text = "Thank you for playing! <br> You may now take off the headset.";
        RelocatePlayer();
    }

    //Move player to a random location in the map
    private void RelocatePlayer()
    {
        if(!isGameEnded)
        {
            StartCoroutine(MovePlayer());
        }
        else
        {
            StartCoroutine(EndingGame());
        }
    }

    private IEnumerator MovePlayer()
    {
        helperArrow.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        //fade out
        playerScreen = playerCamera.GetComponent<OVRScreenFade>();
        playerScreen.fadeTime = 2; //increase fade time
        playerScreen.FadeOut();

        yield return new WaitForSeconds(3f);

        RandomSpawnPoint();

        yield return new WaitForSeconds(2f);

        //fade in
        playerScreen.FadeIn();
        playerScreen.fadeTime = 1; //resets back to default

        timeTracker.startTimer();
        StartCoroutine(HelperUI(5f));
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

        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.identity;

        playerScreen.FadeIn();
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
                    break;
                //objSpawnLocations[1]
                case "Point 2":
                    //Debug.Log("Going to Point 4");
                    objectToFind.transform.position = objSpawnLocations[3].position;
                    objectToFind.transform.rotation = objSpawnLocations[3].rotation;
                    break;
                //objSpawnLocations[2]
                case "Point 3":
                    //Debug.Log("Going to Point 1");
                    objectToFind.transform.position = objSpawnLocations[0].position;
                    objectToFind.transform.rotation = objSpawnLocations[0].rotation;
                    break;
                //objSpawnLocations[3]
                case "Point 4":
                    //Debug.Log("Going to Point 2");
                    objectToFind.transform.position = objSpawnLocations[1].position;
                    objectToFind.transform.rotation = objSpawnLocations[1].rotation;
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
        }

    }
    private IEnumerator HelperUI(float time)
    {
        StartCoroutine(ShowHelperArrow());

        yield return new WaitForSeconds(time);

        userFollowUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);

        userFollowUI.gameObject.SetActive(false);
    }

    private IEnumerator ShowHelperArrow()
    {
        Debug.Log("Pointing the way...");
        yield return new WaitForSeconds(90f); //90
        helperArrow.gameObject.SetActive(true);
    }

}