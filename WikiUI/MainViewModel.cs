using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Wiki;

namespace WikiUI
{
    public class MainViewModel : ViewModelBase, IDisposable
    {
        private double _latitude = 28.6139; // Delhi Default
        public double Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                if (Math.Abs(_latitude - value) < 0.0001)
                    return;

                _latitude = value;
                RaisePropertyChanged("Latitude");
            }
        }

        private double _longitude = 77.209;
        public double Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                if (Math.Abs(_longitude - value) < 0.0001)
                    return;

                _longitude = value;
                RaisePropertyChanged("Longitude");
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }
        private Dictionary<string, Dictionary<string, Article>> _similarity;
        private Action<string> onerror;

        public Dictionary<string, Dictionary<string, Article>> Similarity
        {
            get
            {
                return _similarity;
            }
            set
            {
                _similarity = value;
                RaisePropertyChanged("Similarity");
            }
        }

        public ICommand SearchCommand { get; private set; }

        public MainViewModel(Action<string> onerror)
        {
            this.onerror = onerror;
            SearchCommand = new RelayCommand(Search);
        }

        private async void Search(object obj)
        {
            await SearchAsync();
        }

        private async Task SearchAsync()
        {
            await Task.Run(() =>
            {
                IsBusy = true;
                var wikiParser = new WikiParser(Latitude, Longitude);
                var response = wikiParser.Parse()
                    .OnSuccess((result) => wikiParser.GetArticlesWithImages(result.Value))
                    .OnSuccess((result) => Similarities.Find(result.Value));
                if (response.IsSuccess)
                {
                    if (response.Value.Count == 0)
                    {
                        onerror("Result empty! No similar images found!");

                    }
                    else
                        Similarity = response.Value.ToDictionary(x => x.Key, y => y.Value);
                }
                else
                {
                    onerror("Operation failed:\n" + response.Error);
                }
                IsBusy = false;

            });
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
