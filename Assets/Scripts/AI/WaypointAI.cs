using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointAI : MonoBehaviour
{
    public List<Transform> waypoints; // List of waypoints the AI will move between
    public float moveSpeed = 3f; // Speed at which the AI moves
    public float waypointWaitTime = 2f; // Time the AI waits at each waypoint
    public float waypointReachedThreshold = 0.1f; // Distance threshold for considering a waypoint reached

    public AnimationClip idleAnimation;
    public AnimationClip walkingAnimation;

    private Animator animator;
    private int currentWaypointIndex = 0;
    private bool isWalking = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator != null && waypoints.Count > 0)
        {
            // Start walking towards the first waypoint
            SetAnimation(true);
            StartCoroutine(MoveToNextWaypoint());
        }
    }

    void SetAnimation(bool walking)
    {
        isWalking = walking;

        if (animator != null)
        {
            if (walking && walkingAnimation != null)
            {
                // Play walking animation with the correct speed
                animator.Play(walkingAnimation.name);
            }
            else if (!walking && idleAnimation != null)
            {
                // Play idle animation with the correct speed
                animator.Play(idleAnimation.name);
            }
        }
    }

    IEnumerator MoveToNextWaypoint()
    {
        while (true)
        {
            // Check if there are waypoints
            if (waypoints.Count == 0)
            {
                Debug.LogError("No waypoints assigned to the AI.");
                yield break;
            }

            // Get the position of the current waypoint
            Vector3 targetPosition = waypoints[currentWaypointIndex].position;

            // Move towards the waypoint
            while (Vector3.Distance(transform.position, targetPosition) > waypointReachedThreshold)
            {
                // Move towards the waypoint
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                // Debug: Visualize the path
                Debug.DrawLine(transform.position, targetPosition, Color.blue);

                yield return null;
            }

            // Wait at the waypoint for a specified time
            SetAnimation(false); // Stop walking
            yield return new WaitForSeconds(waypointWaitTime);

            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;

            // Start walking towards the next waypoint
            SetAnimation(true);
        }
    }
}