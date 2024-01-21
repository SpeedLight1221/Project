using System.Collections.ObjectModel;
using System.Web;

namespace seznam;


[QueryProperty(nameof(Selected), "Selected")]
[QueryProperty(nameof(Images), "List")]
public partial class ViewPage : ContentPage
{

    ImageClass sel;
    int index;
    public ImageClass Selected
    {
        get => sel; 
        set
        {
           sel = value;
            TitleText.Text = Selected.Title;
            MainImg.Source = Selected.ImgPath;
            DescriptionText.Text = Selected.Description;
            LocationText.Text = Selected.Location;
            MadeText.Text = Selected.MadeOn.ToString();
            ModifiedText.Text = Selected.ModifiedOn.ToString();
            

        }
    }

    public ObservableCollection<ImageClass> Images { get; set ; } = new ObservableCollection<ImageClass>();
    public ViewPage()
    {
        InitializeComponent();
        

    }



    private void Left_Clicked(object sender, EventArgs e)
    {
        index = Images.IndexOf(Selected);
        index--;

        try
        {
            Selected = Images[index];
        }
        catch
        {
            return;
        }

    }

    private void Right_Clicked(object sender, EventArgs e)
    {
        index = Images.IndexOf(Selected);
        index++;

        try
        {
            Selected = Images[index];
        }
        catch
        {
            return;
        }
    }

    private async void Exit_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }
}