using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mail
{
    public enum MailOrigin
    {
        Internal,
        External
    }

    public static string[] internalSubjects = { "Weekly Review", "New Task", "Announcement", "Shared a File With You"};
    public static string[] externalSubjects = { "Business Inquiry", "Job Application"};

    public MailOrigin origin;
    public string subject;
    public Employee recipient;
    public bool hostile;
    public File attachment;

    public Mail(MailOrigin origin, string subject, Employee recipient, bool hostile)
    {
        this.origin = origin;
        this.subject = subject;
        this.recipient = recipient;
        this.hostile = hostile;
    }
}