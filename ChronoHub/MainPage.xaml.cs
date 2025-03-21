using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;

namespace ChronoHub;

public partial class MainPage : ContentPage
{
	public ObservableCollection<Chrono> ChronoList { get; set; }
	public DatabaseService dbService = new DatabaseService();

	public MainPage()
	{
		InitializeComponent();

		ChronoList = new ObservableCollection<Chrono>();
		BindingContext = this;

		// Load the ChronoList from the database
		var list_chrono_sql = dbService.GetData();
		foreach (var item in list_chrono_sql)
		{
			item.ButtonWidth = DeviceDisplay.MainDisplayInfo.Width * 0.046;
			ChronoList.Add(item);
		}

		MessagingCenter.Subscribe<NewChronoPage, string>(this, "AddChronoMessage", (sender, item) =>
        {
			var new_chrono = new Chrono(item);
			new_chrono.ButtonWidth = DeviceDisplay.MainDisplayInfo.Width * 0.046;
            ChronoList.Add(new_chrono);
			dbService.SaveData(ChronoList);
        });

		MessagingCenter.Subscribe<Chrono, string>(this, "OnDataChanged", (sender, item) =>
        {
			dbService.SaveData(ChronoList);
        });


	}


	// Method to add a new Chrono to the collection
	private async void OnAddChronoClicked(object sender, EventArgs e)
	{
		var popup = new NewChronoPage();
        this.ShowPopup(popup);
	}

	// Method to remove a Chrono from the collection
    public void OnRemoveChronoClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var item = (Chrono)button.CommandParameter;
        if (ChronoList.Contains(item))
        {
            ChronoList.Remove(item);
			dbService.SaveData(ChronoList);
        }
    }

	public void SaveOnSleep()
	{
		dbService.SaveData(ChronoList);
	}
}

