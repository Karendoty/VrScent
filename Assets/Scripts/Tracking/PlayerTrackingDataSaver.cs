using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerTrackingDataSaver : MonoBehaviour
{
    // This script handles saving and loading player tracking data to/from a file.
    // It converts the data into JSON format for storage and retrieves it for later use.

    // Save all sessions (serialize to JSON)
    public void SaveAllSessions(Dictionary<string, PlayerTrackingData> allSessions)
    {
        List<PlayerTrackingData> sessionList = new List<PlayerTrackingData>(allSessions.Values);
        string json = JsonUtility.ToJson(new TrackingDataContainer(sessionList));

        Debug.Log("Saving sessions to file: " + TrackingConfig.FilePath);
        Debug.Log("Number of sessions being saved: " + sessionList.Count);

        File.WriteAllText(TrackingConfig.FilePath, json);
    }

    // Load all sessions (deserialize from JSON)
    public Dictionary<string, PlayerTrackingData> LoadAllSessions()
    {
        if (File.Exists(TrackingConfig.FilePath))
        {
            string json = File.ReadAllText(TrackingConfig.FilePath);
            Debug.Log("Loading sessions from file: " + TrackingConfig.FilePath);

            TrackingDataContainer loadedData = JsonUtility.FromJson<TrackingDataContainer>(json);
            Dictionary<string, PlayerTrackingData> allSessions = new Dictionary<string, PlayerTrackingData>();

            if (loadedData != null && loadedData.sessions != null)
            {
                Debug.Log("Number of sessions loaded: " + loadedData.sessions.Count);
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

        Debug.LogWarning("Session file not found: " + TrackingConfig.FilePath);
        return new Dictionary<string, PlayerTrackingData>();
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