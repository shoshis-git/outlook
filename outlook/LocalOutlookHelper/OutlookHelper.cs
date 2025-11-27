public class OutlookHelper
{
    public void CreateDraft(string to, string subject, string body, string? attachmentPath)
    {
        try
        {
            Type outlookType = Type.GetTypeFromProgID("Outlook.Application");
            if (outlookType == null)
                throw new Exception("Outlook לא נמצא במחשב");

            dynamic app = Activator.CreateInstance(outlookType);
            dynamic mail = app.CreateItem(0); // 0 = MailItem

            mail.To = to;
            mail.Subject = subject;
            mail.Body = body;

            if (attachmentPath != null)
                mail.Attachments.Add(attachmentPath);

            mail.Display(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine("שגיאה ביצירת טיוטת Outlook: " + ex.Message);
            throw;
        }
    }
}