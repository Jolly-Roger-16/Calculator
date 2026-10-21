using ReactiveUI;
using System;

namespace Calculator.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentPage;

        public ViewModelBase CurrentPage
        {
            get { return _currentPage; }
            set { this.RaiseAndSetIfChanged(ref _currentPage, value); }
        }

        public MainViewModel()
        {
            LoginViewModel loginVm = new LoginViewModel();

            loginVm.OnLoginSuccess += LoginVm_OnLoginSuccess;

            _currentPage = loginVm;
        }

        private void LoginVm_OnLoginSuccess()
        {
            CurrentPage = new CalculatorViewModel();
        }
    }
}