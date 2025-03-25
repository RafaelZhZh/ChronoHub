using CommunityToolkit.Maui.Views;

namespace ChronoHub;
public partial class NewChronoPage : Popup
{
    public string selectedValue { get; set; }
    public List<string> filter_options { get; set; }
    public NewChronoPage(Dictionary<string,bool> actual_filter)
    {
        InitializeComponent();
        filter_options = new List<string>();
        foreach (var item in actual_filter)
        {
            filter_options.Add(item.Key);
        }
        GenerateRadioButtons();
    }

    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(chrono_name_form.Text))
        {

            MessagingCenter.Send(this, "AddChronoMessage", new List<string>{
                chrono_name_form.Text,
                selectedValue
            });
            
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

    private void GenerateRadioButtons()
    {
        RadioButtonStackLayout.Children.Clear();

        foreach (var option in filter_options)
        {
            var radioButton = new RadioButton
            {
                Content = option,
                GroupName = "Group1"
            };
            if (option == "None")
            {
                radioButton.IsChecked = true;
                selectedValue = "None";
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
