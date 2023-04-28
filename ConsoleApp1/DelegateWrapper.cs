using InParameterWithReferenceType;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    
    class AuthAttribute : Attribute
    {
        public string? Role;
        public AuthAttribute(string role)
        {
            Role = role;
        }
    }

    public class Demo
    {

            
        public string role { get; set; }

        [Auth("Admin")]
        public void Admin()
        {
            Console.WriteLine("Admin here!");
        }

        [Auth("User")]
        public void User()
        {
            Console.WriteLine("User here!");
        }
    }
    public static class Authorization
    {
        public static string getRole(this Demo demo)
        {
            var type = typeof(Demo);
            MethodInfo[] methodInfo = type.GetMethods();
            for(int i=0; i< methodInfo.GetLength(0); i++)
            {
                object[] attr = methodInfo[i].GetCustomAttributes(true);
                foreach(Attribute attribute in attr)
                {
                    if(attribute is AuthAttribute)
                    {
                        AuthAttribute authAttribute = (AuthAttribute)attribute;
                        return authAttribute.Role;
                    }
                }
            }

            return null;

        }
    }
    interface A
    {
        string printString();
    }
    internal class D<T> : A where T : A
    {
        

        public string printString()
        {
            return "Hello";
        }
    }

    internal class DelegateWrapper
    {
        
        public static void Main(string[] args)
        {
            D<A> a = new D<A>();
            Console.WriteLine(a.printString());

            Demo demo = new Demo() { role = "User"};
            if(demo.role == demo.getRole())
            {
                demo.Admin();
            }
            else
            {
                demo.User();
            }
        }

        
    }
}
