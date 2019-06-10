using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MyItems.ViewModels
{
    public class TaskViewModel
    {
        private ObservableCollection<Task> _taskCollection; // private List<Task> _task;
        private IOrderedEnumerable<Task> _sortedList;
        public ICommand ListCommand { private set; get; }
        private List<Task> generalList;

        public TaskViewModel(Task currentTask)
        {
            // TO DO
            generalList = new List<Task>();
            //generalList = App.ItemController.GetTasks();
            //myList = new ObservableCollection<Task>(generalList);
            _sortedList = from cTask in generalList orderby cTask.Date, cTask.Date select cTask;
        }

        private bool CheckDuplicates(string possibleText, int taskType)
        {
            try
            {
                foreach (var item in _taskCollection.Where(x => x.Type == taskType))
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
