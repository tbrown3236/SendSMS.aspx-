using System;
using System.Collections;
using System.Net.Mail;
using System.Net;


public partial class SendSMS : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        string myRecipient="";
        ArrayList attachments = new ArrayList();

        //To add an attachment you can add the path to the attachments ArrayList:
        //attachmenst.add(complete path to file).
        //You will need to pass attachments to AttachmentPaths in the SendMail() method for either email or SMS.
        //You can use a fileUpload object to identify the attachments' path.
        //If there is an attachment in SMS you will need to modify the AT&T domain from txt.ATT.Net to mms.att.net
        //In my example below I am only doing Mail.  I am not taking advantage of the SMS objects (carrier, etc.)
        //If you are not using AT&T you will need to Google the correct carrier (i.e. Google: Send text to Verizon from email)
        
        if (cboCarrier.Text=="ATT")
        {
            if (attachments.Count>0)  //Has attachments
            {
                myRecipient = txtRecipient.Text+"@txt.ATT.Net";
            }
            else
            {
                myRecipient = txtRecipient.Text+"@mms.ATT.Net";
            }
        }

        //else if (cboCarrier.Text=="Verizon")   //Complete for each carrier you list.
        //{

        //}

        if (SendText(myRecipient, txtBody.Text))
        {
            lblMessage.Text = "Success";
        }
        else
        {
            lblMessage.Text = "Text Failed";
        }
    }
    public bool SendText(string recipient, string message)
    {
        return SendMail(recipient, "", message, null);
    }
    public bool SendMail(string recipient, string subject, string message, ArrayList AttachmentPaths)
    {
        SmtpClient client = new SmtpClient("smtp-mail.outlook.com");
        client.Port = 587;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.UseDefaultCredentials = false;
        NetworkCredential credentials = new NetworkCredential(Globals._sender, Globals._password);
        client.EnableSsl = true;
        client.Credentials = credentials;
        try
        {
            var mail = new MailMessage(Globals._sender.Trim(), recipient.Trim());
            if (AttachmentPaths != null)
            {
                foreach (string item in AttachmentPaths)
                {
                    if (item.Length > 0)
                    {
                        Attachment myAttachment = new Attachment(item);
                        mail.Attachments.Add(myAttachment);
                    }
                }
            }
            mail.Subject = subject;
            mail.Body = message;
            client.Send(mail);
        }
        catch (Exception ex)
        {

            lblMessage.Text = ex.Message;
            return false;
        }
        return true;
    }
}