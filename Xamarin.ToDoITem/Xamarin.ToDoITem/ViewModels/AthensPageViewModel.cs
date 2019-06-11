using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Acr.UserDialogs;
using Xamarin.Forms;

namespace MyItems.ViewModels
{
    public class AthensPageViewModel : BaseViewModel
    {
        private ObservableCollection<Task> _tasks;
        public ICommand ListCommand { private set; get; }
        private List<Task> generalList;
        private IOrderedEnumerable<Task> _sortedList;

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

        protected async void AddNewItem(string newTaskText, string newTaskPrice, int newTaskType, DateTime thisDate)
        {
            try
            {
                var task = new Task
                {
                    Text = newTaskText,
                    Type = newTaskType,
                    Price = newTaskPrice
                };
                generalList.Add(task);
                await App.ItemController.InsertTask(task);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        protected async void DeleteItem(Task currentTask)
        {
            try
            {
                generalList.Remove(currentTask);
                await App.ItemController.DeleteTask(currentTask);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        protected async void UpdateItem(Task currentTask)
        {
            try
            {
                await App.ItemController.UpdateTask(currentTask);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
