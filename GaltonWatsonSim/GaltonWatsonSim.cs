using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;
using System.Windows;

public class TNode
{
    public int ID;
    public static int ID_counter = 0;
    public List<TNode> ChildNodes { get; set; }
    public TNode()
    {
        ChildNodes = new List<TNode>();
        ID = ID_counter;
        ID_counter++;
    }
    public override string ToString()
    {
        return "" + ID;
    }
    public void AddChild()
    {
        ChildNodes.Add(new TNode());
    }
}
public class GaltonWatsonSim {
    // This is the collection of probabilities for having 0 children (index 0), 1 child (index 1), etc.
    static List<double> ChildOdds = new List<double>();

    //Max no. of calls to prevent stack overflow errors
    public const int MaxCallCount = 100;
    static int callCtr = 0;

    // This is the random number generator, used for generating random values.
    static Random RNG = new Random();
    public static void Main(string[] args)
    {
        Console.Title = "Galton Watson SIM";

        string surname = "Galton";
        TNode familyTree = new TNode();
        bool errorOccurred = false;
        bool isRunning = true;
        string input = "";

        Console.WriteLine("\"Galton Watson Sim\" by John Blake");
        Console.WriteLine("Instructor Dr. Glenn Young");
        Console.WriteLine("MATH3262 Sec51 Spring 2023 Kennesaw State University");
        Console.WriteLine("This program was made as part of a math modeling project. \nIt falls under CC-BY copyright: see https://creativecommons.org/about/cclicenses/");
        Console.WriteLine("\nTo select an option from the menu, type your choice as a number and press Enter.");
        Console.WriteLine("For help, type ? then press Enter.");

        while (isRunning)
        {
            Console.WriteLine();
            PrintMenu();
            input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    ReadInput();
                    PrintChildOdds();
                    break;
                case "2":
                    errorOccurred = false;
                    //Are these actually probabilities (and do they sum to 1)?
                    try
                    {
                        CheckOddsAreValid();
                    }
                    catch (Exception ex)
                    {
                        if (ChildOdds.Count == 0)
                        {
                            Console.WriteLine("The probabilities must be assigned first.");
                        }
                        else if (ChildOdds.Sum(x => x) < 0.99 || ChildOdds.Sum(x => x) > 1.01)
                        {
                            Console.WriteLine("The probabilities entered do not sum to 1, try entering them in again.");
                        }
                        else
                        {
                            Console.WriteLine("Error occurred:");
                            Console.WriteLine(ex.Message);
                        }
                        errorOccurred = true;
                    }

                    // If they are valid (no errors occurred), start generating the tree
                    if (!errorOccurred)
                    {
                        int temp = -1;
                        try
                        {
                            Console.WriteLine("Beginning tree generating process...");
                            familyTree = new TNode();
                            temp = CreateChildren(familyTree);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error occurred");
                            Console.WriteLine(e.Message);
                        }
                        if (temp == -1)
                        {
                            Console.WriteLine("The " + surname + " family tree grew too large to be displayed properly.");
                            if (GetExpectedChildren() > 1.0)
                            {
                                Console.WriteLine("Because the expected value " + Math.Round(GetExpectedChildren(), 3) + " for the num. of children an individual has is > 1, the " + surname + " name persists.");
                            }
                            
                        }
                        else
                        {
                            Console.WriteLine("Tree generating complete!");

                            PrintTree(familyTree, 0);
                            Console.WriteLine("A total of " + (TNode.ID_counter-1) + " individuals were in the " + surname + " family tree.");
                            TNode.ID_counter = 1;
                        }
                        callCtr = 0;
                    }
                    break;
                case "3":
                    NameMeSmth();
                    break;
                case "4":
                    Console.WriteLine("Enter your desired surname: ");
                    surname = Console.ReadLine();
                    Console.WriteLine("Your surname is now \"" + surname + "\".");
                    break;
                case "9":
                    Console.Clear();
                    break;
                case "?":
                    PrintHelpInfo();
                    break;
                case "0":
                    isRunning = false;
                    Console.WriteLine("Exiting program - press any key followed by Enter to close the program.");
                    Console.ReadLine();
                    Console.WriteLine("Thank you and have a nice day!");
                    break;
                default:
                    Console.WriteLine("Unknown input, please try again.");
                    break;
            }
        }
        return;
    }
    public static double GetExpectedChildren()
    {
        double sum = 0.0;
        for (int i = 0; i < ChildOdds.Count; i++)
        {
            sum = sum + ( i * ChildOdds[i] );
        }
        return sum;
    }
    public static void PrintHelpInfo()
    {
        Console.WriteLine("" +
            "Set probability values: Enter probability values (between 0 and 1, with 0 being \"impossible\") for any individual having 0, 1, 2, ... n children.\n" +
            "Run one simulation: Simulate the Galton-Watson process using the assigned probability values. The output on screen is the family tree, with each individual having a unique ID.\n" +
            "Set color: Change the text color of the text on screen.\n" +
            "Set name: Change the surname of the individual (this is only cosmetic).\n" +
            "Clear screen: Wipes the screen clean of text. \n" +
            "Exit program: Closes the program. Clicking the X in the top right corner has the same effect.");

    }
    public static void NameMeSmth()
    {
        Console.WriteLine("Enter a color choice, options: ");
        ConsoleColor currBg = Console.BackgroundColor;
        ConsoleColor currFg = Console.ForegroundColor;

        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(" R ");
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(" B ");
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(" G ");
        Console.BackgroundColor = ConsoleColor.DarkYellow;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(" Y ");
        Console.BackgroundColor = ConsoleColor.DarkMagenta;
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write(" M ");
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(" W ");

        Console.BackgroundColor = currBg;
        Console.ForegroundColor = currFg;
        Console.Write("\n> ");
        string input = Console.ReadLine();
        SetConsoleColor(input);
        //set color?
    }
    
    public static void SetConsoleColor(string dtk)
    {
        if (dtk.Length == 0)
        {
            return;
        }
        dtk = dtk.ToLower();
        switch(dtk)
        {
            case "r":

                Console.ForegroundColor = ConsoleColor.Red;
                break;
            case "g":
      
                Console.ForegroundColor = ConsoleColor.Green;
                break;
            case "b":
               
                Console.ForegroundColor = ConsoleColor.Blue;
                break;
            case "y":
               
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;
            case "m":
             
                Console.ForegroundColor = ConsoleColor.Magenta;
                break;
            case "w":
              
                Console.ForegroundColor = ConsoleColor.White;
                break;
            default:
                Console.WriteLine("Unknown color. Please try again.");
                break;
        }
    }
    public static void CheckOddsAreValid()
    {
        double sum = 0;
        foreach (double odds in ChildOdds)
        {
            if (odds < 0 || odds > 1)
            {
                throw new Exception("A probability value cannot be less than 0 or greater than 1.");
            }
            else
            {
                sum += odds;
            }
        }
        if (sum < 0.99 || sum > 1.01)
        {
            throw new Exception("Probabilities do not sum to 1.");
        }
    }
    // Generates a return value of how many child nodes a node should have, using the list of ChildOdds
    public static int GetNumChilds()
    {
        double sum = 0.0;
        double randVal = RNG.NextDouble();  //This value generated by RNG is always between 0 (including 0) and 1.
        for (int i = 0; i < ChildOdds.Count; i++)
        {
            sum = sum + ChildOdds[i];
            if (randVal < sum)
            {
                return i;
            }
        }
        return -1;
    }
    //Adjust createchildren to generate nodes breadth first (pass an array of all the child nodes in a generation as paramenter)
    // and also avoid stackoverflow
    public static int CreateChildren(TNode head)
    {
        int status; //0 is normal, -1 is error
        callCtr++;
        int x = GetNumChilds();
        for (int i = 0; i < x; i++)
        {
            head.AddChild();
        }
        foreach (TNode node in head.ChildNodes)
        {
            if (callCtr < MaxCallCount)
            {
                status = CreateChildren(node);
                if (status == -1)
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }
        
        callCtr--;
        return 0;
    }
    public static void PrintChildOdds()
    {
        Console.Write("Probabilities: ");
        for (int i = 0; i < ChildOdds.Count; i++)
        {
            Console.Write("P_" + i + " = " + ChildOdds[i] + ", ");
            
        }
        Console.WriteLine("with expected value = " + Math.Round(GetExpectedChildren(), 3) + " children");
    }
    public static void PrintTree(TNode head, int indent)
    {
        for (int i = 0; i < indent - 1; i++)
        {
            Console.Write(" ");
        }
        if (indent != 0) //Special char to put next to a child node printed out, e.g. DOWN RIGHT arrow
        {
            Console.Write(" ");
        }
        Console.WriteLine("(" + head.ID + ")");
        foreach (TNode node in head.ChildNodes)
        {
            PrintTree(node, indent + 1);
        }
    }
    public static void PrintMenu()
    {
        Console.WriteLine("----------Program Menu----------");
        Console.WriteLine("1 - Set probability values");
        Console.WriteLine("2 - Run one simulation");
        Console.WriteLine("3 - Set color");
        Console.WriteLine("4 - Set name");
        Console.WriteLine("9 - Clear console");
        Console.WriteLine("0 - Exit program");
        Console.Write("> ");
    }
    public static void ReadInput()
    {
        ChildOdds.Clear();
        Console.WriteLine("\nStart entering probability values for an individual having some number of children.");
        Console.WriteLine("Enter each Decimal value in one at a time as prompted, pressing Enter key each time to submit input.");
        Console.WriteLine("To stop entering values, type this then press Enter: d");
        string input = "";
        double temp = 0.0;
        int count = 0;
        bool errorOccurred = false;
        while (!input.Contains("d"))
        {
            Console.Write("P_" + count + " = ");
            input = Console.ReadLine();
            if (!input.Contains("d"))
            {
                
                try
                {
                    temp = Double.Parse(input);
                    errorOccurred = false;
                }
                catch (FormatException ex)
                {

                    Console.WriteLine("Invalid value, please try again.");
                    errorOccurred = true;
                }
                if (temp < 0.0 || temp > 1.0)
                {
                    Console.WriteLine("Value must be between 0 and 1 inclusive, please try again.");
                    errorOccurred = true;
                }
                else
                {
                    ChildOdds.Add(temp);
                }
            }
            if (!errorOccurred)
            {
                count++;
            }
        }
    }
            
}