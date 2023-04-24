using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
public class GameManager : MonoBehaviour
{
    [SerializeField] string _URI;
    [SerializeField] Transform _StartingPoint;
    [SerializeField] float _StackMoveDelta;
    [SerializeField] float _RightClickTime;
    private Dictionary<string, List<StacksData>> _gradeStacks;
    private List<Stack> _stacks;
    private int _currentSelectedStackIndex;

    private Coroutine _rightClickCoroutine;

    private void Start()
    {
        GetInfoStacksData(OnStackDataSuccess, OnError);
    }

    private void GetInfoStacksData(Action<string> onComplete, Action<string> onError)
    {
        StartCoroutine(Coroutine_GetInfoFromAPI(onComplete, onError));
    }
    private void OnStackDataSuccess(string data)
    {
        List<StacksData> stacksData = JsonConvert.DeserializeObject<List<StacksData>>(data);
        _gradeStacks = new Dictionary<string, List<StacksData>>();
        for (int i = 0; i < stacksData.Count; i++)
        {
            List<StacksData> stack;
            if (_gradeStacks.TryGetValue(stacksData[i].grade, out stack))
            {
                stack.Add(stacksData[i]);
            }
            else
            {
                stack = new List<StacksData>();
                stack.Add(stacksData[i]);
                _gradeStacks.Add(stacksData[i].grade, stack);
            }
        }
        SortStacks();
        BuildAllStacks();

    }

    private void SortStacks()
    {
        foreach(var stack in _gradeStacks)
        {
            for(int i = 0; i < stack.Value.Count; i++)
            {
                for(int j = i+1; j < stack.Value.Count; j++)
                {
                    int comparison = stack.Value[i].domain.CompareTo(stack.Value[j].domain);
                    if (comparison == 0)
                    {
                        comparison = stack.Value[i].cluster.CompareTo(stack.Value[j].cluster);
                        if (comparison == 0)
                        {
                            comparison = stack.Value[i].standardid.CompareTo(stack.Value[j].standardid);
                        }
                    }
                    if (comparison > 0)
                    {
                        StacksData pom = stack.Value[i];
                        stack.Value[i] = stack.Value[j];
                        stack.Value[j] = pom;
                    }
                }
            }
        }
    }

    private void BuildAllStacks()
    {
        if (_stacks != null)
        {
            for (int i = 0; i < _stacks.Count; i++)
            {
                Destroy(_stacks[i].gameObject);
            }
            _stacks.Clear();
        }
        else
        {
            _stacks = new List<Stack>();
        }
        Vector3 stackPosition = _StartingPoint.position;
        foreach (var stackData in _gradeStacks)
        {
            Stack stack = Instantiate(GameDataHolder.Instance.StackPrefab, stackPosition, Quaternion.identity);
            stack.BuildStack(stackData.Value, stackData.Key);
            stack.gameObject.name = "Stack: " + stackData.Key;
            stackPosition += Vector3.right * _StackMoveDelta;
            _stacks.Add(stack);
        }
        SelectStack(0);
    }

    public void SelectStack(int stackIndex)
    {
        if (stackIndex < 0)
        {
            stackIndex = 0;
        }
        if (stackIndex >= _stacks.Count)
        {
            stackIndex = _stacks.Count - 1;
        }
        CameraController.Instance.SetRootPosition(_stacks[stackIndex].transform.position);
        _currentSelectedStackIndex = stackIndex;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectStack(_currentSelectedStackIndex-1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectStack(_currentSelectedStackIndex + 1);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _stacks[_currentSelectedStackIndex].TestStack();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit))
            {
                I3DRightClick obj = PersonalUtility.FindComponentInHierarchyBottomUp<I3DRightClick>(hit.transform);
                if (obj != null)
                {
                    if (_rightClickCoroutine != null)
                    {
                        StopCoroutine(_rightClickCoroutine);
                    }
                    _rightClickCoroutine=StartCoroutine(Coroutine_RightClick(obj));
                }
            }
        }
    }

    IEnumerator Coroutine_RightClick(I3DRightClick obj)
    {
        float time = 0;
        while (Input.GetMouseButton(1))
        {
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        if(time<= _RightClickTime)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                I3DRightClick cmpObj = PersonalUtility.FindComponentInHierarchyBottomUp<I3DRightClick>(hit.transform);
                if (obj == cmpObj)
                {
                    obj.OnRightClick();
                }
            }
        }
    }

    private void OnError(string error)
    {
        Debug.LogError(error);
    }
    IEnumerator Coroutine_GetInfoFromAPI(Action<string> onComplete,Action<string> onError)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(_URI))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            if (!string.IsNullOrEmpty(webRequest.error))
            {
                onError?.Invoke("Error: " + webRequest.error);
            }
            else
            {
                onComplete?.Invoke(webRequest.downloadHandler.text);
            }
        }
    }

    
}
