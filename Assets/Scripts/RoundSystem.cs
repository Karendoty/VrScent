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
    private GameObject objectToFind;
    [SerializeField] private Transform[] objSpawnLocations;
    private Transform initialObjectSpawn;
    private bool switchObjLocation = false;

    [Header("UI")]
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

        StartNewRound();
    }

    public void StartNewRound()
    {
        if (currentRound <= maxRounds)
        {
            currentRound++;
            //Debug.Log("Round " + currentRound);
            //Debug.Log("Find the " + objectToFind.name);
            BoardUI.text = "Find the " + objectToFind.name;

            StartCoroutine(HelperUI());

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
        Debug.Log("Ending Game...");
        timeTracker.Export();
        objectToFind.GetComponent<SphereCollider>().enabled = false;
        //Maybe move the player?
        //Show score and time
    }

    //Move player to a random location in the map
    private void RelocatePlayer()
    {
        StartCoroutine(MovePlayer());
    }

    private IEnumerator MovePlayer()
    {

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
        StartCoroutine(NewObjectiveUI());
    }

    private void RandomSpawnPoint()
    {
        //Debug.Log("Called");

        int randomIndex = Random.Range(0, availableSpawnPoints.Count);

        Transform selectedSpawnPoint = availableSpawnPoints[randomIndex];
        //Debug.Log(selectedSpawnPoint.name);

        userFollowUI.gameObject.SetActive(false);
    }

    private IEnumerator ShowHelperArrow()
    {
        yield return new WaitForSeconds(90f);
        helperArrow.gameObject.SetActive(true);
    }
    
    private IEnumerator MovePlayer()
    {
        Debug.Log("Relocating player...");
        if (previousSpawnPoint != null && !availableSpawnPoints.Contains(previousSpawnPoint))
        {
            availableSpawnPoints.Add(previousSpawnPoint);
            //Debug.Log("Adding " + previousSpawnPoint + " to list...");
        }

        previousSpawnPoint = selectedSpawnPoint;
        //Debug.Log(previousSpawnPoint.name);

        availableSpawnPoints.RemoveAt(randomIndex);
        //Debug.Log(availableSpawnPoints.Count);

        player.transform.position = selectedSpawnPoint.position;
        player.transform.rotation = selectedSpawnPoint.rotation;
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
    private IEnumerator HelperUI()
    {
        yield return new WaitForSeconds(90f);

        userFollowUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);

        userFollowUI.gameObject.SetActive(false);
    }

    private IEnumerator NewObjectiveUI()
    {
        userFollowUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);

        userFollowUI.gameObject.SetActive(false);
    }
}