using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class RoundSystem : MonoBehaviour
{
    [Header("Goal Objects")]
    //public GameObject[] objectsToFind;
    [SerializeField] GameObject objectToFindPrefab;
    private GameObject objectToFind;
    [SerializeField] private Transform[] objSpawnLocations;
    private List<Transform> availableSpawnLocations = new List<Transform>();
    private Transform initialObjectSpawn;
    private bool switchObjLocation = false;

    [Header("UI")]
    public TMP_Text BoardUI;
    public UserFollowUI userFollowUI;
    //[SerializeField] private float UIPopupSpeed = 5f;

    private int currentRound;
    private int maxRounds = 4; //ONLY USE EITHER 4 OR 8!!

    private TimeTracker timeTracker;

    private GameObject player;
    private Camera playerCamera;
    OVRScreenFade playerScreen;

    [Header("Spawning")]
    public Transform[] playerSpawnPoints;
    private List<Transform> availableSpawnPoints = new List<Transform>();
    private Transform previousSpawnPoint;

    void Start()
    {
        timeTracker = GetComponent<TimeTracker>();
        player = GameObject.FindWithTag("Player");
        playerCamera = Camera.main;

        //Randomly gets and object from the array
        //int obj = Random.Range(0, objectsToFind.Length);
        //objectToFind = objectsToFind[obj];

        //objLocations = new Vector3[objectsToFind.Length];

        /*for (int i = 0; i < objectsToFind.Length; i++)
        {
            objLocations[i] = objectsToFind[i].transform.position;
        }*/

        //Populate the temporary list with available spawn points
        availableSpawnLocations.AddRange(objSpawnLocations);
        availableSpawnPoints.AddRange(playerSpawnPoints);

        /*        // Select two random objects
                object1 = GetRandomObject();
                object2 = GetRandomObject();
        */
        objectToFind = Instantiate(objectToFindPrefab);
        objectToFind.name = objectToFindPrefab.name;
        MoveObject();

        StartNewRound();
    }

    /*    public void CheckObject(GameObject goalObject)
        {
            if (goalObject.name == objectToFind.name)
            {
                objectToFind.GetComponent<GoalObjectDectection>().isRightObject = true;
                objectToFind.GetComponent<GoalObjectDectection>().isTimerGoing = true;
            }
        }
    */
    public void StartNewRound()
    {
        if (currentRound <= maxRounds)
        {
            currentRound++;
            Debug.Log("Round " + currentRound);
            Debug.Log("Find the " + objectToFind.name);
            BoardUI.text = "Find the " + objectToFind.name;

            StartCoroutine(HelperUI());

            if (currentRound > 1)
            {
                timeTracker.stopTimer();
                RelocatePlayer();
            }

            //moves object to find to a random position after halfway through.
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

        /*HighlightObject(object1);
        HighlightObject(object2);*/
    }

    private void EndSimulation()
    {
        timeTracker.Export();
        //Maybe move the player?
        //Show score and time
    }

    //Move player to a random location in the map
    private void RelocatePlayer()
    {
        StartCoroutine(MovePlayer());
    }

    /*GameObject GetRandomObject()
    {
        if (availableObjects.Count == 0)
        {
            Debug.LogError("No objects left to find!");
            return null;
        }

        // Choose a random index within the range of available objects
        int randomIndex = Random.Range(0, availableObjects.Count);

        // Retrieve the object at the randomly chosen index
        GameObject selectedObject = availableObjects[randomIndex];

        // Remove the selected object from the temporary list
        availableObjects.RemoveAt(randomIndex);

        //Debug.Log(selectedObject.name);
        return selectedObject;
    }*/

    private IEnumerator HelperUI()
    {
        yield return new WaitForSeconds(90f);

        userFollowUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);

        userFollowUI.gameObject.SetActive(false);
    }
    private IEnumerator MovePlayer()
    {
        Debug.Log("Relocating player...");

        yield return new WaitForSeconds(2f);

        //fade out
        playerScreen = playerCamera.GetComponent<OVRScreenFade>();
        playerScreen.fadeTime = 2; //increase fade time
        playerScreen.FadeOut();

        yield return new WaitForSeconds(3f);

        //Relocate all object to respective spots
        /* for (int i = 0; i < objectsToFind.Length; i++)
         {
             // Get the XRGrabInteractable component attached to the current object
             XRGrabInteractable grabInteractable = objectsToFind[i].GetComponent<XRGrabInteractable>();

             // Check if the grabInteractable is not null before attempting to access its properties
             if (grabInteractable != null)
             {
                 // Optionally, you might want to set the object as not being currently grabbed
                 if (grabInteractable.isSelected)
                 {
                     grabInteractable.enabled = false;
                     grabInteractable.enabled = true;
                 }
                 // Set the position of the current object to the corresponding position from objLocations
                 objectsToFind[i].transform.position = objLocations[i];
             }
             else
             {
                 Debug.LogWarning("XRGrabInteractable component not found on object: " + objectsToFind[i].name);
             }
         }*/


        //right now it only teleports them to the begining but ideally it would be randomly in the map
        //player.transform.position = new Vector3(0, player.transform.position.y, -6);
        //player.transform.rotation = Quaternion.identity;

        // Teleport player to a random spawn point
        Transform randomSpawnPoint = playerSpawnPoints[UnityEngine.Random.Range(0, playerSpawnPoints.Length)];
        player.transform.position = randomSpawnPoint.position;
        player.transform.rotation = randomSpawnPoint.rotation;


        yield return new WaitForSeconds(2f);

        //fade in
        playerScreen.FadeIn();
        playerScreen.fadeTime = 1; //resets back to default

        timeTracker.startTimer();
        StartCoroutine(NewObjectiveUI());
    }

    /*    private Transform RandomSpawnPoint()
        {
            if (availableSpawnPoints.Count == 0)
            {
                Debug.LogError("No spawn points available!");
                return null;
            }

            Transform selectedSpawnPoint;

            // If the previous spawn point is not null, remove it from available spawn points
            if (previousSpawnPoint != null)
            {
                availableSpawnPoints.Remove(previousSpawnPoint);
            }

            Debug.Log("Available spawn points count: " + availableSpawnPoints.Count);

            // Choose a random index within the range of available spawn points
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);

            Debug.Log("Generated random index: " + randomIndex);

            // Retrieve the spawn point at the randomly chosen index
            selectedSpawnPoint = availableSpawnPoints[randomIndex];

            // Set the current spawn point as the previous spawn point for next spawn
            previousSpawnPoint = selectedSpawnPoint;

            // Return the selected spawn point
            return selectedSpawnPoint;

        }
    */
    /*    private Transform MoveObject()
        {

            if (availableSpawnLocations.Count == 0)
            {
                Debug.LogError("No spawn locations found!");
                return null;
            }

            if(currentRound == (maxRounds / 2))
            {
                string initialSpawnName = initialObjectSpawn.name;
                switch (initialSpawnName)
                {
                    case "Point 1":
                        return objSpawnLocations[2];
                    case "Point 2":
                        return objSpawnLocations[3];
                    case "Point 3":
                        return objSpawnLocations[0];
                    case "Point 4":
                        return objSpawnLocations[1];
                }

            }

            int randomIndex = UnityEngine.Random.Range(0, availableSpawnLocations.Count);

            Transform selectedTransform = objSpawnLocations[randomIndex];

            initialObjectSpawn = selectedTransform;

            availableSpawnLocations.RemoveAt(randomIndex);

            Debug.Log(selectedTransform.name);
            return selectedTransform;
        }
    */

    private void MoveObject()
    {

        if (availableSpawnLocations.Count == 0)
        {
            Debug.LogError("No spawn locations found!");
        }

        if (switchObjLocation)
        {
            string initialSpawnName = initialObjectSpawn.name;
            switch (initialSpawnName)
            {
                //objSpawnLocations[0]
                case "Point 1":
                    Debug.Log("Going to Point 3");
                    objectToFind.transform.position = objSpawnLocations[2].position;
                    objectToFind.transform.rotation = objSpawnLocations[2].rotation;
                    break;
                //objSpawnLocations[1]
                case "Point 2":
                    Debug.Log("Going to Point 4");
                    objectToFind.transform.position = objSpawnLocations[3].position;
                    objectToFind.transform.rotation = objSpawnLocations[3].rotation;
                    break;
                //objSpawnLocations[2]
                case "Point 3":
                    Debug.Log("Going to Point 1");
                    objectToFind.transform.position = objSpawnLocations[0].position;
                    objectToFind.transform.rotation = objSpawnLocations[0].rotation;
                    break;
                //objSpawnLocations[3]
                case "Point 4":
                    Debug.Log("Going to Point 2");
                    objectToFind.transform.position = objSpawnLocations[1].position;
                    objectToFind.transform.rotation = objSpawnLocations[1].rotation;
                    break;
            }

        }
        else
        {
            int randomIndex = UnityEngine.Random.Range(0, objSpawnLocations.Length);

            Transform selectedTransform = objSpawnLocations[randomIndex];

            initialObjectSpawn = selectedTransform;

            Debug.Log(selectedTransform.name);

            objectToFind.transform.position = selectedTransform.position;
            objectToFind.transform.rotation = selectedTransform.rotation;
        }

    }


    private IEnumerator NewObjectiveUI()
    {
        userFollowUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);

        userFollowUI.gameObject.SetActive(false);
    }
}