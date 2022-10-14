using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InnovitaBarcodes
{

    /**
     * 
     */
    public class ArticoloBarcode
    {

        /** */
        public string ItemCode { get; set; }

        /** */
        public string DistNumber { get; set; }

        /** */
        public double Quantity { get; set; }


        /**
         * 
         *   @Paaa  => aaa è il codice articolo
         *   @Qnnn => nnn è la quantità separata da .
         *   @1Taaa => aaa è il lotto
         *   @Uaaa  => aaa è il codice ubicazione
         *   
         */
        public override string ToString()
        {
            var ret = "";

            if (ItemCode != "") ret += $"@P{ItemCode}";
            if (DistNumber != "") ret += $"@1T{DistNumber}";
            if (Quantity != 0) ret += $"@Q{Quantity.ToString("#", System.Globalization.CultureInfo.InvariantCulture)}";


            return ret;
        }


    }

}
