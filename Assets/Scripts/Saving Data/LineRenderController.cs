using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Camera projectionCamera;
    [SerializeField] private Transform startCube;
    [SerializeField] private Transform endCube;
    [SerializeField] private List<Material> lineMaterials = new List<Material>();
    private List<Transform> playerTransforms = new List<Transform>();
    private LineRenderer lineRenderer;

    GameObject playerPoint;

    private int totalPoints = 0;
    private int round = 0;
    private bool enableRenderer = false;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        
    }

    private void Update()
    {
        if (enableRenderer)
        {
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                lineRenderer.SetPosition(i, playerTransforms[i].position);
            }
            Screenshot();
            enableRenderer = false;
            ResetTransformPoints();
        }
    }

    public void LogPlayersTransform()
    {
        
        totalPoints++;
        playerPoint = new GameObject("point " + totalPoints);
        playerPoint.transform.position = player.transform.position;
        //playerPoint.transform.rotation = player.transform.rotation;
        playerTransforms.Add(playerPoint.transform);

        if(playerTransforms.Count == 1)
        {
            startCube.position = playerTransforms[0].position;
        }
    }

    public void RoundDone()
    {
        round++;
        Debug.Log("Rendering lines...");
        enableRenderer = true;
        lineRenderer.positionCount = playerTransforms.Count;
        if (round > 1)
        {
            lineRenderer.material = lineMaterials[round];
        }
    }

    public void MarkEndLocation()
    {
        endCube.position = playerTransforms[playerTransforms.Count - 1].position;
    }

    private void Screenshot()
    {
        Debug.Log("Saving image...");
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        projectionCamera.targetTexture = renderTexture;

        projectionCamera.Render();

        RenderTexture.active = renderTexture;

        Texture2D screenshotTexture = new Texture2D(renderTexture.width, renderTexture.height);

        screenshotTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

        screenshotTexture.Apply();

        // Save the texture as a PNG file
        byte[] bytes = screenshotTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes("Round " + round + ".png", bytes);

        projectionCamera.targetTexture = null;

        RenderTexture.active = null;
        Destroy(renderTexture);
    }

    private void ResetTransformPoints()
    {
        foreach (Transform point in playerTransforms)
        {
            Destroy(point.gameObject);
        }
        playerTransforms.Clear();
    }
}
