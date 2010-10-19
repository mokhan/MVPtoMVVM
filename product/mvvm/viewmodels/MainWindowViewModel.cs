using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MVPtoMVVM.domain;
using MVPtoMVVM.repositories;

namespace MVPtoMVVM.mvvm.viewmodels
{
    public class MainWindowViewModel : Observable<MainWindowViewModel>, Presenter
    {
        public ICollection<TodoItemViewModel> TodoItems { get; set; }
        public ICommand CancelChangesCommand { get; set; }
        public ICommand AddNewItemCommand { get; set; }

        public MainWindowViewModel(UICommandBuilder command_builder)
        {
            AddNewItemCommand = command_builder.build<AddNewItemCommand>(this);
            CancelChangesCommand = command_builder.build<RefreshChangesCommand>(this);
            TodoItems = new ObservableCollection<TodoItemViewModel>();
        }

        public void present()
        {
            CancelChangesCommand.Execute(null);
        }
    }

    public class RefreshChangesCommand : UICommand<MainWindowViewModel>
    {
        ITodoItemRepository todoItemRepository;

        public RefreshChangesCommand(ITodoItemRepository todo_item_repository)
        {
            todoItemRepository = todo_item_repository;
        }

        protected override void run(MainWindowViewModel presenter)
        {
            presenter.TodoItems.Clear();
            foreach (var item in todoItemRepository.GetAll().Select(map_from))
            {
                item.Parent = presenter;
                presenter.TodoItems.Add(item);
            }
        }

        TodoItemViewModel map_from(TodoItem item)
        {
            return new TodoItemViewModel(todoItemRepository)
                {
                    Id = item.Id,
                    Description = item.Description,
                    DueDate = item.DueDate,
                };
        }
    }

    public class AddNewItemCommand : UICommand<MainWindowViewModel>
    {
        ITodoItemRepository todoItemRepository;

        public AddNewItemCommand(ITodoItemRepository todo_item_repository)
        {
            todoItemRepository = todo_item_repository;
        }

        protected override void run(MainWindowViewModel presenter)
        {
            presenter
                .TodoItems
                .Add(new TodoItemViewModel(todoItemRepository)
                    {
                        Parent = presenter,
                        DueDate = DateTime.Today,
                        Description = string.Empty
                    });
        }
    }
}