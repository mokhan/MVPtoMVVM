using StructureMap;

namespace MVPtoMVVM.mvvm.viewmodels
{
    public interface UICommandBuilder
    {
        ObservableCommand build<T>(Presenter presenter) where T : UICommand;
    }

    public interface Presenter
    {
        void present();
    }

    public interface UICommand
    {
        void run<T>(T presenter) where T : Presenter;
        bool can_run<T>(T presenter) where T : Presenter;
    }

    public abstract class UICommand<T> : UICommand where T : class, Presenter
    {
        void UICommand.run<T1>(T1 presenter)
        {
            run(presenter as T);
        }

        public bool can_run<T1>(T1 presenter) where T1 : Presenter
        {
            return can_run(presenter as T);
        }

        protected abstract void run(T presenter);
        protected virtual bool can_run(T presenter)
        {
            return true;
        }
    }

    public class WPFCommandBuilder : UICommandBuilder
    {
        public ObservableCommand build<T>(Presenter presenter) where T : UICommand
        {
            var command = ObjectFactory.GetInstance<T>();
            return new SimpleCommand(() => { command.run(presenter); }, () => command.can_run(presenter));
        }
    }
}