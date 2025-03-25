using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using System.Timers;

namespace ChronoHub;

public partial class MainPage : ContentPage
{
	public ObservableCollection<Chrono> ChronoList { get; set; }
	public DatabaseService dbService = new DatabaseService();
	public Dictionary<string, bool> actualFilter = new Dictionary<string, bool>{
		{"None", true},
		{"Red", true},
		{"Green", true},
		{"Blue", true}
	};
	public bool SomethingSelected { get; set; }

	private System.Timers.Timer _timer;

	public MainPage()
	{
		InitializeComponent();

		ChronoList = new ObservableCollection<Chrono>();
		BindingContext = this;

		var list_chrono_sql = dbService.GetData();
		foreach (var item in list_chrono_sql)
		{
			item.ButtonWidth = DeviceDisplay.MainDisplayInfo.Width * 0.046;
			ChronoList.Add(item);
		}
		_timer = new System.Timers.Timer(1);  // Timer que se dispara cada segundo
		_timer.Elapsed += OnTimerElapsed;
		_timer.Start();

		MessagingCenter.Subscribe<NewChronoPage, List<string>>(this, "AddChronoMessage", (sender, item) =>
        {
			if (ChronoList.Any(x => x.Name == item[0]))
			{
				Application.Current.MainPage.DisplayAlert("Error", "There is already a chronometer with that name", "OK");
			}
			else{
				var new_chrono = new Chrono(item[0]);
				new_chrono.ChangeFilter(item[1]);
				new_chrono.ButtonWidth = DeviceDisplay.MainDisplayInfo.Width * 0.046;
				ChronoList.Add(new_chrono);
				dbService.SaveData(ChronoList);
			}
        });

		MessagingCenter.Subscribe<EditChronoPage, List<string>>(this, "ChangeMessage", (sender, item) =>
        {
			if (ChronoList.Any(x => x.Name == item[0]) && item[0] != item[1])
			{
				Application.Current.MainPage.DisplayAlert("Error", "There is already a chronometer with that name", "OK");
			}
			else{
				Chrono? existingChrono = ChronoList.FirstOrDefault(x => x.Name == item[1]);
				if (existingChrono != null)
				{
					existingChrono.ChangeName(item[0]);
					existingChrono.ChangeFilter(item[2]);
					existingChrono.ChangeFiltered(actualFilter);
				}
			}
        });

		MessagingCenter.Subscribe<Chrono, string>(this, "OnDataChanged", (sender, item) =>
        {
			dbService.SaveData(ChronoList);
        });

		MessagingCenter.Subscribe<FilterPage, Dictionary<string,bool>>(this, "FilterMessage", (sender, item) =>
        {
			actualFilter = item;
			foreach(Chrono chrono in ChronoList)
			{
				chrono.ChangeSelected(false);
				chrono.ChangeFiltered(actualFilter);
			}
		}); 

		MessagingCenter.Subscribe<DeletePage, List<Chrono>>(this, "DeleteMessage", (sender, item) =>
        {
			foreach (var chrono in item)
			{
				ChronoList.Remove(chrono);
			}
			dbService.SaveData(ChronoList);
        });

	}

	// Method to add a new Chrono to the collection
	public void OnAddChronoClicked(object sender, EventArgs e)
	{		
		var popup = new NewChronoPage(actualFilter);
		this.ShowPopup(popup);
	}

	// Method to remove a Chrono from the collection
    public void OnRemoveChronoClicked(object sender, EventArgs e)
    {
		
        var button = (Button)sender;
        var item = (Chrono)button.CommandParameter;

		var popup = new DeletePage(new List<Chrono>{item});
        this.ShowPopup(popup);
    }

	public void SaveOnSleep()
	{
		dbService.SaveData(ChronoList);
	}

	public void OnStartSelectedClicked(object sender, EventArgs e)
	{
		DateTime dateStart = DateTime.Now;
		foreach (var item in ChronoList)
		{
			if (item.IsSelected)
			{
				item.StartChrono(dateStart);
			}
		}
	}

	public void OnStopSelectedClicked(object sender, EventArgs e)
	{
		DateTime dateStop = DateTime.Now;
		foreach (var item in ChronoList)
		{
			if (item.IsSelected)
			{
				item.StopChrono(dateStop);
			}
		}
	}

	public void OnRemoveSelectedClicked(object sender, EventArgs e)
	{
		var itemsToRemove = ChronoList.Where(x => x.IsSelected).ToList();

		var popup = new DeletePage(itemsToRemove);
        this.ShowPopup(popup);
	}

	public void OnCheckboxChanged(object sender, EventArgs e)
	{
		SomethingSelected = ChronoList.Any(x => x.IsSelected);
		OnPropertyChanged(nameof(SomethingSelected));
	}

	public void OnEditChronoClicked(object sender, EventArgs e)
	{
		var button = (Button)sender;
		var item = (Chrono)button.CommandParameter;
		var popup = new EditChronoPage(item,actualFilter);
		this.ShowPopup(popup);
	}

	public void OnFilterClicked(object sender, EventArgs e)
	{
		var popup = new FilterPage(actualFilter);
		this.ShowPopup(popup);
	}

	private void OnTimerElapsed(object sender, ElapsedEventArgs e)
	{
		DateTime dateUpdate = DateTime.Now;
		foreach (var item in ChronoList){
			item.UpdateTimeRunning(dateUpdate);
		}
	}
}

