using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleApp1
{

    class Priorities
    {
        string operation;
        int priority;

        public Priorities(string value, int priortiy)
        {
            this.operation = value;
            this.priority = priortiy;
        }

        public int getPriority()
        {
            return this.priority;
        }

        public string getOperation()
        {
            return this.operation;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Priorities plus = new Priorities("+", 2);
            Priorities minus = new Priorities("-", 2);
            Priorities multiply = new Priorities("*", 1);
            Priorities divide = new Priorities("/", 1);
            Priorities modulo = new Priorities("%", 1);

            Stack<Priorities> operatorStack = new Stack<Priorities>();
            Stack<int> finalStack = new Stack<int>();
            ArrayList outputArray = new ArrayList();

            

            string[] characterArray = args;

            //Shunting Yard Algorithm
            for (int i = 0; i < characterArray.Length; i++)
            {
                switch (characterArray[i])
                {
                    case ("+"):
                        if (operatorStack.Count == 0)
                        {
                            operatorStack.Push(plus);
                        } else if (plus.getPriority() >= operatorStack.Peek().getPriority())
                        {
                            operatorStack.Push(plus);
                        } else
                        {
                            operatorStack.Push(plus);
                        }
                        break;

                    case ("-"):
                        if (operatorStack.Count == 0)
                        {
                            operatorStack.Push(minus);
                        }
                        else if (minus.getPriority() >= operatorStack.Peek().getPriority())
                        {
                            operatorStack.Push(minus);
                        }
                        else
                        {
                            operatorStack.Push(minus);
                        }
                        break;

                    case ("*"):
                        if (operatorStack.Count == 0)
                        {
                            operatorStack.Push(multiply);
                        }
                        else if (multiply.getPriority() >= operatorStack.Peek().getPriority())
                        {
                            operatorStack.Push(multiply);
                        }
                        else
                        {
                            operatorStack.Push(multiply);
                        }
                        break;

                    case ("/"):
                        if (operatorStack.Count == 0)
                        {
                            operatorStack.Push(divide);
                        }
                        else if (divide.getPriority() >= operatorStack.Peek().getPriority())
                        {
                            operatorStack.Push(divide);
                        }
                        else
                        {
                            operatorStack.Push(divide);
                        }
                        break;

                    case ("%"):
                        if (operatorStack.Count == 0)
                        {
                            operatorStack.Push(modulo);
                        }
                        else if (modulo.getPriority() >= operatorStack.Peek().getPriority())
                        {
                            operatorStack.Push(modulo);
                        }
                        else
                        {
                            operatorStack.Push(modulo);
                        }
                        break;
                    default:
                        outputArray.Add(characterArray[i]);
                        break;
                }
            }

            //Combines
            while (operatorStack.Count > 0)
            {
                outputArray.Add(operatorStack.Pop().getOperation());
            }



            //Evaluates
            int firstNumber;
            int secondNumber;
            for (int i = 0; i < outputArray.Count; i++)
            {
                switch (outputArray[i])
                {
                    case ("+"):
                        firstNumber = finalStack.Pop();
                        secondNumber = finalStack.Pop();
                        finalStack.Push(secondNumber + firstNumber);
                        break;

                    case ("-"):
                        firstNumber = finalStack.Pop();
                        secondNumber = finalStack.Pop();
                        finalStack.Push(secondNumber - firstNumber);
                        break;

                    case ("*"):
                        firstNumber = finalStack.Pop();
                        secondNumber = finalStack.Pop();
                        finalStack.Push(secondNumber * firstNumber);
                        break;

                    case ("/"):
                        firstNumber = finalStack.Pop();
                        secondNumber = finalStack.Pop();
                        finalStack.Push(secondNumber / firstNumber);
                        break;

                    case ("%"):
                        firstNumber = finalStack.Pop();
                        secondNumber = finalStack.Pop();
                        finalStack.Push(secondNumber % firstNumber);
                        break;

                    default:
                        finalStack.Push(Convert.ToInt32(outputArray[i]));
                        break;
                }
            }


            Console.WriteLine(finalStack.Pop());



        }
    }



}
