using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class PersonalUtility
{

    public static bool InRange(float value, float otherValue, float offsetMin, float offsetMax)
    {
        return (value >= otherValue - offsetMin && value <= otherValue + offsetMax);
    }
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    public static T FindComponentInHierarchyBottomUp<T>(Transform obj, int overflowBreak = 1000)
    {
        T comp = obj.GetComponent<T>();
        overflowBreak--;
        if (overflowBreak < 0)
        {
            return default;
        }
        if (comp !=null && !comp.Equals(null))
        {
            return comp;
        }
        if (obj.parent == null)
        {
            return default;
        }
        return FindComponentInHierarchyBottomUp<T>(obj.parent, overflowBreak);
    }

    public static Coroutine Delay(Action action, float time, MonoBehaviour behaviour)
    {
        return behaviour.StartCoroutine(DelayCoroutine(action, time));
    }

    public static Coroutine DelayByFrames(Action action, int numberOfFrames, MonoBehaviour behaviour)
    {
        return behaviour.StartCoroutine(DelayCoroutineByFrames(action, numberOfFrames));
    }

    static IEnumerator DelayCoroutineByFrames(Action action, int numberOfFrames)
    {
        for(int i = 0; i < numberOfFrames; i++)
        {
            yield return null;
        }
        action?.Invoke();
    }

    static IEnumerator DelayCoroutine(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    public static T FindComponentInScene<T>()
    {
        for (int j = 0; j < SceneManager.sceneCount; j++)
        {
            GameObject[] rootObjects = SceneManager.GetSceneAt(j).GetRootGameObjects();
            foreach (var i in rootObjects)
            {
                T comp = FindComponentInHierarchyTopDown<T>(i.transform);
                if (comp != null)
                {
                    return comp;
                }
            }
        }
        return default;
    }

    public static List<T> FindComponentsInScene<T>()
    {
        List<T> list = new List<T>();
        for (int j = 0; j < SceneManager.sceneCount; j++)
        {
            GameObject[] rootObjects = SceneManager.GetSceneAt(j).GetRootGameObjects();
            foreach (var i in rootObjects)
            {
                List<T> foundList = FindComponentsInHierarchyTopDown<T>(i.transform);
                list.AddRange(foundList);
            }
        }
        return list;
    }

    public static T FindComponentInHierarchyTopDown<T>(Transform obj)
    {
        T comp = obj.GetComponent<T>();
        if (comp != null)
        {
            return comp;
        }

        foreach (Transform i in obj)
        {
            comp = FindComponentInHierarchyTopDown<T>(i);
            if (comp != null)
            {
                return comp;
            }
        }

        return default;
    }

    public static List<T> FindComponentsInHierarchyTopDown<T>(Transform obj)
    {
        List<T> list = new List<T>();
        FindComponentsInHierarchyTopDown_Helper(obj, list);
        return list;
    }
    static void FindComponentsInHierarchyTopDown_Helper<T>(Transform obj, List<T> list)
    {
        if (list == null)
        {
            list = new List<T>();
        }

        T comp = obj.GetComponent<T>();
        if (comp != null)
        {
            list.Add(comp);
        }

        foreach (Transform i in obj)
        {
            FindComponentsInHierarchyTopDown_Helper<T>(i,list);
        }
    }
}
