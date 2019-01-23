using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Graphics3D;
using Lib_Thermo;
using WcfCalculationLib;


namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {      
        [TestMethod]
        public void InterfaceTest()// StartButton test
        {
            MainWindow Form = new MainWindow();
            bool origin = Form.start;
            Form.Start_Stop_Calculations(origin);
            bool next = Form.start;
            Assert.AreNotEqual(origin, next);
           
        }

        [TestMethod]

        public void LoggerTest()//logger test
        {
            double[,,] check = new double[10, 10, 10];
            double time = 1;
            double tau = 0.1;
            double h = 1;
            ILogger log = new NLogAdapter();
            Thermo teplo = new Thermo(log);
            teplo.PoslCulс3D(check, time, tau, h);
            log = teplo._Logger;
            Assert.AreNotEqual(log, null);
        }

        [TestMethod]
        public void CalculationTest()//calculation test
        {
            int n = 10;
            double[,,] test = new double[n, n, n];
            double[,,] actual = new double[n, n, n];
            test = initial();
            actual = initial();
            ILogger log = new NLogAdapter();
            Thermo teplo = new Thermo(log);
            double time = 1;
            double tau = 0.1;
            double h = 1;
            teplo.PoslCulс3D(test, time, tau, h);
            Assert.AreNotEqual(test,actual);
        }

        public double [,,] initial()
        {
            int n = 10;
            double[,,] check = new double[n, n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    for (int k = 0; k < n; k++)
                    {
                        check[i, j, k] = 10;
                    }
                }
            }

            //Считывание начальных границ
            for (int j = 0; j < n; j++)
            {
                for (int k = 0; k < n; k++)
                {
                    check[j, 0, k] = 10;
                }
            }
            for (int j = 0; j < n; j++)
            {
                for (int k = 0; k < n; k++)
                {
                    check[j, k, 0] = 100;
                }
            }
            for (int j = 0; j < n; j++)
            {
                for (int k = 0; k < n; k++)
                {
                    check[0, j, k] = 0;
                }
            }
            for (int j = 0; j < n; j++)
            {
                for (int k = 0; k < n; k++)
                {
                    check[n - 1, j, k] = 0;
                }
            }
            for (int j = 0; j < n; j++)
            {
                for (int k = 0; k < n; k++)
                {
                    check[k, j, n - 1] = 500;
                }
            }
            for (int j = 0; j < n; j++)
            {
                for (int k = 0; k < n; k++)
                {
                    check[j, n - 1, k] = 10;
                }
            }
            return check;
        }
    }
}
