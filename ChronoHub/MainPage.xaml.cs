using System.Collections.ObjectModel;

namespace ChronoHub;

public partial class MainPage : ContentPage
{
	public ObservableCollection<Chrono> ChronoList { get; set; }

	public MainPage()
	{
		InitializeComponent();

		ChronoList = new ObservableCollection<Chrono>();
		BindingContext = this;
	}


	// Method to add a new Chrono to the collection
	private void OnAddChronoClicked(object sender, EventArgs e)
	{
		var newChrono = new Chrono($"Chrono {ChronoList.Count + 1}");
		ChronoList.Add(newChrono);

		// Comandos para cada cronómetro
		newChrono.ButtonWidth = DeviceDisplay.MainDisplayInfo.Width * 0.046;
		Console.WriteLine(DeviceDisplay.MainDisplayInfo.Width);
		Console.WriteLine($"ButtonWidth: {newChrono.ButtonWidth}");
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

