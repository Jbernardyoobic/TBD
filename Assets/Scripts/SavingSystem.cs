using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public static class SavingSystem {

    public static void SaveRecords(PlayerData data) {
        BinaryFormatter fm = new BinaryFormatter();
        string path = Application.persistentDataPath + "/records.bin";
        FileStream fs = new FileStream(path, FileMode.Create);
        fm.Serialize(fs, data);
        fs.Close();
    }

    public static PlayerData LoadRecords(PlayerData defaultData, int totalLevel) {
        string path = Application.persistentDataPath + "/records.bin";
        if (File.Exists(path)) {
            BinaryFormatter fm = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);
            PlayerData data = fm.Deserialize(fs) as PlayerData;
            fs.Close();
            return data;
        } else {
            Debug.Log("Save file not found.");
            if (defaultData == null) {
                defaultData = new PlayerData();
            }
            defaultData.InitiateData(totalLevel);
            return defaultData;
        }
    }

    public static void CleanSave() {
        string path = Application.persistentDataPath + "/records.bin";
        File.Delete(path);
    }
}
