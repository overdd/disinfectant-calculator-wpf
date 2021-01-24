using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication1
{
    public partial class AddPositionWindow : Window
    {
        public AddPositionWindow()
        {
            InitializeComponent();
        }

        private void AddPositionButton_OnClick(object sender, RoutedEventArgs e)
        {
            string objectNameDez = ObjectNameDez.Text;
            string objectCountDez = ObjectCount.Text;
            string objectArea = ObjectArea.Text;
            string processType = ProcessType.Text;
            string processesPerMonth = ProcessesPerMonth.Text;
            string dezinfectionThing = DezinfectionThing.Text;
            string workConcentration = WorkConcentration.Text;
            string rashodRastvora = RashodRastvora.Text;
            string kolichestvoRashodaOdnokrat = KolichestvoRashodaOdnokrat.Text;
            string dezType = DezType.Text;

            try
            {
                ValidationMethods.ValidatePositionForm(
                    objectNameDez,
                    objectCountDez,
                    objectArea,
                    processType,
                    processesPerMonth,
                    dezinfectionThing,
                    workConcentration,
                    rashodRastvora,
                    kolichestvoRashodaOdnokrat,
                    dezType
                );
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            
            var record = new Record
            {
                TableIndex = AppState.CurrentRecordIndex,
                Id = -1,
                Meta = AppState.CurrentReport,
                ObjectNameDez = objectNameDez,
                ObjectCountDez = objectCountDez,
                ObjectArea = objectArea,
                ProcessType = processType,
                ProcessesPerMonth = Int32.Parse(processesPerMonth),
                DezinfectionThing = dezinfectionThing,
                WorkConcentration = Single.Parse(workConcentration),
                RashodRastvora = rashodRastvora,
                KolichestvoRashodaOdnokrat = Single.TryParse(kolichestvoRashodaOdnokrat, out var rest) ? rest : -1,
                DezType = dezType
            };
            
            record.CalcValues();
            AppState.AddRecordRow(record);
            AppState.CurrentRecordIndex += 1;
            AppState.UpdateResultRecords();
        }
        
        public void RemoveText(object sender, EventArgs e)
        {
            if ((sender as TextBox).Text == "Текст...") 
            {
                (sender as TextBox).Text = "";
            }
        }

        public void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace((sender as TextBox).Text))
                (sender as TextBox).Text = "Текст...";
        }
    }
}