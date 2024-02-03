using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundSystem : MonoBehaviour
{
    public GameObject[] objectsToFind;
    private List<string> foundObjects = new List<string>();

    [SerializeField] TMP_Text BoardUI;

    private int currentRound;
    private int maxRounds = 6;

    GameObject object1;
    GameObject object2;

    void Start()
    {
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
        return objectsToFind[Random.Range(0, objectsToFind.Length)];
    }

    /*void HighlightObject(GameObject obj)
    {
        Debug.Log("Highlighting " + obj.name);

        // Check if the object has already been found
        if (!foundObjects.Contains(obj.name))
        {

            foundObjects.Add(obj.name);
            ShowMessage("You found " + obj.name);
        }
    }

    void ShowMessage(string message)
    {
        // Display the message in the debug log
        Debug.Log(message);
    }*/
}