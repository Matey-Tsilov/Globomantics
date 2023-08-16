using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Globomantics.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Globomantics.Windows.ViewModels
{
    public abstract class BaseToDoViewModel<T> : ObservableObject, IToDoViewModel where T : Todo
    {
        private string? title;
        private T? model;
        private Todo? parent;
        private bool isCompleted;

        public T? Model
        {
            get => model;
            set { 
                model = value;
                OnPropertyChanged(nameof(Model));
                OnPropertyChanged(nameof(IsExisting));

            }
        }
        public string? Title
        {
            get => title;
            set
            {
                Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public Todo? Parent
        {
            get => parent;
            set
            {
                Parent = value;
                OnPropertyChanged(nameof(Parent));
                OnPropertyChanged(nameof(IsExisting));

            }
        }
        public bool IsCompleted
        {
            get => isCompleted;
            set
            {
                IsCompleted = value;
                OnPropertyChanged(nameof(IsCompleted));
                OnPropertyChanged(nameof(IsExisting));

            }
        }
        public bool IsExisting => Model is not null;


        #region From IToDoViewModel and IViewModel
        public IEnumerable<Todo>? AvailableParentTasks { get; set; }

        public ICommand DeleteCommand { get; }

        public ICommand SaveCommand { get; set; } = default!;

    public Action<string>? ShowAlert { get; set; }
        public Action<string>? ShowError { get; set; }
        public Func<IEnumerable<string>>? ShowOpenFileDialog { get; set; }
        public Func<string>? ShowSaveFileDialog { get; set ; }
        public Func<string, bool>? AskForConfirmation { get; set; }
        #endregion

        public abstract Task SaveAsync();

        public virtual void UpdateModel(Todo model)
        {
            if (model is null)
            {
                return;
            }
            var parent = AvailableParentTasks?.SingleOrDefault(t => t.Parent is not null 
            && t.Parent.Id == model.Parent.Id);
            
            Model = model as T;
            Title = model.Title;
            Parent = parent;
            IsCompleted = model.IsCompleted;

        }

        public BaseToDoViewModel()
        {
            DeleteCommand = new RelayCommand(() =>
            {
                if (Model is not null)
                {
                        Model = Model with { IsDeleted = true }; 
                }
            });
        }
        
            
    }
}
