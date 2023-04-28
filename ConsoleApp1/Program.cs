using AutoMapper;
using Jil.Calculator.Calculation;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace InParameterWithReferenceType
{
    public partial class Subject
    {
        public void Math() { Console.WriteLine("Math"); }
        public void Science() { Console.WriteLine("Science"); }
    }

    public static class NewSubject
    {
        public static void Physic(this Subject sub)
        {
            Console.WriteLine("Physic");
        }
    }

    public enum Country
    {
        [CustomCountry("IND")]
        India,
        [CustomCountry("US")]
        UnitedState,
        [CustomCountry("JNP")]
        Japan
    }

    public enum Sub
    {

        [Description("MATH")]
        Mathematic,
        [Description("PHY")]
        Physic,
        [Description("CHEM")]
        Chemistry,
        Science = Mathematic | Physic
    }

    public class CustomCountry : Attribute
    {
        [NoMap("Set on method")]
        public string Info { get; set; }

        
        public void m()
        {

        }

        public string Country { get; set; }
        public CustomCountry(string info)
        {
            Info = info;
        }
    }

    public static class EnumInfo
    {
        public static string getShortSubject(this Enum val)
        {
            DescriptionAttribute[] info = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return info.Length > 0 ? info[0].Description : String.Empty;
        }
    }

    public static class EnumHelper
    {
        public static string GetEnumInfo(this Enum country)
        {
            Type enumType = country.GetType();
            var enumField = enumType.GetField(country.ToString());
            var enumInfo = (CustomCountry[])enumField.GetCustomAttributes(typeof(CustomCountry), false);
            var result = enumInfo.Length > 0 ? enumInfo[0].Info : String.Empty;
            return result;

        }
    }

    class OddNumberDivisionException : Exception
    {
        public OddNumberDivisionException()
        {

        }
        public OddNumberDivisionException(string message) : base(message) { }
    }

    class GenericDemo<T> where T : class
    {
        public T GetValue(T obj)
        {
            return obj;
        }
    }

    public class NoMapAttribute : Attribute
    {
        public string val { get; set; }
        public NoMapAttribute(string s)
        {
            this.val = s;
        }
    }
    class Map1
    {
        public string FName { get; set; }

        public string LName { get; set; }
    }

    class MaxNameLength : ValidationAttribute
    {
        private readonly int len;
        public MaxNameLength(int val) : base()
        {
            len = val;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            int size = value.ToString().Length;
            if (size > len)
            {
                return new ValidationResult("Error !!!");
            }

            return ValidationResult.Success;
        }
    }
    class Map2
    {
        [MaxNameLength(8)]
        public string FName { get; set; }

        public string LName { get; set; }
    }

    public static class IngnorAutoMapper
    {
        public static IMappingExpression<T1, T2> IgnorMap<T1, T2>(this IMappingExpression<T1, T2> exp)
        {
            var type = typeof(T1);

            foreach (var p in type.GetProperties())
            {
                PropertyDescriptor? descriptor = TypeDescriptor.GetProperties(type)[p.Name];
                NotMappedAttribute notMappedAttribute = (NotMappedAttribute)descriptor.Attributes[typeof(NotMappedAttribute)];

                if (notMappedAttribute != null)
                {
                    exp.ForMember(p.Name, ap => ap.Ignore());
                }
            }
            return exp;
        }
    }
    class Program
    {
        public int A;
        public int B;
        public Program(int a, int b)
        {

            this.A = a;
            this.B = b;
        }

        public object this[int i]
        {
            get
            {
                if (i == 0)
                {
                    return A;
                }
                else if (i == 1)
                {
                    return B;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (i == 0)
                {
                    A = Convert.ToInt32(value);
                }
                else if (i == 1)
                {
                    B = Convert.ToInt32(value);
                }
            }
        }
        public delegate TResult FunC<in T, out TResult>(T arg);

        static void Main(string[] args)
        {
            Console.WriteLine(Country.Japan.GetEnumInfo());
            Subject subject = new Subject();
            subject.Science();
            subject.Math();
            subject.Physic();
            Console.WriteLine((int)Sub.Science + " ***************************");
            Console.WriteLine();



            try
            {
                Console.WriteLine("Enter num 1");
                int num1 = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter num 2");
                int num2 = Convert.ToInt32(Console.ReadLine());
                if (num2 % 2 == 0)
                {
                    Console.WriteLine("Division is " + num1 / num2);
                }
                else
                {
                    throw new OddNumberDivisionException("Cannot divide by odd number !!!!!");
                }
            }
            catch (OddNumberDivisionException ex)
            {
                Console.WriteLine(ex.Message);
            }

            bool IsLetterOrSeparator(char c) => c is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z') or '.' or ',';

            Console.WriteLine(getCountryName(Country.Japan));
            Console.WriteLine(IsLetterOrSeparator('%'));

            string getCountryName(Country country) =>
             country switch
             {
                 Country.UnitedState => "US",
                 Country.India => "IND",
                 _ => "INVALID!",
             };

            int[][][] i = new int[2][][];

            i[0] = new int[2][];
            i[0][0] = new int[] { 1, 2 };
            i[0][1] = new int[] { 1, 2, 3 };

            i[1] = new int[2][];
            i[1][0] = new int[] { 1, 2, 5, 8, 9 };
            i[1][1] = new int[] { 1 };

            foreach (int[][] val in i)
            {
                foreach (int[] val2 in val)
                {
                    foreach (int val3 in val2)
                    {
                        Console.Write(val3);

                    }
                    Console.WriteLine();
                }
            }

            Console.WriteLine("*********************");

            LinkedList<int> list = new LinkedList<int>();
            LinkedListNode<int> j = list.AddLast(3);
            list.AddLast(2);
            list.AddLast(1);
            list.AddLast(0);
            list.AddAfter(j, 6);
            foreach (int l in list)
            {
                Console.WriteLine(l);
            }

            //File operations
            string path = $@"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent}/demo.txt";
            Console.WriteLine();
            int[] arr = { 1, 2, 3 };
            Console.WriteLine(arr is [1, 2, 3]);
            Console.WriteLine(arr is [1, 2, 4]);
            Console.WriteLine(arr is [1, <= 3, >= 3]);

            if (arr is [var f, .. var h])
            {
                Console.WriteLine(h[0] + h[1] + f);
            }

            //Console.WriteLine(Process.GetCurrentProcess().Threads.Count);

            int sum(int a, int b, [Optional] int[] i)
            {
                int res = a + b;
                if (i != null)
                {
                    foreach (int x in i)
                    {
                        res += x;
                    }
                }

                return res;
            }

            Console.WriteLine(sum(10, 20, new int[] { 30, 40, 50 }));

            Program program = new Program(10, 20);

            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<Map1, Map2>().IgnorMap();
            });
            var map = new Mapper(config);

            Map1 map1 = new Map1() { FName = "Jil", LName = "Patel" };

            var v = map.Map<Map2>(map1);
            Console.WriteLine(v.FName + " : " + v.LName);

            Console.WriteLine("**********************************"); 


            var type = typeof(CustomCountry);
            foreach (var p in type.GetProperties())
            {
                
                Console.WriteLine("Property : " + p.Name);
                PropertyDescriptor? descriptor = TypeDescriptor.GetProperties(type)[p.Name];
                Console.WriteLine("Desc : " + descriptor);
                NoMapAttribute vv = (NoMapAttribute)descriptor.Attributes[typeof(NoMapAttribute)];
                if (vv != null)
                {
                    Console.WriteLine("NoMap Attribute is set on " + vv.val + " Property!");

                }
                else
                {
                    Console.WriteLine("***");
                }
            }


            Map2 m = new Map2() { FName = "jil patel patel patel" };

            int SS(int x)
            {
                return x;
            }

            FunC<int, int> fff = SS;

            int ss(FunC<int, int> c)
            {
                return c(10);
            }

            Console.WriteLine(ss(SS));
            Console.WriteLine(fff(4001000));

            Console.WriteLine(Guid.NewGuid());

        }

    }

}

// What is Exception and how to make custom exception
// when to use enum and is enum contain string if yes how ?
// how to set generic constraint nad how to set constraint
// Difference between the Equality Operator (==) and Equals() Method in C