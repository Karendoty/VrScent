using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ZoneTutorial : MonoBehaviour
{
    public Transform centerPosition;
    public Transform[] zones;
    public GameObject arrowPrefab;
    public Canvas canvas;

    private int currentZoneIndex = 0;
    private GameObject arrow;

    void Start()
    {
     
        StartCoroutine(StartTutorial());
    }

    IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(1f);

        // Create arrow pointing towards the first zone
        arrow = Instantiate(arrowPrefab, canvas.transform);
        UpdateArrowDirection(zones[currentZoneIndex].position);

        // Wait for the player to reach each zone and then move to the next one
        for (int i = 0; i < zones.Length; i++)
        {
            while (Vector3.Distance(transform.position, zones[currentZoneIndex].position) > 1f)
            {
                UpdateArrowDirection(zones[currentZoneIndex].position);
                yield return null;
            }

            currentZoneIndex++;

            // If all zones visited, return to center
            if (currentZoneIndex >= zones.Length)
            {
                break;
            }

            // Create arrow pointing towards the next zone
            UpdateArrowDirection(zones[currentZoneIndex].position);
        }

       
        UpdateArrowDirection(centerPosition.position);
        Debug.Log("Tutorial completed!");
    }

    void UpdateArrowDirection(Vector3 targetPosition)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetPosition);
        Vector3 direction = (targetPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        arrow.transform.position = screenPos;
    }
}