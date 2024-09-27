using Avalonia.Controls;
using Metsys.Bson;
using ProdManufacturer.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using Avalonia.Media.Imaging;
using Microsoft.EntityFrameworkCore;

namespace ProdManufacturer
{
    public partial class MainWindow : Window
    {
        public List<Tovar> tovar = new List<Tovar>();
        public List<Sale> sales = new List<Sale>();
        public List<TovarDop> tovarDops = new List<TovarDop>();
        public List<string> manufacturers = new List<string>();
        public List<Status> statuses = new List<Status>();
        public MainWindow()
        {
            InitializeComponent();
            loadServices();
        }

        public void loadServices()
        {
            manufacturers.Clear();
            manufacturers.Add("Все производители");
            manufacturers.AddRange(Helper.User724Context.Manufacturers.Select(x => x.NameManufacturer).ToList());

            filter.ItemsSource = manufacturers;

            tovar = Helper.User724Context.Tovars.Include(z => z.IdManufacturerNavigation).Include(a => a.IdDopTovs).Include(b => b.DopImgs).ToList();
            sales = Helper.User724Context.Sales.ToList();
            statuses = Helper.User724Context.Statuses.ToList();



            string searchText = search.Text ?? "";
            int count = searchText.Split(' ').Length;
            string[] values = new string[count];

            values = searchText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string s in values)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    tovar = tovar.Where(x => x.NameTovar.Contains(s)
                    || x.Description.Contains(s)).ToList();
                }
                else
                {
                    continue;
                }
            }

            if (filter.SelectedIndex == 0)
            {
            }
            else if (filter.SelectedValue != null)
            {
                string selectedMan = filter.SelectedValue.ToString();
                tovar = (List<Tovar>)tovar.Where(x => x.IdManufacturerNavigation.NameManufacturer.ToString() == selectedMan).ToList();

            }


            switch (sort.SelectedIndex)
            {
                case 0:
                    tovar = tovar.OrderBy(x => x.Cost).ToList();
                    break;
                case 1:
                    tovar = tovar.OrderByDescending(x => x.Cost).ToList();
                    break;
            }

            listbox.ItemsSource = tovar.Select(x => new
            {
                x.IdTovar,
                x.NameTovar,
                x.Description,
                x.Cost,
                PhotoPath = new Bitmap($"Assets/{x.Image}"),
                x.IdManufacturerNavigation.NameManufacturer,
                DopTov = x.IdDopTovs.Count().ToString(),
                checkDopImgs = checkTovHasDopImgs(x.IdTovar, tovar)
            }).ToList();
            allTovs.Text = Helper.User724Context.Tovars.Count().ToString();
            currTovs.Text = tovar.Count().ToString();


        }

        private bool checkTovHasDopImgs(int id, List<Tovar> tovar)
        {
            bool res = false;

                   if (tovar.Where(x => x.IdTovar == id).FirstOrDefault().DopImgs.Count() > 0)
                    {
                        res = true;
                        return true;
                    }
                    else
                    {
                        return res;

                    }

        }


        private void ComboBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
            loadServices();
        }

        private void ComboBox_SelectionChanged_1(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
            loadServices();
        }

        private void search_KeyUp(object? sender, Avalonia.Input.KeyEventArgs e)
        {

            loadServices();

        }

        private void Delete_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            int ind = (int)((sender as Button)!).Tag!;
            var tov = Helper.User724Context.Tovars.FirstOrDefault(x => x.IdTovar == ind);
            if (tov.IdDopTovs.Count > 0)
            {
                tov.IdDopTovs.Clear();
            }
            Helper.User724Context.Tovars.Remove(tov);
            Helper.User724Context.SaveChanges();
            loadServices();
        }

        private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Window1 window = new Window1();
            window.Show();
            this.Close();

        }


        private void Edit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            int ind = (int)((sender as Button)!).Tag!;
            var tov = Helper.User724Context.Tovars.FirstOrDefault(x => x.IdTovar == ind);
            Helper.tovarEdit = ind;
            Window1 window = new Window1();
            window.Show();
            this.Close();
        }

        private void nazadBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
        }

        private void vperedBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
        }
    }
}
