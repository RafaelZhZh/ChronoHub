using System.ComponentModel;
using System.Windows.Input;
using Java.Sql;

namespace ChronoHub;
public class Chrono : INotifyPropertyChanged
{
    public string Name { get; set; }
    public string Time { get; set; }
    public bool CanStart { get; set; }
    public bool CanStop { get; set; }
    public DateTime DateTimeLastStart { get; set; }
    public double ButtonWidth { get; set; }
    public int _milliseconds;
    public bool IsSelected { get; set; }
    public string FilterColor { get; set; }
    public int MaxHeight { get; set; }

    
    public ICommand StartCommand { get; set;}
    public ICommand StopCommand { get; set;}
    public void changedData()
    {
        MessagingCenter.Send(this, "OnDataChanged", "Changed");
    }

    public Chrono(string _name,int _time = 0, bool _canStart = true, bool _canStop = false, DateTime _dateTimeLastStart = new DateTime(), string _FilterColor = "None")
    {
        Name = _name;
        DateTimeLastStart = _dateTimeLastStart;
        _milliseconds = _time;
        CanStart = _canStart;
        CanStop = _canStop;
        IsSelected = false;
        FilterColor = _FilterColor;
        MaxHeight = 9000;

        StartCommand = new Command(() => StartChrono(DateTime.Now));
        StopCommand = new Command(() => StopChrono(DateTime.Now));

        UpdateTime(_milliseconds);
    }

    public void UpdateTimeRunning(DateTime _dateUpdate)
    {
        if (CanStop){
            UpdateTime(_milliseconds+(int)(_dateUpdate - DateTimeLastStart).TotalMilliseconds);  
        }
        else{
            UpdateTime(_milliseconds);
        }
    }

    private void UpdateTime(int _auxmilliseconds)
    {
        TimeSpan auxtime = TimeSpan.FromMilliseconds(_auxmilliseconds);
        Time = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", (int)auxtime.TotalHours, auxtime.Minutes, auxtime.Seconds, auxtime.Milliseconds);
        OnPropertyChanged(nameof(Time));
    }

    public void StartChrono(DateTime _dateStart)
    {
        if (CanStart)
        {
            DateTimeLastStart = _dateStart;
            CanStart = false;
            CanStop = true;
            OnPropertyChanged(nameof(CanStart));
            OnPropertyChanged(nameof(CanStop));
            changedData();
        }
    }

    public void StopChrono(DateTime _dateStop)
    {
        if (CanStop)
        {
            _milliseconds = _milliseconds+(int)(_dateStop - DateTimeLastStart).TotalMilliseconds;
            CanStart = true;
            CanStop = false;
            OnPropertyChanged(nameof(CanStart));
            OnPropertyChanged(nameof(CanStop));
            changedData();
        }
    }
    public void ChangeSelected(bool selected){
        IsSelected = selected;
        OnPropertyChanged(nameof(IsSelected));
    }

    public void ChangeName(string new_name){
        Name = new_name;
        OnPropertyChanged(nameof(Name));
        changedData();
    }

    public void ChangeFilter(string new_filter){
        FilterColor = new_filter;
        changedData();
    }

    public void ChangeFiltered(Dictionary<string,bool> actualFilter){
        if (actualFilter.ContainsKey(FilterColor))
        {
            MaxHeight = actualFilter[FilterColor] ? 9000 : 0;
            OnPropertyChanged(nameof(MaxHeight));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
