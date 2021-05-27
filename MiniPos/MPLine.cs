using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniPos
{
    public class MPLine
    {
        private int qty = 1;
        private decimal prc = 0m;
        private decimal discount = 0m;

        public string Name { get; set; } = "";

        public string Qty 
        {
            get { return qty.ToString(MPHelper.FMT_QTY); }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                if (!int.TryParse(value, out qty))
                {
                    throw new ArgumentException("Invalid qty!");
                }
            }
        }

        public string Prc 
        { 
            get { return prc.ToString(MPHelper.FMT_PRC); } 
            
            set 
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                if (!decimal.TryParse(value, out prc))
                {
                    throw new ArgumentException("Invalid price!");
                }
            } 
        }

        public string Discount 
        {
            get { return discount.ToString(MPHelper.FMT_DISCOUNT); }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    discount = 0;
                    return;
                }


                if (!decimal.TryParse(value, out discount))
                {
                    throw new ArgumentException("Invalid discount value!");
                }

                if ((discount < 0) || (discount > 100))
                {
                    discount = 0;
                    throw new ArgumentException("Invalid discount value!");
                }
            }
        }

        [JsonIgnore]
        public decimal Total { get { return qty * prc; } } // no need to round, because the qty is an int

        [JsonIgnore]
        public decimal TotalDiscount { get { return Total - TotalTarget; } }

        [JsonPropertyName("Total")]
        public decimal TotalTarget { get { return decimal.Round(Total * (1 - (discount / 100)), MPHelper.PREC_TOTAL_DOC_LINE); } }

    }
}
