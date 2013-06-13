using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.ViewModels
{
    public class DateTimeViewModel : DateViewModel
    {
        public ObservableCollection<PairViewModel<int>> Hour { get; set; }
        public ObservableCollection<PairViewModel<int>> Minute { get; set; }

        private PairViewModel<int> selectedHour;

        public PairViewModel<int> SelectedHour
        {
            get
            {
                return selectedHour;
            }
            set
            {
                SetProperty(ref selectedHour, value);
            }
        }

        private PairViewModel<int> selectedMinute;

        public PairViewModel<int> SelectedMinute
        {
            get
            {
                return selectedMinute;
            }
            set
            {
                SetProperty(ref selectedMinute, value);
            }
        }

        public DateTime ToDateTime() 
        {
            return new DateTime(SelectedYear.Value, SelectedMonth.Value, SelectedDay.Value, SelectedHour.Value, SelectedMinute.Value, 0);
        }

        private void AddHours()
        {
            Hour.Clear();
            
            for (var h = 0; h < 24; h++)
            {
                Hour.Add(new PairViewModel<int>()
                {
                    Text = string.Format("{0:00}", h)
                });
            }
        }

        private void AddMinutes()
        {
            Minute.Clear();

            for (var m = 0; m < 60; m++)
            {
                Minute.Add(new PairViewModel<int>()
                {
                    Text = string.Format("{0:00}", m)
                });
            }
        }

        public void SetDateTime(DateTime dateTime)
        {            
            SelectedHour = Hour.Where(h => h.Value == dateTime.Hour).First();
            SelectedMinute = Minute.Where(m => m.Value == dateTime.Minute).First();           
            
            SetDate(dateTime);
        }

        public DateTimeViewModel(DateTime dateTime)
            : base(dateTime)
        {
            Hour = new ObservableCollection<PairViewModel<int>>();
            Minute = new ObservableCollection<PairViewModel<int>>();

            AddHours();
            AddMinutes();

            SetDateTime(dateTime);
        }

        public DateTimeViewModel() : this(DateTime.UtcNow)
        {

        }
    }
}