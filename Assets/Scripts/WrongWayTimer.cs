using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class WrongWayTimer
{
    Stopwatch timer = new Stopwatch();



    public void WrongWay(){
        timer.Start();
    }

    public void RightWay(){
        timer.Stop();
    }

    public TimeSpan GetTimeSpan(){
        timer.Stop();
        TimeSpan saveElapese = timer.Elapsed;
        timer.Reset();
        return timer.Elapsed;
    }
}
