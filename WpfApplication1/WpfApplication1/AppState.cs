using System.Collections.Generic;
using System.Linq;

namespace WpfApplication1
{
    public static class AppState
    {
        public static int CurrentRecordIndex { get; set; } = 1;
        public static List<Record> CurrentRecords { get; set; }
        public static RecordMeta CurrentReport { get; set; }
        public static bool ReportSaved { get; set; } = true;
        public static User CurrentUser { get; set; }
        
        public static List<RecordMeta> CachedReports { get; set; } = new List<RecordMeta>();
        public static ReportWindow ReportWindow { get; set; }

        public static void InitEmptyRecords()
        {
            CurrentRecords = new List<Record>();
            // ReportWindow.reportGrid.ItemsSource = CurrentRecords;
        }

        public static void AddRecordRow(Record record)
        {
            CurrentRecords.Add(record);
            UpdateCells();
        }

        public static void UpdateCells()
        {
            ReportWindow.reportGrid.ItemsSource = new List<Record>(CurrentRecords);
        }

        public static void CalcAllValues()
        {
            foreach (var currentRecord in CurrentRecords)
            {
                currentRecord.CalcValues();
            }
        }

        public static void UpdateAllIndexes()
        {
            CurrentRecordIndex = 1;
            foreach (var currentRecord in CurrentRecords)
            {
                currentRecord.TableIndex = CurrentRecordIndex;
                CurrentRecordIndex += 1;
            }
        }

        public static void UpdateResultRecords()
        {
            ReportWindow.resultGrid.ItemsSource =
                CurrentRecords
                    .GroupBy(r => r.DezinfectionThing)
                    .Select(r =>
                        new {
                            DezinfectionThing = r.Key,
                            OdinMonth = r.Sum(x => x.OdinMonth > 0 ? x.OdinMonth : 0),
                            OdinKvartal = r.Sum(x => x.OdinKvartal > 0 ? x.OdinKvartal : 0),
                            OdinYear = r.Sum(x => x.OdinYear > 0 ? x.OdinYear : 0)
                        });
        } 
    }
}