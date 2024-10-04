using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerTrackingData
{
    // Data Structure to hold Players Tracking Info
    public string playerId;  // Unique ID for each player
    public List<Vector3> playerPath = new List<Vector3>();  // Store the path
    public float totalTime;  // Time taken to complete the demo
    public Dictionary<string, List<float>> poiTimes = new Dictionary<string, List<float>>();  // Track time spent at each POI with multiple entries/exits

    public PlayerTrackingData(string id)
    {
        playerId = id;
    }
}