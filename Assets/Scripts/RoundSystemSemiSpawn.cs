using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Random = UnityEngine.Random;

public class RoundSystemSemiSpawn: MonoBehaviour
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
    [Tooltip("ONLY USE EITHER 4 OR 8!!")]
    [SerializeField] private int maxRounds = 6; //ONLY USE NUMBERS DIVISIBLE BY 2!!
    private int currentRound;

    [Header("Player Timer")]
    public TimeTracker timeTracker;

    private GameObject player;
    private Camera playerCamera;
    [SerializeField] private FadeToBlack fade;

    [Header("Player Spawning")]
    public Transform[] playerSpawnPoints;
    private List<Transform> availableSpawnPoints = new List<Transform>();
    private Transform previousSpawnPoint;
    public Transform originalSpawnPoint;

    private bool isGameEnded;

    private void Start()
    {
        playerCamera = Camera.main;
        availableSpawnPoints.AddRange(playerSpawnPoints);
        objectToFind = Instantiate(objectToFindPrefab);
        MoveObject();
        isUITimerGoing = true;
        isArrowTimerGoing = true;
        uiTimer = 120;
        uiTimerDuration = maxUITimeUp;
        arrowTimer = 180f;
        StartNewRound();
    }

    private void Update()
    {
        // UI Timer logic
        if (isUITimerGoing)
        {
            if (uiTimer > 0)
                uiTimer -= Time.deltaTime;
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
                uiTimerDuration -= Time.deltaTime;
            else if (uiTimerDuration <= 0)
            {
                userFollowUI.gameObject.SetActive(false);
                uiTimerDuration = maxUITimeUp;
                // Start the player timer
            }
        }

        // Arrow Timer logic
        if (isArrowTimerGoing)
        {
            if (arrowTimer > 0)
                arrowTimer -= Time.deltaTime;
            else if (arrowTimer <= 0)
            {
                helperArrow.gameObject.SetActive(true);
                isArrowTimerGoing = false;
            }
        }
    }

    public void StartNewRound()
    {
        if (currentRound < maxRounds)
        {
            currentRound++;
            PopupUI.text = "Find the " + objectToFind.name;

            if (currentRound > 1)
            {
                // Stop player timer
                RelocatePlayer();
                ResetUI();
            }

            if (currentRound == (maxRounds / 2) + 1)
            {
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
        // Stop player timer
        isGameEnded = true;
        fade.fadeDuration = 2;
        fade.FadeOut();
        timeTracker.saveAllData();
        timeTracker.Export();
        timeTracker.ShowEnding();
        objectToFind.GetComponent<SphereCollider>().enabled = false;
        BoardUI.gameObject.SetActive(false);
        RelocatePlayer();
    }

    private void RelocatePlayer()
    {
        if (!isGameEnded)
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
        userFollowUI.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        fade.fadeDuration = 2;
        fade.FadeOut();
        yield return new WaitForSeconds(3f);
        SpawnPlayer();
        yield return new WaitForSeconds(2f);
        fade.FadeIn();
        fade.fadeDuration = 1;
        isUITimerGoing = true;
        isArrowTimerGoing = true;
        uiTimer = 5f;
    }

    private void SpawnPlayer()
    {
        int randomIndex = Random.Range(0, availableSpawnPoints.Count);
        Transform selectedSpawnPoint = availableSpawnPoints[randomIndex];
        availableSpawnPoints.RemoveAt(randomIndex);
        GameObject player = GameObject.FindWithTag("Player");
        player.transform.position = selectedSpawnPoint.position;
        player.transform.rotation = selectedSpawnPoint.rotation;
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
        int randomIndex = Random.Range(0, objSpawnLocations.Length);
        Transform selectedTransform = objSpawnLocations[randomIndex];
        objectToFind.transform.position = selectedTransform.position;
        objectToFind.transform.rotation = selectedTransform.rotation;
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
