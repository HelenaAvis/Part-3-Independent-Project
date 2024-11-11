using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    private static System.Random rng = new System.Random();

    public int score;
    public int evidenceScore;
    public int totalAvailableEvidenceScore;
    public int timeStars;
    public float timeTaken;
    public int evidenceStars;
    public bool correctSuspectChoice;
    public int suspectScore;

    public List<Evidence> undiscoveredEvidence = new List<Evidence>();
    public List<Evidence> discoveredEvidence = new List<Evidence>();
    public List<string> discoveredHostileIPs = new List<string>();

    public List<string> publicIPs = new List<string>();
    public List<Evidence> evidenceList = new List<Evidence>();
    public List<Asset> compromisedAssets = new List<Asset>();

    public string GenerateNewPublicIP()
    {
        string ip = rng.Next(256) + "." + rng.Next(256) + "." + rng.Next(256) + "." + rng.Next(256);
        while (publicIPs.Contains(ip))
        {
            ip = rng.Next(256) + "." + rng.Next(256) + "." + rng.Next(256) + "." + rng.Next(256);
        }
        publicIPs.Add(ip);
        return ip;
    }

    public void AddNewEvidence(Evidence evidence)
    {
        evidenceList.Add(evidence);
        undiscoveredEvidence.Add(evidence);
    }

    public List<Evidence> GetEvidenceOfType(Type type)
    {
        List<Evidence> evidence = new List<Evidence>();
        foreach (Evidence e in evidenceList)
        {
            if (e.GetType() == type)
            {
                evidence.Add(e);
            }
        }
        return evidence;
    }

    public int CountDiscoveredEvidence(bool access)
    {
        int count = 0;
        if (access)
        {
            //count access evidence
            foreach (Evidence e in discoveredEvidence)
            {
                if (e.access)
                {
                    count += 1;
                }
            }
        } else
        {
            count = discoveredEvidence.Count;
        }
        return count;
    }

    /// <summary>
    /// Count how many pieces of evidence are undiscovered.
    /// </summary>
    /// <param name="access">Whether the count should be limited to access evidence.</param>
    /// <returns>The number of undiscovered pieces of evidence.</returns>
    public int CountUndiscoveredEvidence(bool access)
    {
        int count = 0;
        if (access)
        {
            //count access evidence
            foreach (Evidence e in undiscoveredEvidence)
            {
                if (e.access)
                {
                    count += 1;
                }
            }
        }
        else
        {
            count = undiscoveredEvidence.Count;
        }
        return count;
    }

    public int CountAccessEvidence()
    {
        int count = 0;
        foreach (Evidence e in evidenceList)
        {
            if (e.access)
            {
                count += 1;
            }
        }
        return count;
    }

    /// <summary>
    /// Find the piece of evidence associated with the provided object.
    /// </summary>
    /// <param name="evidenceObject">The object to search for.</param>
    /// <returns>The evidence object, or null if there is no evidence associate with the provided object.</returns>
    public Evidence GetEvidence(object evidenceObject)
    {
        foreach (Evidence ev in evidenceList)
        {
            if (ev.evidenceObject == evidenceObject)
            {
                return ev;
            }
        }
        return null;
    }
}
