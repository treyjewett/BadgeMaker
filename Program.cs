using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Collections.Generic;

namespace CatWorx.BadgeMaker
{
  class Util
  {
    // Method declared as "static"
    public static void PrintEmployees(List<Employee> employees)
    {
      for (int i = 0; i < employees.Count; i++)
      {
        string template = "{0,-10}\t{1,-20}\t{2}"; // This is a shortcut formatting done to make the CLI easier to read.
        Console.WriteLine(String.Format(template, employees[i].GetId(), employees[i].GetName(), employees[i].GetPhotoUrl()));
      }
    }

    public static void MakeCSV(List<Employee> employees)
    {
      if (!Directory.Exists("data"))
      {
        Directory.CreateDirectory("data");
      }
      using (StreamWriter file = new StreamWriter("data/employees.csv"))
      {
        file.WriteLine("Id,Name,PhotoUrl");
        for (int i = 0; i < employees.Count; i++)
        {
          string template = "{0},{1},{2}";
          file.WriteLine(String.Format(template, employees[i].GetId(), employees[i].GetName(), employees[i].GetPhotoUrl()));
        }
      }
    }

    public static void MakeBadges(List<Employee> employees)
    {
        // Layout params
        int BADGE_WIDTH = 669;
        int BADGE_HEIGHT = 1044;

        int COMPANY_NAME_START_X = 0;
        int COMPANY_NAME_START_Y = 110;
        int COMPANY_NAME_WIDTH = 100;

        int PHOTO_START_X = 184;
        int PHOTO_START_Y = 215;
        int PHOTO_WIDTH = 302;
        int PHOTO_HEIGHT = 302;

        int EMPLOYEE_NAME_START_X = 0;
        int EMPLOYEE_NAME_START_Y = 560;
        int EMPLOYEE_NAME_WIDTH = BADGE_WIDTH;
        int EMPLOYEE_NAME_HEIGHT = 100;

        int EMPLOYEE_ID_START_X = 0;
        int EMPLOYEE_ID_START_Y = 690;
        int EMPLOYEE_ID_WIDTH = BADGE_WIDTH;
        int EMPLOYEE_ID_HEIGHT = 100;

        // Create Image
        Image newImage = Image.FromFile("badge.png");
        newImage.Save("data/employeeBadge.png");

        using(WebClient client = new WebClient())
        {
            for (int i = 0; i < employees.Count; i++)
            {
                // This creates an Image variable "photo" and sets it to the employee photoUrl. In order to set it correctly,
                // The photoUrl needs to be set to a Stream, then read from the stream via Image.FromStream.
                Image photo = Image.FromStream(client.OpenRead(employees[i].GetPhotoUrl()));
                Image background = Image.FromFile("badge.png");
                Image badge = new Bitmap(BADGE_WIDTH, BADGE_HEIGHT);
                Graphics graphic = Graphics.FromImage(badge);
                graphic.DrawImage(background, new Rectangle(0, 0, BADGE_WIDTH, BADGE_HEIGHT));
                // background.Save("data/employeeBadge.png");
                Console.WriteLine("Photo: " + photo.GetType());
                Console.WriteLine("Background: " + background.GetType());
            }
        }
    }
  }
  class Program
  {
    static List<Employee> GetEmployees()
    {
      // Create an empty list of strings to be filled later.
      List<Employee> employees = new List<Employee>() { };
      // Keep adding names to the list until the user inputs and empty string.
      while (true)
      {
        Console.WriteLine("Please enter first name (leave empty to exit): ");
        string firstName = Console.ReadLine();
        if (firstName == "")
        {
          break;
        }
        Console.Write("Enter last name: ");
        string lastName = Console.ReadLine();
        Console.Write("Enter ID: ");
        int id = Int32.Parse(Console.ReadLine());
        Console.Write("Enter Photo URL: ");
        string photoUrl = Console.ReadLine();
        Employee currentEmployee = new Employee(firstName, lastName, id, photoUrl);
        employees.Add(currentEmployee);
      }
      // return the list to fulfill the function.
      return employees;
    }
    static void Main(string[] args)
    {
      List<Employee> employees = GetEmployees();
      Util.PrintEmployees(employees);
      Util.MakeCSV(employees);
      Util.MakeBadges(employees);
    }
  }
}