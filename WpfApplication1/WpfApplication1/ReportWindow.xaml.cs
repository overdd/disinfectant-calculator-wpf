using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication1
{
    public partial class ReportWindow : Window
    {
        public ReportWindow()
        {
            InitializeComponent();
            AddNewPosition.IsEnabled = AppState.CurrentReport != null;
            SaveReport.IsEnabled = AppState.CurrentReport != null;
        }

        private void CreateNewReport_OnClick(object sender, RoutedEventArgs e)
        {
            new CreateReportWindow().Show();
        }

        private void AddNewPosition_OnClick(object sender, RoutedEventArgs e)
        {
            new AddPositionWindow().Show();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            FileLogger.WriteToFile(DateTime.Now + ": Программа закрыта");
        }

        private void SaveReport_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                AppState.CurrentReport.Id = DataBase.SaveReport(AppState.CurrentReport);
                DataBase.SaveRecords(AppState.CurrentRecords, AppState.CurrentReport);
                AppState.ReportWindow.Title = $"Отчеты - {AppState.CurrentReport.Name}";
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }

        private void DeleteReport_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Удалить отчет?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            try
            {
                DataBase.DeleteReport(AppState.CurrentReport);
                AppState.CurrentReport = null;
                AppState.ReportWindow.Title = "Отчеты";
                AppState.ReportWindow.AddNewPosition.IsEnabled = false;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void OpenReport__OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                AppState.CachedReports = DataBase.GetReports();
                new OpenReport().Show();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }

        private void ReportGrid_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var editingTb = (e.EditingElement as TextBox).Text;
            var cIndex = e.Column.DisplayIndex;
            var recordToUpdate = (e.Row.Item as Record);
            try
            {
                ValidationMethods.ValidateCellEdit(cIndex, editingTb, recordToUpdate);
                recordToUpdate.CalcValues();
                AppState.UpdateCells();
                AppState.UpdateResultRecords();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка редактирования", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                e.Cancel = true;
            }

        }

        private void HelpView_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Текст справки", "Справка", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void resultGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}