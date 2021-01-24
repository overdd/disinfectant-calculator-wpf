using System.Windows;

namespace WpfApplication1
{
    public partial class CreateReportWindow : Window
    {
        public CreateReportWindow()
        {
            InitializeComponent();
        }

        private void CreateReportButton_OnClick(object sender, RoutedEventArgs e)
        {
            string reportName = ReportName.Text;
            
            AppState.CurrentReport = new RecordMeta();
            AppState.CurrentReport.Id = -1;
            AppState.CurrentReport.Name = reportName;

            AppState.ReportWindow.Title = $"Отчеты - {reportName}*";
            AppState.ReportWindow.AddNewPosition.IsEnabled = true;
            AppState.ReportWindow.SaveReport.IsEnabled = true;
            AppState.ReportSaved = false;
            AppState.InitEmptyRecords();
            
            Close();
            
        }
    }
}