using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Graphics3D.CalcService;

namespace Graphics3D {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            this.DataContext = new ViewModel();
        }

        public class ViewModel {
            public ViewModel() {
                /* Set default age */
                this.Age = 200;
            }

            public int Age { get; set; }

        }

        //Размерность решетки
        int n = 10;

        double time;
        double tau;
        double h;
        double[,,] u;
        Draw draw;
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        InputDate3D inputdate = new InputDate3D();
        OutputDate3D outputDate = new OutputDate3D();
        CalcServiceClient client;

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

        //Начальная отрисовка
        void StartCalc() {
            Helix.Children.Clear();
            Helix.Children.Add(light);
            Helix.InvalidateArrange();
            Helix.InvalidateVisual();

            //Проверка доступности сервиса
            try {
                client = new CalcServiceClient();
            }
            catch {
                if (client == null) {
                    MessageBox.Show("Нет доспупа к сервису");
                    return;
                }

            }

            time = 1;
            tau = 0.1;
            h = 1;
            u = new double[n, n, n];
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    for (int k = 0; k < n; k++) {
                        u[i, j, k] = Convert.ToDouble(TempPlan.Text);
                    }
                }
            }
            
            //Считывание начальных границ
            for (int j = 0; j < n; j++) {
                for (int k = 0; k < n; k++) {
                    u[j, 0, k] = Convert.ToDouble(TeilTemp.Text);
                }
            }
            for (int j = 0; j < n; j++) {
                for (int k = 0; k < n; k++) {
                    u[j, k, 0] = Convert.ToDouble(BottomGran.Text);
                }
            }
            for (int j = 0; j < n; j++) {
                for (int k = 0; k < n; k++) {
                    u[0, j, k] = Convert.ToDouble(RightGran.Text);
                }
            }
            for (int j = 0; j < n; j++) {
                for (int k = 0; k < n; k++) {
                    u[n - 1, j, k] = Convert.ToDouble(LeftGran.Text);
                }
            }
            for (int j = 0; j < n; j++) {
                for (int k = 0; k < n; k++) {
                    u[k, j, n - 1] = Convert.ToDouble(TopGran.Text);
                }
            }
            for (int j = 0; j < n; j++) {
                for (int k = 0; k < n; k++) {
                    u[j, n - 1, k] = Convert.ToDouble(FrontTemp.Text);
                }
            }

                timer.Tick += Timer_Tick;
                timer.Interval = new TimeSpan(0, 0, 100);
                timer.Start();
            

            draw = new Draw(n);
            draw.StartDraw(Helix);
        }

        void Timer_Tick(object sender, EventArgs e) {
            timer.Stop();
            Calc();
            timer.Start();
        }

        //Основной расчет
        async void Calc() {

            if (CheckBoxParallel.IsChecked == true) {
                await Task.Run(() => {

                    inputdate.H = h;
                    inputdate.Tau = tau;
                    inputdate.Time = time;

                    inputdate.Mass_u = ToJagged3D(u);
                    outputDate = client.CulcTeploParal3D(inputdate);
                    u = ToMulti3D(outputDate.Culc_Teplo);
                });
                draw.draw(u, Convert.ToDouble(TempPlan.Text));
            }
            else {
                await Task.Run(() => {

                    inputdate.H = h;
                    inputdate.Tau = tau;
                    inputdate.Time = time;

                    inputdate.Mass_u = ToJagged3D(u);
                    outputDate = client.CulcTeploPosl3D(inputdate);
                    u = ToMulti3D(outputDate.Culc_Teplo);
                });
                draw.draw(u, Convert.ToDouble(TempPlan.Text));
            }

        }

        private void Start_button_Click(object sender, RoutedEventArgs e) {
            StartCalc();
            Calc();
            Start_button.IsEnabled = false;
        }  
    }
}
