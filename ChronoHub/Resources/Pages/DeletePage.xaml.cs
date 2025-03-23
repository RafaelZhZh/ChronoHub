using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;


namespace ChronoHub;
public partial class DeletePage : Popup
{
    private List<Chrono> chronos { get; set; }
    public DeletePage(List<Chrono> chronos_to_delete)
    {
        InitializeComponent();
        chronos = chronos_to_delete;
        GenerateListNames();
    }

    private async void OnYesButtonClicked(object sender, EventArgs e)
    {
        MessagingCenter.Send(this, "DeleteMessage", chronos);
        await CloseAsync();
    }
    private async void OnNoButtonClicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }

    private void GenerateListNames()
    {
        NamesStackLayout.Children.Clear();
        Console.WriteLine(Device.GetNamedSize(NamedSize.Small, typeof(Label)));
        foreach (var item in chronos)
        {
            var nameLabel = new Label
            {
                Text = item.Name,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };
            NamesStackLayout.Children.Add(nameLabel);
        }
    }
}
