using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Threading;
using Calculator.Models;
using ReactiveUI;
using System;
using System.Text;

namespace Calculator.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public event Action OnLoginSuccess;

        private DispatcherTimer _timer = new DispatcherTimer();
        private string _captchaText = "";
        private bool _isFirstAttempt = true;

        private string _login = "";
        private string _password = "";
        private string _captchaInput = "";
        private Canvas _captchaCanvas;
        private bool _showCaptcha;
        private bool _showLoginButton = true;
        private bool _isInputEnabled = true;
        private string _errorMessage = "";

        public string Login
        {
            get { return _login; }
            set { this.RaiseAndSetIfChanged(ref _login, value); }
        }

        public string Password
        {
            get { return _password; }
            set { this.RaiseAndSetIfChanged(ref _password, value); }
        }

        public string CaptchaInput
        {
            get { return _captchaInput; }
            set { this.RaiseAndSetIfChanged(ref _captchaInput, value); }
        }

        public Canvas CaptchaCanvas
        {
            get { return _captchaCanvas; }
            set { this.RaiseAndSetIfChanged(ref _captchaCanvas, value); }
        }

        public bool ShowCaptcha
        {
            get { return _showCaptcha; }
            set { this.RaiseAndSetIfChanged(ref _showCaptcha, value); }
        }

        public bool ShowLoginButton
        {
            get { return _showLoginButton; }
            set { this.RaiseAndSetIfChanged(ref _showLoginButton, value); }
        }

        public bool IsInputEnabled
        {
            get { return _isInputEnabled; }
            set { this.RaiseAndSetIfChanged(ref _isInputEnabled, value); }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { this.RaiseAndSetIfChanged(ref _errorMessage, value); }
        }

        public LoginViewModel()
        {
            _timer.Interval = TimeSpan.FromSeconds(5);
            _timer.Tick += Timer_Tick;
        }

        public void LoginCommand()
        {
            if (_isFirstAttempt == true)
            {
                if (Login == User.login && Password == User.password)
                {
                    if (OnLoginSuccess != null)
                    {
                        OnLoginSuccess();
                    }
                    return;
                }

                _isFirstAttempt = false;
                ShowLoginButton = false;
                ShowCaptcha = true;
                ErrorMessage = "Неверный логин или пароль. Введите капчу.";
                GenerateCaptcha();
            }
            else
            {
                bool isCaptchaValid = string.Equals(CaptchaInput, _captchaText, StringComparison.OrdinalIgnoreCase);

                if (Login == User.login && Password == User.password && isCaptchaValid == true)
                {
                    if (OnLoginSuccess != null)
                    {
                        OnLoginSuccess();
                    }
                    return;
                }

                ShowCaptcha = false;
                IsInputEnabled = false;
                ErrorMessage = "Слишком много попыток. Подождите 5 секунд...";
                Login = "";
                Password = "";
                CaptchaInput = "";
                _timer.Start();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            IsInputEnabled = true;
            ShowCaptcha = true;
            ErrorMessage = "Введите данные и новую капчу.";
            GenerateCaptcha();
        }

        private void GenerateCaptcha()
        {
            Random rnd = new Random();

            const string chars = "23456789ABCDEFGHJKMNPQRSTUVWXYZ";
            int length = rnd.Next(7, 11);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[rnd.Next(chars.Length)]);
            }
            _captchaText = sb.ToString();

            double width = 320;
            double height = 100;

            Canvas canvas = new Canvas();
            canvas.Width = width;
            canvas.Height = height;
            canvas.Background = new SolidColorBrush(Color.FromRgb(245, 245, 245));

            for (int i = 0; i < 10; i++)
            {
                Line line = new Line();
                line.StartPoint = new Point(rnd.Next((int)width), rnd.Next((int)height));
                line.EndPoint = new Point(rnd.Next((int)width), rnd.Next((int)height));
                line.Stroke = new SolidColorBrush(Color.FromRgb(
                    (byte)rnd.Next(0, 200),
                    (byte)rnd.Next(0, 200),
                    (byte)rnd.Next(0, 200)));
                line.StrokeThickness = rnd.Next(1, 3);
                canvas.Children.Add(line);
            }

            double availableWidth = width - 60;
            double step = availableWidth / _captchaText.Length;

            for (int i = 0; i < _captchaText.Length; i++)
            {
                TextBlock tb = new TextBlock();
                tb.Text = _captchaText[i].ToString();
                tb.FontSize = rnd.Next(22, 30);
                tb.FontWeight = rnd.Next(2) == 0 ? FontWeight.Bold : FontWeight.Normal;
                tb.FontStyle = rnd.Next(2) == 0 ? FontStyle.Italic : FontStyle.Normal;
                tb.Foreground = new SolidColorBrush(Color.FromRgb(
                    (byte)rnd.Next(0, 120),
                    (byte)rnd.Next(0, 120),
                    (byte)rnd.Next(0, 120)));

                double xPos = 10 + i * step;
                double yPos = 30 + rnd.Next(-10, 11);

                Canvas.SetLeft(tb, xPos);
                Canvas.SetTop(tb, yPos);
                canvas.Children.Add(tb);
            }

            CaptchaCanvas = canvas;
        }
    }
}