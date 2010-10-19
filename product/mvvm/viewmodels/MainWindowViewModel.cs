using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MVPtoMVVM.domain;
using MVPtoMVVM.repositories;
using MVPtoMVVM.utility;

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
        UICommandBuilder command_builder;

        public RefreshChangesCommand(ITodoItemRepository todo_item_repository, UICommandBuilder command_builder)
        {
            todoItemRepository = todo_item_repository;
            this.command_builder = command_builder;
        }

        protected override void run(MainWindowViewModel presenter)
        {
            presenter.TodoItems.Clear();
            todoItemRepository.GetAll().Select(map_from).each(x =>
            {
                x.Parent = presenter;
                presenter.TodoItems.Add(x);
            });
        }

        TodoItemViewModel map_from(TodoItem item)
        {
            return new TodoItemViewModel(command_builder)
                {
                    Id = item.Id,
                    Description = item.Description,
                    DueDate = item.DueDate,
                };
        }
    }

    public class AddNewItemCommand : UICommand<MainWindowViewModel>
    {
        UICommandBuilder command_builder;

        public AddNewItemCommand(UICommandBuilder command_builder)
        {
            this.command_builder = command_builder;
        }

        protected override void run(MainWindowViewModel presenter)
        {
            presenter
                .TodoItems
                .Add(new TodoItemViewModel(command_builder)
                    {
                        Parent = presenter,
                        DueDate = DateTime.Today,
                        Description = string.Empty
                    });
        }
    }
}