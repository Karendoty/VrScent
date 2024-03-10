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

    List<SaveData> dataList = new List<SaveData>();

    SaveAndLoadData saveSystem = new SaveAndLoadData();

    public TimeSpan ts;
    // Start is called before the first frame update
    void Start()
    {
        stopWatch = new Stopwatch();
        startTimer(); //<-- we might want to change it later to start after player is done w/ tutorial
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void startTimer()
    {
        stopWatch.Reset();
        stopWatch.Start();
        //Invoke("stopTimer",6.1f);
    }
    public void stopTimer()
    {
        stopWatch.Stop();
        ts = stopWatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

        UnityEngine.Debug.Log(elapsedTime);

        Save();

    }

    public TimeSpan time()
    {
        return stopWatch.Elapsed;
    }

    public void Save()
    {
        SaveData send = new SaveData(teleports,ts);
        dataList.Add(send);
        startTimer();
    }

    public void saveAllData(){
        SaveAndLoadData.SaveVRData(dataList);
        

    }

    public List<SaveData> Load()
    {
        //SaveData send = new SaveData(teleports,ts);
        List<SaveData> data = SaveAndLoadData.LoadData();
        UnityEngine.Debug.Log(data.Count);
        return data;
    }

    
    public void LoadShow()
    {
        //SaveData send = new SaveData(teleports,ts);
        
        UnityEngine.Debug.Log(SaveAndLoadData.LoadData().Count);
       // UnityEngine.Debug.Log(data[1].teleports);
        //UnityEngine.Debug.Log(data[1].time);
    }

    string OrganizeOutput(){
        List<SaveData> data = Load();
        string ret = "";
        for(int x = 0; x<data.Count;x++){
            ret+="Teleports: "+data[x].teleports+" time: "+data[x].time+"\n";
        }
        return ret;
    }

    public void Export()
    {
        string data = OrganizeOutput();
        string path = Application.persistentDataPath;

        using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, "export.txt")))
        {
            UnityEngine.Debug.Log(data);
            outputFile.WriteLine(data);
        }


    }

}
