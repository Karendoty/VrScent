using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GoalObjectDectection : MonoBehaviour
{
    private RoundSystem roundSystem;
    private SphereCollider col;

    [SerializeField] private float timeToStayInZone;
    private float timer;

    private bool thresholdPassed = false;
    //public bool isRightObject = false;
    public bool isTimerGoing = false;

    private void Start()
    {
        col = GetComponent<SphereCollider>();
        roundSystem = GameObject.Find("GameManager").GetComponent<RoundSystem>();
        timer = timeToStayInZone;
    }

    //When player enters the sphere colider it will ask round system if this is the right object. If so start
    //timer and when it reached the end start a new round.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player found!");

            if (!thresholdPassed)
            {
                thresholdPassed = true;
                //roundSystem.CheckObject(gameObject);
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") /*&& isTimerGoing*/)
        {
            if (thresholdPassed)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    isTimerGoing = false;
                    StartCoroutine(DetectionCooldown());
                    roundSystem.StartNewRound();

                    timer = timeToStayInZone;
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

    IEnumerator DetectionCooldown()
    {
        Debug.Log("Cooling down...");
        col.enabled = false;
        yield return new WaitForSeconds(8f);
        col.enabled = true;

    }
}
