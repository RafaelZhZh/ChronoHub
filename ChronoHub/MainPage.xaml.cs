using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;

namespace ChronoHub;

public partial class MainPage : ContentPage
{
	public ObservableCollection<Chrono> ChronoList { get; set; }

	public MainPage()
	{
		InitializeComponent();

		ChronoList = new ObservableCollection<Chrono>();
		BindingContext = this;

		MessagingCenter.Subscribe<NewChronoPage, Chrono>(this, "AddChronoMessage", (sender, item) =>
        {
            ChronoList.Add(item);
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
        }
    }
}

