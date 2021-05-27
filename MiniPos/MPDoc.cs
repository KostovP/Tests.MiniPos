using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public decimal Total 
        { 
            get { return decimal.Round(Lines.Sum(line => line.Total), MPHelper.PREC_TOTAL_DOC); } 
        }

        [JsonIgnore]
        public decimal TotalDiscount 
        { 
            get { return Total - TotalTarget; } 
        }

        [JsonPropertyName("Total")]
        public decimal TotalTarget 
        { 
            get { return decimal.Round(Lines.Sum(line => line.TotalTarget), MPHelper.PREC_TOTAL_DOC); } 
        }

        [JsonIgnore]        
        public decimal TotalTargetSingleQty
        {
            get
            {
                return Lines.Where(line => line.Qty == "1").Sum(line => line.TotalTarget);
            }
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
