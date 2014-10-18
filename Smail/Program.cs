using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Smail
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = GetApplicationTitle();

            string fromAddress = String.Empty;
            string displayName = String.Empty;
            string toAddress = String.Empty;
            string subject = String.Empty;
            string body = String.Empty;
            string smtpServer = String.Empty;
            string smtpUsername = String.Empty;
            string smtpPassword = String.Empty;
            bool debug = false;

            Arguments commandLine = new Arguments(args);

            if (commandLine["f"] != null) fromAddress = commandLine["f"].Trim();
            if (commandLine["t"] != null) toAddress = commandLine["t"].Trim();
            if (commandLine["u"] != null) subject = commandLine["u"].Trim();
            if (commandLine["m"] != null) body = commandLine["m"].Trim();
            if (commandLine["s"] != null) smtpServer = commandLine["s"].Trim();
            if (commandLine["xu"] != null) smtpUsername = commandLine["xu"].Trim();
            if (commandLine["xp"] != null) smtpPassword = commandLine["xp"].Trim();
            if (commandLine["dn"] != null) displayName = commandLine["dn"].Trim();
            if (commandLine["debug"] != null) debug = true;

            if (IsValidMinimumCommands(commandLine))
            {
                MailMessage mail = new MailMessage();
                mail.From = String.IsNullOrWhiteSpace(displayName) ? new MailAddress(fromAddress) : new MailAddress(fromAddress, displayName);
                mail.To.Add(toAddress);
                mail.Subject = subject;
                mail.Body = body;

                SmtpClient smtpClient = new SmtpClient(smtpServer);
                if (!String.IsNullOrWhiteSpace(smtpUsername) && !String.IsNullOrWhiteSpace(smtpPassword))
                {
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                }

                try
                {
                    Console.Write("Sending email...");
                    smtpClient.Send(mail);
                    Console.WriteLine("success!");
                }
                catch (SmtpException e)
                {
                    Console.WriteLine("error!");
                    Console.WriteLine("Error description: {0}", e.Message);
                }
            }
            else
            {
                DisplayHelp();
            }

            if (debug) Console.Read();
        }

        static void DisplayHelp()
        {
            ConsoleColor currentConsoleColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(GetApplicationTitle());
            Console.WriteLine("by Robert Gonzalez <robert@nurvx.com>");
            Console.ForegroundColor = currentConsoleColor;
            Console.WriteLine();
            Console.WriteLine("Synopsis:    semail -f ADDRESS [options]");
            Console.WriteLine();
            Console.WriteLine("   Required:");
            Console.WriteLine("     -f ADDRESS              from (sender) email address");
            Console.WriteLine("     * At least one recipient required via -t");
            Console.WriteLine("     * Subject line required via -u");
            Console.WriteLine("     * Message body required via -m");
            Console.WriteLine("     * Smtp server required via -s");
            Console.WriteLine();
            Console.WriteLine("   Common:");
            Console.WriteLine("     -t ADDRESS [ADDR,...]  to email address(es)");
            Console.WriteLine("     -u 'SUBJECT'            message subject");
            Console.WriteLine("     -m 'MESSAGE'            message body");
            Console.WriteLine("     -s SERVER               smtp mail relay, default is mail.unisoftonline.com");
            Console.WriteLine();
            Console.WriteLine("   Optional:");
            Console.WriteLine("     -dn    'DISPLAY NAME'  from email address display name");
            Console.WriteLine("     -xu    USERNAME        username for SMTP authentication");
            Console.WriteLine("     -xp    PASSWORD        password for SMTP authentication");
            Console.WriteLine("     -debug DEBUG           pauses at the end to view output");
        }

        static string GetApplicationTitle()
        {
            return String.Format("Smail {0}", GetAssemblyVersionString()); ;
        }

        static string GetAssemblyVersionString()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            return String.Format("v{0}.{1}.{2}", version.Major, version.Minor, version.Revision);
        }

        static bool IsValidMinimumCommands(Arguments args)
        {
            return (args["f"] != null &&
                    args["t"] != null &&
                    args["s"] != null &&
                    args["u"] != null &&
                    args["m"] != null);
        }
    }
}

