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
using System.Drawing;
using Graphics.CalcService;
//using WcfCalculationLib;
using System.Timers;

namespace Graphics {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        double[,] u = new double[n, n];
        SolidColorBrush brush = new SolidColorBrush();
        double clr;
        Color color;

        public MainWindow() {

            InitializeComponent();

            //initial timer
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(10);

            //initial 
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    u[i, j] = 5;
                }
            }
        }

        public void initial_plotting() {
            //это я типо попробовал написать коммент
            //Бог даст получиться сделать pull requist
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    rects[i, j] = new Rectangle();
                }
            }
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    rects[i, j].Width = 4;
                    rects[i, j].Height = 4;
                    Canvas.SetLeft(rects[i, j], i * 4);
                    Canvas.SetTop(rects[i, j], j * 4);
                    Canva.Children.Add(rects[i, j]);
                }
            }
        }

        private void timerTick(object sender, EventArgs e) {
            calculate();
        }

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
        const int n = 75;
        public Rectangle[,] rects = new Rectangle[n, n];


        async void calculate() {
            double time = 0.2;
            double tau = 0.1;
            double h = 1;


            for (int j = 0; j < n; j++) //left
                u[0, j] = Convert.ToInt32(left.Text);

            for (int i = 0; i < n; i++) //bottom
                u[i, n - 1] = Convert.ToInt32(bottom.Text);

            for (int j = 0; j < n; j++) //rigth
                u[n - 1, j] = Convert.ToInt32(right.Text);

            for (int i = 0; i < n; i++) //top
                u[i, 0] = Convert.ToInt32(top.Text);

            //CalcService calcservice = new CalcService();
            CalcServiceClient calcservice = new CalcServiceClient();
            InputDate inputdate = new InputDate();
            OutputDate outputDate = new OutputDate();


            if (CheckBoxParallel.IsChecked == false) {
                await Task.Run(() => {
                    inputdate.H = h;
                    inputdate.Tau = tau;
                    inputdate.Time = time;

                    inputdate.Mass_u = ToJagged(u);

                    outputDate = calcservice.CulcTeploPosl(inputdate);

                    u = ToMultiD(outputDate.Culc_Teplo);

                });
                //отрисовка
                plotting(u);
            }
            else {
                await Task.Run(() => {
                    inputdate.H = h;
                    inputdate.Tau = tau;
                    inputdate.Time = time;

                    inputdate.Mass_u = ToJagged(u);

                    outputDate = calcservice.CulcTeploParal(inputdate);

                    u = ToMultiD(outputDate.Culc_Teplo);
                });
                //отрисовка
                plotting(u);
            }
        }

        public void plotting(double[,] u) {

            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    clr = (u[i, j] * 255) / 500;
                    color = Color.FromRgb((byte)clr, 0, (byte)(255 - (int)clr));
                    brush = new SolidColorBrush(color);
                    rects[i, j].Fill = brush;

                }
            }
        }
        private void Start_Click(object sender, RoutedEventArgs e) {
            initial_plotting();
            timer.Start();
        }
    }
}
