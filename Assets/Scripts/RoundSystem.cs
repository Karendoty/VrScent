using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RoundSystem : MonoBehaviour
{
    [Header("Scent Objects")]
    public GameObject[] objectsToFind;
    private Vector3[] objLocations;
    private List<GameObject> availableObjects = new List<GameObject>();

    [Header("UI")]
    public TMP_Text BoardUI;
    public UserFollowUI userFollowUI;
    //[SerializeField] private float UIPopupSpeed = 5f;

    private int currentRound;
    private int maxRounds = 6;

    GameObject object1;
    GameObject object2;

    private TimeTracker timeTracker;

    private GameObject player;
    private Camera playerCamera;
    OVRScreenFade playerScreen;

    void Start()
    {
        timeTracker = GetComponent<TimeTracker>();
        player = GameObject.FindWithTag("Player");
        playerCamera = Camera.main;

        objLocations = new Vector3[objectsToFind.Length];

        for (int i = 0; i < objectsToFind.Length; i++)
        {
            objLocations[i] = objectsToFind[i].transform.position;
        }

        //Populate the temporary list with available objects
        availableObjects.AddRange(objectsToFind);

        // Select two random objects
        object1 = GetRandomObject();
        object2 = GetRandomObject();

        StartNewRound();
    }

    public void CheckObject(GameObject scentObject)
    {
        Debug.Log(scentObject.name);
        //Debug.Log("1");
        if (currentRound <= 3)
        {
            //Debug.Log("2");

            if (scentObject.name == object1.name)
            {
                //Debug.Log("3");

                StartNewRound();
            }
        }
        else
        {
            //Debug.Log("4");

            if (scentObject.name == object2.name)
            {
                //Debug.Log("5");
                StartNewRound();
            }
        }
    }

    void StartNewRound()
    {
        
        if (currentRound <= maxRounds)
        {
            currentRound++;
            Debug.Log("Round " + currentRound);
            StartCoroutine(HelperUI());

            if (currentRound > 1)
            {
                timeTracker.stopTimer();
                RelocatePlayer();
            }

            if (currentRound <= 3)
            {
                
                Debug.Log("Find the " + object1.name);
                BoardUI.text = "Find the " + object1.name;
            }
            else if (currentRound <= 4)
            {
                currentRound++;
                Debug.Log("Round " + currentRound);
                Debug.Log("Find the " + object2.name);
                BoardUI.text = "Find the " + object2.name;
            }
            else
            {
                EndSimulation();
            }
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

    GameObject GetRandomObject()
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

        Debug.Log(selectedObject.name);
        return selectedObject;
    }

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
        for (int i = 0; i < objectsToFind.Length; i++)
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
        }


        //right now it only teleports them to the begining but ideally it would be randomly in the map
        player.transform.position = new Vector3(0, player.transform.position.y, -6);
        player.transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(2f);

        //fade in
        playerScreen.FadeIn();
        playerScreen.fadeTime = 1; //resets back to default

        timeTracker.startTimer();
        StartCoroutine(NewObjectiveUI());
    }

    private IEnumerator NewObjectiveUI()
    {
        userFollowUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);

        userFollowUI.gameObject.SetActive(false);
    }
}