using Lopuh.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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


namespace Lopuh.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddAgentPage.xaml
    /// </summary>
    public partial class AddAgentPage : Page
    {
        public AddAgentPage()
        {
            InitializeComponent();
            TypeProduct.ItemsSource = App.DemoDb.ProductType.ToList();
            TypeProduct.DisplayMemberPath = "Title";
        }

        private void AddAgentBtn_Click(object sender, RoutedEventArgs e)
        {
            Model.Product prod = new Model.Product();
            {
                prod.Title = TitleTxt.Text;
                var idType = TypeProduct.SelectedItem;
                prod.ProductTypeID = ((Model.ProductType)idType).ID;
                prod.ArticleNumber = ArticleNum.Text;
                prod.ProductionPersonCount = Convert.ToInt32(CountHumProd.Text);
                prod.ProductionWorkshopNumber = Convert.ToInt32(NumbProd.Text);
                prod.MinCostForAgent = Convert.ToInt32(MinCostFA.Text);
                prod.Image = openFileDialog.FileName;
            }
            App.DemoDb.Product.Add(prod);
            App.DemoDb.SaveChanges();
            NavigationService.Navigate(new ListProdPage());
        }
        OpenFileDialog openFileDialog = new OpenFileDialog();
        private void LogoBtn_Click(object sender, RoutedEventArgs e)
        {

            openFileDialog.Filter = "Image files|*.bmp;*.jpg;*.png|All files|*.*";
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == true)
            {
                File.ReadAllBytes(openFileDialog.FileName);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(openFileDialog.FileName);
                image.EndInit();
                LogoFrame.Source = image;
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("", "", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            NavigationService.Navigate(new ListProdPage());
        }
    }
}
