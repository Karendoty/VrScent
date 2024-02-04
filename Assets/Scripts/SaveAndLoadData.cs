using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveAndLoadData 
{

    public static void SaveVRData(int tp, TimeSpan ts){
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath+"/data.vrscent";
        FileStream stream = new FileStream(path,FileMode.Create);

        SaveData data = new SaveData(tp,ts);

        formatter.Serialize(stream,data);
        stream.Close();
    }

    public static SaveData LoadData(){
        string path = Application.persistentDataPath + "/data.vrscent";
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;

            return data;
        }else{
            return null;
        }
    }

}
