using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    static System.Random rng = new System.Random();
    [SerializeField] private State gameState;

    public enum TargetType
    {
        Government,
        DefenceCompany,
        InfrastructureCompany,
        StandardCompany
    }

    public string[] governmentNamelist = { "Camelot", "Dether", "Caicro", "Holu", "Tiusba", "Mosaint", "Ceansee", "Ngohu", "Tijomamu", "Riastan" };
    public string[] companyNamelist = { "Bright", "Open", "Nimble", "Brave", "Harmonious", "Interesting", "Colossal", "Cautious", "Grumpy", "Elite" };
    public List<Asset> assets = new List<Asset>();
    public List<string> assetIPs = new List<string>();
    public List<Employee> employees = new List<Employee>();
    public List<FirewallPort> firewallPorts = new List<FirewallPort>();

    [SerializeField] public TargetType type;
    [SerializeField] public string targetName;
    [SerializeField] public string publicIP;

    /// <summary>
    /// Adds the specified asset to this target's list of assets.
    /// </summary>
    /// <param name="asset">The asset to add.</param>
    public void AddAsset(Asset asset)
    {
        assets.Add(asset);
        assetIPs.Add(asset.localIP);
    }

    /// <summary>
    /// Generates a local IP address that is not already in use.
    /// </summary>
    /// <returns>The generated IP address.</returns>
    public string GenerateLocalIP()
    {
        string IP = "192.168." + rng.Next(256) + "." + rng.Next(256);
        while (assetIPs.Contains(IP))
        {
            IP = "192.168." + rng.Next(256) + "." + rng.Next(256);
        }
        return IP;
    }

    /// <summary>
    /// Generates this target's assets at game start.
    /// </summary>
    public void GenerateAssets()
    {
        Debug.Log("Generating target...");

        //generate target type
        Array targetTypes = Enum.GetValues(typeof(TargetType));
        type = (TargetType)targetTypes.GetValue(rng.Next(targetTypes.Length));
        Debug.Log("Chosen target type as: " + type.ToString());

        //generate target name
        if (type == Target.TargetType.Government)
        {
            //generate a government name
            targetName = governmentNamelist[rng.Next(governmentNamelist.Length)] + " National Government";
        }
        else
        {
            //generate a company name
            targetName = companyNamelist[rng.Next(companyNamelist.Length)] + "Corp";
        }
        Debug.Log("Created target name: " + targetName);

        //target asset generation
        //generate the target's public IP address
        publicIP = gameState.GenerateNewPublicIP();
        Debug.Log("Generated target public IP as: " + publicIP);

        //generate router
        Router router = new Router(targetName + " Router");
        router.localIP = "192.168.1.1";
        AddAsset(router);
        Debug.Log("Generated router with local IP: " + router.localIP);

        //generate data storage server
        DataServer dataServer = new DataServer(targetName + " Data Server");
        dataServer.localIP = GenerateLocalIP();
        AddAsset(dataServer);
        Debug.Log("Generated data storage server with local IP: " + dataServer.localIP);

        //generate employees and their workstations
        for (int i = 0; i < rng.Next(5, 25); i++)
        {
            Employee employee = new Employee();
            employee.employeeID = i;
            employees.Add(employee);
            Debug.Log("Generated employee: " + employee.employeeName);
            Workstation workstation = new Workstation(employee.employeeName + "'s Workstation", employee);
            workstation.localIP = GenerateLocalIP();
            AddAsset(workstation);
            Debug.Log("Created workstation for " + employee.employeeName + " with local IP: " + workstation.localIP);
        }
        Debug.Log("Generated " + employees.Count + " employees");

        //generate mail server
        MailServer mailServer = new MailServer(targetName + " Mail Server");
        mailServer.localIP = GenerateLocalIP();
        AddAsset(mailServer);
        Debug.Log("Generated mail server with local IP: " + mailServer.localIP);

        //generate web server
        WebServer webServer = new WebServer(targetName + " Web Server");
        webServer.localIP = GenerateLocalIP();
        AddAsset(webServer);
        Debug.Log("Generated web server with local IP: " + webServer.localIP);

        //if target is not a government, generate industrial machinery
        if (type != TargetType.Government)
        {
            if (rng.Next(100) < 50)
            {
                IndustrialMachinery industrialMachinery = new IndustrialMachinery(targetName + " Industrial Machinery");
                industrialMachinery.localIP = GenerateLocalIP();
                AddAsset(industrialMachinery);
                Debug.Log("Generated industrial machinery with local IP: " + industrialMachinery.localIP);
            }
        }
    }

    /// <summary>
    /// Generates the files for this target's assets.
    /// </summary>
    public void GenerateFiles()
    {
        foreach (Asset a in assets)
        {
            //generate up to 4 files for this asset
            for (int i = 0; i <= rng.Next(0,5); i++)
            {
                Array fileTypes = Enum.GetValues(typeof(File.FileType));
                //add file to asset file list
                a.files.Add(new File(a, File.GenerateFileName(6), (File.FileType)fileTypes.GetValue(rng.Next(fileTypes.Length)), false));
            }
        }
    }

    /// <summary>
    /// Generates the traffic for this target's assets.
    /// </summary>
    public void GenerateTraffic()
    {
        Debug.Log("Generating traffic...");
        //generate open firewall ports
        //FTP
        firewallPorts.Add(new FirewallPort(20, true));
        firewallPorts.Add(new FirewallPort(21, true));
        //SSH
        firewallPorts.Add(new FirewallPort(22, true));
        //Historic SMTP
        firewallPorts.Add(new FirewallPort(25, true));
        //DNS
        firewallPorts.Add(new FirewallPort(53, true));
        //HTTP
        firewallPorts.Add(new FirewallPort(80, true));
        //NTP
        firewallPorts.Add(new FirewallPort(123, true));
        //BGP
        firewallPorts.Add(new FirewallPort(179, true));
        //HTTPS
        firewallPorts.Add(new FirewallPort(443, true));
        //ISAKMP
        firewallPorts.Add(new FirewallPort(500, true));
        //Modern SMTP
        firewallPorts.Add(new FirewallPort(587, true));
        //RDP
        firewallPorts.Add(new FirewallPort(3389, true));

        foreach (Asset a in assets)
        {
            //generate up to 10 traffic entries for this asset
            for (int i = 0; i <= rng.Next(0, 10); i++)
            {
                //generate traffic type
                Array trafficTypes = Enum.GetValues(typeof(Traffic.TrafficType));
                Traffic.TrafficType trafficType = (Traffic.TrafficType)trafficTypes.GetValue(rng.Next(trafficTypes.Length));

                //generate traffic destination and source
                string destination;
                TrafficSource source;
                if (trafficType == Traffic.TrafficType.Incoming)
                {
                    destination = a.localIP;

                    //generate source - either from another asset or outside the network
                    if (rng.Next(2) == 0)
                    {
                        //choose the source asset
                        Asset sourceAsset = assets[rng.Next(assets.Count)];
                        while (sourceAsset == a)
                        {
                            sourceAsset = assets[rng.Next(assets.Count)];
                        }
                        //source is another asset, decide if traffic was from the device or a file on the device
                        if (rng.Next(2) == 0)
                        {
                            //traffic came from device
                            source = new TrafficSource(sourceAsset.localIP, sourceAsset.assetName, TrafficSource.SourceType.Device);
                        } else
                        {
                            //traffic came from a file
                            //choose a file on the asset
                            File file = sourceAsset.files[rng.Next(sourceAsset.files.Count)];
                            source = new TrafficSource(sourceAsset.localIP, file.filename, TrafficSource.SourceType.File);
                        }
                    } else
                    {
                        //source is outside target network
                        //generate source address
                        string address = gameState.GenerateNewPublicIP();
                        source = new TrafficSource(address, "Foreign Device @ " + address, TrafficSource.SourceType.Device);
                    }
                }
                else
                {
                    //either send the traffic to another asset, or outside the target's network
                    if (rng.Next(2) == 0)
                    {
                        //send to another asset
                        destination = assetIPs[rng.Next(assetIPs.Count)];
                    }
                    else
                    {
                        //send to a random IP outside the network
                        destination = gameState.GenerateNewPublicIP();
                    }

                    //source is this asset
                    //determine if the traffic came from the device or a file
                    if (rng.Next(2) == 0)
                    {
                        //traffic came from device
                        source = new TrafficSource(a.localIP, a.assetName, TrafficSource.SourceType.Device);
                    } else
                    {
                        //traffic came from a file
                        source = new TrafficSource(a.localIP, a.files[rng.Next(a.files.Count)].filename, TrafficSource.SourceType.File);
                    }
                }

                Traffic traffic = new Traffic(trafficType, source, destination, firewallPorts[rng.Next(firewallPorts.Count)], false);
                //add traffic to the asset's list
                a.assetTraffic.Add(traffic);
            }
        }

        Debug.Log("Finished generating traffic.");
    }

    /// <summary>
    /// Generates the mail for each of this target's employees.
    /// </summary>
    public void GenerateMail()
    {
        foreach (Employee e in employees)
        {
            //generate up to 4 emails for each employee
            for (int i = 0; i <= rng.Next(0, 5); i++)
            {
                if (rng.Next(2) == 0)
                {
                    //mail is internal origin
                    e.mailList.Add(new Mail(Mail.MailOrigin.Internal, Mail.internalSubjects[rng.Next(Mail.internalSubjects.Length)], e, false));
                }
                else
                {
                    //mail is external origin
                    e.mailList.Add(new Mail(Mail.MailOrigin.External, Mail.externalSubjects[rng.Next(Mail.externalSubjects.Length)], e, false));
                }
            }
        }
    }

    /// <summary>
    /// Gets the list of open firewall ports for this target.
    /// </summary>
    /// <returns>A list of these open ports.</returns>
    public List<FirewallPort> GetOpenPorts()
    {
        List<FirewallPort> ports = new List<FirewallPort>();
        foreach (FirewallPort port in firewallPorts)
        {
            if (port.open)
            {
                ports.Add(port);
            }
        }
        return ports;
    }
    
    /// <summary>
    /// Get the router asset for this target.
    /// </summary>
    /// <returns>The router object, or null if there is no router.</returns>
    public Router GetRouter()
    {
        foreach (Asset a in assets)
        {
            if (a.GetType() == typeof(Router))
            {
                return (Router)a;
            }
        }
        return null;
    }
    
    /// <summary>
    /// Get the web server asset for this target.
    /// </summary>
    /// <returns>The web server object, or null if there is no web server.</returns>
    public WebServer GetWebServer()
    {
        foreach (Asset a in assets)
        {
            if (a.GetType() == typeof(WebServer))
            {
                return (WebServer)a;
            }
        }
        return null;
    }
    
    /// <summary>
    /// Get the data server asset for this target.
    /// </summary>
    /// <returns>The data server object, or null if there is no data server.</returns>
    public DataServer GetDataServer()
    {
        foreach (Asset a in assets)
        {
            if (a.GetType() == typeof(DataServer))
            {
                return (DataServer)a;
            }
        }
        return null;
    }
    
    /// <summary>
    /// Get the mail server asset for this target.
    /// </summary>
    /// <returns>The mail server object, or null if there is no mail server.</returns>
    public MailServer GetMailServer()
    {
        foreach (Asset a in assets)
        {
            if (a.GetType() == typeof(MailServer))
            {
                return (MailServer)a;
            }
        }
        return null;
    }
    
    /// <summary>
    /// Get the industrial machinery asset for this target.
    /// </summary>
    /// <returns>The industrial machinery asset, or null is there is no industrial machinery.</returns>
    public IndustrialMachinery GetIndustrialMachinery()
    {
        foreach (Asset a in assets)
        {
            if (a.GetType() == typeof(IndustrialMachinery))
            {
                return (IndustrialMachinery)a;
            }
        }
        return null;
    }
    
    /// <summary>
    /// Get the employee workstations for this target.
    /// </summary>
    /// <returns>A list of the workstation objects which will be empty if there are no workstations.</returns>
    public List<Workstation> GetWorkstations()
    {
        List<Workstation> workstations = new List<Workstation>();
        foreach (Asset a in assets)
        {
            if (a.GetType() == typeof(Workstation))
            {
                workstations.Add((Workstation)a);
            }
        }
        return workstations;
    }
    
    /// <summary>
    /// Find a workstation asset by its employee.
    /// </summary>
    /// <param name="employee">The employee whos worksation is being searched for.</param>
    /// <returns>The workstation object, or null if the employee has no workstation.</returns>
    public Workstation GetWorkstationByEmployee(Employee employee)
    {
        foreach (Workstation workstation in GetWorkstations())
        {
            if (workstation.employee == employee)
            {
                return workstation;
            }
        }
        return null;
    }
}

public class Employee
{
    static System.Random rng = new System.Random();
    static string[] firstnames = { "Josefine", "Jette", "Carter", "Ezekiel", "Ghadir", "Anne-Laure", "Vira", "Jannike", "Declan", "Sigiheri", "Jerold", "Catharine", "Joel", "Rose", "Daniel", "Mia", "James", "Layla", "Constantine", "Mara" };
    static string[] surnames = { "Smith", "Archer", "Johnson", "Falkenrath", "Paulson", "Fisker", "Kubo", "Scarpa", "Parker", "Rasmussen", "Seward", "Kraus", "Toller", "Thatcher", "Michaels", "Stevenson", "Freeman", "Windsor", "Goldhirsch", "Dwight" };

    public string employeeName;
    public int employeeID;
    public List<Mail> mailList = new List<Mail>();

    public Employee()
    {
        employeeName = firstnames[rng.Next(firstnames.Length)] + " " + surnames[rng.Next(surnames.Length)];
    }
}
