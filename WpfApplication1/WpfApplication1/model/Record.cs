using System;

namespace WpfApplication1
{
    public class Record
    {
        public int TableIndex { get; set; }
        public int Id { get; set; }
        public RecordMeta Meta { get; set; }
        public string ObjectNameDez { get; set; }
        public string ObjectCountDez { get; set; }
        public string ObjectArea { get; set; }
        public string ProcessType { get; set; }
        public int ProcessesPerMonth { get; set; }
        public string DezinfectionThing { get; set; }
        public float WorkConcentration { get; set; }
        public string RashodRastvora { get; set; }
        
        public string DezType { get; set; }
        public float KolichestvoRashodaOdnokrat { get; set; }
        public float OdnaObrabotka { get; set; }
        public float OdinMonth { get; set; }
        public float OdinKvartal { get; set; }
        public float OdinYear { get; set; }

        
        
        public void CalcValues()
        {
            if (!string.IsNullOrWhiteSpace(RashodRastvora) &&
                Single.TryParse(RashodRastvora, out float parsedRashod) &&
                Single.TryParse(ObjectArea, out float parsedObjectArea)
                )
            {
                KolichestvoRashodaOdnokrat = parsedRashod * parsedObjectArea;
            }
            else
            {
                KolichestvoRashodaOdnokrat = -1; 
            }

            if (KolichestvoRashodaOdnokrat >= 0)
            {
                if (DezType == "Т")
                {
                    OdnaObrabotka = KolichestvoRashodaOdnokrat * WorkConcentration / 10;
                }
                else if (DezType == "Ж")
                {
                    OdnaObrabotka = KolichestvoRashodaOdnokrat * WorkConcentration / 100;
                }
                else
                {
                    OdnaObrabotka = -1;
                }
            }
            else
            {
                OdnaObrabotka = -1;
            }

            if (OdnaObrabotka >= 0)
            {
                OdinMonth = OdnaObrabotka * ProcessesPerMonth;
            }
            else
            {
                OdinMonth = -1;
            }

            if (OdinMonth > 0)
            {
                OdinKvartal = OdinMonth * 3;
            }
            else
            {
                OdinKvartal = -1;
            }

            if (OdinKvartal > 0)
            {
                OdinYear = OdinKvartal * 4;
            }
            else
            {
                OdinYear = -1;
            }
        }
    }
}