using MVPtoMVVM.domain;
using MVPtoMVVM.mvvm.viewmodels;
using StructureMap;

namespace MVPtoMVVM.mvvm
{
    public class Bootstrap
    {
        public void Execute()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scanner =>
                {
                    scanner.AssemblyContainingType(typeof (TodoItem));
                    scanner.WithDefaultConventions();
                    scanner.RegisterConcreteTypesAgainstTheFirstInterface();
                });
                x.AddType(typeof(UICommandBuilder), typeof(WPFCommandBuilder));
            });
        }
    }
}