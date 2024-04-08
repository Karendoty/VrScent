using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointAI : MonoBehaviour
{
    public List<Transform> waypoints; // List of waypoints the AI will move between
    public float moveSpeed = 3f; // Speed at which the AI moves
    public float waypointWaitTime = 2f; // Time the AI waits at each waypoint

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
            // Get the position of the current waypoint
            Vector3 targetPosition = waypoints[currentWaypointIndex].position;

            // Move towards the waypoint
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Wait at the waypoint for a specified time
            yield return new WaitForSeconds(waypointWaitTime);

            // Stop walking and play idle animation
            SetAnimation(false);

            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            targetPosition = waypoints[currentWaypointIndex].position;

            // Start walking towards the next waypoint
            SetAnimation(true);
        }
    }
}
