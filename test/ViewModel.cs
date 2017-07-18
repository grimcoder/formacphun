using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using test.Annotations;

namespace test
{
    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ViewModel()
        {
            SelectedImages = new ObservableCollection<string>();

            CloseFullScreenCommand = new Command(() => { }, () => true);
        }

        private ICommand CloseFullScreenCommand { get; set; } 

        
        public ObservableCollection<string> SelectedImages { get; set; }
    }

    class Command: ICommand
    {

        public Command(Action execute, Func<bool> canExecute)
        {
            this.ExecuteCommand = execute;
            this.CanExecuteCommand = canExecute;
        }



        public Func<bool> CanExecuteCommand { get; set; }

        public Action ExecuteCommand { get; set; }

        public bool CanExecute(object parameter)
        {
            return CanExecuteCommand();
        }

        public void Execute(object parameter)
        {
            ExecuteCommand();
        }

        public event EventHandler CanExecuteChanged;
    }
}
