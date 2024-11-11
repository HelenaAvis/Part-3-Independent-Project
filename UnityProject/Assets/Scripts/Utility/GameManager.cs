using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Target target;
    private AttackerSuspect suspect1;
    private AttackerSuspect suspect2;
    private AttackerSuspect suspect3;

    public State gameState;

    [SerializeField] private TextMeshProUGUI timeDisplay;
    private float time;
    public float minutesPerEvidence;
    private float minutesPerStar;
    public int scorePerTimeStar;
    public int scorePerEvidence;
    public int correctSuspectAward;
    public int incorrectSuspectPenalty;

    [SerializeField] private TextMeshProUGUI targetNameTitle;
    [SerializeField] private GameObject employeeListContent;
    [SerializeField] private GameObject assetListContent;
    [SerializeField] private GameObject employeeListButtonPrefab;
    [SerializeField] private GameObject assetListButtonPrefab;

    [SerializeField] private Canvas desktopCanvas;

    [SerializeField] private GameObject guidebookPrefab;

    [SerializeField] private GameObject objectivesWindowPrefab;
    [SerializeField] private GameObject suspectChoiceWindowPrefab;

    [SerializeField] private GameObject workstationInspectionWindowPrefab;
    [SerializeField] private GameObject storageDeviceInspectionPrefab;
    [SerializeField] private GameObject webServerInspectionPrefab;
    [SerializeField] private GameObject mailServerInspectionPrefab;
    [SerializeField] private GameObject dataServerInspectionPrefab;
    [SerializeField] private GameObject machineryInspectionPrefab;
    [SerializeField] private GameObject firewallInspectionPrefab;

    [SerializeField] private GameObject networkScannerPrefab;
    [SerializeField] private GameObject emailInspectionPrefab;

    [SerializeField] private GameObject evidencePopupPrefab;

    [SerializeField] private AudioClip evidenceFoundClip;
    [SerializeField] private AudioClip openWindowClip;

    private AudioSource soundPlayer;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameObject.Find("GameState").GetComponent<State>();
        soundPlayer = gameObject.GetComponent<AudioSource>();
        soundPlayer.loop = false;

        //score setup
        time = 0f;
        gameState.evidenceScore = 0;
        gameState.totalAvailableEvidenceScore = scorePerEvidence * gameState.evidenceList.Count;
        minutesPerStar = (minutesPerEvidence * gameState.evidenceList.Count) / 5;
        gameState.timeStars = 5;

        //get the target and suspect objects
        target = GameObject.FindGameObjectWithTag("Target").GetComponent<Target>();
        suspect1 = GameObject.FindGameObjectWithTag("Suspect1").GetComponent<AttackerSuspect>();
        suspect2 = GameObject.FindGameObjectWithTag("Suspect2").GetComponent<AttackerSuspect>();
        suspect3 = GameObject.FindGameObjectWithTag("Suspect3").GetComponent<AttackerSuspect>();

        targetNameTitle.text = target.targetName + ": " + target.publicIP;

        GameObject employeeButton;
        //build the list of employees
        foreach (Employee employee in target.employees)
        {
            //create a new instance of the button prefab for the employee
            employeeButton = Instantiate(employeeListButtonPrefab, employeeListContent.transform);
            employeeButton.name = employee.employeeName + " Button";
            employeeButton.GetComponentInChildren<TextMeshProUGUI>().text = employee.employeeID + ": " + employee.employeeName;
            employeeButton.GetComponent<EmployeeButton>().employee = employee;
        }

        GameObject assetButton;
        //build the list of assets
        foreach (Asset asset in target.assets)
        {
            //create a new instance of the button prefab for the asset
            assetButton = Instantiate(assetListButtonPrefab, assetListContent.transform);
            assetButton.name = asset.assetName + " Button";
            assetButton.GetComponentInChildren<TextMeshProUGUI>().text = asset.assetName;
            assetButton.GetComponent<AssetButton>().gameManager = this;
            assetButton.GetComponent<AssetButton>().asset = asset;
        }

        //open the objectives window
        OpenObjectives();
    }

    private void PlayOpenWindowSound()
    {
        soundPlayer.clip = openWindowClip;
        soundPlayer.time = 0f;
        soundPlayer.Play();
    }

    private void PlayEvidenceFoundSound()
    {
        soundPlayer.clip = evidenceFoundClip;
        soundPlayer.time = 0f;
        soundPlayer.Play();
    }

    public void OpenObjectives()
    {
        PlayOpenWindowSound();
        GameObject objectivesWindow = Instantiate(objectivesWindowPrefab, desktopCanvas.transform);
    }

    public void OpenSuspectChoice()
    {
        PlayOpenWindowSound();
        Debug.Log("Opening suspect choice window.");
        GameObject suspectChoiceWindow = Instantiate(suspectChoiceWindowPrefab, desktopCanvas.transform);
    }

    //TODO: submit choice, and end game
    public void CalculateFinalScore(AttackerSuspect choice)
    {
        Debug.Log("Starting game end process with player choice as " + choice.name);

        float score = 0f;

        //calculate time score
        gameState.timeTaken = time;
        score += gameState.timeStars * scorePerTimeStar;
        //calculate evidence score
        score += gameState.evidenceScore;
        if (gameState.evidenceScore < (gameState.totalAvailableEvidenceScore / 5))
        {
            gameState.evidenceStars = 1;
        }
        else if (gameState.evidenceScore < (gameState.totalAvailableEvidenceScore / 5) * 2)
        {
            gameState.evidenceStars = 2;
        }
        else if (gameState.evidenceScore < (gameState.totalAvailableEvidenceScore / 5) * 3)
        {
            gameState.evidenceStars = 3;
        }
        else if (gameState.evidenceScore < (gameState.totalAvailableEvidenceScore / 5) * 4)
        {
            gameState.evidenceStars = 4;
        }
        else
        {
            gameState.evidenceStars = 5;
        }

        if (choice.guilty)
        {
            //correct suspect choice
            score += correctSuspectAward;
            gameState.suspectScore = correctSuspectAward;
            gameState.correctSuspectChoice = true;
        } else
        {
            //incorrect choice
            score -= incorrectSuspectPenalty;
            gameState.suspectScore = incorrectSuspectPenalty;
            gameState.correctSuspectChoice = false;
        }

        gameState.score = Mathf.RoundToInt(score);

        //load end screen
        SceneManager.LoadScene("EndScreen");
    }

    public void OpenAssetInspection(Asset inspectionTarget)
    {
        PlayOpenWindowSound();
        //find the type of the target
        GameObject inspectionWindow;
        if (inspectionTarget.GetType() == typeof(Workstation))
        {
            //open workstation inspection window
            Debug.Log("Opening workstation inspection for: " + inspectionTarget.assetName);
            inspectionWindow = Instantiate(workstationInspectionWindowPrefab, desktopCanvas.transform);
            inspectionWindow.GetComponent<WorkstationInspection>().asset = inspectionTarget;
            inspectionWindow.GetComponent<WorkstationInspection>().Initialise();
        }
        else if (inspectionTarget.GetType() == typeof(ExternalStorageDevice))
        {
            //open external storage device inspection window
            Debug.Log("Opening storage device inspection for: " + inspectionTarget.assetName);
            inspectionWindow = Instantiate(storageDeviceInspectionPrefab, desktopCanvas.transform);
            inspectionWindow.GetComponent<StorageDeviceInspection>().asset = inspectionTarget;
            inspectionWindow.GetComponent<StorageDeviceInspection>().Initialise();
        }
        else if (inspectionTarget.GetType() == typeof(WebServer))
        {
            //open web server inspection window
            Debug.Log("Opening web server inspection for: " + inspectionTarget.assetName);
            inspectionWindow = Instantiate(webServerInspectionPrefab, desktopCanvas.transform);
            inspectionWindow.GetComponent<WebServerInspection>().asset = inspectionTarget;
            inspectionWindow.GetComponent<WebServerInspection>().Initialise();
        } else if (inspectionTarget.GetType() == typeof(MailServer))
        {
            //open mail server inspection window
            Debug.Log("Opening mail server inspection for: " + inspectionTarget.assetName);
            inspectionWindow = Instantiate(mailServerInspectionPrefab, desktopCanvas.transform);
            inspectionWindow.GetComponent<MailServerInspection>().asset = inspectionTarget;
            inspectionWindow.GetComponent<MailServerInspection>().initialise();
        } else if (inspectionTarget.GetType() == typeof(DataServer))
        {
            //open data server inspection window
            Debug.Log("Opening data server inspection for: " + inspectionTarget.assetName);
            inspectionWindow = Instantiate(dataServerInspectionPrefab, desktopCanvas.transform);
            inspectionWindow.GetComponent<DataServerInspection>().asset = inspectionTarget;
            inspectionWindow.GetComponent<DataServerInspection>().Initialise();
        } else if (inspectionTarget.GetType() == typeof(IndustrialMachinery))
        {
            //open industrial machinery inspection window
            Debug.Log("Opening industrial machinery inspection for: " + inspectionTarget.assetName);
            inspectionWindow = Instantiate(machineryInspectionPrefab, desktopCanvas.transform);
            inspectionWindow.GetComponent<MachineryInspection>().asset = inspectionTarget;
            inspectionWindow.GetComponent<MachineryInspection>().Initialise();
        } else if (inspectionTarget.GetType() == typeof(Router))
        {
            //open router/firewall inspection window
            Debug.Log("Opening router/firewall inspection for: " + inspectionTarget.assetName);
            inspectionWindow = Instantiate(firewallInspectionPrefab, desktopCanvas.transform);
            inspectionWindow.GetComponent<FirewallInspection>().asset = inspectionTarget;
            inspectionWindow.GetComponent<FirewallInspection>().target = target;
            inspectionWindow.GetComponent<FirewallInspection>().Initialise();
        }
    }

    public void OpenNetworkScanner()
    {
        PlayOpenWindowSound();
        Debug.Log("Opening network scanner window.");
        GameObject scannerWindow = Instantiate(networkScannerPrefab, desktopCanvas.transform);
        scannerWindow.name = "NetworkScanner";
        scannerWindow.GetComponent<NetworkScannerChoice>().desktopTransform = desktopCanvas.transform;
        scannerWindow.GetComponent<NetworkScannerChoice>().target = target;
        scannerWindow.GetComponent<NetworkScannerChoice>().Initialise();
    }

    public void OpenEmailInspection(Employee employee)
    {
        PlayOpenWindowSound();
        //Debug.Log("Opening email inspection window for " + employee.employeeName);
        GameObject mailWindow = Instantiate(emailInspectionPrefab, desktopCanvas.transform);
        mailWindow.name = "MailInspection" + employee.employeeName;
        mailWindow.GetComponent<EmailInspection>().employee = employee;
        mailWindow.GetComponent<EmailInspection>().Initialise();
    }

    //TODO: implement open guidebook function
    public void OpenGuidebook()
    {
        PlayOpenWindowSound();
        GameObject guidebookWindow = Instantiate(guidebookPrefab, desktopCanvas.transform);
        guidebookWindow.name = "Guidebook";
    }

    //handle evidence discovery - assign score
    public void DiscoverEvidence(System.Object evidenceObject)
    {
        if (gameState.discoveredEvidence.Contains(gameState.GetEvidence(evidenceObject)))
        {
            Debug.Log("Evidence already discovered");
        } else
        {
            PlayEvidenceFoundSound();

            Debug.Log("Discovered evidence: " + evidenceObject.ToString());
            gameState.undiscoveredEvidence.Remove(gameState.GetEvidence(evidenceObject));
            gameState.discoveredEvidence.Add(gameState.GetEvidence(evidenceObject));
            Debug.Log("Discovered pieces of access evidence: " + gameState.CountDiscoveredEvidence(true) + "/" + gameState.CountAccessEvidence());
            Debug.Log("Discovered pieces of evidence: " + gameState.CountDiscoveredEvidence(false) + "/" + gameState.evidenceList.Count);

            //show evidence discovered popup
            GameObject evidencePopup = Instantiate(evidencePopupPrefab, desktopCanvas.transform);
            evidencePopup.name = "EvidencePopup";

            //assign score
            gameState.evidenceScore += scorePerEvidence;
        }   
    }

    private void Update()
    {
        time += Time.deltaTime;
        //update timer
        timeDisplay.text = String.Format("Time - {0}:{1}{2}", Mathf.FloorToInt(time / 60), Mathf.FloorToInt((time % 60) / 10), Mathf.FloorToInt((time % 60) % 10));

        if (time < (minutesPerStar * 60))
        {
            gameState.timeStars = 5;
        } else if (time >= (minutesPerStar * 60) && time < (minutesPerStar * 120))
        {
            gameState.timeStars = 4;
        } else if (time >= (minutesPerStar * 120) && time < (minutesPerStar * 180))
        {
            gameState.timeStars = 3;
        } else if (time >= (minutesPerStar * 180) && time < (minutesPerStar * 240))
        {
            gameState.timeStars = 2;
        } else
        {
            gameState.timeStars = 1;
        }
    }
}
