using Microsoft.Maui.Controls;
using SQLite;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace seznam;

public partial class MainPage : ContentPage
{
    private readonly DB dB;
    public ObservableCollection<ImageClass> Images { get; set; } = new ObservableCollection<ImageClass>();
    ImageClass sel;

    public MainPage(DB dB)
    {

        InitializeComponent();
        BindingContext = this;
        this.dB = dB;
        DB_all();

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
            ImageClass ee = new ImageClass(PreviewImg.Source, TitleEntry.Text, DescEntry.Text, LocationEntry.Text, DateEntry.Date, null, Images.Count);
            Images.Add(ee);



            DB_add(ee);
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

            DB_modify(sel);
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

            await Shell.Current.GoToAsync("///ViewPage", par);
        }
    }


    private async void DB_add(ImageClass c)
    {
        await dB.Insert(new DBImage
        {
            Img = c.ImgPath.ToString(),
            Title = c.Title,
            Description = c.Description,
            Location = c.Location,
            Modified = c.ModifiedOn,
            Created = c.MadeOn

        }) ;
    }

    private async void DB_modify(ImageClass c)
    {
        await dB.Update(new DBImage
        {
            Id = c.Id,
            Img = c.ImgPath.ToString(),
            Title = c.Title,
            Description = c.Description,
            Location = c.Location,
            Modified = c.ModifiedOn,
            Created = c.MadeOn

        });
    }

    private async void DB_all()
    {
        List<DBImage> res = await dB.GetAll();

        foreach (DBImage img in res)
        {
            Images.Add(
                new ImageClass(ImageSource.FromFile(img.Img), img.Title, img.Description, img.Location, img.Created, img.Modified,img.Id)

                );
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


   public int Id;
    public ImageClass(ImageSource img, string title, string description, string location, DateTime madeOn, DateTime? modifiedOn, int id)
    {
        this.ImgPath = img;
        Title = title;
        Description = description;
        Location = location;
        MadeOn = madeOn;
        if (modifiedOn != null) { this.modifiedOn = Convert.ToDateTime(modifiedOn); }
        else { this.modifiedOn = DateTime.Now; }
        Id = id;
    }



}








