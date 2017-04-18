using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("----CustomConfigurationFirst---------------------");
            CustomConfigurationFirst settingFirst = CustomConfigurationFirst.Setting;
            Console.WriteLine("settingFirst.Id:" + settingFirst.Id);
            Console.WriteLine("settingFirst.Name:" + settingFirst.Name);
            Console.WriteLine("settingFirst.FirstProperty" + settingFirst.FirstProperty);
            Console.WriteLine("--------------------------------------------------");
        }
    }
}
