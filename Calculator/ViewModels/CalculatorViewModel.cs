using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Calculator.ViewModels
{
    public class CalculatorViewModel : ViewModelBase
    {
        private string _selectedOperation;
        private string _operand1;
        private string _operand2;
        private string _result;

        public ObservableCollection<string> Operations { get; } = new()
        {
            "Сложение",
            "Вычитание",
            "Умножение",
            "Деление"
        };

        public string SelectedOperation
        {
            get => _selectedOperation;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedOperation, value);
                this.RaisePropertyChanged(nameof(OperationSymbol));
            }
        }

        public string OperationSymbol
        {
            get
            {
                switch (SelectedOperation)
                {
                    case "Сложение": return "+";
                    case "Вычитание": return "-";
                    case "Умножение": return "*";
                    case "Деление": return "÷";
                    default: return "+";
                }
            }
        }

        public string Operand1
        {
            get => _operand1;
            set => this.RaiseAndSetIfChanged(ref _operand1, value);
        }

        public string Operand2
        {
            get => _operand2;
            set => this.RaiseAndSetIfChanged(ref _operand2, value);
        }

        public string Result
        {
            get => _result;
            set => this.RaiseAndSetIfChanged(ref _result, value);
        }

        public CalculatorViewModel()
        {
            SelectedOperation = Operations[0]; 
        }

        public void Calculate()
        {
            if (double.TryParse(Operand1, NumberStyles.Any, CultureInfo.InvariantCulture, out double num1) &&
                double.TryParse(Operand2, NumberStyles.Any, CultureInfo.InvariantCulture, out double num2))
            {
                double calcResult = 0;

                switch (SelectedOperation)
                {
                    case "Сложение":
                        calcResult = num1 + num2;
                        break;
                    case "Вычитание":
                        calcResult = num1 - num2;
                        break;
                    case "Умножение":
                        calcResult = num1 * num2;
                        break;
                    case "Деление":
                        if (num2 != 0)
                            calcResult = num1 / num2;
                        else
                        {
                            Result = "Деление на ноль!";
                            return;
                        }
                        break;
                }

                Result = calcResult.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                Result = "Ошибка ввода";
            }
        }
    }
}