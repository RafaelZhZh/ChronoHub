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
    public double ButtonWidth { get; set; }
    private System.Timers.Timer _timer;
    private int _seconds;

    // Comandos
    
    public ICommand StartCommand { get; set;}
    public ICommand StopCommand { get; set;}

    public Chrono(string _name)
    {
        Name = _name;
        Time = "00:00";
        _seconds = 0;
        _timer = new System.Timers.Timer(1000);  // Timer que se dispara cada segundo
        _timer.Elapsed += OnTimerElapsed;
        CanStart = true;
        CanStop = false;
        StartCommand = new Command(() => StartChrono());
        StopCommand = new Command(() => StopChrono());
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        _seconds++;
        Time = TimeSpan.FromSeconds(_seconds).ToString(@"mm\:ss");
        OnPropertyChanged(nameof(Time));
    }

    public void StartChrono()
    {
        if (CanStart)
        {
            _timer.Start();
            CanStart = false;
            CanStop = true;
            OnPropertyChanged(nameof(CanStart));
            OnPropertyChanged(nameof(CanStop));
        }
    }

    public void StopChrono()
    {
        if (CanStop)
        {
            _timer.Stop();
            CanStart = true;
            CanStop = false;
            OnPropertyChanged(nameof(CanStart));
            OnPropertyChanged(nameof(CanStop));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
