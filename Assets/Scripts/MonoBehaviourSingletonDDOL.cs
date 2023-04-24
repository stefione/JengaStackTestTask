using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MonoBehaviourSingletonDDOL<T> : MonoSingleton where T:MonoBehaviour
{
    public static T Instance { get; private set; }

    public override void Initialize()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = PersonalUtility.FindComponentInScene<T>();
        PersonalUtility.DelayByFrames(DontDestroy_Handler,1,this);
    }

    void DontDestroy_Handler()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this || Instance == null)
        {
            Instance = null;
        }
    }
}
