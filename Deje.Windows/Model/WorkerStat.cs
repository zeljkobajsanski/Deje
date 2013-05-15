using System.ComponentModel;
using System.Diagnostics;

namespace Deje.Windows.Model
{
    public class WorkerStat : INotifyPropertyChanged
    {

        private readonly Stopwatch m_Stopwatch = new Stopwatch();

        public string Name { get; set; }

        public long ElapsedTime
        {
            get { return m_ElapsedTime; }
            set
            {
                if (ElapsedTime != value)
                {
                    m_ElapsedTime = value;
                    OnPropertyChanged("ElapsedTime");
                }
                
            }
        }

        private int m_Requests;
        private long m_ElapsedTime;
        private double m_AverageTime;

        public int Requests
        {
            get { return m_Requests; }
            set
            {
                if (Requests != value)
                {
                    m_Requests = value;
                    OnPropertyChanged("Requests");
                }
            }
        }

        public double AverageTime
        {
            get { return m_AverageTime; }
            private set
            {
                if (AverageTime != value)
                {
                    m_AverageTime = value;
                    OnPropertyChanged("AverageTime");
                }
                
            }
        }

        private int m_Errors;
        public int Errors
        {
            get { return m_Errors; }
            set
            {
                if (m_Errors != value)
                {
                    m_Errors = value;
                    OnPropertyChanged("Errors");
                }
                
            }
        }

        public void Start()
        {
            m_Stopwatch.Start();
        }

        public void Stop()
        {
            m_Stopwatch.Stop();
            ElapsedTime = m_Stopwatch.ElapsedMilliseconds;
            AverageTime = ElapsedTime / (double)Requests;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}