using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Web,
            CyberPhysic,
            Digital,
            Data
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
                    case Facultets.Web:
                        return "09.03.01_Веб-технологии";
                    case Facultets.Digital:
                        return "09.03.02_Цифровая трансформация";
                    case Facultets.Data:
                        return "09.03.03_Большие и открытые данные";
                    case Facultets.CyberPhysic:
                        return "09.03.01_Киберфизические системы";
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
