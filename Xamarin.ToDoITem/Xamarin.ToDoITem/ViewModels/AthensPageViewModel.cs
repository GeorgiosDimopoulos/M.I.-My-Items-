using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyItems.ViewModels
{
    public class AthensPageViewModel : BaseViewModel
    {
        private ObservableCollection<Task> _tasks;
        public ICommand ListCommand { private set; get; }

        public ObservableCollection<Task> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                OnPropertyChanged(nameof(Tasks));
            }
        }

        public AthensPageViewModel()
        {
            try
            {
                ViewName = "Διαχείριση Σελίδας Για Αθηνα";
                ListCommand = new Command(GetService); //,CanGetService
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void GetService(object obj)
        {
            try
            {
                //Shops = service.GetCustomers();
                ((Command)ListCommand).ChangeCanExecute();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private bool CheckDuplicates(string possibleText, int taskType)
        {
            try
            {
                foreach (var item in _tasks.Where(x => x.Type == taskType))
                {
                    if (possibleText == item.Text)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        protected async void AddNewItem(Task thisTask, Type thisType, DateTime thisDate)
        {
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
