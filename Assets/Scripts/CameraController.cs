using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviourSingleton<CameraController>
{
    [SerializeField] float _MoveSpeed;
    [SerializeField] float _RotationSensitivity;
    [SerializeField] Camera _Cam;


    public void SetRootPosition(Vector3 position)
    {
        StopAllCoroutines();
        StartCoroutine(Coroutine_MoveToPosition(position));
    }
    IEnumerator Coroutine_MoveToPosition(Vector3 pos)
    {
        float lerp = 0;
        while (lerp < 1)
        {
            transform.position = Vector3.Lerp(transform.position, pos, lerp);
            lerp += Time.deltaTime* _MoveSpeed;
            yield return null;
        }
        transform.position = pos;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1) && !PersonalUtility.IsPointerOverUIObject())
        {
            float y = Input.GetAxis("Mouse X");
            float x = Input.GetAxis("Mouse Y");
            Vector3 rotateValue = new Vector3(x, y * -1, 0) * _RotationSensitivity;
            transform.eulerAngles = transform.eulerAngles - rotateValue;
        }
        if (Input.mouseScrollDelta.y != 0 && !PersonalUtility.IsPointerOverUIObject())
        {
            _Cam.transform.localPosition -= Vector3.forward * Input.mouseScrollDelta.y;
        }
    }
}
