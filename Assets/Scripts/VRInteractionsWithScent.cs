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
    [SerializeField] private float thirdThreshold;

    private bool firstThresholdPassed = false;
    private bool secondThresholdPassed = false;
    private bool thirdThresholdPassed = false;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        //grabInteractable.onSelectEntered.AddListener(OnSelectEntered);
    }

    private void Update()
    {

        direction = player.position - transform.position;

        // Create a RaycastHit variable to store information about the hit
        RaycastHit hit;

        // Check if the ray hits something
        if (Physics.Raycast(transform.position, direction, out hit, firstThreshold))
        {

            // Check if the hit object is within the scent detection radius
            if (Vector3.Distance(transform.position, player.position) <= firstThreshold)
            {
                if (!firstThresholdPassed)
                {
                    Debug.Log("Entered " + gameObject.name + " threshold one...");

                    //Send signal to Atomizer to fire for a small amount of time HERE -->

                    //<--

                    firstThresholdPassed = true;
                }

                // Check scent thresholds
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                if (distanceToPlayer <= secondThreshold && !secondThresholdPassed)
                {
                    Debug.Log("Entered " + gameObject.name + " threshold two...");

                    //Send signal to activate Atomizer longer HERE -->

                    //<--

                    secondThresholdPassed = true;
                }
                else if (distanceToPlayer >= secondThreshold && secondThresholdPassed)
                {
                    secondThresholdPassed = false;
                }

                if (distanceToPlayer <= thirdThreshold && !thirdThresholdPassed)
                {
                    Debug.Log("Entered " + gameObject.name + " threshold three...");

                    //Send signal to activate Atomizer even longer HERE -->

                    //<--

                    thirdThresholdPassed = true;
                }
                else if (distanceToPlayer >= thirdThreshold && thirdThresholdPassed)
                {
                    thirdThresholdPassed = false;
                }
            }
            else
            {
                firstThresholdPassed = false;
                secondThresholdPassed = false;
                thirdThresholdPassed = false;
            }
        }
    }

    private void OnSelectEntered(XRBaseInteractor interactor)
    {

        /*        else
                {
                    // The ray didn't hit anything
                    Debug.Log("No hit");
                }
        */
    }
}