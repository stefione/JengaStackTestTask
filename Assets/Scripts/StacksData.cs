using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StacksData
{
    public int id;
    public string subject;
    public string grade;
    public Mastery mastery;
    public string domainid;
    public string domain;
    public string cluster;
    public string standardid;
    public string standarddescription;
}


public enum Mastery
{
    Glass,
    Wood,
    Stone
}