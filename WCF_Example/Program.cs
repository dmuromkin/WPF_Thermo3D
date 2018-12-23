using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfCalculationLib;

namespace WCF_Example
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = 100;
            List<double> massA = new List<double>();
            List<double> massB = new List<double>();
            for(int i=0;i<n;i++)
            {
                massA.Add(1.0);
                massB.Add(1.0);
            }
            CalcService calcservice = new CalcService();
            InputMatrixes inputMatr = new InputMatrixes();
            inputMatr.Mass_a = massA.ToArray();
            inputMatr.Mass_b = massB.ToArray();
            calcservice.SumMatrixes(inputMatr);
            OutputMatrixes output = calcservice.SumMatrixes(inputMatr);
            for (int j = 0; j < output.Mass_summ.Length; j+=2)
                Console.Write(output.Mass_summ[j]);
            Console.ReadKey();

        }
    }
}
