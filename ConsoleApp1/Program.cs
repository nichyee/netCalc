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
            int leftOrRight = 0;

            Console.WriteLine("Enter statement to be evaluated:");
            string userInput = Console.ReadLine();
            string[] twoSides = userInput.Split(" = ");
            string[] leftHandSide = twoSides[0].Split(" ");
            string[] rightHandSide = twoSides[1].Split(" ");


            try
            {
                leftHandSide = removeX(leftHandSide);
                if (operatorX.Count > 0)
                {
                    leftOrRight = 1;
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

                if (operatorX.Count > 0)
                {
                    operatorX = flipOperator(operatorX);
                    Console.WriteLine("X = {0}", resolveX(leftHandResult, rightHandResult));
                }
                

                Console.WriteLine("LHS = {0} ... RHS = {1}", leftHandResult, rightHandResult);
                foreach (string item in operatorX)
                {
                    Console.Write(item + " ");
                }
            }
            catch (OverflowException)
            {
                Console.WriteLine("Index out of Range");
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Cannot Divide by Zero");
            }
            catch (FormatException)
            {
                Console.WriteLine("Incorrect Format");
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

                for (int i = localArrayList.Count -1; i >= 0; i--)
                {
                    if (localArrayList[i].ToString().Contains("X"))
                    {
                        remove = i;
                        leftOrRight = 1;
                    }
                }

                if (remove > 0) {
                    operatorX.Add(localArrayList[remove -1 ]);
                    operatorX.Add(localArrayList[remove]);
                    localArrayList.RemoveRange(remove-1, 2);
                }

                return (string[])localArrayList.ToArray(typeof(string));
            }

            ArrayList flipOperator(ArrayList operatorAndX)
            {
                switch (operatorAndX[0].ToString())
                {
                    case ("+"):
                        operatorAndX[0] = "-";
                        break;

                    case ("-"):
                        operatorAndX[0] = "+";
                        break;

                    case ("*"):
                        operatorAndX[0] = "/";
                        break;

                    case ("/"):
                        operatorAndX[0] = "*";
                        break;
                    default:
                        break;
                }

                return operatorAndX;
            }

            int resolveX(int leftSide, int rightSide)
            {
                int result = 0;
                char[] xArray = operatorX[1].ToString().ToCharArray();
                int xCoefficient = 1;

                if (operatorX.Count > 0 && leftOrRight == 1)
                {
                    result = leftSide - rightSide;
                } else if (operatorX.Count > 0 && leftOrRight == 0)
                {
                    result = rightSide - leftSide ;
                }

                switch (xArray.Length)
                {
                    case (2):
                        xCoefficient = Convert.ToInt32(xArray[0]);
                        result = result / xCoefficient;
                        break;
                    case (3):
                        xCoefficient = (10* Convert.ToInt32(xArray[0])) + Convert.ToInt32(xArray[1]);
                        result = result / xCoefficient;
                        break;
                    case (4):
                        xCoefficient = (100 * Convert.ToInt32(xArray[0])) + (10 * Convert.ToInt32(xArray[1])) + Convert.ToInt32(xArray[0]);
                        result = result / xCoefficient;
                        break;
                }

                return result;
            }

        }
    }
}
