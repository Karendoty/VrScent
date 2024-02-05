using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SaveData 
{
  public int teleports;
  public TimeSpan time;
    
    public SaveData(int tp, TimeSpan ts){
        time=ts;
        teleports=tp;
    }
}
