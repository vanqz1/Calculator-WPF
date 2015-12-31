using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalculatorSecondType
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<String> outputRecord;
        private Stack<String> operators;
        private Stack<double> numbers;
        public MainWindow()
        {
            Uri iconUri = new Uri("http://www.veryicon.com/icon/ico/Application/Long%20Shadow%20Media/Calculator.ico", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);
            outputRecord = new List<String>();
            operators = new Stack<String>();
            numbers = new Stack<double>();
            InitializeComponent();
        }


        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            string content = (sender as Button).Content.ToString();
            this.Input.Text += content;
            if (operators.Count > 0)
            {
                var priorityContent = GetOperatorPriority(content);
                var priorityOperatorTopStack = GetOperatorPriority(operators.Peek());
                if (priorityOperatorTopStack >= priorityContent)
                {
                    outputRecord.Add(operators.Pop());
                }
            }

            operators.Push(content);
        }
        private void AddNumberBtn_Click(object sender, RoutedEventArgs e)
        {

            if (this.AddNumber.Text == String.Empty)
            {
                MessageBox.Show("You should write number");
            }
            else
            {
                this.Input.Text += this.AddNumber.Text;
                outputRecord.Add(this.AddNumber.Text);
                this.AddNumber.Text = String.Empty;
            }
        }

        private void OpenBracketBtn_Click(object sender, RoutedEventArgs e)
        {
            string content = (sender as Button).Content.ToString();
            this.Input.Text += content;
            operators.Push(content);
        }

        private void CloseBracketBtn_Click(object sender, RoutedEventArgs e)
        {
            string content = (sender as Button).Content.ToString();
            bool foundOpenBracket = false;
            List<String> getOperators = new List<String>();
            for (int i = 0; i < operators.Count; i++)
            {
               var currentOperator = operators.Pop();
               if (currentOperator != "(")
               {
                   getOperators.Add(currentOperator);
               }
               else
               {
                   foundOpenBracket = true;
                   this.Input.Text += content;
                   for (int j = 0; j < getOperators.Count; j++)
                   {
                       outputRecord.Add(getOperators[j]);
                   }
                   break;
               }
            }

            if(!foundOpenBracket)
            {
                foundOpenBracket = true;
                for (int j = getOperators.Count-1; j >= 0; j--)
                {
                    operators.Push(getOperators[j]);
                }
                MessageBox.Show("Wrong expression!");
            }
        }

        private void EquelsBtn_Click(object sender, RoutedEventArgs e)
        {
            EmptyOperatorsStack();
            CalculateRezult();
            this.Input.Text = numbers.Pop().ToString();
        }

        private void TrigonometryBtn_Click(object sender, RoutedEventArgs e)
        {
            string content = (sender as Button).Content.ToString();
            if (GetTrigonometricTextBoxText(content) != String.Empty)
            {
                this.Input.Text += content + "(" + GetTrigonometricTextBoxText(content) + ")";
                var rezult = DoTrigonomethryOperations(double.Parse(GetTrigonometricTextBoxText(content)), content);
                this.SinTextBox.Text = String.Empty;
                outputRecord.Add(rezult.ToString());
            }
            else
            {
                MessageBox.Show("Please fill value for " + content);
            }
        }

        private void LogBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.LogATextBox.Text == String.Empty || this.LogBTextBox.Text == String.Empty)
            {
                MessageBox.Show("The Log fields should be filled!");
            }
            else if (double.Parse(this.LogATextBox.Text) < 0 || double.Parse(this.LogBTextBox.Text) < 0)
            {
                MessageBox.Show("Log fields should be possitive numbers!");
            }
            else
            {
                this.Input.Text += "Log" + this.LogATextBox.Text + "(" + this.LogBTextBox.Text + ")";
                outputRecord.Add(Math.Log(double.Parse(this.LogBTextBox.Text), double.Parse(this.LogATextBox.Text)).ToString());
                this.LogATextBox.Text = String.Empty;
                this.LogBTextBox.Text = String.Empty;
            }
        }

        private void LnBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.LnTextBox.Text == String.Empty)
            {
                MessageBox.Show("The Ln field should be filled!");
            }
            else
            {
                if (double.Parse(this.LnTextBox.Text) <= 0)
                {
                    MessageBox.Show("It should be possitive number!");
                }
                else
                {
                    this.Input.Text += "Ln(" + this.LnTextBox.Text + ")";
                    outputRecord.Add(Math.Log(double.Parse(this.LnTextBox.Text)).ToString());
                    this.LnTextBox.Text = String.Empty;
                }
            }
        }

        private void XY_Click(object sender, RoutedEventArgs e)
        {
            if (this.X.Text == String.Empty || this.Y.Text == String.Empty)
            {
                MessageBox.Show("Fields for pow should be filled");
            }
            else
            {
                this.Input.Text += this.X.Text + "^" + this.Y.Text;
                var rezult = Math.Pow(double.Parse(this.X.Text), double.Parse(this.Y.Text));
                outputRecord.Add(rezult.ToString());
                this.X.Text = String.Empty;
                this.Y.Text = String.Empty;
            }
        }

        private void SqrtRootBtn(object sender, RoutedEventArgs e)
        {
            if (this.XSqrtRoot.Text == String.Empty || this.YSqrtRoot.Text == String.Empty)
            {
                MessageBox.Show("Fields for squart toot should be filled");
            }
            else
            {
                this.Input.Text += this.XSqrtRoot.Text + "^" + "1/" + this.YSqrtRoot.Text;
                var rezult = Math.Pow(double.Parse(this.XSqrtRoot.Text), 1 / double.Parse(this.YSqrtRoot.Text));
                outputRecord.Add(rezult.ToString());
                this.XSqrtRoot.Text = String.Empty;
                this.YSqrtRoot.Text = String.Empty;
            }
        }

        private void EXBtn(object sender, RoutedEventArgs e)
        {
            if (this.EXtextBox.Text == String.Empty)
            {
                MessageBox.Show("Fields for e^X should be filled");
            }
            else
            {
                this.Input.Text += "e" + "^" + this.EXtextBox.Text;
                var rezult = Math.Pow(Math.E, double.Parse(this.EXtextBox.Text));
                outputRecord.Add(rezult.ToString());
                this.EXtextBox.Text = String.Empty;
            }
        }

        private int GetOperatorPriority(String operator1)
        { 
            switch(operator1)
            {
                case "+":
                    return 1;
                case "-":
                    return 1;
                case "/":
                    return 2;
                case "*":
                    return 2;
                default:
                    return 0;
            }
        }

        private void CalculateRezult()
        {
            for (int i = 0; i < outputRecord.Count; i++)
            {
                var currentItem = outputRecord[i];
                var priorityCurrentItem = GetOperatorPriority(currentItem);
                if (priorityCurrentItem == 0)
                {
                    if (numbers.Count == 1 && i == outputRecord.Count-1)
                    {
                        break;
                    }
                    numbers.Push(double.Parse(currentItem));
                }
                else
                {
                    if (numbers.Count < 2)
                    {
                        MessageBox.Show("Invalid Expression!");
                    }
                    else
                    {
                        var number1 = numbers.Pop();
                        var number2 = numbers.Pop();
                        var rezult = DoOperations(number2, number1, currentItem);
                        numbers.Push(rezult);
                    }
                }
            }
        }

        private double DoOperations(double operand1,double operand2,String theOperator)
        { 
            switch(theOperator)
            {
                case "+":
                    return operand1 + operand2;
                case "-":
                    return operand1 - operand2;
                case "/":
                    return operand1 / operand2;
                case "*":
                    return operand1 * operand2;
                default:
                    throw new Exception("Unrecognized operation");
            }
        }

        private void EmptyOperatorsStack()
        {
            if (operators.Count != 0)
            {
                for (int i = 0; i <= operators.Count; i++)
                {
                    outputRecord.Add(operators.Pop());
                }
            }
        }

        private double DoTrigonomethryOperations(double number, String theOperator)
        {
            switch (theOperator)
            {
                case "Sin":
                    return Math.Sin(Math.PI * number);
                case "Cos":
                    return Math.Cos(Math.PI * number);
                case "Tg":
                    return Math.Tan(Math.PI * number);
                case "Cotg":
                    return 1 / Math.Tan(Math.PI * number);
                default:
                    throw new Exception("Unrecognized operation");
            }
        }

        

        private String GetTrigonometricTextBoxText(String type)
        { 
            switch(type)
            {
                case "Sin":
                    return this.SinTextBox.Text;
                case "Cos":
                    return this.CosTextBox.Text;
                case "Tg":
                    return this.TgTextBox.Text;
                case "Cotg":
                    return this.CotgTextBox.Text;
                default:
                    throw new Exception("Unrecognized operation");
            }
        }

        private void CleanBtn(object sender, RoutedEventArgs e)
        {
            outputRecord = new List<String>();
            operators = new Stack<String>();
            numbers = new Stack<double>();
            this.Input.Text = String.Empty;
        }

        

        
    }
}
