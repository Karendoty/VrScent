using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundSystem : MonoBehaviour
{
    public GameObject[] objectsToFind;
    //private List<string> foundObjects = new List<string>();
    private List<GameObject> availableObjects = new List<GameObject>();

    public TMP_Text BoardUI;
    public UserFollowUI userFollowUI;

    private int currentRound;
    private int maxRounds = 6;

    GameObject object1;
    GameObject object2;

    void Start()
    {
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
        Debug.Log("1");
        if (currentRound <= 3)
        {
            Debug.Log("2");

            if (scentObject.name == object1.name)
            {
                Debug.Log("3");

                StartNewRound();
            }
        }
        else
        {
            Debug.Log("4");

            if (scentObject.name == object2.name)
            {
                Debug.Log("5");
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
                RelocatePlayer();
            }

            if (currentRound <= 3)
            {
                Debug.Log("Find " + object1.name);
                BoardUI.text = "Find " + object1.name;
            }
            else
            {
                Debug.Log("Find " + object2.name);
                BoardUI.text = "Find " + object2.name;
            }
        }

        /*HighlightObject(object1);
        HighlightObject(object2);*/
    }

    //Move player to a random location in the map
    private void RelocatePlayer()
    {
        Debug.Log("Relocating player...");
        //fade out
        //teleport player to random position
        //fade in
        //show new objective
    }

    GameObject GetRandomObject()
    {
        //Need to make it so that there are no chances of duplicates
        //return objectsToFind[Random.Range(0, objectsToFind.Length)];

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

        yield return new WaitForSeconds(10f);

        userFollowUI.gameObject.SetActive(false);
    }
}