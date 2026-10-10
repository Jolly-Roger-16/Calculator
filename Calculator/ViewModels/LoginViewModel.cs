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
        public event Action? OnLoginSuccess;

        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private string _captchaText = string.Empty;
        private bool _isFirstAttempt = true;

        private string _login = string.Empty;
        private string _password = string.Empty;
        private string _captchaInput = string.Empty;
        private Canvas? _captchaCanvas;
        private bool _showCaptcha;
        private bool _showLoginButton = true;
        private bool _isInputEnabled = true;
        private string _errorMessage = string.Empty;

        public string Login
        {
            get => _login;
            set => this.RaiseAndSetIfChanged(ref _login, value);
        }

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public string CaptchaInput
        {
            get => _captchaInput;
            set => this.RaiseAndSetIfChanged(ref _captchaInput, value);
        }

        public Canvas? CaptchaCanvas
        {
            get => _captchaCanvas;
            set => this.RaiseAndSetIfChanged(ref _captchaCanvas, value);
        }

        public bool ShowCaptcha
        {
            get => _showCaptcha;
            set => this.RaiseAndSetIfChanged(ref _showCaptcha, value);
        }

        public bool ShowLoginButton
        {
            get => _showLoginButton;
            set => this.RaiseAndSetIfChanged(ref _showLoginButton, value);
        }

        public bool IsInputEnabled
        {
            get => _isInputEnabled;
            set => this.RaiseAndSetIfChanged(ref _isInputEnabled, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        public LoginViewModel()
        {
            _timer.Interval = TimeSpan.FromSeconds(5);
            _timer.Tick += Timer_Tick;
        }

        public void LoginCommand()
        {
            if (_isFirstAttempt)
            {
                if (Login == User.login && Password == User.password)
                {
                    OnLoginSuccess?.Invoke();
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

                if (Login == User.login && Password == User.password && isCaptchaValid)
                {
                    OnLoginSuccess?.Invoke();
                    return;
                }

                ShowCaptcha = false;
                IsInputEnabled = false;
                ErrorMessage = "Слишком много попыток. Подождите 5 секунд...";
                Login = string.Empty;
                Password = string.Empty;
                CaptchaInput = string.Empty;
                _timer.Start();
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _timer.Stop();
            IsInputEnabled = true;
            ShowCaptcha = true;
            ErrorMessage = "Введите данные и новую капчу.";
            GenerateCaptcha();
        }

        private void GenerateCaptcha()
        {
            var rnd = new Random();

            const string chars = "23456789ABCDEFGHJKMNPQRSTUVWXYZ";
            int length = rnd.Next(7, 11);
            var sb = new StringBuilder();
            for (int i = 0; i < length; i++)
                sb.Append(chars[rnd.Next(chars.Length)]);
            _captchaText = sb.ToString();

            double width = 320;
            double height = 100;

            var canvas = new Canvas
            {
                Width = width,
                Height = height,
                Background = new SolidColorBrush(Color.FromRgb(245, 245, 245))
            };

            for (int i = 0; i < 10; i++)
            {
                canvas.Children.Add(new Line
                {
                    StartPoint = new Point(rnd.Next((int)width), rnd.Next((int)height)),
                    EndPoint = new Point(rnd.Next((int)width), rnd.Next((int)height)),
                    Stroke = new SolidColorBrush(Color.FromRgb(
                        (byte)rnd.Next(0, 200),
                        (byte)rnd.Next(0, 200),
                        (byte)rnd.Next(0, 200))),
                    StrokeThickness = rnd.Next(1, 3)
                });
            }

            double availableWidth = width - 60;
            double step = availableWidth / _captchaText.Length;

            for (int i = 0; i < _captchaText.Length; i++)
            {
                var tb = new TextBlock
                {
                    Text = _captchaText[i].ToString(),
                    FontSize = rnd.Next(22, 30),
                    FontWeight = rnd.Next(2) == 0 ? FontWeight.Bold : FontWeight.Normal,
                    FontStyle = rnd.Next(2) == 0 ? FontStyle.Italic : FontStyle.Normal,
                    Foreground = new SolidColorBrush(Color.FromRgb(
                        (byte)rnd.Next(0, 120),
                        (byte)rnd.Next(0, 120),
                        (byte)rnd.Next(0, 120)))
                };

                double xPos = 10 + i * step + rnd.Next(0, Math.Max(1, (int)(step / 4)));
                double yPos = rnd.Next(5, (int)(height - 40));

                Canvas.SetLeft(tb, xPos);
                Canvas.SetTop(tb, yPos);
                canvas.Children.Add(tb);
            }

            CaptchaCanvas = canvas;
        }
    }
}