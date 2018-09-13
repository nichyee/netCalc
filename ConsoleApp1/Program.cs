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

            Stack xStack = new Stack();
            Stack<Priorities> priorities = new Stack<Priorities>();
            ArrayList arrayList = new ArrayList();
            Stack<int> final = new Stack<int>();
            int leftHandResult;
            int rightHandResult;
            ArrayList operatorX = new ArrayList();
            int leftOrRight = 1;
            string opBeforeStatement = "+";

            try
            {
                //Console.WriteLine("Enter statement to be evaluated:");
                string userInput = "";
                for (int i = 0; i < args.Length; i++)
                {
                    userInput += (args[i] + " ");
                }
                string[] twoSides = userInput.Split('=');
                string[] leftHandSide = twoSides[0].Split(' ');
                string[] rightHandSide = twoSides[1].Split(' ');

                ArrayList tempLeft = new ArrayList(leftHandSide);
                tempLeft.RemoveAt(tempLeft.Count - 1);
                ArrayList tempRight = new ArrayList(rightHandSide);
                tempRight.RemoveAt(tempRight.Count - 1);
                tempRight.RemoveAt(0);

                leftHandSide = (string[])tempLeft.ToArray(typeof(string));
                rightHandSide = (string[])tempRight.ToArray(typeof(string));

                leftHandSide = removeX(leftHandSide);
                if (operatorX.Count > 0)
                {
                    leftOrRight = 0;
                }
                priorities = shuntingYard(leftHandSide).Item1;
                arrayList = shuntingYard(leftHandSide).Item2;
                arrayList = combineStacks(priorities, arrayList);
                final = evaluateStatement(arrayList);
                leftHandResult = final.Pop();

                rightHandSide = removeX(rightHandSide);
                priorities = shuntingYard(rightHandSide).Item1;
                arrayList = shuntingYard(rightHandSide).Item2;
                arrayList = combineStacks(priorities, arrayList);
                final = evaluateStatement(arrayList);
                rightHandResult = final.Pop();

                Console.WriteLine("X = {0}", resolveX(leftHandResult, rightHandResult));
            }
            catch (OverflowException)
            {
                Console.WriteLine("Numbers are too large");
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Cannot Divide by Zero");
            }
            catch (FormatException)
            {
                Console.WriteLine("Incorrect Format");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Missing X");
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Missing = sign");
            }

            //Shunting Yard Algorithm
            (Stack<Priorities>, ArrayList) shuntingYard(string[] numbers)
            {
                Stack<Priorities> operatorStack = new Stack<Priorities>();
                ArrayList outputArray = new ArrayList();

                for (int i = 0; i < numbers.Length; i++)
                {
                    switch (numbers[i])
                    {
                        case ("+"):
                            if (operatorStack.Count == 0)
                            {
                                operatorStack.Push(plus);
                            }
                            else if (plus.getPriority() >= operatorStack.Peek().getPriority())
                            {
                                outputArray.Add(operatorStack.Pop().getOperation());
                                operatorStack.Push(plus);
                            }
                            else
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
                                outputArray.Add(operatorStack.Pop().getOperation());
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
                                outputArray.Add(operatorStack.Pop().getOperation());
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
                                outputArray.Add(operatorStack.Pop().getOperation());
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
                                outputArray.Add(operatorStack.Pop().getOperation());
                                operatorStack.Push(modulo);
                            }
                            else
                            {
                                operatorStack.Push(modulo);
                            }
                            break;
                        case ("X"):
                            xStack.Push("X");
                            break;
                        default:
                            outputArray.Add(numbers[i]);
                            break;
                    }
                }
                return (operatorStack, outputArray);
            }

            ArrayList combineStacks(Stack<Priorities> operatorStack, ArrayList outputArray)
            {

                while (operatorStack.Count > 0)
                {
                    outputArray.Add(operatorStack.Pop().getOperation());
                }
                return (outputArray);
            }

            Stack<int> evaluateStatement(ArrayList combinedArray)
            {
                //Evaluates
                Stack<int> finalStack = new Stack<int>();
                int firstNumber;
                int secondNumber;
                for (int i = 0; i < combinedArray.Count; i++)
                {
                    switch (combinedArray[i])
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
                            finalStack.Push(Convert.ToInt32(combinedArray[i]));
                            break;
                    }
                }
                return finalStack;
            }

            String[] removeX(string[] passedInArray)
            {
                int remove = -1;
                ArrayList localArrayList = new ArrayList();
                localArrayList.AddRange(passedInArray);

                for (int i = localArrayList.Count - 1; i >= 0; i--)
                {
                    if (localArrayList[i].ToString().Contains("X"))
                    {
                        remove = i;
                    }
                }

                if (remove > 0)
                {
                    operatorX.Add(localArrayList[remove - 1]);
                    operatorX.Add(localArrayList[remove]);
                    localArrayList.RemoveRange(remove - 1, 2);
                }
                else if (remove == 0)
                {
                    operatorX.Add("+");
                    operatorX.Add(localArrayList[remove]);
                    if (localArrayList.Count > 1)
                    {
                        opBeforeStatement = localArrayList[remove + 1].ToString();
                        localArrayList.RemoveRange(remove, 2);
                    }
                    else
                    {
                        localArrayList.RemoveAt(remove);
                    }
                    localArrayList.Reverse();
                    localArrayList.Add("0");
                    localArrayList.Reverse();
                }

                return (string[])localArrayList.ToArray(typeof(string));
            }

            int resolveX(int leftSide, int rightSide)
            {
                int result = 0;
                char[] temp = operatorX[1].ToString().ToCharArray();
                string[] xArray = new string[temp.Length];
                for (int i = 0; i < temp.Length; i++)
                {
                    xArray[i] = temp[i].ToString();
                }
                int xCoefficient = 1;


                if (operatorX.Count > 0 && leftOrRight == 1)
                {
                    switch (opBeforeStatement)
                    {
                        case ("+"):
                            result = leftSide - rightSide;
                            break;
                        case ("-"):
                            result = leftSide + rightSide;
                            break;
                        case ("*"):
                            result = leftSide / rightSide;
                            break;
                        case ("/"):
                            result = rightSide * leftSide;
                            break;
                    }
                }
                else if (operatorX.Count > 0 && leftOrRight == 0)
                {
                    switch (opBeforeStatement)
                    {
                        case ("+"):
                            result = rightSide - leftSide;
                            break;
                        case ("-"):
                            result = rightSide + leftSide;
                            break;
                        case ("*"):
                            result = rightSide / leftSide;
                            break;
                        case ("/"):
                            result = leftSide * rightSide;
                            break;
                    }
                }

                if (xArray.Length > 1)
                {
                    xCoefficient = coefficientCreator(xArray);
                    result = result / xCoefficient;
                }


                return result;
            }

            int coefficientCreator(string[] array)
            {
                double coefficient = 0;
                int power = 0;
                for (int i = array.Length - 2; i >= 0; i--)
                {
                    coefficient += Math.Pow(10.0, power) * Convert.ToInt32(array[i]);
                    power++;
                }
                return (int)coefficient;
            }
        }
    }
}
