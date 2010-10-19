using MVPtoMVVM.mvvm.viewmodels;
using StructureMap;

namespace MVPtoMVVM.mvvm
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = ObjectFactory.GetInstance<MainWindowViewModel>();
            ViewModel.present();
        }

        public MainWindowViewModel ViewModel
        {
            get { return (MainWindowViewModel) DataContext; }
            set { DataContext = value; }
        }
    }
}