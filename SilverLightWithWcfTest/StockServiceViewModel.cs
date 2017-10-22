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
using System.Windows.Threading;
using SilverLightWithWcfTest.StockServiceRef;
using SilverLightWithWcfTest.WebMesageSvc;

namespace SilverLightWithWcfTest
{
    public class StockServiceViewModel: INotifyPropertyChanged
    {
        private ICommand _getStockPricesCommand;

        private ICommand _getWebMessageCommand;

        private string _webMessage;

        public string WebMessage
        {
            get { return _webMessage; }
            set
            {
                _webMessage = value;
                NotifyPropertyChanged("WebMessage");
            }
        }

        readonly StockServiceClient _client = new StockServiceClient();

        WebMesageSvc.Service1SoapClient _webSvcClient = new Service1SoapClient();

        private ObservableCollection<StockPriceContract> _stockPricesColl;

        private DispatcherTimer _timer;

        public ObservableCollection<StockPriceContract> StockPricesCollection
        {
            get { return _stockPricesColl; }
            set
            {
                _stockPricesColl = value;
                NotifyPropertyChanged("StockPrices");
            }
        }

       public StockServiceViewModel()
        {
            StockPricesCollection = new ObservableCollection<StockPriceContract>();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(2);
            _timer.Tick += _timer_Tick;

            _webSvcClient.HelloWorldCompleted += _webSvcClient_HelloWorldCompleted;
            _client.GetStockPricesCompleted += client_GetStockPricesCompleted; 
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            _client.GetStockPricesAsync();
        }
        public ICommand GetStockPricesCommand
        {
            get
            {
                if (_getStockPricesCommand == null)
                {
                    _getStockPricesCommand = new RelayCommand(a => _timer.Start());
                }
                return _getStockPricesCommand;
            }
            set { _getStockPricesCommand = value; }
        }


        public ICommand GetWebMessageCommand
        {
            get
            {
                if (_getWebMessageCommand == null)
                {
                    _getWebMessageCommand = new RelayCommand(a => _webSvcClient.HelloWorldAsync());
                }
                return _getWebMessageCommand;
            }
            set { _getWebMessageCommand = value; }
        }

        void _webSvcClient_HelloWorldCompleted(object sender, HelloWorldCompletedEventArgs e)
        {
            WebMessage = e.Result;
        }

        void client_GetStockPricesCompleted(object sender, GetStockPricesCompletedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => ProcessOnUiThread(e));
        }

        private void ProcessOnUiThread(GetStockPricesCompletedEventArgs e)
        {
            e.Result.ToList().ForEach(a =>
            {
                if (_stockPricesColl.FirstOrDefault(s => s.Name == a.Name) == null)
                {
                    _stockPricesColl.Add(a);
                }
                else
                {
                    var stock = _stockPricesColl.First(s => s.Name == a.Name);
                    var index = _stockPricesColl.IndexOf(stock);
                    _stockPricesColl[index].Name = a.Name;
                    _stockPricesColl[index].OpenPrice = a.OpenPrice;
                    _stockPricesColl[index].ClosePrice = a.ClosePrice;
                    _stockPricesColl[index].TradedVolume = a.TradedVolume;
                    _stockPricesColl[index].LastTradedPrice = a.LastTradedPrice;
                    _stockPricesColl[index].TimeStamp = a.TimeStamp;
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
