using System.Collections.Generic;
using UnityEngine;
using System;

public class SessionTrackingManager : MonoBehaviour
{
    // This script handles tracking a single player session but will append to a file that contains multiple sessions.
    // Each session will be appended to the saved JSON file after each playthrough.

    private PlayerTrackingData currentSession;
    public string currentPlayerId = null;

    // Reference to the PlayerTrackingDataSaver to handle saving/loading the session
    public PlayerTrackingDataSaver trackingDataSaver;

    // File path for saving session data
    private string filePath;

    private void Start()
    {
        // Set the file path to save the session data (same file for all sessions)
        filePath = Application.persistentDataPath + "/player_sessions.json"; // Use plural because it holds multiple sessions

        // Ensure trackingDataSaver is assigned
        if (trackingDataSaver == null)
        {
            trackingDataSaver = GetComponent<PlayerTrackingDataSaver>();
        }
    }

    // Generate a new unique ID using GUID
    private string GenerateUniquePlayerId()
    {
        return Guid.NewGuid().ToString();
    }

    // Start a new session for a player with a unique ID
    public void StartNewSession()
    {
        // Automatically generate a unique player ID
        string playerId = GenerateUniquePlayerId();
        currentPlayerId = playerId;

        // Create a new session for the player
        currentSession = new PlayerTrackingData(playerId);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) // Press 'L' to log the player's session data
        {
            LogCurrentPlayerData();
            LogCurrentPlayerPOIData();
        }

        if (Input.GetKeyDown(KeyCode.S)) // Press 'S' to save the session
        {
            SaveSession();
        }
    }

    // Method to save the current player session to a file, appending it to the existing JSON
    public void SaveSession()
    {
        if (trackingDataSaver != null && currentSession != null)
        {
            // Load existing sessions from the file
            Dictionary<string, PlayerTrackingData> existingSessions = trackingDataSaver.LoadAllSessions(filePath);

            // If no sessions exist, create a new dictionary
            if (existingSessions == null)
            {
                Debug.LogWarning("No existing sessions found. Creating new session dictionary.");
                existingSessions = new Dictionary<string, PlayerTrackingData>();
            }

            // Debug: Check how many sessions are loaded before saving
            Debug.Log("Number of existing sessions: " + existingSessions.Count);

            // Ensure current session has a unique ID
            if (!existingSessions.ContainsKey(currentPlayerId))
            {
                existingSessions.Add(currentPlayerId, currentSession);
            }
            else
            {
                Debug.LogWarning("Session with this ID already exists. Consider generating a new session.");
            }

            // Save all sessions (old + new) back to the file
            trackingDataSaver.SaveAllSessions(existingSessions, filePath);

            Debug.Log("Session saved to " + filePath);
        }
        else
        {
            Debug.LogError("No session to save or PlayerTrackingDataSaver not assigned!");
        }
    }

    // Record the player's path during gameplay
    public void RecordPlayerPosition(Vector3 position)
    {
        if (currentSession != null)
        {
            currentSession.playerPath.Add(position);
        }
    }

    // Update total time for the player
    public void UpdatePlayerTime(float deltaTime)
    {
        if (currentSession != null)
        {
            currentSession.totalTime += deltaTime;
        }
    }

    // Track time spent in POIs for the player (multiple entries/exits tracked separately)
    public void UpdatePOITime(string poiName, float timeSpent)
    {
        if (currentSession != null)
        {
            if (!currentSession.poiTimes.ContainsKey(poiName))
            {
                currentSession.poiTimes[poiName] = new List<float>();
            }

            currentSession.poiTimes[poiName].Add(timeSpent);
        }
    }

    // Get the current session data for visualization later
    public PlayerTrackingData GetCurrentSessionData()
    {
        return currentSession;
    }

    // Method to log all data for the current player session
    public void LogCurrentPlayerData()
    {
        if (currentSession == null)
        {
            Debug.LogWarning("No current session is active.");
            return;
        }

        Debug.Log($"Logging data for Player ID: {currentPlayerId}");
        Debug.Log($"Total Time: {currentSession.totalTime} seconds");

        Debug.Log("Player Path:");
        foreach (var position in currentSession.playerPath)
        {
            Debug.Log($"Position: {position}");
        }
    }

    // Method to log all POI data for the current player session
    public void LogCurrentPlayerPOIData()
    {
        if (currentSession == null)
        {
            Debug.LogWarning("No current session is active.");
            return;
        }

        Debug.Log($"Logging POI data for Player ID: {currentPlayerId}");

        foreach (var poi in currentSession.poiTimes)
        {
            string poiName = poi.Key;
            List<float> timesSpent = poi.Value;

            Debug.Log($"POI: {poiName}");
            float totalTime = 0f;
            int interactionCount = 1;

            foreach (float time in timesSpent)
            {
                Debug.Log($"    Interaction {interactionCount++}: Time spent = {time} seconds");
                totalTime += time;
            }

            Debug.Log($"    Total time spent in {poiName}: {totalTime} seconds");
        }
    }
}