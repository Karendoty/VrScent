using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRInteractionsWithScent : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    [SerializeField] private Transform player;
    private Vector3 direction;
    [SerializeField] private float firstThreshold;
    [SerializeField] private float secondThreshold;
    private float detectionRadius = 5f;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.onSelectEntered.AddListener(OnSelectEntered);
    }

    private void OnSelectEntered(XRBaseInteractor interactor)
    {
        // Cast a ray from the interactor's position and forward direction
        Ray ray = new Ray(interactor.transform.position, interactor.transform.forward);

        // Create a RaycastHit variable to store information about the hit
        RaycastHit hit;

        // Check if the ray hits something
        if (Physics.Raycast(ray, out hit))
        {
            // The ray hit an object
            Debug.Log("Hit object: " + hit.collider.gameObject.name);

            // Check if the hit object is within the scent detection radius
            if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
            {
                Debug.Log("Player detected...");

                // Check scent thresholds
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                if (distanceToPlayer <= firstThreshold)
                {
                    Debug.Log("Player passed threshold one...");
                    // Perform actions for passing threshold one
                }

                if (distanceToPlayer <= secondThreshold)
                {
                    Debug.Log("Player passed threshold two...");
                    // Perform actions for passing threshold two
                }
            }
        }
        else
        {
            // The ray didn't hit anything
            Debug.Log("No hit");
        }
    }
}