using Autofac;
using ProgressiveRate.Services;
using ProgressiveRate.ViewModels;
using ProgressiveRate.Views;
using System.Windows;

namespace ProgressiveRate
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var container = BuildContainer();
            new MainWindow() { DataContext = container.Resolve<MainViewModel>() }.Show();
        }


        private IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<CargoManager>().As<ICargoManager>();
            builder.RegisterType<CustomDialog>().As<ICustomDialog>();
            builder.RegisterType<ExcelReader>().As<IExcelReader>();
            builder.RegisterType<MainViewModel>().AsSelf();


            return builder.Build();
        }
    }
}
