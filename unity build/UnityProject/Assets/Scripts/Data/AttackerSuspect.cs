using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerSuspect : MonoBehaviour
{
    static System.Random rng = new System.Random();

    [SerializeField] private State gameState;

    public enum AttackerType
    {
        StateSponsored,
        IndependentHacker,
        Criminal,
        Hacktivist,
        RivalCompany
    }

    public AttackerType attackerType;
    public List<string> knownAddresses = new List<string>();
    public bool guilty;
    public Target target;

    /// <summary>
    /// Generate the details about this attacker.
    /// </summary>
    /// <param name="target">Specify the target of the attack.</param>
    /// <param name="guilty">Determines if this attacker is the guilty suspect.</param>
    /// <param name="randomAttackChance">Determines if the target type influences the attacker type of this suspect.</param>
    public void GenerateAttacker(Target target, bool guilty, int randomAttackChance)
    {
        this.guilty = guilty;
        this.target = target;

        //generate 1-2 knownAddresses
        for (int i = 0; i <= rng.Next(0,2); i++)
        {
            knownAddresses.Add(gameState.GenerateNewPublicIP());
        }

        //decide if target type has any effect on attacker 
        if (rng.Next(100) < randomAttackChance)
        {
            //randomly choose attacker type
            Array attackerTypes = Enum.GetValues(typeof(AttackerType));
            attackerType = (AttackerType)attackerTypes.GetValue(rng.Next(attackerTypes.Length));

        } else
        {
            //the target type affects the attacker type
            if (target.type == Target.TargetType.Government)
            {
                AttackerType[] governmentAttackers = { AttackerType.StateSponsored, AttackerType.Hacktivist, AttackerType.IndependentHacker };
                attackerType = governmentAttackers[rng.Next(governmentAttackers.Length)];
            } else if (target.type == Target.TargetType.DefenceCompany)
            {
                AttackerType[] defenceAttackers = { AttackerType.StateSponsored, AttackerType.Hacktivist, AttackerType.RivalCompany, AttackerType.Criminal, AttackerType.IndependentHacker };
                attackerType = defenceAttackers[rng.Next(defenceAttackers.Length)];
            } else if (target.type == Target.TargetType.InfrastructureCompany)
            {
                AttackerType[] infrastructureAttackers = { AttackerType.StateSponsored, AttackerType.Hacktivist, AttackerType.RivalCompany, AttackerType.Criminal, AttackerType.IndependentHacker };
                attackerType = infrastructureAttackers[rng.Next(infrastructureAttackers.Length)];
            } else if (target.type == Target.TargetType.StandardCompany)
            {
                AttackerType[] standardAttackers = { AttackerType.Hacktivist, AttackerType.RivalCompany, AttackerType.Criminal, AttackerType.IndependentHacker };
                attackerType = standardAttackers[rng.Next(standardAttackers.Length)];
            }
        }
    }

    /// <summary>
    /// Generates a new file and associated traffic as evidence on the specified target asset.
    /// </summary>
    /// <param name="asset">The asset to generate the evidence on.</param>
    /// <param name="access">If the evidence is related to how the attacker gained access to the target.</param>
    /// <returns>The object of the file that was generated.</returns>
    public File AddEvidenceFileAndTraffic(Asset asset, bool access)
    {
        Array fileTypes = Enum.GetValues(typeof(File.FileType));
        File evidenceFile = new File(asset, File.GenerateFileName(6), (File.FileType)fileTypes.GetValue(rng.Next(fileTypes.Length)), true);
        asset.files.Add(evidenceFile);
        gameState.AddNewEvidence(new Evidence(evidenceFile, access));
        Traffic evidenceTraffic = new Traffic(Traffic.TrafficType.Outgoing, new TrafficSource(asset.localIP, evidenceFile.filename, TrafficSource.SourceType.File), knownAddresses[rng.Next(knownAddresses.Count)], target.GetOpenPorts()[rng.Next(target.GetOpenPorts().Count)], true);
        asset.assetTraffic.Add(evidenceTraffic);
        gameState.AddNewEvidence(new Evidence(evidenceTraffic, access));
        gameState.compromisedAssets.Add(asset);
        return evidenceFile;
    }

    /// <summary>
    /// Generates a new email to the specified employee as evidence.
    /// </summary>
    /// <param name="e">The employee to send the email to.</param>
    /// <param name="access">If the evidence is related to how the attacker gained access to the target.</param>
    /// <returns>The object of the mail that was generated.</returns>
    public Mail AddEvidenceMail(Employee e, bool access)
    {
        Mail evidenceMail = new Mail(Mail.MailOrigin.External, Mail.externalSubjects[rng.Next(Mail.externalSubjects.Length)], e, true);
        File evidenceFile = AddEvidenceFileAndTraffic(target.GetWorkstationByEmployee(e), access);
        evidenceMail.attachment = evidenceFile;
        e.mailList.Add(evidenceMail);
        gameState.AddNewEvidence(new Evidence(evidenceMail, access));
        return evidenceMail;
    }

    /// <summary>
    /// Generates attack evidence on the target's assets. Evidence of the attacker gaining access will be generated first, and then evidence will be placed on other assets.
    /// </summary>
    /// <param name="target">The target of the attack.</param>
    public void GenerateEvidence(Target target)
    {
        //generate access evidence
        List<FirewallPort> openPorts = target.GetOpenPorts();
        //generate further open firewall ports
        for (int i = 0; i <= rng.Next(4); i++)
        {
            FirewallPort port = new FirewallPort(rng.Next(1, 65536), true);
            while (openPorts.Contains(port))
            {
                port = new FirewallPort(rng.Next(1, 65536), true);
            }
            target.firewallPorts.Add(port);
        }

        //decide if the attacker gained access through the firewall
        int chance = rng.Next(100);
        if (chance <= 14)
        {
            //not through firewall, place evidence on a device as a file - and add a connected storage device
            //select a random asset
            Asset accessAsset = target.assets[rng.Next(target.assets.Count)];
            accessAsset.externalStorageDeviceConnected = true;
            ExternalStorageDevice externalDevice = new ExternalStorageDevice("USB Drive");
            //add the file and outgoing traffic
            externalDevice.files.Add(AddEvidenceFileAndTraffic(accessAsset, true));
            accessAsset.connectedDevice = externalDevice;
        }
        else if (chance > 14 && chance <= 29)
        {
            //not through firewall, place evidence as an email to an employee
            //select a random employee
            Employee accessEmployee = target.employees[rng.Next(target.employees.Count)];
            AddEvidenceMail(accessEmployee, true);
        }
        else
        {
            //access was through firewall
            Router firewall = target.GetRouter();
            //generate file for evidence, and outgoing traffic from it
            AddEvidenceFileAndTraffic(firewall, true);
        }

        //decide if mail server is attacked
        if (attackerType == AttackerType.RivalCompany || attackerType == AttackerType.StateSponsored || attackerType == AttackerType.IndependentHacker)
        {
            //higher chance the mail server will be attacked
            if (rng.Next(100) <= 74)
            {
                AddEvidenceFileAndTraffic(target.GetMailServer(), false);
            }
        } else
        {
            //standard chance the mail server will be attacked
            if (rng.Next(100) <= 59)
            {
                AddEvidenceFileAndTraffic(target.GetMailServer(), false);
            }
        }

        //decide if web server is attacked
        if (attackerType == AttackerType.Hacktivist || attackerType == AttackerType.Criminal || attackerType == AttackerType.IndependentHacker)
        {
            //higher chance the web server will be attacked
            if (rng.Next(100) <= 74)
            {
                AddEvidenceFileAndTraffic(target.GetWebServer(), false);
                target.GetWebServer().webSiteAttacked = true;
            }
        }
        else
        {
            //standard chance the web server will be attacked
            if (rng.Next(100) <= 59)
            {
                AddEvidenceFileAndTraffic(target.GetWebServer(), false);
                target.GetWebServer().webSiteAttacked = true;
            }
        }

        //decide if data server is attacked
        if (attackerType == AttackerType.RivalCompany || attackerType == AttackerType.Criminal || attackerType == AttackerType.StateSponsored || attackerType == AttackerType.IndependentHacker)
        {
            //higher chance the data server will be attacked
            if (rng.Next(100) <= 74)
            {
                AddEvidenceFileAndTraffic(target.GetDataServer(), false);
            }
        }
        else
        {
            //standard chance the data server will be attacked
            if (rng.Next(100) <= 59)
            {
                AddEvidenceFileAndTraffic(target.GetDataServer(), false);
            }
        }

        //decide if industrial machinery is attacked
        if (target.GetIndustrialMachinery() != null)
        {
            if (attackerType == AttackerType.RivalCompany || attackerType == AttackerType.Criminal || attackerType == AttackerType.StateSponsored || attackerType == AttackerType.IndependentHacker)
            {
                //higher chance industrial machinery will be attacked
                if (rng.Next(100) <= 74)
                {
                    AddEvidenceFileAndTraffic(target.GetIndustrialMachinery(), false);
                    target.GetIndustrialMachinery().functioning = false;
                }
            }
            else
            {
                //standard chance industrial machinery will be attacked
                if (rng.Next(100) <= 59)
                {
                    AddEvidenceFileAndTraffic(target.GetIndustrialMachinery(), false);
                    target.GetIndustrialMachinery().functioning = false;
                }
            }
        }

        //decide how many employee workstations are attacked
        int count = rng.Next((int)Math.Ceiling(decimal.Multiply(target.GetWorkstations().Count, (decimal)0.1)), (int)Math.Ceiling(decimal.Multiply(target.GetWorkstations().Count, (decimal)0.3)));
        for (int i = 0; i < count; i++)
        {
            Workstation targetWorkstation = target.GetWorkstations()[i];
            //generate evidence for the workstation
            File file = AddEvidenceFileAndTraffic(targetWorkstation, false);
            //add either a connected storage device, an email to the employee, or incoming traffic from a file on another device
            switch (rng.Next(3))
            {
                case 0:
                    //connected storage device
                    targetWorkstation.externalStorageDeviceConnected = true;
                    ExternalStorageDevice connectedDevice = new ExternalStorageDevice("USB Drive");
                    connectedDevice.files.Add(file);
                    targetWorkstation.connectedDevice = connectedDevice;
                    break;
                case 1:
                    //email to employee
                    //get the employee who owns the workstation
                    Employee employee = targetWorkstation.employee;
                    employee.mailList.Add(AddEvidenceMail(employee, false));
                    break;
                case 2:
                    //incoming traffic from file on other compromised asset
                    //choose another compromised asset
                    Asset otherAsset = gameState.compromisedAssets[rng.Next(gameState.compromisedAssets.Count)];
                    Traffic evidenceTraffic = new Traffic(Traffic.TrafficType.Incoming, new TrafficSource(otherAsset.localIP, otherAsset.assetName, TrafficSource.SourceType.Device), targetWorkstation.localIP, target.GetOpenPorts()[rng.Next(target.GetOpenPorts().Count)], true);
                    gameState.AddNewEvidence(new Evidence(evidenceTraffic, false));
                    break;
            }
        }

        Debug.Log("Generated " + gameState.evidenceList.Count + " pieces of evidence, with " + gameState.CountAccessEvidence() + " as access evidence");

    }
}
