using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MyItems.ViewModels
{
    public class TaskViewModel
    {
        private ObservableCollection<Task> _taskCollection; // private List<Task> _task;

        public TaskViewModel(Task currentTask)
        {

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
