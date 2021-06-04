using System;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CatWorx.BadgeMaker
{
  class PeopleFetcher
  {
    // This is where we will call the randomuser API to populate the badges.
    public static List<Employee> GetEmployees()
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
    public static List<Employee> GetFromAPI()
    {
      // Create API call here
      // create a new employee from every result that is returned from the API call.
      List<Employee> employees = new List<Employee>() { };
      using (WebClient client = new WebClient())
      {
        string response = client.DownloadString("https://randomuser.me/api/?results=10&nat=us&inc=name,id,picture");
        JObject json = JObject.Parse(response);
        // Console.WriteLine(response);
        // loop through the results so we end up with the 10 employees that we originally wanted.
        foreach (JToken token in json.SelectToken("results"))
        {
          Employee emp = new Employee
          (
            token.SelectToken("name.first").ToString(),
            token.SelectToken("name.last").ToString(),
            Int32.Parse(token.SelectToken("id.value").ToString().Replace("-", "")),
            token.SelectToken("picture.large").ToString()
          );
          Console.WriteLine(emp);
          employees.Add(emp);
        }
      }
      return employees;
    }
  }
}