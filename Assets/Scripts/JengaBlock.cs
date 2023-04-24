using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JengaBlock : MonoBehaviour,I3DRightClick
{
    public Mastery MasteryType;
    private StacksData _data;
    public StacksData Data
    {
        get { return _data; }
    }

    private Rigidbody _rb;

    public void SetPhysics(bool value)
    {
        if (_rb == null)
        {
            _rb=GetComponent<Rigidbody>();
        }
        _rb.isKinematic = !value;
    }

    public void AssignData(StacksData data)
    {
        _data = data;
    }

    public void OnRightClick()
    {
        string info = _data.grade + " : " + _data.domain+"\n"+
            _data.cluster+"\n"+
            _data.standardid+" : "+_data.standarddescription;
        InfoPanel.Instance.Open(info);
    }
}
