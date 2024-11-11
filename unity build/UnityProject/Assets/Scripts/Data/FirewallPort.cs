using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallPort
{
    public int portNumber;
    public bool open;

    public FirewallPort(int portNumber, bool open)
    {
        this.portNumber = portNumber;
        this.open = open;
    }
}
