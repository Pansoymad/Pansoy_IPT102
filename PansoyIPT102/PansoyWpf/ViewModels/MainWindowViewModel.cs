using Microsoft.Extensions.DependencyInjection;
using PansoyWpf.Stores;
using System.Windows.Input;

namespace PansoyWpf.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly IServiceProvider _serviceProvider;

        public MainWindowViewModel(
            NavigationStore navigationStore,
            IServiceProvider serviceProvider)
        {
            _navigationStore = navigationStore;
            _serviceProvider = serviceProvider;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            NavigateHomeCommand = new PansoyWpf.Commands.RelayCommand(_ => NavigateHome());
            NavigateToManagerCommand = new PansoyWpf.Commands.RelayCommand(_ => NavigateToManager());

            // Start with home page
            NavigateHome();
        }

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateToManagerCommand { get; }

        private void NavigateHome()
        {
            _navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<HomeViewModel>();
        }

        private void NavigateToManager()
        {
            _navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
