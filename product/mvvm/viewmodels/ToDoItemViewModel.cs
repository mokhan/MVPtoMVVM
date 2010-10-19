using System;
using System.ComponentModel;
using MVPtoMVVM.domain;
using MVPtoMVVM.repositories;

namespace MVPtoMVVM.mvvm.viewmodels
{
    public class TodoItemViewModel : Observable<TodoItemViewModel>, IDataErrorInfo, Presenter
    {
        public TodoItemViewModel(UICommandBuilder command_builder)
        {
            SaveCommand = command_builder.build<SaveCommand>(this);
            DeleteCommand = command_builder.build<DeleteCommand>(this);
            present();
        }

        public int Id { get; set; }
        public ObservableCommand SaveCommand { get; set; }
        public ObservableCommand DeleteCommand { get; set; }
        public MainWindowViewModel Parent { get; set; }
        bool is_dirty;

        public bool IsDirty
        {
            get { return is_dirty; }
            set
            {
                is_dirty = value;
                update(x => x.Description, x => x.DueDate);
                SaveCommand.Changed();
            }
        }

        string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                IsDirty = true;
            }
        }

        DateTime dueDate;
        public DateTime DueDate
        {
            get { return dueDate; }
            set
            {
                dueDate = value;
                IsDirty = true;
            }
        }

        public string this[string columnName]
        {
            get { return notification[columnName]; }
        }

        public string Error
        {
            get { return notification.Error; }
        }

        public void present()
        {
            notification = new Notification<TodoItemViewModel>()
                .Register<Error>(x => x.Description, () => string.IsNullOrEmpty(Description), () => "Cannot have an empty description.")
                .Register<Error>(x => x.DueDate, () => DueDate >= DateTime.Now, () => "Due Date must occur on or after today.");
        }

        public bool is_valid()
        {
            return !notification.AreAnyRulesViolatedAndMoreSevereThan<Error>();
        }

        Notification<TodoItemViewModel> notification;
    }

    public class SaveCommand : UICommand<TodoItemViewModel>
    {
        ITodoItemRepository todoItemRepository;

        public SaveCommand(ITodoItemRepository todo_item_repository)
        {
            todoItemRepository = todo_item_repository;
        }

        protected override void run(TodoItemViewModel presenter)
        {
            var todo_item = todoItemRepository.Get(presenter.Id) ?? new TodoItem();
            todo_item.DueDate = presenter.DueDate;
            todo_item.Description = presenter.Description;
            todoItemRepository.Save(todo_item);
            presenter.IsDirty = false;
        }

        protected override bool can_run(TodoItemViewModel presenter)
        {
            return presenter.IsDirty && presenter.is_valid();
        }
    }

    public class DeleteCommand : UICommand<TodoItemViewModel>
    {
        ITodoItemRepository todoItemRepository;

        public DeleteCommand(ITodoItemRepository todo_item_repository)
        {
            todoItemRepository = todo_item_repository;
        }

        protected override void run(TodoItemViewModel presenter)
        {
            todoItemRepository.Delete(presenter.Id);
            presenter.Parent.TodoItems.Remove(presenter);
        }
    }
}