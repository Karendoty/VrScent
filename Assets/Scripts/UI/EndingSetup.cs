using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndingSetup : MonoBehaviour
{
    public TextMeshProUGUI tp;
    public TextMeshProUGUI ts;
    public TimeTracker tracker;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        ts.SetText("Round Times: <br>"+tracker.getTs().ToString());
        tp.SetText("Total Teleports: " + tracker.getTps().ToString());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
