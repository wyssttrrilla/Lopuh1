using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lopuh.Model;

namespace Lopuh.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListProdPage.xaml
    /// </summary>
    public partial class ListProdPage : Page
    {
        public ListProdPage()
        {
            InitializeComponent();
            RefreshPagination();
            SortCBAdd();
            FilterCB.Text = "Фильтрация";
            FilterCB.ItemsSource = App.DemoDb.ProductType.ToList();
            FilterCB.DisplayMemberPath = "Title";
            RefreshButtons();
            ImageNull();
            SortInfo();
        }
        private void SortCBAdd()
        {
            SortCB.Text = "Сортировка";
            SortCB.Items.Add("Наименование а-я");
            SortCB.Items.Add("Наименование я-а");
        }
        private void ImageNull()
        {
            foreach (var item in App.DemoDb.Product)
            {
                if (item.Image == "")
                    item.Image = @"\products\picture.png";
            }
            App.DemoDb.SaveChanges();
        }
        int pageNumber;
        private void RefreshPagination()
        {
            DGWrites.ItemsSource = null;
            if (SortCB.Text != null)
            {
                SortInfo();
            }
            else
            {
                DGWrites.ItemsSource = App.DemoDb.Product.OrderBy(x => x.Title).Skip(pageNumber * 10).Take(10).ToList();
            }
        }
        private void BLeft_Click(object sender, RoutedEventArgs e)
        {
            if (pageNumber == 0)
                return;
            pageNumber--;
            RefreshPagination();
        }

        private void BRight_Click(object sender, RoutedEventArgs e)
        {
            if (App.DemoDb.Product.ToList().Count % 10 == 0)
            {
                if (pageNumber == (App.DemoDb.Product.ToList().Count / 10) - 1)
                    return;
            }
            else
            {
                if (pageNumber == (App.DemoDb.Product.ToList().Count / 10))
                    return;
            }
            pageNumber++;
            RefreshPagination();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            pageNumber = Convert.ToInt32(button.Content) - 1;
            RefreshPagination();
        }

        private void RefreshButtons()
        {
            WPButtons.Children.Clear();
            if (App.DemoDb.Product.ToList().Count % 10 == 0)
            {
                for (int i = 0; i < App.DemoDb.Product.ToList().Count / 10; i++)
                {
                    Button button = new Button();
                    button.Content = i + 1;
                    button.Click += Button_Click;
                    button.Margin = new Thickness(5);
                    button.Width = 20;
                    button.Height = 20;
                    button.FontSize = 14;
                    WPButtons.Children.Add(button);
                }
            }
            else
            {
                for (int i = 0; i < App.DemoDb.Product.ToList().Count / 10 + 1; i++)
                {
                    Button button = new Button();
                    button.Content = i + 1;
                    button.Click += Button_Click;
                    button.Margin = new Thickness(5);
                    button.Width = 20;
                    button.Height = 20;
                    button.FontSize = 14;
                    WPButtons.Children.Add(button);
                }
            }
        }

        private void SearchTB_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var res = App.DemoDb.Product.ToList();
            res = res.Where(x => x.Title.ToLower().Contains(SearchTB.Text.ToLower())).Skip(pageNumber * 10).Take(10).ToList();
            DGWrites.ItemsSource = res.ToList();


        }

        private void SortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortInfo();
        }
        private void SortInfo()
        {
            switch (SortCB.SelectedItem)
            {
                case "Наименование а-я":
                    DGWrites.ItemsSource = null;
                    DGWrites.ItemsSource = App.DemoDb.Product.OrderBy(x => x.Title).Skip(pageNumber * 10).Take(10).ToList();
                    break;
                case "Наименование я-а":
                    DGWrites.ItemsSource = null;
                    DGWrites.ItemsSource = App.DemoDb.Product.OrderByDescending(x => x.Title).Skip(pageNumber * 10).Take(10).ToList();
                    break;
                default:
                    DGWrites.ItemsSource = null;
                    DGWrites.ItemsSource = App.DemoDb.Product.ToList();
                    break;
            }
        }
        private void FilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedType = FilterCB.SelectedItem;
            var type = ((Model.ProductType)selectedType).ID;
            DGWrites.ItemsSource = App.DemoDb.Product.Where(x => x.ProductTypeID == type).OrderBy(x => x.Title).Skip(pageNumber * 10).Take(10).ToList();
        }

        private void DGWrites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Windows.DeleteItemWindow del = new Windows.DeleteItemWindow();
            //if (del.ShowDialog() == true)
            {
                try
                {
                    var item = DGWrites.SelectedItem as Model.Product;
                    App.DemoDb.ProductMaterial.RemoveRange(App.DemoDb.ProductMaterial.Where(x => x.ProductID == item.ID).ToList());
                    App.DemoDb.ProductSale.RemoveRange(App.DemoDb.ProductSale.Where(x => x.ProductID == item.ID).ToList());
                    App.DemoDb.ProductCostHistory.RemoveRange(App.DemoDb.ProductCostHistory.Where(x => x.ProductID == item.ID).ToList());
                    App.DemoDb.Product.Remove(item);
                    App.DemoDb.SaveChanges();
                    RefreshButtons();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex}");
                }
            }
        }

        private void AddAgentBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddAgentPage());
        }
    }
}
