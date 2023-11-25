using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;


public class FileDataHandler
{
    private string dataPath = "";
    private string dataFileName = "";

    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "word";

    private readonly string backupExtension = ".bak";


    public FileDataHandler(string dataPath, string dataFileName, bool useEncryption)
    {
        this.dataPath = dataPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public SaveData Load(string profileID,bool allowRestoreFromBackup = true)
    {
        // if the profileId is null, return right away
        if (profileID == null)
        {
            return null;
        }

        // use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataPath, profileID,dataFileName);
        SaveData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                // load the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                // optionally decrypt the data
                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                //deserialize the data from JSON back C# Object
                loadedData = JsonUtility.FromJson<SaveData>(dataToLoad);
            }
            catch (Exception e)
            {
                // since we're calling Load(..) recursively, we need to account for the case where
                // the rollback succeeds, but data is still failing to load for some other reason,
                // which without this check may cause an infinite recursion loop.
                if (allowRestoreFromBackup)
                {
                    Debug.LogWarning("Failed to load data file. Attempting to roll back.\n" + e);
                    bool rollbackSuccess = AttemptRollback(fullPath);
                    if (rollbackSuccess)
                    {
                        // try to load again recursively
                        loadedData = Load(profileID, false);
                    }
                }
                // if we hit this else block, one possibility is that the backup file is also corrupt
                else
                {
                    Debug.LogError("Error occured when trying to load file at path: "
                        + fullPath + " and backup did not work.\n" + e);
                }
            }
        }
        return loadedData;
    }

    public void Save(SaveData data, string profileID)
    {
        // if the profileId is null, return right away
        if (profileID == null)
        {
            return;
        }

        string fullPath = Path.Combine(dataPath,profileID, dataFileName);
        string backupFilePath = fullPath + backupExtension;
        try
        {
            // create the directory
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serialize the C# game data object into JSON
            string dataToStore = JsonUtility.ToJson(data, true);
            // optionally encrypt the data
            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            // write the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

            // verify the newly saved file can be loaded successfully
            SaveData verifiedGameData = Load(profileID);
            // if the data can be verified, back it up
            if (verifiedGameData != null)
            {
                File.Copy(fullPath, backupFilePath, true);
            }
            // otherwise, something went wrong and we should throw an exception
            else
            {
                throw new Exception("Save file could not be verified and backup could not be created.");
            }

        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public void Delete(string profileID)
    {
        //if the profileId is null, return right away
        if (profileID == null)
        {
            return;
        }

        string fullPath = Path.Combine(dataPath, profileID, dataFileName);
        try
        {
            // ensure the data file exists at this path before deleting the directory
            if (File.Exists(fullPath))
            {
                // delete the profile folder and everything within it
                Directory.Delete(Path.GetDirectoryName(fullPath), true);
            }
            else
            {
                Debug.LogWarning("Tried to delete profile data, but data was not found path: " + fullPath);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed to delete Profile: " 
                + profileID + "at path: " + fullPath + "\n" + e);
        }
    }

    public Dictionary<string, SaveData> LoadAllProfiles()
    {
        Dictionary<string, SaveData> allProfiles = new Dictionary<string, SaveData>();

        // Loop over all directory names in the data directory path
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataPath).EnumerateDirectories();
        foreach (DirectoryInfo directoryInfo in dirInfos)
        {
            string profileId = directoryInfo.Name;

            //Check if data file exists, Error Check

            string fullPath = Path.Combine(dataPath, profileId, dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory: " + profileId);
                continue;
            }

            // Load the game data for this profie
            SaveData profilData = Load(profileId);
            // Ensure thre profile data isnt null
            if (profilData != null)
            {
                allProfiles.Add(profileId, profilData);
            }
            else
            {
                Debug.LogError("Tried to load profile but something went wromg. Profile: " + profileId);
            }

        }

        return allProfiles;
    }

    public string GetMostRecentlyUpdatedProfileID()
    {
        string mostRecentProfileID = null;

        Dictionary<string, SaveData> profilesGameData = LoadAllProfiles();
        foreach (KeyValuePair<string,SaveData> pair in profilesGameData)
        {
            string profileID = pair.Key;
            SaveData saveData = pair.Value;

            // skip this entry if the gamedata is null
            if (saveData == null)
            {
                continue;
            }

            // if this is the first data we've come across that exists, it's the most recent so far
            if (mostRecentProfileID == null)
            {
                mostRecentProfileID = profileID;
            }
            // otherwise, compare to see which date is the most recent
            else
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecentProfileID].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(saveData.lastUpdated);
                // the greatest DateTime value is the most recent
                if (newDateTime > mostRecentDateTime)
                {
                    mostRecentProfileID = profileID;
                }
            }
        }
        return mostRecentProfileID;

    }


    // implementation of XOR encryption
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;

    }

    private bool AttemptRollback(string fullPath)
    {
        bool success = false;
        string backupFilePath = fullPath + backupExtension;
        try
        {
            // if the file exists, attempt to roll back to it by overwriting the original file
            if (File.Exists(backupFilePath))
            {
                File.Copy(backupFilePath, fullPath, true);
                success = true;
                Debug.LogWarning("Had to roll back to backup file at: " + backupFilePath);
            }
            // otherwise, we don't yet have a backup file - so there's nothing to roll back to
            else
            {
                throw new Exception("Tried to roll back, but no backup file exists to roll back to.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to roll back to backup file at: "
                + backupFilePath + "\n" + e);
        }

        return success;
    }

}
