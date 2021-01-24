using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace WpfApplication1
{
    public partial class OpenReport : Window
    {
        public OpenReport()
        {
            InitializeComponent();
            foreach (var cachedReport in AppState.CachedReports)
            {
                reportList.Items.Add(cachedReport.Name);
            }
        }

        private void ReportList_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string reportName = reportList.SelectedValue.ToString();
            AppState.CurrentReport = AppState.CachedReports.Find(report => report.Name == reportName);
            AppState.CurrentRecords = DataBase.GetRecords(AppState.CurrentReport);
            AppState.ReportWindow.Title = $"Отчеты - {AppState.CurrentReport.Name}";
            AppState.ReportWindow.AddNewPosition.IsEnabled = true;
            AppState.CalcAllValues();
            AppState.UpdateAllIndexes();
            AppState.UpdateCells();

            AppState.UpdateResultRecords();
            
            Close();
        }
    }
}