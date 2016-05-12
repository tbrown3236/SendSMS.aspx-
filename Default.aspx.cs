using System;
using System.Collections;
using System.Net.Mail;
using System.Net;

 
public partial class _Default : System.Web.UI.Page
{
    ArrayList attachments = new ArrayList();
    //To add an attachment you can add the path to the attachments ArrayList:
    //attachment.add(complete path to file).
    //You will need to pass attachments to AttachmentPaths in the SendMail() method for either email or SMS.
    //You can use a fileUpload object to identify the attachments' path.
     protected void Page_Load(object sender, EventArgs e)
    { 
        if (!IsPostBack)
        {
            other.Visible = false;
        }
        
    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
       
        string mysubject="";
        if (cboSubj.Text=="Other")
         {
             mysubject = txtOther.Text;
         }
        else
        {
            mysubject = cboSubj.Text;
        }
        if (SendMail(Globals._sender, mysubject, txtBody.Text, null))
        {
            lblMessage.Text = "Success";
         }
        else
        {
            lblMessage.Text = "Email Failed";
        }
  
        lblMessage.Visible = true;
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
        NetworkCredential credentials =new NetworkCredential(Globals._sender, Globals._password);
        client.EnableSsl = true;
        client.Credentials = credentials;
        try
        {
            var mail = new MailMessage(Globals._sender.Trim(), recipient.Trim());
            if (AttachmentPaths!= null)
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
    protected void cboSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cboSubj.Text == "Other")
        {
            other.Visible = true;
            txtOther.Focus();
        }
        else
        {
            other.Visible = false;
        }
    }
}