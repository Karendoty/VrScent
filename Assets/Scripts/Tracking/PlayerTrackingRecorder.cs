using System.Collections.Generic;
using UnityEngine;

public class PlayerTrackingRecorder : MonoBehaviour
{
    // This script handles the real-time tracking of the player's movement and interactions in the game.
    // It records the player's position and time spent around Points of Interest (POIs) by communicating with the SessionTrackingManager.
    // A unique player session is automatically generated and tracked throughout gameplay.

    public SessionTrackingManager sessionManager;  // Reference to the SessionTrackingManager
    private float sessionStartTime;

    // A dictionary to keep track of when the player enters each POI
    private Dictionary<string, float> poiEntryTimes = new Dictionary<string, float>();

    void Start()
    {
        sessionStartTime = Time.time;

        // Start a new session in the SessionTrackingManager
        sessionManager.StartNewSession();

        // Debug log to confirm that the session was started
        Debug.Log("Session started for player ID: " + sessionManager.currentPlayerId);
    }

    void Update()
    {
        // Record player position
        Vector3 playerPosition = transform.position;
        sessionManager.RecordPlayerPosition(playerPosition);

        // Debug log to track player position in real time
        Debug.Log("Player position recorded at: " + playerPosition);

        // Update player total time
        float deltaTime = Time.deltaTime;
        sessionManager.UpdatePlayerTime(deltaTime);
    }

    // Handle POI entry
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("POI"))
        {
            string poiName = other.gameObject.name;

            // Record the time when the player enters the POI
            if (!poiEntryTimes.ContainsKey(poiName))
            {
                poiEntryTimes[poiName] = Time.time;  // Store the entry time
                Debug.Log("Player entered POI: " + poiName);
            }
        }
    }

    // Handle POI exit
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("POI"))
        {
            string poiName = other.gameObject.name;

            if (poiEntryTimes.ContainsKey(poiName))
            {
                // Calculate the time spent in the POI
                float entryTime = poiEntryTimes[poiName];
                float timeSpent = Time.time - entryTime;

                // Track this individual interaction in the SessionTrackingManager
                sessionManager.UpdatePOITime(poiName, timeSpent);

                // Debug log to track POI exit and individual time spent
                Debug.Log("Player exited POI: " + poiName + ", Time spent in this interaction: " + timeSpent + " seconds");

                // Remove the entry record for this POI
                poiEntryTimes.Remove(poiName);
            }
        }
    }
}