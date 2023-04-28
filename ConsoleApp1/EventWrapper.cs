using System.Diagnostics;
using System.Numerics;
using System.Reflection;

[assembly: AssemblyDescription("Hello Assembly version here!")]
class E
{
    static void Main(string[] args)
    {
        Employee employee = new Employee();
        string empId = null;
        string name = null;
        Type type = typeof(Employee);
        if(GetInput(type,"Please Enter The Employee ID. ","Id",out empId))
        {
            employee.Id = int.Parse(empId);
        }
        if (GetInput(type, "Please Enter The Employee Name. ", "Name", out name))
        {
            employee.Name = name;
        }

        Console.WriteLine();
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"Thank You Employee with ID : {employee.Id} and Name : {employee.Name} is SuccessFully Added!");
        Console.ReadKey();

    }

    private static bool GetInput(Type type,string str,string fieldName,out string fieldValue)
    {
        fieldValue = "";
        string? errorMsg = null;
        string? enterVal = "";

        do
        {
            Console.WriteLine(str);
            enterVal = Console.ReadLine();
            if(!Validation.PropertyValueIsValid(type,enterVal,fieldName,out errorMsg))
            {
                fieldValue = null;
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(errorMsg);
                Console.WriteLine();
                Console.ResetColor();
            }
            else
            {
                fieldValue = enterVal;
                break;
            }

        } while (true);

        return true;
    }
}

class Employee
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }

}

[AttributeUsage(AttributeTargets.All,AllowMultiple =false)]
class RequiredAttribute : Attribute
{
    public string ErrorMessage { get; set; }

    public RequiredAttribute() 
    {
        ErrorMessage = "{0} is Required!";
    }

    public RequiredAttribute(string message)
    {
        ErrorMessage = message;
    }
}


public static class Validation
{
    private static bool FieldRequiredIsValid(string str)
    {
        if(!(string.IsNullOrEmpty(str)))
        {
            return true;
        }

        return false;
    }

    public static bool PropertyValueIsValid(Type type,string enterValue,string elementName,out string errorMessage)
    {
        PropertyInfo? propertyInfo = type.GetProperty(elementName);

        Attribute[] attributes = propertyInfo.GetCustomAttributes().ToArray();
        errorMessage = "";
        foreach(Attribute attribute in attributes)
        {
            switch(attribute)
            {
                case RequiredAttribute requiredAttribute:
                    if(!FieldRequiredIsValid(enterValue))
                    {
                        errorMessage = requiredAttribute.ErrorMessage;
                        errorMessage = String.Format(errorMessage,propertyInfo.Name);
                        return false;
                    }
                    break;
                default:
                    Console.WriteLine("Invalid Case !");
                    break;
            }
        }
        return true;
    }
}
