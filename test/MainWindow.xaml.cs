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
using System.Windows.Media.Effects;
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

        private void HideImage()
        {
            ImageBorder.Visibility = Visibility.Hidden;
        }

        private void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && ImageBorder.Visibility == Visibility.Visible)
            {
                HideImage();
                return;
            }

            if (e.Key == Key.B && ImageBorder.Visibility == Visibility.Visible)
            {
                RemoveBlur();
                return;
            }

            if ((e.Key == Key.Up  || e.Key == Key.Right) && ImageBorder.Visibility == Visibility.Visible)
            {
                UpClick(this, null);
                return;
            }


            if ((e.Key == Key.Down || e.Key == Key.Left) && ImageBorder.Visibility == Visibility.Visible)
            {
                DownClick(this, null);
                return;
            }


        }

        private void ApplyBlur()
        {
            BlurBitmapEffect myBlurEffect = new BlurBitmapEffect();
            myBlurEffect.Radius = 10;
            myBlurEffect.KernelType = KernelType.Box;
            Image.BitmapEffect = myBlurEffect;
        }

        private void RemoveBlur()
        {
            Image.BitmapEffect = null;
        }



        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.B && ImageBorder.Visibility == Visibility.Visible)
            {
                ApplyBlur();
            }
        }


        private void DownClick(object sender, RoutedEventArgs e)
        {
            var currImageSource = Image.Source;
            var images = FindVisualChildren<Image>(ItemsControl).ToList();
            var currImage = images.First(image => image.Source == currImageSource);
            var currImageIndex = images.ToList().IndexOf(currImage);

            if (currImageIndex >= images.Count - 1)
            {
                Image.Source = images[0].Source;
            }

            else
            {
                    Image.Source = images[++currImageIndex].Source;
            }
        }

        private void UpClick(object sender, RoutedEventArgs e)
        {
            var currImageSource = Image.Source;
            var images = FindVisualChildren<Image>(ItemsControl).ToList();
            var currImage = images.First(image => image.Source == currImageSource);
            var currImageIndex = images.ToList().IndexOf(currImage);

            if (currImageIndex < 1)
            {
                Image.Source = images[images.Count-1].Source;
            }

            else
            {
                Image.Source = images[--currImageIndex].Source;
            }

        }


        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }


    }
}
