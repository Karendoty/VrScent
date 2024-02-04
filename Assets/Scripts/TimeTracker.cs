using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Threading;
using System.IO;

public class TimeTracker : MonoBehaviour
{
    Stopwatch stopWatch;
    public int teleports;

    public TimeSpan ts;
    // Start is called before the first frame update
    void Start()
    {
        stopWatch=new Stopwatch();
        startTimer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startTimer(){
        stopWatch.Reset();
        stopWatch.Start();
        //Invoke("stopTimer",6.1f);
    }
    public void stopTimer(){
        stopWatch.Stop();
                ts = stopWatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

            UnityEngine.Debug.Log(elapsedTime);
    
        Save();

    }
 
    public TimeSpan time(){
        return stopWatch.Elapsed;
    }

    public void Save(){
        //SaveData send = new SaveData(teleports,ts);
        SaveAndLoadData.SaveVRData(teleports,ts);
    }

        public void Load(){
        //SaveData send = new SaveData(teleports,ts);
       SaveData data = SaveAndLoadData.LoadData();
       UnityEngine.Debug.Log(data.teleports);
       UnityEngine.Debug.Log(data.time);
    }

    public void Export(){
        string data = "Teleports: "+teleports+" time: "+ts;
        string path = Application.persistentDataPath;

        using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, "export.txt")))
        {
            
                outputFile.WriteLine(data);
        }


    }

}
