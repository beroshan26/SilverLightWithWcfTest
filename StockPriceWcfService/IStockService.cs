using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using StockServiceLibrary;

namespace StockPriceWcfService
{
 
    [ServiceContract]
    public interface IStockService
    {
        [OperationContract]
        List<StockPriceContract> GetStockPrices();
    }
}
