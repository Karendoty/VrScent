using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SaveData 
{
  public int teleports;
  public TimeSpan time;
  public TimeSpan goalTime;
  public int goalTeleports;

    
    public SaveData(int tp, TimeSpan ts,TimeSpan gTs, int gTp){
        time=ts;
        teleports=tp;
        goalTime=gTs;
        goalTeleports=gTp;
    }
}
