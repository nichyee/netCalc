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

    class Calc
    {
        static void Main(string[] args)
        {
            Priorities plus = new Priorities("+", 2);
            Priorities minus = new Priorities("-", 2);
            Priorities multiply = new Priorities("*", 1);
            Priorities divide = new Priorities("/", 1);
            Priorities leftBracket = new Priorities("(", 3);
            Priorities rightBracket = new Priorities(")", 3);

            Stack<Priorities> priorities = new Stack<Priorities>();
            ArrayList arrayList = new ArrayList();
            Stack<int> final = new Stack<int>();
            int leftHandResult;
            int rightHandResult;
            ArrayList operatorX = new ArrayList();
            int leftOrRight = 1;
            string opBeforeStatement = "+";
            ArrayList parenthesis = new ArrayList();
            int parenthesisSide = 3;
            int parenthesisIndex = 0;
            string userInput = "";

            try
            {
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

                var parenthesisHandle = sortOutParenthesis(leftHandSide, rightHandSide);
                leftHandSide = parenthesisHandle.Item1;
                rightHandSide = parenthesisHandle.Item2;

                leftHandResult = solveString(leftHandSide);
                if (operatorX.Count > 0)
                {
                    leftOrRight = 0;
                }

                rightHandResult = solveString(rightHandSide);


                int result = 0;
                if (rightHandResult != 0 && leftHandResult != 0)
                {
                    result = resolveX(leftHandResult, rightHandResult);
                }
                else if (parenthesisSide == 0)
                {
                    result = rightHandResult;
                }
                else if (parenthesisSide == 1)
                {
                    result = leftHandResult;
                }
                else
                {
                    result = resolveX(leftHandResult, rightHandResult);
                }

                if (parenthesis.Count > 0)
                {
                    string[] parenthesisStringArray = (string[])parenthesis.ToArray(typeof(string));
                    int parenthesisResult = solveString(parenthesisStringArray);
                    result = resolveX(result, parenthesisResult);
                }

                Console.WriteLine("X = {0}", result);
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

                        case ("("):
                            operatorStack.Push(leftBracket);
                            break;
                        case (")"):
                            do
                            {
                                outputArray.Add(operatorStack.Pop().getOperation());
                            } while (operatorStack.Pop() != leftBracket);
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
                        localArrayList.Reverse();
                        localArrayList.Add("0");
                        localArrayList.Reverse();
                    }

                }

                return (string[])localArrayList.ToArray(typeof(string));
            }

            (bool, int, int, bool, int) checkParenthesis(string[] statement)
            {
                bool brackets = false;
                bool hasX = false;
                int leftBracketIndex = 0;
                int rightBracketIndex = 0;
                int xIndex = 0;
                for (int i = 0; i < statement.Length; i++)
                {
                    if (statement[i].ToString().Contains("("))
                    {
                        leftBracketIndex = i;
                        brackets = true;
                    }
                    else if (statement[i].ToString().Contains(")"))
                    {
                        rightBracketIndex = i;
                    }
                    else if (statement[i].ToString().Contains("X"))
                    {
                        hasX = true;
                        xIndex = i;
                    }
                }
                int index = leftBracketIndex + 1;
                ArrayList temp = new ArrayList(statement);

                while (index < rightBracketIndex)
                {
                    parenthesis.Add(statement[index]);
                    index++;
                }

                return (hasX, leftBracketIndex, (rightBracketIndex - leftBracketIndex) + 1, brackets, xIndex);
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

            int solveString(string[] toSolve)
            {
                toSolve = removeX(toSolve);
                priorities = shuntingYard(toSolve).Item1;
                arrayList = shuntingYard(toSolve).Item2;
                arrayList = combineStacks(priorities, arrayList);
                final = evaluateStatement(arrayList);

                return final.Pop();
            }

            (string[], string[]) sortOutParenthesis(string[] firstStringArray, string[] secondStringArray)
            {
                ArrayList tempLeft = new ArrayList(firstStringArray);
                ArrayList tempRight = new ArrayList(secondStringArray);
                var parenthesisCheckLeft = checkParenthesis(firstStringArray);
                var parenthesisCheckRight = checkParenthesis(secondStringArray);


                if (parenthesisCheckLeft.Item4)
                {
                    parenthesisIndex = parenthesisCheckLeft.Item2;
                    parenthesisSide = 0;
                }
                else if (parenthesisCheckRight.Item4)
                {
                    parenthesisIndex = parenthesisCheckRight.Item2;
                    parenthesisSide = 1;
                }

                if (parenthesisCheckLeft.Item1 && parenthesisSide == 0)
                {
                    string[] parenthesisArray = (string[])parenthesis.ToArray(typeof(string));
                    tempLeft = new ArrayList(firstStringArray);
                    tempLeft.Insert(parenthesisIndex, firstStringArray[parenthesisCheckLeft.Item5]);
                    tempLeft.RemoveRange(parenthesisIndex + 1, parenthesisCheckLeft.Item3);
                    firstStringArray = (string[])tempLeft.ToArray(typeof(string));
                }
                else if (parenthesisCheckRight.Item1 && parenthesisSide == 1)
                {
                    string[] parenthesisArray = (string[])parenthesis.ToArray(typeof(string));
                    tempRight = new ArrayList(secondStringArray);
                    tempRight.Insert(parenthesisIndex, secondStringArray[parenthesisCheckRight.Item5]);
                    tempRight.RemoveRange(parenthesisIndex + 1, parenthesisCheckRight.Item3);
                    secondStringArray = (string[])tempRight.ToArray(typeof(string));
                }

                return (firstStringArray, secondStringArray);
            }
        }
    }
}
