using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Lib_Thermo;

namespace WcfCalculationLib
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде и файле конфигурации.
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


            public OutputDate CulcTeploPosl(InputDate inputDate) {
                OutputDate mass_data = new OutputDate();

                double[,] array1 = ToMultiD(inputDate.Mass_u);

                double h = inputDate.H;
                double time = inputDate.Time;
                double tau = inputDate.Tau;
                Thermo teplo = new Thermo();

                double[,] array2 = teplo.PoslCulc(array1, time, tau, h);

                mass_data.Culc_Teplo = ToJagged(array2);
                return mass_data;
            }


            //Jagged array

            public OutputDate CulcTeploParal(InputDate inputDate) {
                OutputDate mass_data = new OutputDate();

                double[,] array1 = ToMultiD(inputDate.Mass_u);

                double h = inputDate.H;
                double time = inputDate.Time;
                double tau = inputDate.Tau;
                Thermo teplo = new Thermo();
                double[,] array2 = teplo.ParalCulc(array1, time, tau, h);
                mass_data.Culc_Teplo = ToJagged(array2);
                return mass_data;
            }
        }
    }

