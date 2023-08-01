//Nicole Morrison
//Week 5 Worker-Manager Salary Calculate
//July 6, 2023
using System;
using System.IO;
using System.Collections.Generic;

//Create interface for SalaryCalculate
interface ISalaryCalculate
{
    void CalcYearWorked(int currentYear);
    void CalcCurSalary();
}

//Create SalaryCalculate class that inherits ISalaryCalculate interface
class SalaryCalculate : ISalaryCalculate
{
    //Delcare variables needed
    public int nYearWked { get; set; }
    public double curSalary { get; set; }
    public int yearStartedWked { get; set; }
    public int initSalary { get; set; }

    //Function to calculate years worked
    public void CalcYearWorked(int currentYear)
    {
        nYearWked = currentYear - yearStartedWked;
    }

    //function to calculate salary for workers
    public void CalcCurSalary()
    {
        curSalary = initSalary;
        for (int i = 0; i < nYearWked; i++)
        {
            curSalary *= 1.03;
            curSalary = Math.Round(curSalary, 2);
        }
    }
}

//Create Employee class
class Employee
{
    //Declare public variables for class
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string workID { get; set; }

    public Employee(string firstName, string lastName, string workID)
    {
        this.firstName = firstName;
        this.lastName = lastName;
        this.workID = workID;
    }
}

//Create Worker class that inherits Employy class and ISalaryCalculate
class Worker : Employee, ISalaryCalculate
{
    //Delcare variables needed
    public int nYearWked { get; set; }
    public double curSalary { get; set; }
    public int yearStartedWked { get; set; }
    public int initSalary { get; set; }

    public Worker(string firstName, string lastName, string workID, int yearStartedWked, int initSalary) : base(firstName, lastName, workID)
    {
        this.yearStartedWked = yearStartedWked;
        this.initSalary = initSalary;
    }

    public void CalcYearWorked(int currentYear)
    {
        nYearWked = currentYear - yearStartedWked;
    }

    public virtual void CalcCurSalary()
    {
        curSalary = initSalary;
        for (int i = 0; i < nYearWked; i++)
        {
            curSalary *= 1.03;
            //round to 2 decimals
            curSalary = Math.Round(curSalary, 2);
        }
    }

    //Import worker.txt and call functions to calculate then add to array
    public static List<Worker> ImportWorker(int currentYear)
    {
        List<Worker> workers = new List<Worker>();
        string[] lines = File.ReadAllLines("worker.txt");
        int numWorkers = int.Parse(lines[0]);
        for (int i = 0; i < numWorkers; i++)
        {
            Worker worker = new Worker(lines[i * 5 + 1], lines[i * 5 + 2], lines[i * 5 + 3], int.Parse(lines[i * 5 + 4]), int.Parse(lines[i * 5 + 5]));
            worker.CalcYearWorked(currentYear);
            worker.CalcCurSalary();
            workers.Add(worker);
        }
        return workers;
    }

    //Overload ToString so that console displays title for each field
    public override string ToString()
    {
        return $"First Name: {firstName}, Last Name: {lastName}, Years Worked: {nYearWked}, Current Salary: ${curSalary}";
    }
}

//Create Manager class that inherits Worker Class
class Manager : Worker
{
    private double wrkSalary { get; set; }
    private double mngSalary { get; set; }
    public int yearPromo { get; set; }
    public int nYearMngr { get; set; }
    public int nYearWrkr { get; set; }

    public Manager(string firstName, string lastName, string workID, int yearStartedWked, int initSalary, int yearPromo) : base(firstName, lastName, workID, yearStartedWked, initSalary)
    {
        this.yearPromo = yearPromo;
    }

    //function to calculate years as manager
    public void CalcYearManager(int currentYear)
    {
        nYearMngr = currentYear - yearPromo;
    }
    //function to calculate years as worker
    public void CalcYearWrkr()
    {
        nYearWrkr = nYearWked - nYearMngr;
    }
    //Override the CalcCurSalary funcdtion from the base class to do different calculation for managers
    public override void CalcCurSalary()
    {
        //Could do by using curSalary from start to finish, but wrkSalary, then mngSalary then curSlary 
        //makes it easier for someone else in the future to understand what is happening step by step
        wrkSalary = initSalary;
        //first calculate salary for years as worker at the 3% per year increase
        for (int i = 0; i < nYearWrkr; i++)
        {
            wrkSalary *= 1.03;
        }
        mngSalary = wrkSalary;
        //then increase the result by 5% per year for each year being a manager
        for (int i = 0; i < nYearMngr; i++)
        {
            mngSalary *= 1.05;
        }
        //then add a 10% bonus and round to 2 decimals
        curSalary = Math.Round(mngSalary * 1.1, 2);
    }
    //Import manager.txt, calculate, then save to array.
    public static List<Manager> ImportManager(int currentYear)
    {
        List<Manager> managers = new List<Manager>();
        string[] lines = File.ReadAllLines("manager.txt");
        int numManagers = int.Parse(lines[0]);
        for (int i = 0; i < numManagers; i++)
        {
            Manager manager = new Manager(lines[i * 6 + 1], lines[i * 6 + 2], lines[i * 6 + 3], int.Parse(lines[i * 6 + 4]), int.Parse(lines[i * 6 + 5]), int.Parse(lines[i * 6 + 6]));
            manager.CalcYearWorked(currentYear);
            manager.CalcYearManager(currentYear);
            manager.CalcYearWrkr();
            manager.CalcCurSalary();
            managers.Add(manager);
        }
        return managers;
    }
    //Override the string function to put title for each field.
    //Also added years manager and years worker to the end so it is easier to check the math calculations if needed
    public override string ToString()
    {
        return $"First Name: {firstName}, Last Name: {lastName}, Years Worked: {nYearWked}, Current Salary: ${curSalary}, Years Worker: {nYearWrkr}, Years Manager: {nYearMngr}";
    }
}

//Main program file displays menu then either prints out arrays or exits program.
class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("1. Import and Calculate Salaries");
            Console.WriteLine("2. Exit Program");
            string input = Console.ReadLine();
            if (input == "1")
            {
                int currentYear;
                while (true)
                {
                    Console.Write("Enter the current year: ");
                    input = Console.ReadLine();
                    if (int.TryParse(input, out currentYear) && currentYear >= 1900 && currentYear <= 2200)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a number between 1900 and 2200.");
                    }
                }
                List<Worker> workers = Worker.ImportWorker(currentYear);

                Console.WriteLine(" ");
                Console.WriteLine("-----Workers Salaries-----");
                foreach (Worker worker in workers)
                {
                    Console.WriteLine(worker);
                }

                List<Manager> managers = Manager.ImportManager(currentYear);

                Console.WriteLine(" ");
                Console.WriteLine("-----Managers Salaries-----");
                foreach (Manager manager in managers)
                {
                    Console.WriteLine(manager);
                }

                Console.WriteLine(" ");
            }
            else if (input == "2")
            {
                break;
            }
        }
    }
}