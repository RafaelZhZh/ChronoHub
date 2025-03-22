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
    public bool IsSelected { get; set; }

    
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
        IsSelected = false;

        StartCommand = new Command(() => StartChrono());
        StopCommand = new Command(() => StopChrono());

        TimeSpan auxtime = TimeSpan.FromSeconds(_seconds);
        Time = string.Format("{0:D2}:{1:D2}:{2:D2}", (int)auxtime.TotalHours, auxtime.Minutes, auxtime.Seconds);
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        TimeSpan auxtime = TimeSpan.FromSeconds(_seconds+(int)(DateTime.Now - DateTimeLastStart).TotalSeconds);
        Time = string.Format("{0:D2}:{1:D2}:{2:D2}", (int)auxtime.TotalHours, auxtime.Minutes, auxtime.Seconds);
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
