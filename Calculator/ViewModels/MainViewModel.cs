using ReactiveUI;

namespace Calculator.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentPage;

        public ViewModelBase CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        public MainViewModel()
        {
            var loginVm = new LoginViewModel();


            loginVm.OnLoginSuccess += () =>
            {
                CurrentPage = new CalculatorViewModel();
            };

            _currentPage = loginVm;
        }
    }
}