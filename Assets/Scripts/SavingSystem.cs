using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SavingSystem {

    public static void SaveRecords (PlayerRecordData data) {
        BinaryFormatter fm = new BinaryFormatter();
        string path = Application.persistentDataPath + "/records.bin";
        FileStream fs = new FileStream(path, FileMode.Create);
        fm.Serialize(fs, data);
        fs.Close();
    }

    public static PlayerRecordData LoadRecords () {
        string path = Application.persistentDataPath + "/records.bin";
        if (File.Exists(path)) {
            BinaryFormatter fm = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);
            PlayerRecordData data = fm.Deserialize(fs) as PlayerRecordData;
            fs.Close();
            return data;
        } else {
            Debug.Log("Save file not found.");
            return null;
        }
    }
}
