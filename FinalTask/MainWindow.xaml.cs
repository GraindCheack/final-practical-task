using System;
using System.IO;
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
using Microsoft.Win32;
using System.Text.Json;
using System.Collections.ObjectModel;

namespace FinalTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = Class.ApplicationViewModel.appViewModel;
        }

        private void topBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ListViewItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Class.ApplicationViewModel.appViewModel.ActiveBusFile == null)
            {
                Class.ApplicationViewModel.appViewModel.ActiveBusFile = new Class.BusFile(Class.ApplicationViewModel.appViewModel.BusFiles.Count + 1);
                Class.ApplicationViewModel.appViewModel.BusFiles.Add(Class.ApplicationViewModel.appViewModel.ActiveBusFile);
            }
            OpenFileDialog openDialog = new OpenFileDialog()
            {
                FileName = Class.ApplicationViewModel.appViewModel.ActiveBusFile.FileName,
                DefaultExt = ".json",
                AddExtension = true,
                Filter = "Сериализованные объекты класса (.json)|*.json"
            };
            openDialog.ShowDialog();
            if (openDialog.FileName != "")
            {
                using (StreamReader file = new StreamReader(openDialog.FileName))
                {
                    try
                    {
                        Class.BusFile tempFile = JsonSerializer.Deserialize<Class.BusFile>(file.ReadLine());
                        Class.ApplicationViewModel.appViewModel.ActiveBusFile.BusWays = tempFile.BusWays;
                        Class.ApplicationViewModel.appViewModel.ActiveBusFile.FileName = tempFile.FileName;
                        Class.ApplicationViewModel.appViewModel.ActiveBusFile.PathToFile = tempFile.PathToFile;
                        Class.ApplicationViewModel.appViewModel.ActiveBusFile.Amount = tempFile.BusWays.Count;
                    }
                    catch (System.Text.Json.JsonException)
                    {
                        MessageBox.Show("Файл некорректный");
                    }
                }
            }
            else
            {
                Class.ApplicationViewModel.appViewModel.BusFiles.Remove(Class.ApplicationViewModel.appViewModel.ActiveBusFile);
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonUp_ClearAll(object sender, MouseButtonEventArgs e)
        {
            Class.ApplicationViewModel.appViewModel.BusFiles.Clear();
        }

        private void ListViewItem_PreviewMouseLeftButtonUp_AddEmpty(object sender, MouseButtonEventArgs e)
        {
            Class.ApplicationViewModel.appViewModel.BusFiles.Add(new Class.BusFile(
                Class.ApplicationViewModel.appViewModel.BusFiles.Count + 1));
        }

        private void PackIcon_PreviewMouseLeftButtonUp_RemoveFile(object sender, MouseButtonEventArgs e)
        {
            Class.ApplicationViewModel.appViewModel.BusFiles.Remove(Class.ApplicationViewModel.appViewModel.ActiveBusFile);
            for (int i = 0; i < Class.ApplicationViewModel.appViewModel.BusFiles.Count; i++)
            {
                Class.ApplicationViewModel.appViewModel.BusFiles[i].Number = i + 1;
            }
        }

        private void saving(string pathToFile)
        {
            string json = JsonSerializer.Serialize<Class.BusFile>(Class.ApplicationViewModel.appViewModel.ActiveBusFile);
            using (StreamWriter file = new StreamWriter(pathToFile, false))
            {
                file.Write(json);
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonUp_Save(object sender, MouseButtonEventArgs e)
        {
            if (Class.ApplicationViewModel.appViewModel.ActiveBusFile != null)
            {
                if (Class.ApplicationViewModel.appViewModel.ActiveBusFile.PathToFile != null)
                {
                    saving(Class.ApplicationViewModel.appViewModel.ActiveBusFile.PathToFile);
                }
                else
                {
                    ListViewItem_PreviewMouseLeftButtonUp_SaveAs(sender, e);
                }
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonUp_SaveAs(object sender, MouseButtonEventArgs e)
        {
            if (Class.ApplicationViewModel.appViewModel.ActiveBusFile != null)
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    FileName = Class.ApplicationViewModel.appViewModel.ActiveBusFile.FileName,
                    DefaultExt = ".json",
                    AddExtension = true,
                    Filter = "Сериализованные объекты класса (.json)|*.json"
                };
                saveDialog.ShowDialog();
                Class.ApplicationViewModel.appViewModel.ActiveBusFile.PathToFile = saveDialog.FileName;
                Class.ApplicationViewModel.appViewModel.ActiveBusFile.FileName = System.IO.Path.GetFileName(saveDialog.FileName);
                saving(saveDialog.FileName);
            }
        }

        private void OpenFileCont_Click(object sender, RoutedEventArgs e)
        {
            if (Class.ApplicationViewModel.appViewModel.BusFiles.Count == 0)
            {
                Class.ApplicationViewModel.appViewModel.ActiveBusFile = new Class.BusFile(1);
                Class.ApplicationViewModel.appViewModel.BusFiles.Add(Class.ApplicationViewModel.appViewModel.ActiveBusFile);
            }
            else if (Class.ApplicationViewModel.appViewModel.ActiveBusFile == null)
            {
                Class.ApplicationViewModel.appViewModel.ActiveBusFile = Class.ApplicationViewModel.appViewModel.BusFiles[0];
            }
            Class.BusFile.activeBusFile = Class.ApplicationViewModel.appViewModel.ActiveBusFile;
            DataContext = Class.BusFile.activeBusFile;
            waysList.Height = (Class.BusFile.activeBusFile.Amount + 1) * 560;
            OpenFileCont.Visibility = Visibility.Collapsed;
            CloseFileCont.Visibility = Visibility.Visible;
            addWay.Visibility = Visibility.Visible;
            removeWay.Visibility = Visibility.Visible;
        }

        private void CloseFileCont_Click(object sender, RoutedEventArgs e)
        {
            Class.BusFile.activeBusFile.ActiveWay = null;
            DataContext = Class.ApplicationViewModel.appViewModel;
            OpenFileCont.Visibility = Visibility.Visible;
            CloseFileCont.Visibility = Visibility.Collapsed;
            grid5.Visibility = Visibility.Collapsed;
        }

        private void addWay_Click(object sender, RoutedEventArgs e)
        {
            if (Class.BusFile.activeBusFile != null)
            {
                Class.BusFile.activeBusFile.Amount = Class.BusFile.activeBusFile.BusWays.Count + 1;
                Class.BusFile.activeBusFile.ActiveWay = new Class.BusWay();
                Class.BusFile.activeBusFile.BusWays.Add(Class.BusFile.activeBusFile.ActiveWay);
                waysList.Height += 560;
            }
        }

        private void removeWay_Click(object sender, RoutedEventArgs e)
        {
            if (Class.BusFile.activeBusFile.ActiveWay != null)
            {
                Class.BusFile.activeBusFile.BusWays.Remove(Class.BusFile.activeBusFile.ActiveWay);
                Class.BusFile.activeBusFile.Amount--;
            }
            else if (Class.BusFile.activeBusFile.BusWays.Count != 0)
            {
                Class.BusFile.activeBusFile.BusWays.RemoveAt(Class.BusFile.activeBusFile.BusWays.Count - 1);
                Class.BusFile.activeBusFile.Amount--;
            }
            if (Class.BusFile.activeBusFile.BusWays.Count != 0)
            {
                Class.BusFile.activeBusFile.ActiveWay = Class.BusFile.activeBusFile.BusWays[0];
                waysList.Height -= 560;
            }
        }

        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (Class.BusFile.activeBusFile.ActiveWay == null)
            {
                e.Handled = true;
            }
        }

        private void Picker_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Class.BusFile.activeBusFile.ActiveWay == null)
            {
                e.Handled = true;
            }
            
        }

        private void DatePicker_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Class.BusFile.activeBusFile.ActiveWay == null)
            {
                (sender as DatePicker).Text = "";
            }       
        }

        private void TimePicker_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Class.BusFile.activeBusFile.ActiveWay == null)
            {
                (sender as MaterialDesignThemes.Wpf.TimePicker).Text = "";
            }

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Class.BusFile.activeBusFile.ActiveWay != null)
            {
                DateDeparture.SelectedDate = Class.BusFile.activeBusFile.ActiveWay.DateDepartureSpec;
                DateArrival.SelectedDate = Class.BusFile.activeBusFile.ActiveWay.DateArrivalSpec;
            }
            
        }

        private void Number_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Class.BusFile.activeBusFile.ActiveWay != null)
            {
                switch ((sender as TextBox).Name)
                {
                    case "Number": Class.BusFile.activeBusFile.ActiveWay.Number = (sender as TextBox).Text; break;
                    case "Type": Class.BusFile.activeBusFile.ActiveWay.Type = (sender as TextBox).Text; break;
                    case "WayStart": Class.BusFile.activeBusFile.ActiveWay.WayStart = (sender as TextBox).Text; break;
                    case "WayStop": Class.BusFile.activeBusFile.ActiveWay.WayStop = (sender as TextBox).Text; break;
                }
            }
            
        }

        private void scrollUp(object sender, RoutedEventArgs e)
        {
            if (Class.BusFile.activeBusFile.ScrollPage > 1)
            {
                scrollWays.ScrollToVerticalOffset(scrollWays.VerticalOffset - 560.1);
                Class.BusFile.activeBusFile.ScrollPage--;
            }
        }

        private void scrollDown(object sender, RoutedEventArgs e)
        {
            if (Class.BusFile.activeBusFile.ScrollPage <= Class.BusFile.activeBusFile.BusWays.Count / 3)
            {
                scrollWays.ScrollToVerticalOffset(scrollWays.VerticalOffset + 560.1);
                Class.BusFile.activeBusFile.ScrollPage++;
            }
        }

        private void butWayClose_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Class.BusFile.activeBusFile.BusWays.Remove(Class.BusFile.activeBusFile.ActiveWay);
            Class.BusFile.activeBusFile.Amount--;
            waysList.Height -= 560;
        }

        private void scrollWays_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                scrollUp(sender, e);
            }
            else
            {
                scrollDown(sender, e);
            }
        }

        private void ListBox_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenFileCont_Click(sender, e);
        }

        private void LimitInputDate_SelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Class.BusFile.activeBusFile.ActiveWay != null && Class.BusFile.activeBusFile.ActiveWay.DateArrivalSpec < Class.BusFile.activeBusFile.ActiveWay.DateDepartureSpec)
            {
                (sender as DatePicker).SelectedDate = Class.BusFile.activeBusFile.ActiveWay.DateDepartureSpec;
            }
        }

        private void LimitInputDate_SelectedChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            if (Class.BusFile.activeBusFile.ActiveWay != null && Class.BusFile.activeBusFile.ActiveWay.DateArrivalSpec < Class.BusFile.activeBusFile.ActiveWay.DateDepartureSpec)
            {
                (sender as MaterialDesignThemes.Wpf.TimePicker).Text = Class.BusFile.activeBusFile.ActiveWay.DateDepartureSpec.ToString("hh:mm tt");
            }
        }

        private void listViewItem6_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Class.ApplicationViewModel.appViewModel.ActiveBusFile != null)
            {
                grid4.Visibility = Visibility.Visible;
            }
        }

        private void selectByFind(object sender, RoutedEventArgs e)
        {
            OpenFileCont_Click(sender, e);
            Class.BusFile.activeBusFile = new Class.BusFile(Class.ApplicationViewModel.appViewModel.ActiveBusFile.Number);
            Class.BusFile.activeBusFile.BusWays = new ObservableCollection<Class.BusWay>();
            Class.BusFile.activeBusFile.FileName = Class.ApplicationViewModel.appViewModel.ActiveBusFile.FileName;
            addWay.Visibility = Visibility.Collapsed;
            removeWay.Visibility = Visibility.Collapsed;
            DataContext = Class.BusFile.activeBusFile;
            grid5.Visibility = Visibility.Visible;
        }

        private void selectByStartWay_Click(object sender, RoutedEventArgs e)
        {
            selectByFind(sender, e);
            grid4.Visibility = Visibility.Collapsed;
            foreach (Class.BusWay value in Class.ApplicationViewModel.appViewModel.ActiveBusFile.BusWays)
            {
                if (value.WayStart == textBoxSelectByStartWay.Text)
                {
                    Class.BusFile.activeBusFile.BusWays.Add(value);
                }
            }
        }

        private void selectByEndWay_Click(object sender, RoutedEventArgs e)
        {
            selectByFind(sender, e);
            grid4.Visibility = Visibility.Collapsed;
            foreach (Class.BusWay value in Class.ApplicationViewModel.appViewModel.ActiveBusFile.BusWays)
            {
                if (value.WayStop == textBoxSelectByEndWay.Text)
                {
                    Class.BusFile.activeBusFile.BusWays.Add(value);
                }
            }
        }

        private void buttonFindWays_Click(object sender, RoutedEventArgs e)
        {
            selectByFind(sender, e);
            grid6.Visibility = Visibility.Collapsed;
            foreach (Class.BusWay value in Class.ApplicationViewModel.appViewModel.ActiveBusFile.BusWays)
            {
                if (value.WayStop == textBoxEndWay.Text && value.DateArrivalSpec <= DateTime.Parse(
                    datePickerFind.SelectedDate.Value.ToString("dd.MM.yyyy ") + 
                    timePickerFind.SelectedTime.Value.ToString("hh:mm tt"))
                    )
                {
                    Class.BusFile.activeBusFile.BusWays.Add(value);
                }
            }
        }

        private void listViewItem4_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Class.ApplicationViewModel.appViewModel.ActiveBusFile != null)
            {
                grid6.Visibility = Visibility.Visible;
            }
        }
    }
}
