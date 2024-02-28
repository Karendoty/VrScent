using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GoalObjectDectection : MonoBehaviour
{
    private RoundSystem roundSystem;

    [SerializeField] private float timer = 5f;

    private bool thresholdPassed = false;
    public bool isRightObject = false;

    private void Start()
    {
        roundSystem = GameObject.Find("GameManager").GetComponent<RoundSystem>();
    }

    //When player enters the sphere colider it will ask round system if this is the right object. If so start
    //timer and when it reached the end start a new round.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!thresholdPassed)
            {
                thresholdPassed = true;
                roundSystem.CheckObject(gameObject);
            }
            if(isRightObject)
            {
                timer -= Time.deltaTime;
                if (timer >= 0)
                {
                    roundSystem.StartNewRound();
                }
            }
        }
    }

    //Resets timer and bool when player leaves colider.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            thresholdPassed = false;

            timer = 5f;
        }
    }
}
