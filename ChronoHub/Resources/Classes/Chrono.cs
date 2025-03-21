using System.ComponentModel;
using System.Timers;
using System.Windows.Input;

namespace ChronoHub;
public class Chrono : INotifyPropertyChanged
{
    public string Name { get; set; }
    public string Time { get; set; }
    public bool CanStart { get; set; }
    public bool CanStop { get; set; }
    public DateTime DateTimeLastStart { get; set; }
    public double ButtonWidth { get; set; }
    private System.Timers.Timer _timer;
    public int _seconds;

    
    public ICommand StartCommand { get; set;}
    public ICommand StopCommand { get; set;}
    public void changedData()
    {
        MessagingCenter.Send(this, "OnDataChanged", "Changed");
    }

    public Chrono(string _name,int _time = 0, bool _canStart = true, bool _canStop = false, DateTime _dateTimeLastStart = new DateTime())
    {
        Name = _name;
        DateTimeLastStart = _dateTimeLastStart;
        _seconds = _time;
        _timer = new System.Timers.Timer(1000);  // Timer que se dispara cada segundo
        _timer.Elapsed += OnTimerElapsed;
        if(_canStop){
            _seconds = _seconds+(int)(DateTime.Now - DateTimeLastStart).TotalSeconds;
            _timer.Start();
        }
        CanStart = _canStart;
        CanStop = _canStop;

        StartCommand = new Command(() => StartChrono());
        StopCommand = new Command(() => StopChrono());
        Time = TimeSpan.FromSeconds(_seconds).ToString(@"mm\:ss");
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        Time = TimeSpan.FromSeconds(_seconds+(int)(DateTime.Now - DateTimeLastStart).TotalSeconds).ToString(@"mm\:ss");
        OnPropertyChanged(nameof(Time));
    }

    public void StartChrono()
    {
        if (CanStart)
        {
            _timer.Start();
            DateTimeLastStart = DateTime.Now;
            CanStart = false;
            CanStop = true;
            OnPropertyChanged(nameof(CanStart));
            OnPropertyChanged(nameof(CanStop));
            changedData();
        }
    }

    public void StopChrono()
    {
        if (CanStop)
        {
            _timer.Stop();
            _seconds = _seconds+(int)(DateTime.Now - DateTimeLastStart).TotalSeconds;
            CanStart = true;
            CanStop = false;
            OnPropertyChanged(nameof(CanStart));
            OnPropertyChanged(nameof(CanStop));
            changedData();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
