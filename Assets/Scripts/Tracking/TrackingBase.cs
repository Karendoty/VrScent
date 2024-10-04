using UnityEngine;
using System.Collections.Generic;
using System.IO;
/*
public class TrackingBase : MonoBehaviour
{
    // Use Unity's persistent data path for saving and loading
    protected string filePath;

    void Awake()
    {
        // Construct the full file path in the persistent data path
        filePath = Path.Combine(Application.persistentDataPath, "player_sessions.json");
    }

    // Log actions to keep track of file saving/loading operations
    protected void LogAction(string message)
    {
        Debug.Log(message);
    }

    // Common method for checking if file exists
    protected bool FileExists(string path)
    {
        return File.Exists(path);
    }

    // Load data from file (common for all subclasses)
    protected string LoadFromFile(string path)
    {
        if (FileExists(path))
        {
            return File.ReadAllText(path);
        }
        LogAction("File not found: " + path);
        return null;
    }

    // Save data to file (common for all subclasses)
    protected void SaveToFile(string path, string data)
    {
        File.WriteAllText(path, data);
        LogAction("Data saved to: " + path);
    }

    // Save all sessions (common for all subclasses)
    protected void SaveAllSessions(Dictionary<string, PlayerTrackingData> allSessions)
    {
        List<PlayerTrackingData> sessionList = new List<PlayerTrackingData>(allSessions.Values);
        string json = JsonUtility.ToJson(new TrackingDataContainer(sessionList));

        SaveToFile(filePath, json);
    }

    // Load all sessions (common for all subclasses)
    protected Dictionary<string, PlayerTrackingData> LoadAllSessions()
    {
        string json = LoadFromFile(filePath);

        if (!string.IsNullOrEmpty(json))
        {
            TrackingDataContainer container = JsonUtility.FromJson<TrackingDataContainer>(json);
            Dictionary<string, PlayerTrackingData> allSessions = new Dictionary<string, PlayerTrackingData>();

            foreach (PlayerTrackingData session in container.sessions)
            {
                allSessions[session.playerId] = session;
            }

            LogAction("Loaded " + allSessions.Count + " sessions from file.");
            return allSessions;
        }

        LogAction("No data loaded.");
        return new Dictionary<string, PlayerTrackingData>();
    }
}

// Data container for serialization
[System.Serializable]
public class TrackingDataContainer
{
    public List<PlayerTrackingData> sessions;

    public TrackingDataContainer(List<PlayerTrackingData> sessions)
    {
        this.sessions = sessions;
    }
}*/