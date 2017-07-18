using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


namespace test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModel();
            DContext = this.DataContext as ViewModel;
            
        }

        private ViewModel DContext;

        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };

        private static bool isImage(string img)
        {
                if (ImageExtensions.Contains(Path.GetExtension(img).ToUpperInvariant()))
                {
                    return true;
                }

            return false;

        }
        private void UIElement_OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {

                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                files.ToList().ForEach((file) =>
                {
                    if (isImage(file))
                    {
                        AddImageToList(file);
                    }


                });
            }
        }


        private void AddImageToList(string file)
        {
            if (!DContext.SelectedImages.Contains(file))
            {

                DContext.SelectedImages.Add(file);

            }
        }


        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = e.Source as Image;
            if (e.ClickCount == 2 && image != null)
            {
                ShowImage(image.Source);
            }
        }


        private void ShowImage(ImageSource imageSource)
        {
            Image.Source = imageSource;
            ImageBorder.Visibility = Visibility.Visible;
        }

        private void HideImage(ImageSource imageSource)
        {
            
            ImageBorder.Visibility = Visibility.Hidden;
        }

    }
}
