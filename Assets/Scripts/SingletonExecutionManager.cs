using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SingletonExecutionManager
{
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeInitialization()
    {
        Initialize();
        SceneManager.sceneLoaded += SceneManager_sceneLoaded_Handler;
    }

    private static void Initialize()
    {
        List<MonoSingleton> monoSingletons = PersonalUtility.FindComponentsInScene<MonoSingleton>();
        foreach (var i in monoSingletons)
        {
            i.Initialize();
        }
        List<ISingleton> singletons = PersonalUtility.FindComponentsInScene<ISingleton>();
        foreach (var i in singletons)
        {
            i.Initialize();
        }
    }

    private static void SceneManager_sceneLoaded_Handler(Scene arg0, LoadSceneMode arg1)
    {
        Initialize();
    }
}
