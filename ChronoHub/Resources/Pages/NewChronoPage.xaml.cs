using CommunityToolkit.Maui.Views;

namespace ChronoHub;
public partial class NewChronoPage : Popup
{
    public NewChronoPage()
    {
        InitializeComponent();
    }

    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(chrono_name_form.Text))
        {
            var newChrono = new Chrono(chrono_name_form.Text);
            newChrono.ButtonWidth = DeviceDisplay.MainDisplayInfo.Width * 0.046;
            MessagingCenter.Send(this, "AddChronoMessage", newChrono);

            await CloseAsync();
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "You must enter a name to the new chronometer.", "OK");
        }
    }
    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }
}
