namespace webapi.Domain.Models
{
    public class SmtpSettings
    {
        public int Id { get; set; } // SMTP server host name

        public string Host { get; set; } // SMTP server host name

        public int Port { get; set; } // SMTP server port (usually 587 for TLS/SSL)

        public string? Username { get; set; } // SMTP username


        public string? Password { get; set; } // SMTP password

        public bool UseSsl { get; set; } // Use SSL/TLS for secure connection
        public bool UseDefaultCredentials { get; set; } // Use default credentials

        public string FromAddress { get; set; } // Default sender email address


        public string DisplayName { get; set; } // Display name for the sender

    }

    public class EmailRequest
    {

        public string Subject { get; set; } // Email subject


        public string Body { get; set; }    // Email body (HTML or plain text)


        public List<string> To { get; set; } = new List<string>(); // List of recipient email addresses
        public List<string> Cc { get; set; } = new List<string>(); // List of CC email addresses
        public List<string> Bcc { get; set; } = new List<string>(); // List of BCC email addresses

    }

}
