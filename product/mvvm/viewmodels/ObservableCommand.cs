using System.Windows.Input;

namespace MVPtoMVVM.mvvm.viewmodels
{
    public interface ObservableCommand : ICommand
    {
        void Changed();
    }
}