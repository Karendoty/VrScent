using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class TrackingConfig
{

    public static string FilePath { get; private set; }
    static TrackingConfig()
    {
        // Determine the base directory:
        // In the Editor, use one level up from the Assets folder (i.e. the project root).
        // In a build, use the current directory (where the executable resides).
        string baseDirectory = Application.isEditor
            ? Path.Combine(Application.dataPath, "..")
            : Directory.GetCurrentDirectory();

        // Create (or use) a subfolder "TrackingData" within the base directory.
        string trackingFolder = Path.Combine(baseDirectory, "TrackingData");
        if (!Directory.Exists(trackingFolder))
        {
            Directory.CreateDirectory(trackingFolder);
        }

        // Set the FilePath to point to player_sessions.json within the TrackingData folder.
        FilePath = Path.Combine(trackingFolder, "player_sessions.json");

        // If the file does not exist, create it with an empty TrackingDataContainer.
        if (!File.Exists(FilePath))
        {
            Debug.LogWarning("Session file not found. Creating a new one at " + FilePath);
            File.WriteAllText(FilePath, JsonUtility.ToJson(new TrackingDataContainer(new List<PlayerTrackingData>())));
        }
    }

}