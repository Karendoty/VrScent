using System.Collections.Generic;
using UnityEngine;

public class TrackingVisualizer : MonoBehaviour
{
    public SessionTrackingManager sessionManager;  // Reference to the session manager
    public PlayerTrackingDataSaver trackingDataSaver;  // Reference to the tracking data saver

    public GameObject startMarkerPrefab;
    public GameObject endMarkerPrefab;

    private Dictionary<string, LineRenderer> lineRenderers = new Dictionary<string, LineRenderer>();

    public Material[] lineMaterials;  // LineRenderer materials to cycle through
    private int materialIndex = 0;  // To track the current material being used

    private string filePath;
    private Dictionary<string, PlayerTrackingData> allSessionsCache;

    private GameObject startMarker;

    private void Start()
    {
        filePath = Application.persistentDataPath + "/player_sessions.json";

        if (trackingDataSaver == null)
        {
            trackingDataSaver = GetComponent<PlayerTrackingDataSaver>();
        }

        allSessionsCache = trackingDataSaver.LoadAllSessions(filePath);

        if (startMarkerPrefab != null)
        {
            startMarker = Instantiate(startMarkerPrefab);
            startMarker.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            VisualizeAllSessions();
        }
    }

    public void VisualizeAllSessions()
    {
        if (allSessionsCache != null)
        {
            foreach (var session in allSessionsCache)
            {
                Debug.Log("Visualizing path for Player ID: " + session.Key);
                VisualizePlayerPath(session.Key, allSessionsCache);
            }
        }
        else
        {
            Debug.LogWarning("No saved sessions found in the file.");
        }
    }

    public void VisualizePlayerPath(string playerId, Dictionary<string, PlayerTrackingData> allSessions)
    {
        if (allSessions.ContainsKey(playerId))
        {
            PlayerTrackingData sessionData = allSessions[playerId];
            if (sessionData != null && sessionData.playerPath.Count > 0)
            {
                LineRenderer lineRenderer;

                if (!lineRenderers.ContainsKey(playerId))
                {
                    GameObject lineObj = new GameObject("LineRenderer_" + playerId);
                    lineRenderer = lineObj.AddComponent<LineRenderer>();

                    lineRenderer.startWidth = 0.1f;
                    lineRenderer.endWidth = 0.1f;

                    if (lineMaterials.Length > 0)
                    {
                        lineRenderer.material = lineMaterials[materialIndex];
                        materialIndex = (materialIndex + 1) % lineMaterials.Length;
                    }

                    lineRenderers.Add(playerId, lineRenderer);
                }
                else
                {
                    lineRenderer = lineRenderers[playerId];
                }

                Vector3[] positions = sessionData.playerPath.ToArray();
                lineRenderer.positionCount = positions.Length;
                lineRenderer.SetPositions(positions);

                PlaceMarkers(lineRenderer.gameObject, sessionData);  // Pass the line object to make end marker a child
            }
            else
            {
                Debug.LogWarning("No session data found or no path for player: " + playerId);
            }
        }
        else
        {
            Debug.LogWarning("Player session not found for Player ID: " + playerId);
        }
    }

    private void PlaceMarkers(GameObject lineObj, PlayerTrackingData sessionData)
    {
        Vector3 startPosition = sessionData.playerPath[0];
        Vector3 endPosition = sessionData.playerPath[sessionData.playerPath.Count - 1];

        if (startMarker != null)
        {
            startMarker.transform.position = startPosition;
            startMarker.SetActive(true);
        }

        if (endMarkerPrefab != null)
        {
            GameObject endMarker = Instantiate(endMarkerPrefab);
            endMarker.transform.position = endPosition;
            endMarker.transform.SetParent(lineObj.transform);  // Make the end marker a child of the line renderer's GameObject
            endMarker.SetActive(true);
        }
    }
}