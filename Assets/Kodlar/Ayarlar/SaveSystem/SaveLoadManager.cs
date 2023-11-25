using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveLoadManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool disableSave = false;
    [SerializeField] private bool initializeDataIfNull = false;
    [SerializeField] private bool overrideSelectedProfileID = false;
    [SerializeField] private string testSelectedProfileID = "test";


    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    [Header("Auto Saving")]
    [SerializeField] private float autoSaveTime = 60f;

    public SaveData gameData;
    private List<ISaveData> dataObjects;
    private FileDataHandler dataHandler;

    private string selectedProfileID = "";

    private Coroutine autoSaveCoroutine;

    public static SaveLoadManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found More Than One DataManager");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        if (disableSave)
        {
            Debug.LogWarning("Save Load is Disable");
        }

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        InitializeSelectedProfileID();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)  // sahne yuklenirken kayit islemleri, bu islemler scene managerdan olacak
    {
        this.dataObjects = AllDataObjects();
        LoadGame();

        // start up the auto saving coroutine
        if (autoSaveCoroutine != null)
        {
            StopCoroutine(autoSaveCoroutine);
        }
        autoSaveCoroutine = StartCoroutine(AutoSave());
    }


    public void ChanceSelectedProfileID(string newID)
    {
        // update user profile
        this.selectedProfileID = newID;
        // Load game which will use profile
        LoadGame();
    }

    public void DeleteProfileData(string profileID)
    {
        // delete the data for this profile id
        dataHandler.Delete(profileID);
        // initialize the selected profile id
        InitializeSelectedProfileID();
        // reload the game so that our data matches the newly selected profile id

        LoadGame();
    }

    private void InitializeSelectedProfileID()
    {
        this.selectedProfileID = dataHandler.GetMostRecentlyUpdatedProfileID();
        if (overrideSelectedProfileID)
        {
            this.selectedProfileID = testSelectedProfileID;
            Debug.LogWarning("Override selected Profile ID :" + testSelectedProfileID);
        }
    }

    public void NewGame()
    {
        this.gameData = new SaveData();

    }

    public void LoadGame()
    {
        // return right away if Save Function is disabled
        if (disableSave)
        {
            return;
        }

        // load saved data from file 
        this.gameData = dataHandler.Load(selectedProfileID);

        if (this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }
        //if no data file, don't continue

        if (this.gameData == null)
        {
            Debug.Log("No data was found. A new game needs to be started");
            return;
        }
        // push the Loaded data to all other scripts that need it
        foreach (ISaveData dataObj in dataObjects)
        {
            dataObj.LoadData(gameData);
        }

    }

    public void SaveGame()
    {
        // return right away if Save Function is disabled
        if (disableSave)
        {
            return;
        }
        // if we don't have any data to save, log a warning here
        if (this.gameData == null)
        {
            Debug.LogWarning("no data was found. a new game needs to be started");
            return;
        }

        // pass the data to other scripts so they can update it
        foreach (ISaveData dataObj in dataObjects)
        {
            dataObj.SaveData(gameData);
        }

        // data when it was last saved
        gameData.lastUpdated = System.DateTime.Now.ToBinary();
        // save that data to file
        dataHandler.Save(gameData,selectedProfileID);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveData> AllDataObjects()
    {
        // FindObjectsofType takes in optional boolean to include inactive gameobjects
        IEnumerable<ISaveData> dataObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<ISaveData>();

        return new List<ISaveData>(dataObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, SaveData> GetAllProfilesSaveData()
    {
        return dataHandler.LoadAllProfiles();
    }

    private IEnumerator<WaitForSeconds> AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoSaveTime);
            SaveGame();
            Debug.Log("auto save");
        }
    }
}

