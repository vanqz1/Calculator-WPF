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

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String currentNumber = String.Empty;
        private List<String> outputRecord;
        private Stack<String> operators;
        private Stack<float> numbers;
        private float rezult;
        private bool invalidExpresion = false;

        public MainWindow()
        {
            outputRecord = new List<String>();
            operators = new Stack<String>();
            numbers = new Stack<float>();
            rezult = 0;
            InitializeComponent();

        }

        private void ButtonNumbers_Click(object sender, RoutedEventArgs e)
        {
            string content = (sender as Button).Content.ToString();
            currentNumber += content;
            Input.Text += content;
         
        }

        private void ButtonOperator_Click(object sender, RoutedEventArgs e)
        {
            CheckCurrentNumber();

            string content = (sender as Button).Content.ToString();

            if (operators.Count != 0)
            {
                var priorityOperatorTopStack = GetOperatorsPriority(operators.Peek());
                var currentOperatorPriority = GetOperatorsPriority(content);
                if(priorityOperatorTopStack >= currentOperatorPriority)
                {
                    outputRecord.Add(operators.Pop());
                }
            }

            operators.Push(content);
            Input.Text += content;
        }

        private void ButtonBrackets_Click(object sender, RoutedEventArgs e)
        {
            string content = (sender as Button).Content.ToString();
            Input.Text += content;
            if (content == "(")
            {
                operators.Push(content); 
            }
            else if(content == ")")
            {
                bool bracketIsFound = false;
                CheckCurrentNumber();
                for (int i = 0; i < operators.Count; i++)
                {
                    bracketIsFound = false;
                    var operatorStack = operators.Pop();
                    if (operatorStack != "(")
                    {
                        outputRecord.Add(operatorStack);
                    }
                    else
                    {

                        bracketIsFound = true;
                        operators.Peek();
                        break;
                    }
                }

                if(!bracketIsFound)
                {
                    MessageBox.Show("Invalid number of brackets");
                }
            }
        }

        private void ButtonRezult_Click(object sender, RoutedEventArgs e)
        {
            CheckCurrentNumber();
            EmptyTheOperatorsStack();
            CalculationBasedOnOutputRecord();
            if (numbers.Count == 1)
            {
                Input.Text = rezult.ToString();
            }
            else
            {
                invalidExpresion = true;
            }
        }

         private int GetOperatorsPriority(String operator1)
        {
            switch (operator1)
            {
                case "+":
                    return 1;
                break;
                case "-":
                     return 1;
                break;
                case "/":
                     return 2;
                break;
                case "*":
                     return 2;
                break;
                default: 
                    return 0;
            }
        
        }

        private float DoOperation(String operator1, float number1,float number2)
        {
            switch (operator1)
            {
                case "+":
                    return number1 + number2;
                    break;
                case "-":
                    return number1 - number2;
                    break;
                case "/":
                    return number1 / number2;
                    break;
                case "*":
                    return number1 * number2;
                    break;
                default:
                    return 0;
            }
        }

        private void EmptyTheOperatorsStack()
        { 
            if(operators.Count != 0)
            {
                for (int i = 0; i < operators.Count; i++)
                {
                    outputRecord.Add(operators.Pop());
                }
            }
        }

        private void CheckCurrentNumber()
        {
            if (currentNumber != String.Empty)
            {
                outputRecord.Add(currentNumber);
                currentNumber = String.Empty;
            }
        }

        private void CalculationBasedOnOutputRecord()
        {
            for (int i = 0; i < outputRecord.Count; i++)
            {
                var item = outputRecord[i];
                if (GetOperatorsPriority(item) == 0)
                {
                    numbers.Push(float.Parse(item));
                }
                else
                {
                    if (numbers.Count >= 2)
                    {
                        float number1 = numbers.Pop();
                        float number2 = numbers.Pop(); 
                        rezult = DoOperation(item, number2, number1);
                        numbers.Push(rezult);
                    }
                    else 
                    {
                        invalidExpresion = true;
                    }
               }
            }
        }
    }
}
