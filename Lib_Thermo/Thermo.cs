using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lib_Thermo
{
    public class Thermo
    {

        public ILogger _Logger;
        Stopwatch timer = new Stopwatch();
        public Thermo(ILogger logger) {
            SetLogger(logger);
        }

        public void SetLogger(ILogger logger) {
            _Logger = logger;
        }

        //Последовательный двумерный расчет
        public double[,] PoslCulc(double[,] u, double time, double tau, double h) {

            _Logger.Log("Запущен последовательный расчет для размерности " + Convert.ToString(u.GetLength(0)) + " на " + Convert.ToString(u.GetLength(1)));
            _Logger.Log("Шаг по времени = " + Convert.ToString(tau));
            _Logger.Log("Количество шагов = " + Convert.ToString(time / tau));

            int a = u.GetLength(0);
            int b = u.GetLength(1);
            double[,] unew = new double[a, b];
            double Eps = tau / (h * h);
            unew = u;
            double t0 = 0;

            timer.Start();
            for (double t = t0 + tau; t <= time; t += tau) {
                for (int i = 1; i < a - 1; i++)
                    for (int j = 1; j < b - 1; j++)
                        unew[i, j] = u[i, j] + Eps * (u[i + 1, j] + u[i - 1, j] + u[i, j + 1] + u[i, j - 1] - 4 * u[i, j]);

                for (int i = 0; i < a; i++) {
                    for (int j = 0; j < b; j++) {
                        u[i, j] = unew[i, j];
                    }
                }
            }

            timer.Stop();
            if (_Logger != null) {
                _Logger.Log("Закончен последовательный расчет через " + Convert.ToString(timer.ElapsedMilliseconds) + " мс\n\n");

            }
            timer.Reset();
            return u;
        }

        //Последовательный трехмерный расчет
        public double[,,] PoslCulс3D(double[,,] u, double time, double tau, double h) {
            _Logger.Log("Запущен последовательный расчет для размерности " + Convert.ToString(u.GetLength(0)) + " х " + Convert.ToString(u.GetLength(1)) + " х " + Convert.ToString(u.GetLength(2)));
            _Logger.Log("Шаг по времени = " + Convert.ToString(tau));
            _Logger.Log("Количество шагов = " + Convert.ToString(time / tau));
            int a = u.GetLength(0);
            int b = u.GetLength(1);
            int c = u.GetLength(2);
            double[,,] unew = new double[a, b, c];
            double Eps = tau / (h * h);
            unew = u;
            double t0 = 0;
            timer.Start();
            for (double t = t0 + tau; t <= time; t += tau) {
                for (int i = 1; i < a - 1; i++)
                    for (int j = 1; j < b - 1; j++)
                        for (int k = 1; k < c - 1; k++)
                            unew[i, j, k] = u[i, j, k] + Eps * (u[i + 1, j, k] + u[i - 1, j, k] + u[i, j + 1, k] + u[i, j - 1, k] + u[i, j, k - 1] + u[i, j, k + 1] - 6 * u[i, j, k]);

                for (int i = 0; i < a; i++) {
                    for (int j = 0; j < b; j++) {
                        for (int k = 0; k < c; k++) {
                            u[i, j, k] = unew[i, j, k];
                        }
                    }
                }
            }
            timer.Stop();

            if (_Logger != null) {
                _Logger.Log("Закончен последовательный расчет через " + Convert.ToString(timer.ElapsedMilliseconds) + " мс\n\n");

            }
            timer.Reset();
            return u;
        }

        //Параллельный двумерный расчет
        public double[,] ParalCulc(double[,] u, double time, double tau, double h) {

            _Logger.Log("Запущен паралельный расчет для размерности " + Convert.ToString(u.GetLength(0)) + " х " + Convert.ToString(u.GetLength(1)));
            _Logger.Log("Шаг по времени = " + Convert.ToString(tau));
            _Logger.Log("Количество шагов = " + Convert.ToString(time / tau));

            int a = u.GetLength(0);
            int b = u.GetLength(1);
            double[,] unew = new double[a, b];
            double Eps = tau / (h * h);
            unew = u;
            double t0 = 0;

            timer.Start();
            for (double t = t0 + tau; t <= time; t += tau) {
                for (int i = 1; i < a - 1; i++) {
                    Parallel.For(1, b - 1, j => {
                        unew[i, j] = u[i, j] + Eps * (u[i + 1, j] + u[i - 1, j] + u[i, j + 1] + u[i, j - 1] - 4 * u[i, j]);
                    });
                }



                for (int i = 0; i < a; i++) {
                    for (int j = 0; j < b; j++) {
                        u[i, j] = unew[i, j];
                    }
                }
            }
            timer.Stop();

            if (_Logger != null) {
                _Logger.Log("Закончен паралеллный расчет через " + Convert.ToString(timer.ElapsedMilliseconds) + " мс\n\n");

            }
            timer.Reset();
            return u;
        }

        public double[,,] ЗDParalCulc(double[,,] u, double time, double tau, double h) {
            _Logger.Log("nЗапущен паралеллный расчет для размерности " + Convert.ToString(u.GetLength(0)) + " х " + Convert.ToString(u.GetLength(1)) + " х " + Convert.ToString(u.GetLength(2)));
            _Logger.Log("Шаг по времени = " + Convert.ToString(tau));
            _Logger.Log("Количество шагов = " + Convert.ToString(time / tau));
            int a = u.GetLength(0);
            int b = u.GetLength(1);
            int c = u.GetLength(2);
            double[,,] unew = new double[a, b, c];
            double Eps = tau / (h * h);
            unew = u;
            double t0 = 0;
            timer.Start();
            for (double t = t0 + tau; t <= time; t += tau) {
                for (int i = 1; i < a - 1; i++)
                    for (int j = 1; j < b - 1; j++)
                        Parallel.For(1, c - 1, k => {
                            unew[i, j, k] = u[i, j, k] + Eps * (u[i + 1, j, k] + u[i - 1, j, k] + u[i, j + 1, k] + u[i, j - 1, k] + u[i, j, k - 1] + u[i, j, k + 1] - 6 * u[i, j, k]);
                        });


                for (int i = 0; i < a; i++) {
                    for (int j = 0; j < b; j++) {
                        for (int k = 0; k < c; k++) {
                            u[i, j, k] = unew[i, j, k];
                        }
                    }
                }
            }
            timer.Stop();

            if (_Logger != null) {
                _Logger.Log("Закончен паралеллный расчет через " + Convert.ToString(timer.ElapsedMilliseconds) + " мс\n\n");
            }
            timer.Reset();
            return u;
        }
    }
}
