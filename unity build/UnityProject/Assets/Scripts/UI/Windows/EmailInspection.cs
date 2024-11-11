using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EmailInspection : MonoBehaviour
{
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject windowTitle;
    [SerializeField] private GameObject mailListContent;
    [SerializeField] private GameObject mailEntryPrefab;

    public Employee employee;

    public void Initialise()
    {
        Debug.Log("Opening email inspection window for " + employee.employeeName);

        //set asset name
        windowTitle.GetComponent<TextMeshProUGUI>().text = "Email Inbox - " + employee.employeeName;
        title.GetComponent<TextMeshProUGUI>().text = employee.employeeName;

        GameObject listButton;
        //build email list
        foreach (Mail mail in employee.mailList)
        {
            listButton = Instantiate(mailEntryPrefab, mailListContent.transform);
            listButton.GetComponent<MailEntry>().mail = mail;
            //set the text
            listButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = mail.origin.ToString();
            listButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = mail.subject;
            if (mail.attachment != null)
            {
                listButton.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = mail.attachment.filename;
            } else
            {
                listButton.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "None";
            }
        }
    }
}
