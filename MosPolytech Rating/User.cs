using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Telegram.Bot.Types.ReplyMarkups;

namespace MosPolytech_Rating
{
    class User
    {
        public long ChatId = 0;
        public string SNILS = "";
        public Locations? Location = null;
        public Facultets? Facultet = null;
        public FormStudies? FormStudy = null;
        public FinStudies? FinStudy = null;
        private string lastPlace = "";
        public BackgroundWorker bw = new BackgroundWorker();

        public User()
        {
            bw.DoWork += Bw_DoWork;
            bw.WorkerSupportsCancellation = true;
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!bw.CancellationPending)
            {
                WebRequest web = WebRequest.Create("https://raitinglistpk.mospolytech.ru/rating_list_ajax.php");
                web.Method = "POST";
                web.ContentType = "application/x-www-form-urlencoded";
                string postData = "select1=" + Uri.EscapeDataString(LocationStr) +
                    "&specCode=" + Uri.EscapeDataString(FacultetString) +
                    "&eduForm=" + Uri.EscapeDataString(FormStudyString) +
                    "&eduFin=" + Uri.EscapeDataString(FinStudyString);
                var data = Encoding.UTF8.GetBytes(postData);
                web.ContentLength = data.Length;
                using (var stream = web.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                StreamReader sr = new StreamReader(web.GetResponse().GetResponseStream(), Encoding.GetEncoding("windows-1251"));
                string response = sr.ReadToEnd();
                sr.Dispose();
                DateTime lastUpdated = new DateTime(2021, 7, 29, 18, 0, 0);
                for (int i = 0; i < response.Length - 16; i++)
                {
                    if (DateTime.TryParseExact(response.Substring(i, 16), "dd.MM.yyyy HH:mm", new CultureInfo("ru-ru"), DateTimeStyles.None, out lastUpdated))
                        break;
                }
                List<string> rows = new List<string>();
                string row = "";
                for (int i = 0; i < response.Length - 3; i++)
                {
                    row += response[i];
                    if (response.Substring(i, 3) == "<td")
                    {
                        rows.Add(row);
                        row = "<";
                    }
                }
                if (row != "<") rows.Add(row);
                string snils = SNILS;
                Console.WriteLine($"SNILS: {snils}");
                for (int i = 0; i < rows.Count; i++)
                {
                    if (rows[i].Contains(snils))
                    {
                        string temp = rows[i - 1];
                        string konkurs = "";
                        for (int j = 0; j < temp.Length - 20; j++)
                        {
                            if (temp.Substring(j, 20) == "<font color=\"black\">")
                            {
                                j += 20;
                                konkurs = "";
                                for (int m = j; m < temp.Length; m++)
                                {
                                    if (temp[m] == '<') break;
                                    else konkurs += temp[m];
                                }
                                if (lastPlace == "") lastPlace = konkurs;
                                else if (lastPlace != konkurs) Program.bot.SendTextMessageAsync(ChatId, "Ваше место изменилось. Теперь оно " + konkurs, replyMarkup: new ReplyKeyboardRemove());
                            }
                        }
                    }
                }
                for (int i = 0; i < 600000; i++)
                {
                    if (bw.CancellationPending) break;
                    Thread.Sleep(1000);
                }
            }
        }

        public enum Locations
        {
            Moscow,
            Kolomna,
            Ryzen,
            Cheboksary,
            Elektrostal
        }

        public enum Facultets
        {
            InfoMath,
            Web,
            CyberPhysic,
            Digital,
            Data,
            InfoSec,
            EnterpriseInfo,
            InfoSecAuto
        }

        public enum FormStudies
        {
            Present,
            SemiPresent,
            NotPresent
        }

        public enum FinStudies
        {
            Free,
            Paid
        }

        public string LocationStr
        {
            get
            {
                switch (Location)
                {
                    case Locations.Moscow:
                        return "000000017_01";
                    case Locations.Ryzen:
                        return "000000017_02";
                    case Locations.Kolomna:
                        return "000000017_03";
                    case Locations.Cheboksary:
                        return "000000017_05";
                    case Locations.Elektrostal:
                        return "000000017_06";
                    default:
                        return "";
                }
            }
        }

        public string FacultetString
        {
            get
            {
                switch (Facultet)
                {
                    case Facultets.InfoMath:
                        return "01.03.02";
                    case Facultets.Web:
                        return "09.03.01_Веб-технологии";
                    case Facultets.Digital:
                        return "09.03.02_Цифровая трансформация";
                    case Facultets.Data:
                        return "09.03.03_Большие и открытые данные";
                    case Facultets.CyberPhysic:
                        return "09.03.01_Киберфизические системы";
                    case Facultets.InfoSec:
                        return "10.03.01";
                    case Facultets.EnterpriseInfo:
                        return "09.03.03_Корпоративные информационные системы";
                    case Facultets.InfoSecAuto:
                        return "10.05.03";
                    default:
                        return "";
                }
            }
        }
        
        public string FormStudyString
        {
            get
            {
                switch (FormStudy)
                {
                    case FormStudies.Present:
                        return "Очная";
                    case FormStudies.SemiPresent:
                        return "Очно-заочная";
                    case FormStudies.NotPresent:
                        return "Заочная";
                    default:
                        return "";
                }
            }
        }

        public string FinStudyString
        {
            get
            {
                switch (FinStudy)
                {
                    case FinStudies.Free:
                        return "Бюджетная основа";
                    case FinStudies.Paid:
                        return "Полное возмещение затрат";
                    default:
                        return "";
                }
            }
        }
    }
}
