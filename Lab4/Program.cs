using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {

            Statistics();
            Coordinates();
            SumOfSquares();
        }

        #region //Statistics
        static void Statistics() 
        {
            int[] values = { 1, 6, 4, 7, 9, 2, 5, 7, 2, 6, 5, 7, 8, 1, 2, 8 };

            Mean(values);
            Median(values);
            Mode(values);
                
        }
        #endregion //End of Statistics

        #region //Coordinates
        static void Coordinates() 
        {
            Console.Write("\nEnter a coordinate in the form (x, y): ");
            string input = Console.ReadLine();
            double x, y;
            if (TryParsePoint(input, out x, out y)) 
            {
                Console.WriteLine($"x: {x}");
                Console.WriteLine($"y: {y}");

                var polar = RectangularToPolar(x, y);
                Console.WriteLine($"r: {polar.Item1}, angle: {polar.Item2} radians");               
                Console.WriteLine($"r: {polar.Item1}, angle: {RadiansToDegrees(polar.Item2)}");
            }
        }
        #endregion //End of Coordinates

        #region//SumOfSquares
        static void SumOfSquares() 
        {
            long sum;
            Console.Write("Enter an int to calculate it's 'Sum of Squares': ");
            string input = Console.ReadLine();

            int.TryParse(input, out int i);     
            sum = Squares(i);
            Console.WriteLine($"Sum: {sum}");
            Console.WriteLine($"fin");

        }

        static long Squares(long value) 
        {
            if (value == 1)
            {
                return 1;
            }

            /*Each time this recursive method is called this is what happens:
            if ' i = 5' or in other words, if the user inputs '5'
            The first iteration: will execute (5 * 5) and produces 25; 25 gets returned into 'sum' and the value of 'sum' is now '25'; next iteration is executed
            The second iteration: will execute (4 * 4) and produce 16; 16 gets returned into 'sum' and the value of 'sum' is now '25 + 16'; next iteration is executed
            The third iteration: will execute (3 * 3) and produce 9; 9 gets returned into 'sum' and the value of 'sum' is now '25 + 16 + 9'; next iteration is executed
            The fourth iteration: will execute (2 * 2) and produce 4; 4 gets returned into 'sum' and the value of 'sum' is now '25 + 16 + 9 + 4'; next iteration is executed
            The fifth iteration: will execute (1 * 1) and produce 9; 9 gets returned into 'sum' and the value of 'sum' is now '25 + 16 + 9 + 4 + 1'; next iteration is executed
             */
            return (value * value) + Squares(value - 1);

        }
        #endregion //End of SumOfSquares

        #region//Methods for Statistics
        #region //Mean
        static double Mean( int[] values) 
        {
            double? mean = null;
            double sum = 0D;

            //If values is 'Null' or 'Empty' return 'double.NaN'
            //Int[arrays] can be set to 'default' which 
            if (values == null|| values.Length == 0) 
            { 
                //Return 'null'
                mean = double.NaN;
                return (double)mean;
            }

            foreach (int val in values) 
            {
                sum += val;
            }        
            //Calculate mean
            mean = (sum / values.Length);

            //Rertuern 'mean' ; Only pass in 'mean' to printResult()
            printResult(mean: (double)mean);
            return (double)mean;
          

        }
        #endregion//End of Mean

        #region //Median
        static double Median(int[] values) 
        {
            double? median = null;

            //If values is 'Null' or 'Empty' return 'double.NaN'
            //Int[arrays] can be set to 'default' which 
            if (values == null || values.Length == 0)
            {
                //Return 'null'
                median = double.NaN;
                return (double)median;
            }

            int[] sorted = new int[values.Length];
            values.CopyTo(sorted,0);
            Array.Sort(sorted);

            int num1 = default;
            int num2 = default;
            int half = default;

            //If 'sorted' contains an 'even' number of items:
            if (sorted.Length % 2 == 0)
            {
                half = sorted.Length / 2;
                num1 = sorted[half];
                num2 = sorted[half - 1];
                median = ((double)num1 + (double)num2) / 2;
            }
            else //If 'sorted' contains an 'odd' number of items:
            {
                num1 = sorted.Length - 1;
                num2 = num1 / 2;
                median = sorted[num2];
            }

            //Only pass in 'mean' to printResult()
            printResult(median: (double)median);
            return (double)median;
        }
        #endregion//End of Median

        #region //Mode
        static List<int?> Mode(int[] values)
        {

            //int?[] mode = default;
            //int arrSize = default;
            List<int?> result = new List<int?>();

            //If values is 'Null' or 'Empty' return 'double.NaN'
            //Int[arrays] can be set to 'default' which 
            if (values == null || values.Length == 0)
            {
                //Set 'result' to an empty List and return 'resultClear'
                List<int?> resultClear = new List<int?>();
                return resultClear;
            }

            //Make sorted-copy of values & initialize 'max'
            int[] sorted = new int[values.Length];
            values.CopyTo(sorted, 0);
            Array.Sort(sorted);
            int index = 0;
            int value;
            int max = 0;


            //Initialize'counts'
            var counts = new Dictionary<int, int>();

            foreach (int key in sorted)
            {
                int count = 1;
                if (!counts.ContainsKey(key))
                {
                    counts.Add(key, index);
                    counts[key] = count;
                    index++;
                }
                else if (counts.ContainsKey(key))
                {
                    value = counts[key];
                    counts[key] = value + 1;
                    count = counts[key];

                    if (count > max)
                    {
                        max = count;
                    }
                }
            }

            //Loop through all 
                foreach (var key in counts.Keys)
                {
                    if (counts[key] == max)
                    {
                        result.Add(key);
                        continue;
                    }
                }
           
            printResult(mode: result);
            return result;
        }
        #endregion//End of Mode

        //
        static void printResult(double? mean = null, double? median = null, List<int?> mode = null) 
        {
            //If the name-arg is not null, then print a message and the value of the named arg
            if ( mean != null) 
            {
                Console.WriteLine($"Mean: {mean}");
            }
            else if (median != null)
            {
                Console.WriteLine($"Median: {median}");
            }
            else if (mode != null)
            {
                //Create an 'index' which will be used as a 'stop adding commas' point
                int index = 0;
                int lastVal = mode.Count - 1;
                Console.Write($"Mode: ");

                foreach (int val in mode) 
                {
                    //Print  all values in mode[] EXCEPT for the last two value in mode[]
                    if( index != lastVal)
                    {
                        Console.Write($"{val}, ");
                        index++;
                        continue;
                    }
                    //Print out the last value in mode[]
                    if (index == lastVal ) 
                    { 
                        Console.Write($"{val}");   
                    }     
                }
            }

        }
        #endregion //End of Methods for Statistics


        #region //TryParsePoint()
        static bool TryParsePoint(string input, out double x, out double y)
        {
            //Could it set the values of 'x' and 'y' inside the arguments '()' in hte method signature?
            //ph 250 in textbook for example
            x = 0;
            y = 0;
            bool isXNum = false;
            bool isYNum = false;

            input.Trim();

            int first = input.IndexOf("(");
            int mid = input.IndexOf(",");
            int last = input.LastIndexOf(")");
            //The 'if' condition below translates to:
            //IF: the index of '(' is less than the index ','
            //AND the index of ',' is less than the index of ')'
            if ((first < mid && mid < last) == true)
            {
                int len = input.Length;

                char[] separator = { ',' };
                String[] strList = input.Split(separator, 2, System.StringSplitOptions.RemoveEmptyEntries);


                if (strList.Length == 2)
                {
                    string unformattedPointX = String.Copy(strList[0]);
                    string unformattedPointY = String.Copy(strList[1]);

                    char[] charsToTrim = { '(', ')' };

                    //Remove the '(' and ')' from the substrings 'unformattedPointX' and 'unformattedPointY'
                    string pointX = unformattedPointX.Trim(charsToTrim);
                    string pointY = unformattedPointY.Trim(charsToTrim);

                    isXNum = double.TryParse(pointX, out x);
                    isYNum = double.TryParse(pointY, out y);
                    //If 'x' and 'y' are numbers, return 'true'
                    if(isXNum && isYNum)  
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            } 
            return false; 

        }
        #endregion //End of TryParsePoint()

        #region //RectangularToPolar()
        static Tuple<double, double> RectangularToPolar(double x, double y) 
        {
            double r = default;
            double a = default;
            //Scenarios #1 - #5:
            //If either 'x' or 'y' is '0'
            if (x == 0 || y == 0)
            {
                //Scenario 1:
                //If coordinate is: (0, 0)
                if (x == 0 && y == 0)
                {
                    r = 0;
                    a = 0;
                    goto sendPolar;
                }
                //If x-coordinate is: 0
                else if (x == 0)
                {
                    //Scenario 3:
                    //If y-coordinate is: positive int
                    if (y > 0) 
                    {
                        r = y;
                        a = Math.PI / (double)2;
                        goto sendPolar;
                    }
                    //Scenario 5:
                    //OR y-coordinate is: negative int
                    else if (y < 0 ) 
                    {
                        r = Math.Abs(y);
                        a = (Math.PI * (double)3) / (double)2;
                        goto sendPolar;
                    }
                }
                //Scenario 2:
                //If x-coordinate is: positive int and y-coordinate is: 0
                else if (x > 0 && y == 0 ) 
                {
                    r = x;
                    a = 0;
                    goto sendPolar;
                }
                //Scenario 4:
                //If x-coordinate is: negative int and y-coordinate is: 0
                else if (x < 0 && y == 0)
                {
                    r = Math.Abs(x);
                    a = Math.PI;
                    goto sendPolar;
                }
            }
            //Scenarios #6 & #9:
            //If x-coordinate is: positive int
            else if (x > 0)
            {
                //Scenario 6:
                //If y-coordinate is: positive int
                if (y > 0)
                {
                    r = Math.Sqrt((x * x) + ( y * y));
                    double quotient = (double)y / (double)x; 
                    a = Math.Atan(quotient);
                    goto sendPolar;
                }
                //Scenario 9:
                //OR y-coordinate is: negative int
                else if (y < 0)
                {
                    r = Math.Sqrt((x * x) + (y * y));
                    double quotient = (double)y / (double)x;
                    a = (Math.PI * 2) + Math.Atan(quotient);
                    goto sendPolar;
                }
            }
            //Scenarios #7 & #8:
            //If x-coordinate is: negative int
            else if (x < 0 ) 
            {
                //Scenario 7:
                //If y-coordinate is: positive int
                if (y > 0)
                {
                    r = Math.Sqrt((x * x) + (y * y));
                    double quotient = (double)y / (double)x;
                    a = Math.PI + Math.Atan(quotient);
                    goto sendPolar;
                }
                //Scenario 8:
                //OR y-coordinate is: negative int
                else if (y < 0)
                {
                    r = Math.Sqrt((x * x) + (y * y));
                    double quotient = (double)y / (double)x;
                    a = Math.PI + Math.Atan(quotient);
                    goto sendPolar;
                }
            }
            sendPolar:
            var t = new Tuple<double, double>(r, a);
            return t;

        }
        #endregion //End of TryParsePoint()

        #region //RadiansToDegrees()
        static double RadiansToDegrees(double radians) 
        {
            double degrees = default;
            degrees = radians * ((double)180 / Math.PI);
            if (degrees < 0 ) 
            {
                do
                {
                    degrees += 360;
                } while (degrees < 0);

                if (degrees >= 0)
                    return degrees;
            }

            if (degrees > 360)
            {
                do
                {
                    degrees -= 360;
                } while (degrees > 360);

                if (degrees <= 360)
                    return degrees;
            }
            
            return degrees;
        }
        #endregion //End of RadiansToDegrees()

    }
}
