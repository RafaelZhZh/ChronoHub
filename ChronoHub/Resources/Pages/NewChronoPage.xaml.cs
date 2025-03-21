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
            MessagingCenter.Send(this, "AddChronoMessage", chrono_name_form.Text);

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
