using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRInteractionsWithScent : MonoBehaviour
{
    [SerializeField] private string scent;
    [SerializeField] private Transform player;
    private Vector3 direction;

    [Header("Whiff Lengths")]
    public float activationWhiff1 = 2f;
    public float activationWhiff2 = 5f;
    public float activationWhiff3 = 10f;

    [Header("Threashold Distances")]
    [SerializeField] private float firstThreshold;
    [SerializeField] private float secondThreshold;
    [SerializeField] private float thirdThreshold;

    private bool firstThresholdPassed = false;
    private bool secondThresholdPassed = false;
    private bool thirdThresholdPassed = false;

    [Header("Arduino")]
    [SerializeField] private Arduino_Setting_Polling_Read_Write arduino;

    [Header("Tutorial")]
    public bool isTutorial;

    void Start()
    {
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
                    StartCoroutine(ActivateScent(activationWhiff1));
                    //<--

                    firstThresholdPassed = true;
                }

                // Check scent thresholds
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                if (distanceToPlayer <= secondThreshold && !secondThresholdPassed)
                {
                    Debug.Log("Entered " + gameObject.name + " threshold two...");

                    //Send signal to activate Atomizer longer HERE -->
                    StartCoroutine(ActivateScent(activationWhiff2));
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
                    StartCoroutine(ActivateScent(activationWhiff3));
                    //<--

                    thirdThresholdPassed = true;

                    if(isTutorial)
                    {
                        GameObject.Find("TutorialManager").GetComponent<TutorialManager>().NextPanel();
                    }
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

    IEnumerator ActivateScent(float activationLength)
    {
        switch (scent)
        {
            case "coffee":

                arduino.ToggleFirstAtomizer(true);
                yield return new WaitForSeconds(activationLength);
                arduino.ToggleFirstAtomizer(false);
                break;

            case "cinnamon":
                arduino.ToggleSecondAtomizer(true);
                yield return new WaitForSeconds(activationLength);
                arduino.ToggleSecondAtomizer(false);
                break;

            case "rose":
                arduino.ToggleThirdAtomizer(true);
                yield return new WaitForSeconds(activationLength);
                arduino.ToggleThirdAtomizer(false);
                break;

            case "citrus":
                arduino.ToggleFourthAtomizer(true);
                yield return new WaitForSeconds(activationLength);
                arduino.ToggleFourthAtomizer(false);
                break;
        }
    }
}