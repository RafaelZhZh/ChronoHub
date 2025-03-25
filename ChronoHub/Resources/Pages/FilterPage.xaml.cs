using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;

namespace ChronoHub;

public class CheckableItem
{
    public required string Name { get; set; }
    public bool IsChecked { get; set; }
}

public partial class FilterPage : Popup
{
    public ObservableCollection<CheckableItem> Items { get; set; }
    public FilterPage(Dictionary<string,bool> actual_filter)
    {
        InitializeComponent();

        Items = new ObservableCollection<CheckableItem>();
        foreach (var item in actual_filter)
        {
            Items.Add(new CheckableItem { Name = item.Key, IsChecked = item.Value });
        }

        BindingContext = this;
    }

    private async void OnFilterButtonClicked(object sender, EventArgs e)
    {
        if (Items.Any(x => x.IsChecked))
        {
            Dictionary<string,bool> filter = new Dictionary<string,bool>();
            foreach (var item in Items)
            {
                filter[item.Name] = item.IsChecked;
            }
            MessagingCenter.Send(this, "FilterMessage", filter);

            await CloseAsync();
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "You must select at least one color to filter.", "OK");
        }
    }
    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }
}
