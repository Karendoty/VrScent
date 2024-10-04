using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerTrackingDataSaver : MonoBehaviour
{
    // This script handles saving and loading player tracking data to/from a file.
    // It converts the data into JSON format for storage and retrieves it for later use.

    // Save all sessions (serialize to JSON)
    public void SaveAllSessions(Dictionary<string, PlayerTrackingData> allSessions, string filePath)
    {
        List<PlayerTrackingData> sessionList = new List<PlayerTrackingData>(allSessions.Values);
        string json = JsonUtility.ToJson(new TrackingDataContainer(sessionList));

        // Debug: Log what is being saved
        Debug.Log("Saving sessions to file: " + filePath);
        Debug.Log("Number of sessions being saved: " + sessionList.Count);

        File.WriteAllText(filePath, json);
    }

    // Load all sessions (deserialize from JSON)
    public Dictionary<string, PlayerTrackingData> LoadAllSessions(string filePath)
    {
        // Check if the file exists
        if (File.Exists(filePath))
        {
            // Read the file content
            string json = File.ReadAllText(filePath);
            Debug.Log("Loading sessions from file: " + filePath);

            // Deserialize the JSON into TrackingDataContainer object
            TrackingDataContainer loadedData = JsonUtility.FromJson<TrackingDataContainer>(json);

            // Create a dictionary to hold all loaded sessions
            Dictionary<string, PlayerTrackingData> allSessions = new Dictionary<string, PlayerTrackingData>();

            // Ensure that sessions are present
            if (loadedData != null && loadedData.sessions != null)
            {
                Debug.Log("Number of sessions loaded: " + loadedData.sessions.Count);

                // Loop through each session and add to the dictionary
                foreach (var session in loadedData.sessions)
                {
                    Debug.Log($"Loaded session for Player ID: {session.playerId}, Path Points: {session.playerPath.Count}");
                    allSessions.Add(session.playerId, session);
                }
            }
            else
            {
                Debug.LogWarning("No session data found in file.");
            }

            return allSessions;
        }

        Debug.LogWarning("Session file not found: " + filePath);
        return new Dictionary<string, PlayerTrackingData>(); // Return empty dictionary if no file exists
    }
}

[System.Serializable]
public class TrackingDataContainer
{
    public List<PlayerTrackingData> sessions;

    public TrackingDataContainer(List<PlayerTrackingData> sessions)
    {
        this.sessions = sessions;
    }
}