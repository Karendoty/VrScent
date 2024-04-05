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
        tp.SetText("Teleport Percentage: "+tracker.getTps().ToString()+"%");
        ts.SetText("Time Percentage: "+tracker.getTs().ToString()+"%");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
