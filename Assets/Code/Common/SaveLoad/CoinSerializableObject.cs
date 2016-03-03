using System;
using UnityEngine;

[Serializable]
public abstract class CoinSerializableObject : object
{
    private static string sDefaultSaveDirectory = Application.persistentDataPath;
    private const string kDefaultFileName = "SavedData.dat";

    protected virtual string InternalSaveDirectory
    {
        get{ return sDefaultSaveDirectory; }
    }

    protected virtual string InternalDefaultFileName
    {
        get{ return kDefaultFileName; }
    }

    public abstract string SaveDirectory
    {
        get;
    }

    public abstract string DefaultFileName
    {
        get;
    }
}

