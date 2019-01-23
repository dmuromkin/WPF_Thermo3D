using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Lib_Thermo;

namespace WcfCalculationLib
{
    public class CalcService : ICalcService
    {
            public static T[][] ToJagged<T>(T[,] mArray) {
                var rows = mArray.GetLength(0);
                var cols = mArray.GetLength(1);
                var jArray = new T[rows][];
                for (int i = 0; i < rows; i++) {
                    jArray[i] = new T[cols];
                    for (int j = 0; j < cols; j++) {
                        jArray[i][j] = mArray[i, j];
                    }
                }
                return jArray;
            }

        public static T[][][] ToJagged3D<T>(T[,,] mArray) {
            var rows = mArray.GetLength(0);
            var cols = mArray.GetLength(1);
            var wight = mArray.GetLength(2);
            var jArray = new T[rows][][];
            for (int i = 0; i < rows; i++) {
                jArray[i] = new T[cols][];
                for (int j = 0; j < cols; j++) {
                    jArray[i][j] = new T[wight];
                }

            }
            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < cols; j++) {
                    for (int k = 0; k < wight; k++) {
                        jArray[i][j][k] = mArray[i, j, k];
                    }
                }

            }

            return jArray;
        }

        public static T[,] ToMultiD<T>(T[][] jArray) {
                int i = jArray.Count();
                int j = jArray.Select(x => x.Count()).Aggregate(0, (current, c) => (current > c) ? current : c);


                var mArray = new T[i, j];

                for (int ii = 0; ii < i; ii++) {
                    for (int jj = 0; jj < j; jj++) {
                        mArray[ii, jj] = jArray[ii][jj];
                    }
                }

                return mArray;
            }

        public static T[,,] ToMulti3D<T>(T[][][] jArray) {
            int i = jArray.Count();
            int j = jArray.Select(x => x.Count()).Aggregate(0, (current, c) => (current > c) ? current : c);
            int k = jArray[0][0].Length;

            var mArray = new T[i, j, k];

            for (int ii = 0; ii < i; ii++) {
                for (int jj = 0; jj < j; jj++) {
                    for (int kk = 0; kk < k; kk++) {
                        mArray[ii, jj, kk] = jArray[ii][jj][kk];
                    }
                }
            }

            return mArray;
        }

        public OutputDate CulcTeploPosl(InputDate inputDate) {
                OutputDate mass_data = new OutputDate();

                double[,] array1 = ToMultiD(inputDate.Mass_u);

                double h = inputDate.H;
                double time = inputDate.Time;
                double tau = inputDate.Tau;
                ILogger log = new NLogAdapter();
                Thermo teplo = new Thermo(log);

                double[,] array2 = teplo.PoslCulc(array1, time, tau, h);

                mass_data.Culc_Teplo = ToJagged(array2);
                return mass_data;
            }

        public OutputDate3D CulcTeploPosl3D(InputDate3D inputDate) {
            
            OutputDate3D mass_data = new OutputDate3D();
            
            double[,,] array1 = ToMulti3D(inputDate.Mass_u);

            double h = inputDate.H;
            double time = inputDate.Time;
            double tau = inputDate.Tau;
            ILogger log = new NLogAdapter();
            Thermo teplo = new Thermo(log);

            double[,,] array2 = teplo.PoslCulс3D(array1, time, tau, h);

            mass_data.Culc_Teplo = ToJagged3D(array2);
            return mass_data;
        }


        public OutputDate CulcTeploParal(InputDate inputDate) {
                OutputDate mass_data = new OutputDate();

                double[,] array1 = ToMultiD(inputDate.Mass_u);

                double h = inputDate.H;
                double time = inputDate.Time;
                double tau = inputDate.Tau;
                ILogger log = new NLogAdapter();
                Thermo teplo = new Thermo(log);
                double[,] array2 = teplo.ParalCulc(array1, time, tau, h);
                mass_data.Culc_Teplo = ToJagged(array2);
                return mass_data;
            }

        public OutputDate3D CulcTeploParal3D(InputDate3D inputDate) {
            OutputDate3D mass_data = new OutputDate3D();
                      
            double[,,] array1 = ToMulti3D(inputDate.Mass_u);

            double h = inputDate.H;
            double time = inputDate.Time;
            double tau = inputDate.Tau;
            ILogger log = new NLogAdapter();
            Thermo teplo = new Thermo(log);
            double[,,] array2 = teplo.ЗDParalCulc(array1, time, tau, h);
            mass_data.Culc_Teplo = ToJagged3D(array2);
            return mass_data;
        }
    }
    }

