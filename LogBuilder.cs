using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.IO;
namespace OOP_Lab3
{

    class ReportManager
    {
        ReportBuilder builder;

        public ReportManager(ReportBuilder item)
        {
            builder = item;
            builder.NewReport();
        }
        public void SetLogBuilder(ReportBuilder builder)
        {
            this.builder = builder;
        }
        public void AddOperation(string type, string msg)
        {
            builder.AddLog(type, msg);
        }
        public void saveReport()
        {
            builder.EndReport();
            builder.NewReport();
        }

    }
    abstract class ReportBuilder
    {
        protected StringBuilder report = new StringBuilder();
        public abstract void NewReport();
        public abstract void AddLog(string type, string msg);
        public abstract void EndReport();
    }

    class TXTReportBuilder : ReportBuilder
    {
        public override void NewReport()
        {
            report.Clear();
            report.Append("Звіт про роботу файлового менеджера. " + DateTime.Now.ToString() + '\n');
        }
        public override void AddLog(string type, string msg)
        {
            report.Append(DateTime.Now.ToString()+ " " + type + ". " + msg + '\n');
        }

        public override void EndReport()
        {
            report.Append("Дякую! " + DateTime.Now.ToString() + '\n');
            StreamWriter st = new StreamWriter(new FileStream(@"Log.txt", FileMode.Append));
            st.Write(report);
            st.Close();
        }
    }

    class HTMLReportBuilder: ReportBuilder
    {
        public override void NewReport()
        {
            report.Clear();
            report.Append("<HTML><BODY>\n");
            report.Append("<H1> Звіт про роботу файлового менеджера. "+ DateTime.Now + "</H1>\n");
        }

        public override void AddLog(string type, string msg)
        {
            report.Append("<p>" + DateTime.Now + " " + type + ". " + msg + "</p>\n");
        }

        public override void EndReport()
        {
            report.Append("<p><i>" + "Дякую! " + DateTime.Now + "</i></p>\n");
            StreamWriter st = new StreamWriter(new FileStream(@"Log.html", FileMode.Append));
            st.Write(report);
            st.Close();
        }
    }
}
