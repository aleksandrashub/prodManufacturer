using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Metsys.Bson;
using MsBox.Avalonia;
using ProdManufacturer.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
using System;
using System.Linq;
using System.IO;

namespace ProdManufacturer;

public partial class Window1 : Window
{
    public static Tovar CurrentTovar;
    public List<Tovar> tovars = new List<Tovar>();
    public Bitmap bitmapImg;
    public string path;
    public string destPath;
    public bool ok;
    public string resPhoto;
    public string filename;
    public string filenameCurr;
    public Window1()
    {
        InitializeComponent();


        if (Helper.tovarEdit != -1)
        {
            CurrentTovar = Helper.User724Context.Tovars.Where(x => x.IdTovar == Helper.tovarEdit).FirstOrDefault();
            idTov.Text = CurrentTovar.IdTovar.ToString();
            name.Text = CurrentTovar.NameTovar.ToString();
            descr.Text = CurrentTovar.Description;
            cost.Text = CurrentTovar.Cost.ToString();
            manuf.Text = CurrentTovar.IdManufacturerNavigation.NameManufacturer;
            switch (CurrentTovar.IdStatus)
            {
                case 1:
                    activity.IsChecked = true;
                    break;
                case 2:
                    activity.IsChecked = false;
                    break;
            }
        }
        else
        {
            CurrentTovar = new Tovar();
            idTov.Text = (Helper.User724Context.Tovars.Count() + 1).ToString();
        }

        dopTovars.ItemsSource = Helper.User724Context.Tovars.Where(x => x.IdStatus == 1);
        addedDopTovars.ItemsSource = CurrentTovar.IdDopTovs.ToList();
    }

    private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        OpenFileDialog openFile = new OpenFileDialog();
        var res = await openFile.ShowAsync(this);
        if (res == null)
            return;
        path = string.Join("", res);
        resPhoto = res.ToString();
        if (res != null)
        {
            bitmapImg = new Bitmap(path);
        }
        img.Source = bitmapImg;
        filename = Path.GetFileName(path);
        destPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Assets";
        string destPathFile = Path.Combine(destPath, filename);

    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ok = true;
        CurrentTovar.NameTovar = name.Text;
        CurrentTovar.Description = descr.Text;
        float f = 0;
        bool success = float.TryParse(cost.Text, out f);
        if (success)
        {
            CurrentTovar.Cost = f;

        }
        else
        {
            ok = false;
            var msb = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Содержатся недопустимые символы в поле стоимости");
            msb.ShowAsync();
        }
        if (Helper.tovarEdit != -1 && filename != null)
        {
            filenameCurr = CurrentTovar.Image;

            destPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Assets";
            File.Delete(Path.Combine(destPath, filenameCurr));


        }
        if (filename != null)
        {
            CurrentTovar.Image = filename;
            File.Move(path, destPath + "\\" + filename);
        }

        switch (activity.IsChecked)
        {
            case true:
                CurrentTovar.IdStatus = 1;
                break;
            case false:
                CurrentTovar.IdStatus = 2;
                break;
        }

        if (ok)
        {
            bool manExists = false;

            foreach (Manufacturer man in Helper.User724Context.Manufacturers)
            {
                if (manuf.Text == man.NameManufacturer)
                {
                    manExists = true;
                    CurrentTovar.IdManufacturer = man.IdManufacturer;
                }

            }
            if (manExists == false)
            {
                Manufacturer man = new Manufacturer();
                man.IdManufacturer = Helper.User724Context.Manufacturers.OrderBy(x => x.IdManufacturer).Last().IdManufacturer + 1;
                man.NameManufacturer = manuf.Text;
                Helper.User724Context.Manufacturers.Add(man);
                Helper.User724Context.SaveChanges();
                CurrentTovar.IdManufacturer = man.IdManufacturer;

            }


            if (Helper.tovarEdit != -1)
            {
                CurrentTovar.IdTovar = Convert.ToInt32(idTov.Text);
                Helper.User724Context.Tovars.Where(x => x.IdTovar == CurrentTovar.IdTovar).FirstOrDefault().NameTovar = CurrentTovar.NameTovar;
                Helper.User724Context.Tovars.Where(x => x.IdTovar == CurrentTovar.IdTovar).FirstOrDefault().Cost = CurrentTovar.Cost;
                Helper.User724Context.Tovars.Where(x => x.IdTovar == CurrentTovar.IdTovar).FirstOrDefault().Description = CurrentTovar.Description;
                Helper.User724Context.Tovars.Where(x => x.IdTovar == CurrentTovar.IdTovar).FirstOrDefault().IdDopTovs = CurrentTovar.IdDopTovs;
                Helper.User724Context.Tovars.Where(x => x.IdTovar == CurrentTovar.IdTovar).FirstOrDefault().Image = CurrentTovar.Image;
                Helper.User724Context.Tovars.Where(x => x.IdTovar == CurrentTovar.IdTovar).FirstOrDefault().IdManufacturerNavigation.NameManufacturer = CurrentTovar.IdManufacturerNavigation.NameManufacturer;
                Helper.User724Context.Tovars.Where(x => x.IdTovar == CurrentTovar.IdTovar).FirstOrDefault().IdTovar = CurrentTovar.IdTovar;
                Helper.tovarEdit = -1;

            }
            else
            {
                Helper.User724Context.Tovars.Add(CurrentTovar);
            }


            Helper.User724Context.SaveChanges();
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

    }

    private void Button_Click_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainWindow main = new MainWindow();
        main.Show();
        this.Close();
    }

    private void addTovarOk_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (dopTovars.SelectedItem != null)
        {
             var item = dopTovars.SelectedItem as Tovar;
             if (!CurrentTovar.IdDopTovs.Contains(item))
             {
                 CurrentTovar.IdDopTovs.Add(item);
                 addedDopTovars.ItemsSource = CurrentTovar.IdDopTovs.ToList();
             }
             else
             {
                 var msg = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Данный товар уже содержится в связанных товарах");
                 msg.ShowAsync();
             }
        }
        else
        {
            var msg = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Вы не выбрали товар для добавления");
            msg.ShowAsync();
        }
    }

    private void DeleteDop_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var item = addedDopTovars.SelectedItem as Tovar;
          CurrentTovar.IdDopTovs.Remove(item);

        addedDopTovars.ItemsSource = CurrentTovar.IdDopTovs.ToList();
    }
}