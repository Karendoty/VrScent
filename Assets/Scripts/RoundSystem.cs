using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSystem : MonoBehaviour
{
     public GameObject[] objectsToFind;
    private List<string> foundObjects = new List<string>();

    void Start()
    {
        StartNewRound();
    }

    void StartNewRound()
    {
        // Select two random objects
        GameObject object1 = GetRandomObject();
        GameObject object2 = GetRandomObject();

        // Give the player a task in the debug log
        Debug.Log("Find " + object1.name + " and " + object2.name);

        HighlightObject(object1);
        HighlightObject(object2);
    }

    GameObject GetRandomObject()
    {
        return objectsToFind[Random.Range(0, objectsToFind.Length)];
    }

    void HighlightObject(GameObject obj)
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
    }
}