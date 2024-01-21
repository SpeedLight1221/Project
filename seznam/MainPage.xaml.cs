using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace seznam;

public partial class MainPage : ContentPage
{

    public ObservableCollection<ImageClass> Images { get; set; } = new ObservableCollection<ImageClass>();
    ImageClass sel;

    public MainPage()
    {

        InitializeComponent();
        BindingContext = this;
    }



    private async void FilePicker_Clicked(object sender, EventArgs e)
    {
        var img = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Pick An Image",
            FileTypes = FilePickerFileType.Images
        });

        var imgSource = img.FullPath.ToString();
        PreviewImg.Source = imgSource;
    }

    private void Add_Clicked(object sender, EventArgs e)
    {
        if (PreviewImg.Source != null)
        {

            Images.Add(
                new ImageClass(PreviewImg.Source, TitleEntry.Text, DescEntry.Text, LocationEntry.Text, DateEntry.Date)
                );
        }
    }

    private void List_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        sel = e.SelectedItem as ImageClass;
        PreviewImg.Source = sel.ImgPath;
        TitleEntry.Text = sel.Title;
        DescEntry.Text = sel.Description;
        LocationEntry.Text = sel.Location;
        DateEntry.Date = sel.MadeOn;
    }

    private void Modify_Clicked(object sender, EventArgs e)
    {
        if (PreviewImg.Source != null)
        {
            sel.Title = TitleEntry.Text;
            sel.Description = DescEntry.Text;
            sel.Location = LocationEntry.Text;
            sel.ImgPath = PreviewImg.Source;
            sel.MadeOn = DateEntry.Date;
            sel.ModifiedOn = DateTime.Now;
        }





    }

    private void Remove_Clicked(object sender, EventArgs e)
    {
        if (List.SelectedItem != null)
        {
            Images.Remove(List.SelectedItem as ImageClass);
            PreviewImg.Source = null;
            TitleEntry.Text = null;
            DescEntry.Text = null;
            LocationEntry.Text = null;
        }
    }

    private async void View_Clicked(object sender, EventArgs e)
    {
        if (List.SelectedItem != null)
        {
            Dictionary<string, object> par = new Dictionary<string, object>
            {
                {"Selected",List.SelectedItem as ImageClass},
                {"List",Images }
            };

            await Shell.Current.GoToAsync("///ViewPage",par);
        }
    }



}


public class ImageClass : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;
    ImageSource imgPath;
    public ImageSource ImgPath { get => imgPath; set { imgPath = value; PropertyChanged?.Invoke(this.imgPath, new PropertyChangedEventArgs("ImgPath")); } }

    string title;
    public string Title { get => title; set { title = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Title")); } }

    string description;
    public string Description { get => description; set { description = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Description")); } }

    string location;
    public string Location { get => location; set { location = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Location")); } }

    DateTime madeon;
    public DateTime MadeOn { get => madeon; set { madeon = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MadeOn")); } }

    DateTime modifiedOn;
    public DateTime ModifiedOn { get => modifiedOn; set { modifiedOn = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ModifiedOn")); } }

    public ImageClass(ImageSource img, string title, string description, string location, DateTime madeOn)
    {
        this.ImgPath = img;
        Title = title;
        Description = description;
        Location = location;
        MadeOn = madeOn;
        ModifiedOn = DateTime.Now;
    }



}

