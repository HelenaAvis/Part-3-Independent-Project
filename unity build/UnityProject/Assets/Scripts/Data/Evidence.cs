using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evidence
{
    public object evidenceObject;
    public bool discovered;
    public bool access;

    public Evidence(object evidence, bool access)
    {
        evidenceObject = evidence;
        discovered = false;
        this.access = access;
    }
}
