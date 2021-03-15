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

namespace Lab_1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private V3MainCollection Collection { get; set; } = new V3MainCollection();

        private bool CollectionInitialized { get; set; } = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_New_Click(object sender, RoutedEventArgs e)
        {
            Collection = new V3MainCollection();
            CollectionInitialized = true;

            Collection.AddDefaults();
            V3DataElements.Children.Clear();
            foreach (var elem in V3MainCollection.DataCollectionCast(Collection[0]))
            {
                V3DataElements.Children.Add(new TextBlock
                {
                    Text = elem.ToString(),
                    TextWrapping = TextWrapping.Wrap,
                    Style = (Style)this.TryFindResource("MaterialDesignBody1TextBlock")
                });
            }
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
                CollectionInitialized = true;
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

            if (dlg.ShowDialog() == true && CollectionInitialized)
            {
                Collection.Save(dlg.FileName);
            }
        }

        private bool Check_Changes()
        {
            if (CollectionInitialized && Collection.ChangedAfterSaving)
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
            Collection.Add(new V3DataCollection("Default", new DateTime()));
        }

        private void MenuItem_Add_Defaults_V3DataOnGrid_Click(object sender, RoutedEventArgs e)
        {
            Collection.Add(new V3DataOnGrid("Default", new DateTime(), new Grid1D(), new Grid1D()));
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
    }
}
