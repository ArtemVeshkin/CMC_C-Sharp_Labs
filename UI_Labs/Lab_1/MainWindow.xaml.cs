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
using ClassLibrary;
using Microsoft.Win32;
using System.ComponentModel;

namespace Lab_1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private V3MainCollection Collection { get; set; } = new V3MainCollection();

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

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            if (Check_Changes())
            {
                Serialize();
            }

            OpenFileDialog dlg = new OpenFileDialog {
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

        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            Serialize();
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
            SaveFileDialog dlg = new SaveFileDialog {
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

        private void MenuItem_Remove_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_Main.SelectedItem is V3Data selected)
            {
                Collection.Remove(selected.Info, selected.Time);
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
    }
}
