using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveAndLoadData 
{

public static void SaveVRData(List<SaveData> data) {
    BinaryFormatter formatter = new BinaryFormatter();
    string path = Application.persistentDataPath + "/data.vrscent";
    
    using (FileStream stream = new FileStream(path, FileMode.Create)) {
        formatter.Serialize(stream, data);
    }
}

public static List<SaveData> LoadData() {
    string path = Application.persistentDataPath + "/data.vrscent";
    if (File.Exists(path)) {
        try {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open)) {
                List<SaveData> data = formatter.Deserialize(stream) as List<SaveData>;
                if (data != null) {
                    return data;
                } else {
                    Debug.Log("Failed to deserialize data.");
                    return new List<SaveData>();
                }
            }
        } catch (Exception e) {
            Debug.Log("Error loading data: " + e.Message);
            return new List<SaveData>();
        }
    } else {
        Debug.Log("Save file not found.");
        return new List<SaveData>();
    }
}

}
