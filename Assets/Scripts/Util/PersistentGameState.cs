using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentGameState : MonoBehaviour
{
    public bool IsDebug = false;
    public static PersistentGameState instance = null;
    private Dictionary<string, PersistentObject> peristants = new Dictionary<string, PersistentObject>();
    private List<PersistentObject> persistantsSorted = new List<PersistentObject>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        foreach (var persistentObject in FindObjectsOfType<PersistentObject>())
        {
            peristants.Add(persistentObject.ID,persistentObject);
        }
        persistantsSorted.AddRange(peristants.Values);
        persistantsSorted.Sort((x,y ) => x.ExecutionID.CompareTo(y.ExecutionID));
        foreach (var persistentObject in persistantsSorted) persistentObject.Create();
        if(!IsDebug)SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Update()
    {
        foreach (var persistentObject in persistantsSorted) persistentObject.Update();
    }
    
    private void FixedUpdate()
    {
        foreach (var persistentObject in persistantsSorted) persistentObject.FixedUpdate();
    }

    private void OnDestroy()
    {
        foreach (var persistentObject in persistantsSorted) persistentObject.Destroy();
    }

    public static bool TryFindObject(string ID, out PersistentObject obj)
    {
        if (instance.TryFindObjectLocal(ID, out PersistentObject value))
        {
            obj = value;
            return true;
        }
        obj = null;
        return false;
    }
    
    public bool TryFindObjectLocal(string ID, out PersistentObject obj)
    {
        if (peristants.TryGetValue(ID, out PersistentObject value))
        {
            obj = value;
            return true;
        }
        obj = null;
        return false;
    } 
}
