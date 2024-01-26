using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerZoneInteraction : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is a zone
        if (other.CompareTag("Zone"))
        {
            // Get the name of the zone and log it to the console
            string zoneName = other.gameObject.name;
            Debug.Log("Entered Zone: " + zoneName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting object is a zone
        if (other.CompareTag("Zone"))
        {
            // Get the name of the zone and log it to the console
            string zoneName = other.gameObject.name;
            Debug.Log("Exited Zone: " + zoneName);
        }
    }
}