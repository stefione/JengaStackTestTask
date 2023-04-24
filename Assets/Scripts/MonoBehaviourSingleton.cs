using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonoBehaviourSingleton<T> : MonoSingleton where T:MonoBehaviour
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
    }

    private void OnDestroy()
    {
        if (Instance == this || Instance == null)
        {
            Instance = null;
        }
    }
}
