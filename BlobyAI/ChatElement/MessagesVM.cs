using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BlobyAI.ChatElement
{
    class MessagesVM :  INotifyPropertyChanged
    {
        private string _contextOfText="لطفا صبر نمایید...";

        public string ContextOfText
        {
            get
            {
               return _contextOfText;
            }
            set
            {
                _contextOfText = value;
                OnPropertyChanged(nameof(ContextOfText));
            }
        }

           

        public event PropertyChangedEventHandler? PropertyChanged; protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
