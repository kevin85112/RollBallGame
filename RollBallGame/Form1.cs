using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace RollBallGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // 版面上之轉珠
        Ball[,] NowBalls;
        BallAction NowAction;
        bool FirstStart = true;

        private void button_start_Click(object sender, EventArgs e)
        {
            if (FirstStart)
            {
                // 設定轉珠區
                Ball.PlayArea = textBox_PlayArea;
                NowBalls = new Ball[Ball.AxisSize.Width, Ball.AxisSize.Height];

                // 建立新轉珠版面
                CreateBallPanel();

                // 建立行為
                NowAction = new BallAction(textBox_PlayArea, NowBalls);
                NowAction.label_score = this.label_score;
                for (int i = 0, j; i < NowBalls.GetLength(0); i++)
                {
                    for (j = 0; j < NowBalls.GetLength(1); j++)
                    {
                        // 註冊滑鼠事件
                        NowBalls[i, j].BallLabel.MouseDown += NowAction.BallMouseDown;
                        NowBalls[i, j].BallLabel.MouseEnter += XXX;
                    }
                }
                FirstStart = false;
            }
            else
            {
                NowAction.ChangeColor();
            }
        }

        void XXX(object state, EventArgs e)
        {
            CustomBallLabel xx = ((CustomBallLabel)(state));
            Debug.WriteLine(xx.ball.Axis.X.ToString() + " " + xx.ball.Axis.Y.ToString() + " " + xx.ball.cleardata.Number);
        }



        // 建立新轉珠版面
        void CreateBallPanel()
        {
            Color ballcolor;
            for (int i = 0, j; i < NowBalls.GetLength(0); i++)
            {
                for (j = 0; j < NowBalls.GetLength(1); j++)
                {
                    // 建立實體物件
                    NowBalls[i, j] = new Ball();
                    ballcolor = Color.Black;
                    if (j > 1)
                    {
                        if (NowBalls[i, j].BallColor.Equals(NowBalls[i, j - 1].BallColor))
                        {
                            while (NowBalls[i, j].BallColor.Equals(NowBalls[i, j - 2]))
                            {
                                ballcolor = Ball.ToRandomColor(NowBalls[i, j]);
                            }
                        }
                    }
                    if (i > 1)
                    {
                        if (NowBalls[i, j].BallColor.Equals(NowBalls[i - 1, j].BallColor))
                        {
                            while (NowBalls[i, j].BallColor.Equals(NowBalls[i - 2, j].BallColor) || NowBalls[i, j].BallColor.Equals(ballcolor))
                            {
                                Ball.ToRandomColor(NowBalls[i, j]);
                            }
                        }
                    }

                    NowBalls[i, j].BallLabel = new CustomBallLabel(NowBalls[i, j]);
                    // 設定座標
                    NowBalls[i, j].Axis = new Point(i, j);
                    // 加入Label控制項
                    this.Controls.Add(NowBalls[i, j].BallLabel);
                    // 移至位置
                    NowBalls[i, j].SetBallLocation();
                    // 移至最上層
                    NowBalls[i, j].BallLabel.BringToFront();

                }
            }
        }


    }
}
