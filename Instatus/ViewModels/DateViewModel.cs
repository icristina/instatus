using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.ViewModels
{
    public class DateViewModel : BindableBase
    {
        public ObservableCollection<PairViewModel<int>> Day { get; set; }
        public ObservableCollection<PairViewModel<int>> Month { get; set; }
        public ObservableCollection<PairViewModel<int>> Year { get; set; }

        private PairViewModel<int> selectedDay;

        public PairViewModel<int> SelectedDay
        {
            get
            {
                return selectedDay;
            }
            set
            {
                SetProperty(ref selectedDay, value);
            }
        }

        private PairViewModel<int> selectedMonth;

        public PairViewModel<int> SelectedMonth
        {
            get
            {
                return selectedMonth;
            }
            set
            {
                SetProperty(ref selectedMonth, value);
            }
        }

        private PairViewModel<int> selectedYear;

        public PairViewModel<int> SelectedYear
        {
            get
            {
                return selectedYear;
            }
            set
            {
                SetProperty(ref selectedYear, value);
            }
        }

        private DateTime GetNearestValidDate()
        {
            var year = SelectedYear.Value;
            var month = SelectedMonth.Value;
            var day = SelectedDay.Value;

            while (day > 0)
            {
                try
                {
                    return new DateTime(year, month, day);
                }
                catch
                {
                    day--;
                }
            }

            return DateTime.UtcNow;
        }

        public DateTime ToDate()
        {
            return new DateTime(SelectedYear.Value, SelectedMonth.Value, SelectedDay.Value);
        }

        public void SetDate(DateTime date)
        {
            SelectedYear = Year.Where(y => y.Value == date.Year).First();
            SelectedMonth = Month.Where(m => m.Value == date.Month).First();

            Day.Clear();

            var monthEnumerator = new DateTime(date.Year, date.Month, 1);

            while (monthEnumerator.Month == date.Month)
            {
                Day.Add(new PairViewModel<int>()
                {
                    Value = monthEnumerator.Day,
                    Text = monthEnumerator.ToString("dd dddd")
                });

                monthEnumerator = monthEnumerator.AddDays(1);
            }

            SelectedDay = Day.Where(d => d.Value == date.Day).First();
        }

        private void AddYears()
        {
            var year = DateTime.Now.Year;
            
            for (var y = year - 100; y < year + 11; y++)
            {
                Year.Add(new PairViewModel<int>()
                {
                    Value = y,
                    Text = y.ToString()
                });
            }
        }

        private void AddMonths()
        {
            var january = new DateTime(DateTime.Now.Year, 1, 1);
            
            for (var o = 0; o < 12; o++)
            {
                var month = january.AddMonths(o);

                Month.Add(new PairViewModel<int>()
                {
                    Value = o + 1,
                    Text = month.ToString("MMMM")
                });
            }
        }

        public DateViewModel(DateTime date)
        {
            Year = new ObservableCollection<PairViewModel<int>>();
            Month = new ObservableCollection<PairViewModel<int>>();
            Day = new ObservableCollection<PairViewModel<int>>();
            
            AddYears();
            AddMonths();

            PropertyChanged += (c, e) =>
            {
                if (e.PropertyName == "SelectedYear" || e.PropertyName == "SelectedMonth")
                {
                    var validDate = GetNearestValidDate();
                    
                    SetDate(validDate);
                }
            };            
            
            SetDate(date);
        }

        public DateViewModel() : this(DateTime.UtcNow)
        {

        }
    }
}
