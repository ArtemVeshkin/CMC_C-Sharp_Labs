using ClassLibrary;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;

namespace Lab
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private V3MainCollection Collection { get; set; } = new V3MainCollection();

        private DataItemModel DataItemModel { get; set; }

        public static RoutedCommand AddCommand = new RoutedCommand("Add", typeof(Lab.MainWindow));

        public MainWindow()
        {
            InitializeComponent();
            InitDataContext();
        }

        private void PropertyChangedHandler(object source, PropertyChangedEventArgs args)
        {
            SetSavedTextBlock();
        }

        private void InitDataContext()
        {
            DataContext = Collection;
            Collection.PropertyChanged += PropertyChangedHandler;
            SetSavedTextBlock();
        }

        private void SetSavedTextBlock()
        {
            if (Collection.ChangedAfterSaving)
            {
                SavedTextBlock.Text = "Есть несохраненные изменения";
                SavedTextBlock.Foreground = Brushes.Red;
            }
            else
            {
                SavedTextBlock.Text = "Все изменения сохранены";
                SavedTextBlock.Foreground = Brushes.Green;
            }
        }

        private void MenuItem_New_Click(object sender, RoutedEventArgs e)
        {
            if (Check_Changes())
            {
                Serialize();
            }

            Collection = new V3MainCollection();
            InitDataContext();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (Check_Changes())
            {
                Serialize();
            }
        }

        private void Serialize()
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = "Serialization data|*.dat|All|*.*",
                FilterIndex = 2
            };

            if (dlg.ShowDialog() == true)
            {
                Collection.Save(dlg.FileName);
            }
        }

        private bool Check_Changes()
        {
            if (Collection.ChangedAfterSaving)
            {
                if (MessageBox.Show("Сохранить изменения?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    return true;
                }
            }
            return false;
        }

        private void MenuItem_Add_Defaults_Click(object sender, RoutedEventArgs e)
        {
            Collection.AddDefaults();
        }

        private void MenuItem_Add_Defaults_V3DataCollection_Click(object sender, RoutedEventArgs e)
        {
            V3DataCollection data = new V3DataCollection("default", new DateTime());
            data.InitRandom(5, 2, 2, 0, 1);
            Collection.Add(data);
        }

        private void MenuItem_Add_Defaults_V3DataOnGrid_Click(object sender, RoutedEventArgs e)
        {
            V3DataOnGrid data = new V3DataOnGrid("Default", new DateTime(), new Grid1D(1, 2), new Grid1D(1, 2));
            data.InitRandom(0, 1);
            Collection.Add(data);
        }

        private void MenuItem_Add_Element_from_File_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Serialization data|*.dat|All|*.*",
                FilterIndex = 2
            };

            try
            {
                if (dlg.ShowDialog() == true)
                {
                    Collection.Add(new V3DataOnGrid(dlg.FileName));
                }
                else
                {
                    MessageBox.Show("Файл не выбран");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при чтении из файла");
            }
        }

        private void Filter_DataCollection(object sender, FilterEventArgs args)
        {
            args.Accepted = args.Item is V3DataCollection;
        }

        private void Filter_DataOnGrid(object sender, FilterEventArgs args)
        {
            args.Accepted = args.Item is V3DataOnGrid;
        }

        private void Init_DataItemModel()
        {
            if (listBox_DataCollection.SelectedItem as V3DataCollection != null)
            {
                DataItemModel = new DataItemModel(listBox_DataCollection.SelectedItem as V3DataCollection);
                DataItem_X.DataContext = DataItem_Y.DataContext = DataItem_Value.DataContext = DataItemModel;
            }
        }

        private void listBox_DataCollection_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Init_DataItemModel();
        }

        private void OpenCommandHandler_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            try
            {
                if (Check_Changes())
                {
                    Serialize();
                }

                OpenFileDialog dlg = new OpenFileDialog
                {
                    Filter = "Serialization data|*.dat|All|*.*",
                    FilterIndex = 2
                };

                if (dlg.ShowDialog() == true)
                {
                    Collection = new V3MainCollection();
                    Collection.Load(dlg.FileName);
                    InitDataContext();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType()}.MenuItem_Open_Click() raised exception:\n{ex.Message}");
            }
        }

        private void SaveCommandHandler_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Collection.ChangedAfterSaving;
        }

        private void SaveCommandHandler_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            Serialize();
        }

        private void DeleteCommandHandler_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {

            e.CanExecute = listBox_Main != null && listBox_Main.SelectedIndex >= 0;
        }

        private void DeleteCommandHandler_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            Collection.RemoveAt(listBox_Main.SelectedIndex);
        }

        private void AddCommandHandler_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {

            if (DataItemModel != null)
            {
                if (Validation.GetHasError(DataItem_X) == false && Validation.GetHasError(DataItem_Y) == false && Validation.GetHasError(DataItem_Value) == false)
                {
                    e.CanExecute = true;
                    return;
                }
            }
            e.CanExecute = false;
        }

        private void AddCommandHandler_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            DataItemModel.Add();
            Init_DataItemModel();
        }
    }
}
