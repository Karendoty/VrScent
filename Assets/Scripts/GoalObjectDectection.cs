using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/*
 --Attach this script on objective to find--

 This basically is the trigger to start each new round. When the player enters the radius,
it sets off a timer to make sure the player knows this is the right object. Once finished, 
it tells the RoundSystem script to start a new round.
 */

public class GoalObjectDectection : MonoBehaviour
{
    private RoundSystem roundSystem;
    private SphereCollider col;

    [SerializeField] private float timeToStayInZone; //How long it takes the player to stay in the objective area
    private float timer;

    private bool thresholdPassed = false;
    public bool isTimerGoing = false;

    [Header("Tutorial")]
    public bool isTutorial;

    private void Start()
    {
        col = GetComponent<SphereCollider>();
        timer = timeToStayInZone;
        if (!isTutorial)
        {
            roundSystem = GameObject.Find("GameManager").GetComponent<RoundSystem>();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player found!");
            //--This could honestly be taken out
            if (!thresholdPassed)
            {
                thresholdPassed = true;
                //roundSystem.CheckObject(gameObject);
            }//--
            if (isTutorial)
            {
                GameObject.Find("TutorialManager").GetComponent<TutorialManager>().NextPanel();
            }
        }
    }

    //Detecting to see if the player knows for sure that this is the object they should find.
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (thresholdPassed)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    if (!isTutorial)
                    {
                        isTimerGoing = false;
                        StartCoroutine(DetectionCooldown());
                        roundSystem.StartNewRound();

                        timer = timeToStayInZone;

                    }
                }
            }
        }

    }

    //Resets timer and bool when player leaves colider.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTimerGoing = false;
            thresholdPassed = false;

            timer = timeToStayInZone;
        }
    }

    //Makes sure that the player doesn't trigger a new round multiple times before moving
    IEnumerator DetectionCooldown()
    {
        Debug.Log("Cooling down...");
        col.enabled = false;
        yield return new WaitForSeconds(8f);
        col.enabled = true;

    }
}
