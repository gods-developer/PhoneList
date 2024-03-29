﻿using PhoneListGenerator.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Win.Common.Tools;

namespace PhoneListGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Daten werden abgerufen...");
            string html = Resources.ListTemplate;
            StringBuilder sb = new StringBuilder();
            WinAdConnector wac = new WinAdConnector();
            int c = 0;
            var users = wac.GetDomainUsers(true);
            foreach (var user in users.OrderBy(x => x.DisplayName))
            {
                if (!String.IsNullOrEmpty(user.Email) || !String.IsNullOrEmpty(user.Phone) || !String.IsNullOrEmpty(user.Mobile))
                {
                    c++;
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td align=\"right\">" + c + "</td>");
                    sb.AppendLine("<td>" + user.SamAccountName + "</td>");
                    sb.AppendLine("<td>" + user.DisplayName + "</td>");
                    sb.AppendLine("<td><a href=\"mailto:" + user.Email + "\">" + user.Email + "</a></td>");
                    sb.AppendLine("<td>" + user.Phone + "</td>");
                    sb.AppendLine("<td>" + user.Mobile + "</td>");
                    sb.AppendLine("<td>" + user.Department + "</td>");
                    sb.AppendLine("</tr>");
                }
            }
            html = html.Replace("@@datetime@@", "Stand " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
            html = html.Replace("@@rows@@", sb.ToString());
            Console.WriteLine(c + " DomainUsers found");
            Console.WriteLine("Datei " + new FileInfo("Telefonliste.html").FullName + " wird geschrieben...");
            Thread.Sleep(1000);
            string file = "Telefonliste.html";
            File.WriteAllText(file, html, Encoding.UTF8);
            Console.WriteLine("Fertig!");
            Thread.Sleep(5000);
#if DEBUG 
            {
                Console.ReadKey();
                ProcessStartInfo pi = new ProcessStartInfo(file);
                pi.Arguments = Path.GetFileName(file);
                pi.UseShellExecute = true;
                pi.WorkingDirectory = Path.GetDirectoryName(file);
                pi.FileName = file;
                pi.Verb = "OPEN";
                Process.Start(pi);
            }
#endif
        }
    }
}
