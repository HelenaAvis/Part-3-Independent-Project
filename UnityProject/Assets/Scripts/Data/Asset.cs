using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asset
{
    public string assetName;
    public string localIP;
    public List<File> files = new List<File>();
    public List<Traffic> assetTraffic = new List<Traffic>();
    public bool externalStorageDeviceConnected;
    public ExternalStorageDevice connectedDevice;

    public Asset(string assetName)
    {
        this.assetName = assetName;
        externalStorageDeviceConnected = false;
    }

    /// <summary>
    /// Get the incoming traffic for this asset.
    /// </summary>
    /// <returns>A list containing all the incoming traffic objects.</returns>
    public List<Traffic> GetIncomingTraffic()
    {
        List<Traffic> incoming = new List<Traffic>();
        foreach (Traffic t in assetTraffic)
        {
            if (t.type == Traffic.TrafficType.Incoming)
            {
                incoming.Add(t);
            }
        }
        return incoming;
    }

    /// <summary>
    /// Get the outgoing traffic for this asset.
    /// </summary>
    /// <returns>A list containing all the outgoing traffic objects.</returns>
    public List<Traffic> GetOutgoingTraffic()
    {
        List<Traffic> outgoing = new List<Traffic>();
        foreach (Traffic t in assetTraffic)
        {
            if (t.type == Traffic.TrafficType.Outgoing)
            {
                outgoing.Add(t);
            }
        }
        return outgoing;
    }
}

public class Router : Asset
{
    public Router(string assetName) : base(assetName) { }
}

public class Workstation : Asset
{
    public Employee employee;
    public Workstation(string assetName, Employee employee) : base(assetName)
    {
        this.employee = employee;
    }
}

public class MailServer : Asset
{
    public MailServer(string assetName) : base(assetName) { }
}

public class DataServer : Asset
{
    public DataServer(string assetName) : base(assetName) { }
}

public class WebServer : Asset
{
    public bool webSiteAttacked;
    public WebServer(string assetName) : base(assetName)
    {
        webSiteAttacked = false;
    }
}

public class IndustrialMachinery : Asset
{
    public bool functioning;
    public IndustrialMachinery(string assetName) : base(assetName)
    {
        functioning = true;
    }
}

public class ExternalStorageDevice : Asset
{
    public ExternalStorageDevice(string assetName) : base(assetName) { }
}