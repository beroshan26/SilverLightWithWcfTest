using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SilverLightWithWcfTest.StockServiceRef;

namespace SilverLightWithWcfTest
{
    public class StockServiceViewModel: INotifyPropertyChanged
    {
        private ICommand _getStockPricesCommand;
        readonly StockServiceClient _client = new StockServiceClient();

        private ObservableCollection<StockPriceContract> _stockPricesColl;

        public ObservableCollection<StockPriceContract> StockPricesCollection
        {
            get { return _stockPricesColl; }
            set
            {
                _stockPricesColl = value;
                NotifyPropertyChanged("StockPrices");
            }
        }

        private StockPriceContract _stockPrice;
        public StockPriceContract StockPrice
        {
            get { return _stockPrice; }
            set
            {
                _stockPrice = value;
                NotifyPropertyChanged("StockPrice");
            }
        }


        public StockServiceViewModel()
        {
            StockPricesCollection = new ObservableCollection<StockPriceContract>();
        }
        public ICommand GetStockPricesCommand
        {
            get
            {
                if (_getStockPricesCommand == null)
                {
                    _getStockPricesCommand = new RelayCommand(a =>
                    {
                        _client.GetStockPricesCompleted += client_GetStockPricesCompleted; 
                        _client.GetStockPricesAsync();
                    });
                }
                return _getStockPricesCommand;
            }
            set { _getStockPricesCommand = value; }
        }

        void client_GetStockPricesCompleted(object sender, GetStockPricesCompletedEventArgs e)
        {
            e.Result.ToList().ForEach(a=>
                _stockPricesColl.Add(a));

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
