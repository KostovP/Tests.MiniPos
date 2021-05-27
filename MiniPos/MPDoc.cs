using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniPos
{
    public class MPDoc
    {                
        private DateTime dt = DateTime.Now;
        private string contractor = "";

        private readonly List<MPLine> lines = new();

        public string DT
        {
            get { return dt.ToString(MPHelper.FMT_DOC_DATE); }

            set 
            {
                if (!DateTime.TryParse(value, out dt))
                {
                    dt = DateTime.Now;
                }                
            }
        }

        public string Contractor 
        {
            get { return string.IsNullOrEmpty(contractor.Trim()) ? MPHelper.DEF_CLIENT_NAME : contractor; }
            set { contractor = value; }
        }

        public decimal Total 
        { 
            get { return decimal.Round(Lines.Sum(line => line.Total), MPHelper.PREC_TOTAL_DOC); } 
        }

        public decimal TotalDiscount 
        { 
            get { return Total - TotalTarget; } 
        }

        public decimal TotalTarget 
        { 
            get { return decimal.Round(Lines.Sum(line => line.TotalTarget), MPHelper.PREC_TOTAL_DOC); } 
        }

        public List<MPLine> Lines => lines;
       
        public MPLine LineAdd()
        {
            var line = new MPLine();
            lines.Add(line);
            return line;
        }
    }
}
