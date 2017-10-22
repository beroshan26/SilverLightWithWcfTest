using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StockServiceLibrary
{
    [DataContract]
    public class StockPriceContract
    {
        [DataMember]
        //Symbol of the Stock
        public string Symbol { get; set; }

        [DataMember]
        //Name of the Stock
        public string Name { get; set; }

        [DataMember]
        //LastTradedPrice price of the Stock
        public decimal? LastTradedPrice { get; set; }

        [DataMember]
        //ClosePrice price of the Stock
        public decimal? OpenPrice { get; set; }

        [DataMember]
        //ClosePrice price of the Stock
        public decimal? ClosePrice { get; set; }

        [DataMember]
        //Volume of the Stock
        public decimal? TradedVolume { get; set; }
    }
}
