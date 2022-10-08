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


using mychess2;

namespace ChessAdvisor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        Button[,] bitfild = new Button[8, 8];
        ImageBrush[] figus = new ImageBrush[14];

        public mychess2.gmmtrx gam1;
        Point preMouseClick;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            figus[0] = new ImageBrush(new BitmapImage(new Uri("fig\\peshkaW.bmp", UriKind.Relative)));
            figus[1] = new ImageBrush(new BitmapImage(new Uri("fig\\turaW.bmp", UriKind.Relative)));
            figus[2] = new ImageBrush(new BitmapImage(new Uri("fig\\konjW.bmp", UriKind.Relative)));
            figus[3] = new ImageBrush(new BitmapImage(new Uri("fig\\slonW.bmp", UriKind.Relative)));
            figus[4] = new ImageBrush(new BitmapImage(new Uri("fig\\ferzW.bmp", UriKind.Relative)));
            figus[5] = new ImageBrush(new BitmapImage(new Uri("fig\\koroljW.bmp", UriKind.Relative)));
            figus[6] = new ImageBrush(new BitmapImage(new Uri("fig\\peshkaB.bmp", UriKind.Relative)));
            figus[7] = new ImageBrush(new BitmapImage(new Uri("fig\\turaB.bmp", UriKind.Relative)));
            figus[8] = new ImageBrush(new BitmapImage(new Uri("fig\\konjB.bmp", UriKind.Relative)));
            figus[9] = new ImageBrush(new BitmapImage(new Uri("fig\\slonB.bmp", UriKind.Relative)));
            figus[10] = new ImageBrush(new BitmapImage(new Uri("fig\\ferzB.bmp", UriKind.Relative)));
            figus[11] = new ImageBrush(new BitmapImage(new Uri("fig\\koroljB.bmp", UriKind.Relative)));
            figus[12] = new ImageBrush(new BitmapImage(new Uri("fig\\pustoB.bmp", UriKind.Relative)));
            figus[13] = new ImageBrush(new BitmapImage(new Uri("fig\\pustoW.bmp", UriKind.Relative)));



            gam1 = new mychess2.gmmtrx();
            preMouseClick.X = -1;
            preMouseClick.Y = -1;

            // bitfild = new Button[8, 8];
            /*
            GamePanel1.Children.Clear();
             Button btn = new Button();
             btn.Content = "New Button";
             GamePanel1.Children.Add(btn);
            */

            GameCanvas1.Children.Clear();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {





                    bitfild[i, j] = new Button();
                    bitfild[i, j].Content = i.ToString() + ":" + j.ToString();
                    bitfild[i, j].Margin = new Thickness(64 * i, 64 * j, 0, 0);
                    bitfild[i, j].Padding = new Thickness(1);

                    bitfild[i, j].Height = 64;
                    bitfild[i, j].Width = 64;
                    bitfild[i, j].HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    bitfild[i, j].VerticalAlignment = System.Windows.VerticalAlignment.Top;

                    bitfild[i, j].IsEnabled = true;

                    bitfild[i, j].Content = GameCanvas1.Children.Add(bitfild[i, j]);

                    bitfild[i, j].Click += chessbutton_Click;

                    //ImageBrush slon = new ImageBrush(image1.Source);
                    //bitfild[i, j].Foreground = slon;

                    if (((i + j) % 2) == 1)
                        bitfild[i, j].Background = Brushes.Gray;
                    else
                        bitfild[i, j].Background = Brushes.Blue;



                    /*

                   Image myImage3 = new Image();
                   BitmapImage bi3 = new BitmapImage();
                   bi3.BeginInit();
                   bi3.UriSource = new Uri("fig\\ferzB.bmp", UriKind.Relative);
                   bi3.EndInit();
                   myImage3.Stretch = Stretch.Fill;
                   myImage3.Source = bi3;
                   
                    
                   ImageBrush ferz = new ImageBrush(myImage3.Source);                   
                    ferz.Opacity = 0.7;
                    */

                    //figus[0].ImageSource = 


                    //bitfild[i, j].Foreground = ferz;
                    // bitfild[i, j].Background = figus[0];

                    if ((int)(gam1.p_chb.board[j, i]) == 12)
                    {
                        if (((i + j) % 2) == 1)
                            bitfild[i, j].Background = figus[12];
                        else
                            bitfild[i, j].Background = figus[13];
                    }
                    else
                        bitfild[i, j].Background = figus[(int)(gam1.p_chb.board[j, i])];//fldmtr[j, i]; 

                }
        }

        private void chessbutton_Click(object sender, RoutedEventArgs e)
        {
            turn[] hots = new turn[0];
            int X1 = (int)(sender as Button).Margin.Left / 64;
            int Y1 = (int)(sender as Button).Margin.Top / 64;

            if ((preMouseClick.X != -1) && (preMouseClick.Y != -1))
            {
                if (((preMouseClick.X + preMouseClick.Y) % 2) == 1)
                    bitfild[(int)preMouseClick.X, (int)preMouseClick.Y].Background = Brushes.Gray;
                else
                    bitfild[(int)preMouseClick.X, (int)preMouseClick.Y].Background = Brushes.Blue;

                gam1.p_chb.board[Y1, X1] = gam1.p_chb.board[(int)preMouseClick.Y, (int)preMouseClick.X];
                gam1.p_chb.board[(int)preMouseClick.Y, (int)preMouseClick.X] = figura.fCount;

                bitfild[(int)preMouseClick.X, (int)preMouseClick.Y].Background = figus[(int)(gam1.p_chb.board[(int)preMouseClick.Y, (int)preMouseClick.X])];
                bitfild[X1, Y1].Background = figus[(int)(gam1.p_chb.board[Y1, X1])];
                preMouseClick.X = -1;
                preMouseClick.Y = -1;

            }
            else
            {
                (sender as Button).Background = Brushes.BlueViolet;
                preMouseClick.X = X1;
                preMouseClick.Y = Y1;
                if (gam1.p_chb.board[Y1, X1] == figura.fTuraB)
                {
                    gam1.CalculateTuraSlonHod(false, false, X1, Y1, ref hots);
                    for (int i = 0; i < hots.Length; i++)
                        bitfild[hots[i].to_pos.x, hots[i].to_pos.y].Background = Brushes.YellowGreen;
                }

                if (gam1.p_chb.board[Y1, X1] == figura.fPeshkaB)
                {
                    gam1.CalculatePeshkaHod(false, X1, Y1, ref hots);
                    for (int i = 0; i < hots.Length; i++)
                        bitfild[hots[i].to_pos.x, hots[i].to_pos.y].Background = Brushes.YellowGreen;
                }


                if (gam1.p_chb.board[Y1, X1] == figura.fKonjB)
                {
                    gam1.CalculatePferdHod(false, X1, Y1, ref hots);
                    for (int i = 0; i < hots.Length; i++)
                        bitfild[hots[i].to_pos.x, hots[i].to_pos.y].Background = Brushes.YellowGreen;
                }
                if (gam1.p_chb.board[Y1, X1] == figura.fOfizerB)
                {
                    gam1.CalculateTuraSlonHod(true, false, X1, Y1, ref hots);
                    for (int i = 0; i < hots.Length; i++)
                        bitfild[hots[i].to_pos.x, hots[i].to_pos.y].Background = Brushes.YellowGreen;
                }
                if (gam1.p_chb.board[Y1, X1] == figura.fFerzjB)
                {
                    gam1.CalculateFerzHod(false, X1, Y1, ref hots);
                    for (int i = 0; i < hots.Length; i++)
                        bitfild[hots[i].to_pos.x, hots[i].to_pos.y].Background = Brushes.YellowGreen;
                }
                if (gam1.p_chb.board[Y1, X1] == figura.fKoroljB)
                {
                    //  gam1.CalculateKoroljHod(false, X1, Y1, ref hots);
                    //  for (int i = 0; i < hots.Length; i++)
                    //      bitfild[hots[i].to_pos.x, hots[i].to_pos.y].BackColor = Color.YellowGreen;
                }


            }

        }



        private void button2_Click(object sender, RoutedEventArgs e)
        {
            gam1.CalculateAllOfHods();
            gam1.MakeHod(gam1.MakeDecision(true, 3), false);
            textBox1.Text = gam1.s11;
            label1.Content += gam1.s11;

            if (gam1.nShachW == 555) label1.Content += "БЕЛЫМ МАТ !!!!";
            else
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        bitfild[i, j].Background = figus[(int)(gam1.p_chb.board[j, i])];//fldmtr[j, i];

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            gam1.CalculateAllOfHods();
            gam1.MakeHod(gam1.MakeDecision(false, 3), false);
            textBox1.Text = gam1.s11;
            label1.Content += gam1.s11;

            if (gam1.nShachB == 555) label1.Content += "ЧЕРНЫМ МАТ !!!!";
            else
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        bitfild[i, j].Background = figus[(int)(gam1.p_chb.board[j, i])];//fldmtr[j, i];
        }

    }

}
