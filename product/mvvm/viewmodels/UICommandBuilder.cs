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
    }

    public abstract class UICommand<T> : UICommand where T : class, Presenter
    {
        void UICommand.run<T1>(T1 presenter)
        {
            run(presenter as T);
        }

        protected abstract void run(T presenter);
    }

    public class WPFCommandBuilder : UICommandBuilder
    {
        public ObservableCommand build<T>(Presenter presenter) where T : UICommand
        {
            var command = ObjectFactory.GetInstance<T>();
            return new SimpleCommand(() =>
            {
                command.run(presenter);
            });
        }
    }
}