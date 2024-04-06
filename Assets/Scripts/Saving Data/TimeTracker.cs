using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Threading;
using System.IO;
using UnityEngine.XR.Interaction.Toolkit;

public class TimeTracker : MonoBehaviour
{
    Stopwatch stopWatch;
    public int teleports;
    public TeleportationProvider tp;

    List<SaveData> dataList = new List<SaveData>();

    SaveAndLoadData saveSystem = new SaveAndLoadData();

    public TimeSpan goalTS;
    public int goalTP;

    public TimeSpan ts;

    public GameObject endingScreen;
    // Start is called before the first frame update
    void Start()
    {
        goalTS = new TimeSpan(0,1,1);
        stopWatch = new Stopwatch();
        //tp.endLocomotion() += AddTeleport();
        //startTimer(); //<-- we might want to change it later to start after player is done w/ tutorial
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
        SaveData send = new SaveData(teleports,ts,goalTS,goalTP);
        dataList.Add(send);
        //startTimer();
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

    /*public int getTps(){
        //SaveData send = new SaveData(teleports,ts);
        float total = 0;
        TimeSpan totalTime = TimeSpan.Zero;
        
        List<SaveData> loadedData = Load();
        for(int x = 0; x< loadedData.Count; x++){
            total+=loadedData[x].teleports;
            totalTime+=loadedData[x].time;
        }
        float average = total/loadedData.Count;
        TimeSpan averageTime = TimeSpan.FromTicks(totalTime.Ticks / loadedData.Count);


        return CalculatePercentage(average,loadedData[0].goalTeleports);
        //UnityEngine.Debug.Log(CalculatePercentage(averageTime,loadedData[0].goalTime));
    }*/

    public void AddTeleport()
    {
        teleports++;
        UnityEngine.Debug.Log(teleports);
    }

    public int getTps(){
        //SaveData send = new SaveData(teleports,ts);
        int total = 0;
        
        List<SaveData> loadedData = Load();
        for(int x = 0; x< loadedData.Count; x++){
            total+=loadedData[x].teleports;
        }
        float average = total/loadedData.Count;

        return total;
        //UnityEngine.Debug.Log(CalculatePercentage(averageTime,loadedData[0].goalTime));
    }

    public int getTs(){
        //SaveData send = new SaveData(teleports,ts);
        float total = 0;
        TimeSpan totalTime = TimeSpan.Zero;
        
        List<SaveData> loadedData = Load();
        for(int x = 0; x< loadedData.Count; x++){
            total+=loadedData[x].teleports;
            totalTime+=loadedData[x].time;
        }
        float average = total/loadedData.Count;
        TimeSpan averageTime = TimeSpan.FromTicks(totalTime.Ticks / loadedData.Count);


         //CalculatePercentage(average,loadedData[0].goalTeleports);
        return CalculatePercentage(averageTime,loadedData[0].goalTime);
    }
    
    public void LoadShow()
    {




       // UnityEngine.Debug.Log(data[1].teleports);
        //UnityEngine.Debug.Log(data[1].time);
    }

    int CalculatePercentage(float given, int goal){
        if(goal>given){
            return (int)((given/(float)(goal))*100);
        }else{
            return (int)(((float)goal/given)*100);
        }
    }

    int CalculatePercentage(TimeSpan givenTS, TimeSpan goalTS){
        float goal = (float)goalTS.Ticks;
        float given = (float)givenTS.Ticks;
        if(given>goal){
            return((int)((goal/given)*100));
        }else{
            return((int)((given/goal)*100));

        }
    }

    public void ShowEnding(){
        endingScreen.SetActive(true);
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
