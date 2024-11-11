using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MailEntry : MonoBehaviour
{
    [SerializeField] private GameObject originText;
    [SerializeField] private GameObject subjectText;
    [SerializeField] private GameObject attachmentText;
    [SerializeField] private Color32 hoverColour;
    [SerializeField] private Color32 normalColour;

    public Mail mail;

    private void OnMouseEnter()
    {
        //change colour of text to white
        originText.GetComponent<TextMeshProUGUI>().color = hoverColour;
        subjectText.GetComponent<TextMeshProUGUI>().color = hoverColour;
        attachmentText.GetComponent<TextMeshProUGUI>().color = hoverColour;
    }

    private void OnMouseExit()
    {
        //change colour of text back to green
        originText.GetComponent<TextMeshProUGUI>().color = normalColour;
        subjectText.GetComponent<TextMeshProUGUI>().color = normalColour;
        attachmentText.GetComponent<TextMeshProUGUI>().color = normalColour;
    }

    public void Clicked()
    {
        //change colour of text back to green
        originText.GetComponent<TextMeshProUGUI>().color = normalColour;
        subjectText.GetComponent<TextMeshProUGUI>().color = normalColour;
        attachmentText.GetComponent<TextMeshProUGUI>().color = normalColour;

        //register evidence if needed
        if (mail.hostile)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().DiscoverEvidence(mail);
        }
    }
}
