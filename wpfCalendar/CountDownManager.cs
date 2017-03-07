using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfCalendar
{
    public class CountDownManager
    {
        private DateTime endDate;
        
        public DateTime EndDate
        {
            get
            {
                return endDate;
            }
        }

        public int DaysLeft
        {
            get
            {
                return (endDate - DateTime.Now).Days;
            }
        }

        public int HoursLeft
        {
            get
            {
                return (endDate - DateTime.Now).Hours;
            }
        }

        public int MinutesLeft
        {
            get
            {
                return (endDate - DateTime.Now).Minutes;
            }
        }

        public int SecondsLeft
        {
            get
            {
                return (endDate - DateTime.Now).Seconds;
            }
        }

        public CountDownManager(DateTime endDate)
        {
            ChangeDate(endDate);
        }

        public void ChangeDate(DateTime endDate)
        {
            this.endDate = endDate;
        }

        public void SaveDate()
        {
            WidgetFileManager.saveDateToFile(endDate);            
        }
    }
}
