using CommunityToolkit.Maui.Views;

namespace ChronoHub;
public partial class EditChronoPage : Popup
{
    public string old_name { get; set; }
    public string selectedValue { get; set; }
    public List<string> filter_options { get; set; }
    public EditChronoPage(Chrono chrono, Dictionary<string,bool> actual_filter)
    {
        InitializeComponent();
        filter_options = new List<string>();
        foreach (var item in actual_filter)
        {
            filter_options.Add(item.Key);
        }
        GenerateRadioButtons(chrono);

        chrono_name_form.Text = chrono.Name;
        old_name = chrono.Name;
    }

    private async void OnChangeButtonClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(chrono_name_form.Text))
        {
            List<string> chrono_names = [chrono_name_form.Text, old_name, selectedValue];
            MessagingCenter.Send(this, "ChangeMessage", chrono_names);

            await CloseAsync();
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "You must enter the new name to the chronometer.", "OK");
        }
    }
    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }

    private void GenerateRadioButtons(Chrono chrono)
    {
        RadioButtonStackLayout.Children.Clear();

        foreach (var option in filter_options)
        {
            var radioButton = new RadioButton
            {
                Content = option,
                GroupName = "Group1"
            };
            if (option == chrono.FilterColor)
            {
                selectedValue = "None";
                radioButton.IsChecked = true;
            }
            radioButton.CheckedChanged += OnCheckedChanged;
            RadioButtonStackLayout.Children.Add(radioButton);
        }
    }

    private void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            RadioButton radioButton = (RadioButton)sender;
            selectedValue = radioButton.Content.ToString();
        }
    }
}
