using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmellIncrease : MonoBehaviour
{
    public float smellIncreaseRate = 0.1f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SmellZone1"))
        {
            //IncreaseSmell(1);
        }
        else if (other.CompareTag("SmellZone2"))
        {
            //IncreaseSmell(2);
        }
        else if (other.CompareTag("SmellZone3"))
        {
            //IncreaseSmell(3);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
