using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traffic
{
    public enum TrafficType
    {
        Incoming,
        Outgoing
    }

    public TrafficType type;
    public TrafficSource source;
    public string destination;
    public FirewallPort port;
    public bool hostile;

    public Traffic(TrafficType type, TrafficSource source, string destination, FirewallPort port, bool hostile)
    {
        this.type = type;
        this.source = source;
        this.destination = destination;
        this.port = port;
        this.hostile = hostile;
    }
}

public class TrafficSource
{
    public enum SourceType{
        Device,
        File
    }

    public string address;
    public string sourceName;
    public SourceType sourceType;

    public TrafficSource(string address, string sourceName, SourceType sourceType)
    {
        this.address = address;
        this.sourceName = sourceName;
        this.sourceType = sourceType;
    }
}
