using UnityEngine;

using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadController : Singleton<SaveLoadController>
{
    private const string kPlayerDataFileName = "/playerData.dat";

    private BinaryFormatter mBinaryFormatter = null;

    private static string PlayerDataPath
    {
        get
        {
            return Application.persistentDataPath + kPlayerDataFileName;
        }
    }

    private void Start()
    {
        mBinaryFormatter = new BinaryFormatter();
    }

    public bool Save<T>(object objectToSave) where T : class
    {
        CoinSerializableObject obj = objectToSave as CoinSerializableObject;

        return Save<T>(obj, obj.SaveDirectory, obj.DefaultFileName, false);
    }


    public bool Save<T>(object objectToSave, string saveDirectory, string saveFileName, bool overwrite) where T : class
    {
        string fullPath = Path.Combine(saveDirectory, saveFileName);

        saveFileName = GetNextAvailableFilename(fullPath);

        fullPath = Path.Combine(saveDirectory, saveFileName);

        FileStream fileStream = File.Open(fullPath, FileMode.OpenOrCreate);
        mBinaryFormatter.Serialize(fileStream, objectToSave as T);
        fileStream.Close();

        Debug.Log(string.Format("Data saved to {0}", fullPath));

        return true;
    }

    public bool Save(PlayerData playerData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(PlayerDataPath, FileMode.OpenOrCreate);

        bf.Serialize(file, playerData);
        file.Close();

        Debug.Log(string.Format("Data saved to {0}", PlayerDataPath));
        return true;
    }

    public PlayerData Load()
    {
        if (File.Exists(PlayerDataPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(PlayerDataPath, FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();
            return data;
        }
        else 
        {
            return null;
        }
    }

    #region HELPERS
    public string GetNextAvailableFilename(string filename)
    {
        if (!System.IO.File.Exists(filename)) return filename;

        string alternateFilename;
        int fileNameIndex = 0;
        do
        {
            fileNameIndex += 1;
            alternateFilename = CreateNumberedFilename(filename, fileNameIndex);
        } while (System.IO.File.Exists(alternateFilename));

        Debug.Log("alternate name: " + alternateFilename);
        return alternateFilename;
    }

    private string CreateNumberedFilename(string filename, int number)
    {
        string path = System.IO.Path.GetDirectoryName(filename)+ "/";
        string plainName = System.IO.Path.GetFileNameWithoutExtension(filename);
        string extension = System.IO.Path.GetExtension(filename);
        Debug.Log("Numbered name: " + string.Format("{0}{1}{2}{3}", path, plainName, number, extension));
        return string.Format("{0}{1}{2}{3}", path, plainName, number, extension);
    }
    #endregion


}

