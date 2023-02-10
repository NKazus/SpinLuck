using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public interface ISaveManager
{
    public void Save(SaveData data);
    public SaveData Load();
    public void Reset();
}
public class SaveManager : ISaveManager
{
    private BinaryFormatter bFormatter = new BinaryFormatter();
    private string savePath = Application.persistentDataPath + "/MySaveData.dat";

    public void Save(SaveData saveData)
    {
        BinaryFormatter bFormatter = new BinaryFormatter();
        FileStream file = File.Create(savePath);

        bFormatter.Serialize(file, saveData);
        file.Close();
        Debug.Log("Data saved.");

    }

    public SaveData Load()
    {
        SaveData saveData;
        if (File.Exists(savePath))
        {
            FileStream file = File.Open(savePath, FileMode.Open);
            saveData = (SaveData) bFormatter.Deserialize(file);
            file.Close();

            Debug.Log("Data loaded.");            
        }
        else
        {
            Debug.LogError("There is no save data!");
            saveData = null;
        }
        return saveData;
    }

    public void Reset()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
        else
        {
            Debug.LogError("No save data to delete.");
        }
    }
}
