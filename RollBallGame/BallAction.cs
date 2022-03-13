using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Windows.Forms.Control;

namespace RollBallGame
{
    public enum DirectionType
    {
        East,
        South,
        West,
        North
    }

    // 建立轉珠碰撞委派
    public delegate void EventBallCollision(DirectionType direction);

    public class BallAction
    {
        #region variable

        // 執行中標籤
        private CustomBallLabel active_label;
        // 活動區域
        private Control active_area;
        // 計算座標用相關變數
        private Point picking_position;
        private Point correction_offset;
        private Point target_position;
        // 座標調整位移
        private static Point offset { get; }
        // 判斷是否正在拖移
        private bool IsDrag;
        // 版面上之轉珠
        private Ball[,] nowballs;
        private Ball[,] ballaxis;
        // 用於設定轉珠移動時的觸發區範圍
        private Size PickSize;
        // 區域變數
        Point temp_axis;
        Ball temp_ball;
        Queue<Queue<Ball>> clearlist;
        int[] XNullCount;
        bool[,] AxisNull;
        private bool tembool_1;
        private bool tembool_2;
        private bool allstandby;
        private bool[,] standby;
        private int step = 12;
        int score = 0;
        public Label label_score;

        #endregion

        // static Construction
        static BallAction()
        {
            // 位移調整
            offset = new Point(8, 30);
        }

        /// <summary>
        /// 初始化, 
        /// 需指定活動區域及版面上之轉珠
        /// </summary>
        /// <param name="ActiveArea"></param>
        public BallAction(Control ActiveArea, Ball[,] nowballs)
        {
            this.active_area = ActiveArea;
            this.nowballs = nowballs;
            // 初始選取區範圍
            PickSize = new Size(70, 60);
            IsDrag = false;

            ballaxis = new Ball[nowballs.GetLength(0), nowballs.GetLength(1)];
            standby = new bool[nowballs.GetLength(0), nowballs.GetLength(1)];

        }

        // 按下滑鼠之行為
        public void BallMouseDown(object sender, EventArgs e)
        {
            // 設定執行中標籤
            active_label = (CustomBallLabel)sender;

            // 無法操作則返回
            if (!active_label.ball.IsActive)
                return;

            // 將轉珠控制項移至最上層
            active_label.BringToFront();

            // 轉換標籤之座標至螢幕座標值
            picking_position = new Control().PointToScreen(active_label.Location);
            // 設定修正位移
            correction_offset = new Point(MousePosition.X - picking_position.X + offset.X, MousePosition.Y - picking_position.Y + offset.Y);

            // 註冊移動方法
            active_label.MouseMove += BallMove;
            // 註冊放開點擊方法
            active_label.MouseUp += BallMouseUp;

            // 開始拖移
            IsDrag = true;
        }

        // 移動滑鼠之行為
        void BallMove(object sender, EventArgs e)
        {
            // 計算移動之目標位置
            target_position = new Point(MousePosition.X - correction_offset.X, MousePosition.Y - correction_offset.Y);

            // 判斷是否超出移動區域           
            tembool_1 = target_position.X > active_area.Location.X && target_position.X < (active_area.Size.Width - active_area.Location.X - 6);
            tembool_2 = target_position.Y > active_area.Location.Y && target_position.Y < (active_area.Size.Height + active_area.Location.Y - 60);
            if (tembool_1)
                active_label.Location = new Point(target_position.X, active_label.Location.Y);
            if (tembool_2)
                active_label.Location = new Point(active_label.Location.X, target_position.Y);



            // 判斷是否觸發碰撞事件
            if (IsDrag)
            {
                // East
                if (active_label.Location.X >= (Ball.GetBallLocation(active_label.ball.Axis.X + 1, active_label.ball.Axis.Y).X - (PickSize.Width / 2)))
                {
                    FindAxisBall(active_label.ball.Axis.X + 1, active_label.ball.Axis.Y).Axis = new Point(active_label.ball.Axis.X, active_label.ball.Axis.Y);
                    active_label.ball.Axis = new Point(active_label.ball.Axis.X + 1, active_label.ball.Axis.Y);
                    FindAxisBall(active_label.ball.Axis.X - 1, active_label.ball.Axis.Y).SetBallLocation();
                }
                // West
                if (active_label.Location.X <= (Ball.GetBallLocation(active_label.ball.Axis.X - 1, active_label.ball.Axis.Y).X + (PickSize.Width / 2)))
                {
                    FindAxisBall(active_label.ball.Axis.X - 1, active_label.ball.Axis.Y).Axis = new Point(active_label.ball.Axis.X, active_label.ball.Axis.Y);
                    active_label.ball.Axis = new Point(active_label.ball.Axis.X - 1, active_label.ball.Axis.Y);
                    FindAxisBall(active_label.ball.Axis.X + 1, active_label.ball.Axis.Y).SetBallLocation();
                }
                // South
                if (active_label.Location.Y >= (Ball.GetBallLocation(active_label.ball.Axis.X, active_label.ball.Axis.Y + 1).Y - (PickSize.Height / 2)))
                {
                    FindAxisBall(active_label.ball.Axis.X, active_label.ball.Axis.Y + 1).Axis = new Point(active_label.ball.Axis.X, active_label.ball.Axis.Y);
                    active_label.ball.Axis = new Point(active_label.ball.Axis.X, active_label.ball.Axis.Y + 1);
                    FindAxisBall(active_label.ball.Axis.X, active_label.ball.Axis.Y - 1).SetBallLocation();
                }
                // North
                if (active_label.Location.Y <= (Ball.GetBallLocation(active_label.ball.Axis.X, active_label.ball.Axis.Y - 1).Y + (PickSize.Height / 2)))
                {
                    FindAxisBall(active_label.ball.Axis.X, active_label.ball.Axis.Y - 1).Axis = new Point(active_label.ball.Axis.X, active_label.ball.Axis.Y);
                    active_label.ball.Axis = new Point(active_label.ball.Axis.X, active_label.ball.Axis.Y - 1);
                    FindAxisBall(active_label.ball.Axis.X, active_label.ball.Axis.Y + 1).SetBallLocation();
                }
            }
        }

        // 放開滑鼠之行為
        void BallMouseUp(object sender, EventArgs e)
        {
            // 移動轉珠至轉珠區座標位置
            active_label.ball.SetBallLocation();

            // 撤銷移動方法
            active_label.MouseMove -= BallMove;
            // 撤銷放開點擊方法
            active_label.MouseUp -= BallMouseUp;

            // 開始消除轉珠
            if (IsDrag)
            {
                score = 0;
                this.ClearBall();
            }

            // 結束拖移
            IsDrag = false;
        }

        private Ball FindAxisBall(int x, int y)
        {
            foreach (var ball in nowballs)
            {
                if (ball.Axis.X == x && ball.Axis.Y == y)
                {
                    return ball;
                }
            }
            return null;
        }


        bool getclear;
        Timer timer_;
        Timer timer;
        // 判斷並消除轉珠區之轉珠
        void ClearBall()
        {
           
            // 停止操作
            foreach (var ball in nowballs)
            {
                ball.IsActive = false;
                ball.ResetClearData();
                ball.IsClear = false;
            }

            getclear = false;
            int Count, i, j, k, h, l;
            int number = 1;

            Queue<Ball> tempqueue;
            Queue<Ball> balllist = new Queue<Ball>();
            List<Queue<Ball>> balllistset = new List<Queue<Ball>>();
            clearlist = new Queue<Queue<Ball>>();

            for (i = 0; i < ballaxis.GetLength(0); i++)
            {
                for (j = 0; j < ballaxis.GetLength(1); j++)
                {
                    ballaxis[nowballs[i, j].Axis.X, nowballs[i, j].Axis.Y] = nowballs[i, j];
                }
            }

            for (i = 0; i < ballaxis.GetLength(0); i++)
            {
                for (j = 0; j < ballaxis.GetLength(1) - 1; j++)
                {
                    Count = 0;
                    if (ballaxis[i, j + 1].BallColor == ballaxis[i, j].BallColor)
                    {
                        for (k = j + 1; k < ballaxis.GetLength(1); k++)
                        {
                            if (ballaxis[i, k].BallColor == ballaxis[i, j].BallColor)
                            {
                                Count += 1;
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (Count >= 2)
                        {
                            for (k = j; k <= j + Count; k++)
                            {
                                ballaxis[i, k].cleardata.Vert = true;
                                ballaxis[i, k].cleardata.Number = number;
                                balllist.Enqueue(ballaxis[i, k]);
                            }
                            balllistset.Add(new Queue<Ball>(balllist));
                            foreach (Ball ball in balllist)
                            {
                                ball.cleardata.queue = balllistset[balllistset.Count - 1];
                            }
                            balllist.Clear();
                            number++;
                        }
                        j += Count;
                    }
                }
            }
            for (j = 0; j < ballaxis.GetLength(1); j++)
            {
                for (i = 0; i < ballaxis.GetLength(0) - 1; i++)
                {
                    Count = 0;
                    if (ballaxis[i + 1, j].BallColor == ballaxis[i, j].BallColor)
                    {
                        for (k = i + 1; k < ballaxis.GetLength(0); k++)
                        {
                            if (ballaxis[k, j].BallColor == ballaxis[i, j].BallColor)
                            {
                                Count += 1;
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (Count >= 2)
                        {
                            for (k = i; k <= i + Count; k++)
                            {
                                ballaxis[k, j].cleardata.Horz = true;
                                ballaxis[k, j].cleardata.Number = number;
                                balllist.Enqueue(ballaxis[k, j]);
                            }
                            balllistset.Add(new Queue<Ball>(balllist));
                            foreach (Ball ball in balllist)
                            {
                                ball.cleardata.queue = balllistset[balllistset.Count - 1];
                            }
                            balllist.Clear();
                            number++;
                        }
                        i += Count;
                    }
                }
            }

            for (j = 0; j < ballaxis.GetLength(1); j++)
            {
                for (i = 0; i < ballaxis.GetLength(0); i++)
                {
                    if (ballaxis[i, j].cleardata.Horz)
                    {
                        // Up & Down
                        for (k = j - 1; k < ballaxis.GetLength(1)&& k <= j + 1; k += 2)
                        {
                            if (k < 0)
                                continue;

                            if (ballaxis[i, k].BallColor == ballaxis[i, j].BallColor)
                            {
                                if (ballaxis[i, k].cleardata.Vert || ballaxis[i, k].cleardata.Horz)
                                {
                                    if (!ballaxis[i, k].cleardata.Linked)
                                    {
                                        tempqueue = ballaxis[i, k].cleardata.queue;
                                        for (h = ballaxis[i, k].cleardata.queue.Count, l = 0; l < h; l++)
                                        {
                                            temp_ball = tempqueue.Dequeue();
                                            temp_ball.cleardata.Number = ballaxis[i, j].cleardata.Number;
                                            temp_ball.cleardata.Linked = true;
                                            temp_ball.cleardata.queue = ballaxis[i, j].cleardata.queue;
                                            ballaxis[i, j].cleardata.queue.Enqueue(temp_ball);
                                        }
                                        if (!ballaxis[i, j].cleardata.Linked)
                                            foreach (Ball ball in ballaxis[i, j].cleardata.queue)
                                            {
                                                ball.cleardata.Linked = true;
                                            }
                                    }
                                    else // ballaxis[i, k].cleardata.Linked
                                    {

                                        if (!ballaxis[i, j].cleardata.Linked)
                                        {
                                            tempqueue = ballaxis[i, j].cleardata.queue;
                                            for (h = ballaxis[i, j].cleardata.queue.Count, l = 0; l < h; l++)
                                            {
                                                temp_ball = tempqueue.Dequeue();
                                                temp_ball.cleardata.Number = ballaxis[i, k].cleardata.Number;
                                                temp_ball.cleardata.Linked = true;
                                                temp_ball.cleardata.queue = ballaxis[i, k].cleardata.queue;
                                                ballaxis[i, k].cleardata.queue.Enqueue(temp_ball);
                                            }
                                        }
                                        else // ballaxis[i, k].cleardata.Linked && ballaxis[i, j].cleardata.Linked
                                        {
                                            if (!ballaxis[i, j].cleardata.queue.Equals(ballaxis[i, k].cleardata.queue))
                                            {
                                                tempqueue = ballaxis[i, k].cleardata.queue;
                                                for (h = ballaxis[i, k].cleardata.queue.Count, l = 0; l < h; l++)
                                                {
                                                    temp_ball = tempqueue.Dequeue();
                                                    temp_ball.cleardata.Number = ballaxis[i, j].cleardata.Number;
                                                    temp_ball.cleardata.queue = ballaxis[i, j].cleardata.queue;
                                                    ballaxis[i, j].cleardata.queue.Enqueue(temp_ball);
                                                }
                                            }
                                        }

                                    }
                                }
                                break;
                            }
                        }
                    }

                    if (ballaxis[i, j].cleardata.Vert)
                    {
                        // Left & Right
                        for (k = i - 1; k < ballaxis.GetLength(0)&& k <= i + 1; k += 2)
                        {
                            if (k < 0)
                                continue;

                            if (ballaxis[k, j].BallColor == ballaxis[i, j].BallColor)
                            {
                                if (ballaxis[k, j].cleardata.Vert || ballaxis[k, j].cleardata.Horz)
                                {
                                    if (!ballaxis[k, j].cleardata.Linked)
                                    {
                                        tempqueue = ballaxis[k, j].cleardata.queue;
                                        for (h = ballaxis[k, j].cleardata.queue.Count, l = 0; l < h; l++)
                                        {
                                            temp_ball = tempqueue.Dequeue();
                                            temp_ball.cleardata.Number = ballaxis[i, j].cleardata.Number;
                                            temp_ball.cleardata.Linked = true;
                                            temp_ball.cleardata.queue = ballaxis[i, j].cleardata.queue;
                                            ballaxis[i, j].cleardata.queue.Enqueue(temp_ball);
                                        }
                                        if (!ballaxis[i, j].cleardata.Linked)
                                            foreach (Ball ball in ballaxis[i, j].cleardata.queue)
                                            {
                                                ball.cleardata.Linked = true;
                                            }
                                    }
                                    else // ballaxis[k, j].cleardata.Linked
                                    {
                                        if (!ballaxis[i, j].cleardata.Linked)
                                        {
                                            tempqueue = ballaxis[i, j].cleardata.queue;
                                            for (h = ballaxis[i, j].cleardata.queue.Count, l = 0; l < h; l++)
                                            {
                                                temp_ball = tempqueue.Dequeue();
                                                temp_ball.cleardata.Number = ballaxis[k, j].cleardata.Number;
                                                temp_ball.cleardata.Linked = true;
                                                temp_ball.cleardata.queue = ballaxis[k, j].cleardata.queue;
                                                ballaxis[k, j].cleardata.queue.Enqueue(temp_ball);
                                            }
                                        }
                                        else // ballaxis[k, j].cleardata.Linked && ballaxis[i, j].cleardata.Linked
                                        {
                                            if (!ballaxis[i, j].cleardata.queue.Equals(ballaxis[k, j].cleardata.queue))
                                            {
                                                tempqueue = ballaxis[k, j].cleardata.queue;
                                                for (h = ballaxis[k, j].cleardata.queue.Count, l = 0; l < h; l++)
                                                {
                                                    temp_ball = tempqueue.Dequeue();
                                                    temp_ball.cleardata.Number = ballaxis[i, j].cleardata.Number;
                                                    temp_ball.cleardata.queue = ballaxis[i, j].cleardata.queue;
                                                    ballaxis[i, j].cleardata.queue.Enqueue(temp_ball);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            
            for (j = 0; j < ballaxis.GetLength(1); j++)
            {
                for (i = 0; i < ballaxis.GetLength(0); i++)
                {
                    if (ballaxis[i, j].cleardata.Number != 0)
                    {
                        getclear = true;
                        clearlist.Enqueue(ballaxis[i, j].cleardata.queue);
                        foreach (Ball ball in ballaxis[i, j].cleardata.queue)
                        {
                            ball.cleardata.Number = 0;
                        }
                    }
                }
            }

            //--------------------------------------------------

            XNullCount = new int[Ball.AxisSize.Width];
            AxisNull = new bool[Ball.AxisSize.Width, Ball.AxisSize.Height];
            for (i = 0; i < AxisNull.GetLength(0); i++)
            {
                XNullCount[i] = 0;
                for (j = 0; j < AxisNull.GetLength(1); j++)
                {
                    AxisNull[i, j] = false;
                    standby[i, j] = false;
                }
            }
            timer_ = new Timer();
            timer_.Interval = 250;
            timer_.Tick += ClearBallAni;
            timer_.Start();
        }

        private void AfterEffect()
        {
            int i, j, k;
            for (i = 0; i < ballaxis.GetLength(0); i++)
            {
                for (j = ballaxis.GetLength(1) - 1; j >= 0; j--)
                {
                    if (!ballaxis[i, j].IsClear)
                    {
                        standby[i, j] = true;
                        for (k = nowballs.GetLength(1) - 1; k > j; k--)
                        {
                            if (AxisNull[i, k] == true)
                            {
                                temp_axis = ballaxis[i, j].Axis;
                                ballaxis[i, j].Axis = ballaxis[i, k].Axis;
                                ballaxis[i, k].Axis = temp_axis;

                                temp_ball = ballaxis[i, j];
                                ballaxis[i, j] = ballaxis[i, k];
                                ballaxis[i, k] = temp_ball;
                                AxisNull[i, j] = true;
                                AxisNull[i, k] = false;
                                standby[i, j] = false;
                                break;
                            }
                        }
                    }
                }
            }

            active_area.SendToBack();
            timer = new Timer();
            timer.Interval = 2;
            timer.Tick += BallMoveAni;
            timer.Start();

        }

        private void KeepGoing()
        {
            if (getclear)
                ClearBall();
        }

        private void ClearBallAni(object state, EventArgs e)
        {
            if (clearlist.Count != 0)
            {
                score += 1;
                foreach (var ball in clearlist.Dequeue())
                {
                    
                    ball.IsClear = true;
                    XNullCount[ball.Axis.X] += 1;
                    AxisNull[ball.Axis.X, ball.Axis.Y] = true;
                    ball.BallLabel.SendToBack();
                    ball.BallLabel.Location = new Point(ball.BallLabel.Location.X, Ball.GetBallLocation(ball.Axis.X, -1).Y);
                    Ball.ToRandomColor(ball);
                    ball.BallLabel.ForeColor = ball.BallColor;                    
                }
                label_score.Text = "Score : " + score.ToString();
            }
            else
            {
                ((Timer)(state)).Stop();
                ((Timer)(state)).Tick -= ClearBallAni;
                ((Timer)(state)).Dispose();
                AfterEffect();
            }
        }

        private void BallMoveAni(object state, EventArgs e)
        {
            int i, j;
            allstandby = true;

            for (j = ballaxis.GetLength(1) - 1; j >= 0; j--)
            {
                for (i = 0; i < ballaxis.GetLength(0); i++)
                {
                    if (!standby[i, j])
                    {
                        if (j == Ball.AxisSize.Height - 1 || ballaxis[i, j + 1].BallLabel.Location.Y >= Ball.GetBallLocation(0, 0).Y)
                        {
                            ballaxis[i, j].BallLabel.Location = new Point(ballaxis[i, j].BallLabel.Location.X, ballaxis[i, j].BallLabel.Location.Y + step);
                            if (ballaxis[i, j].BallLabel.Location.Y >= Ball.GetBallLocation(i, j).Y - step)
                            {
                                ballaxis[i, j].SetBallLocation();
                                standby[i, j] = true;
                            }
                        }
                    }
                    if (standby[i, j] == false)
                    {
                        allstandby = false;
                    }
                }
            }
            if (allstandby)
            {
                for (i = 0; i < ballaxis.GetLength(0); i++)
                {
                    for (j = 0; j < ballaxis.GetLength(1); j++)
                    {
                        ballaxis[i, j].IsActive = true;
                    }
                }
                ((Timer)(state)).Stop();
                ((Timer)(state)).Tick -= BallMove;
                ((Timer)(state)).Dispose();
                KeepGoing();
            }
        }

        public void ChangeColor()
        {
            Color[,] ballcolor = new Color[Ball.AxisSize.Width, Ball.AxisSize.Height];
            int i, j;
            for (i = 0; i < ballaxis.GetLength(0); i++)
            {
                for (j = 0; j < ballaxis.GetLength(1); j++)
                {
                    nowballs[i, j].BallLabel.ForeColor = Ball.ToRandomColor(nowballs[i, j]);
                    ballaxis[nowballs[i, j].Axis.X, nowballs[i, j].Axis.Y] = nowballs[i, j];
                }
            }

            for (i = 0; i < ballaxis.GetLength(0); i++)
            {
                for (j = 0; j < ballaxis.GetLength(1); j++)
                {
                    ballcolor[i, j] = Color.Black;
                    if (j > 0)
                    {
                        while (ballaxis[i, j].BallColor.Equals(ballaxis[i, j - 1]))
                        {
                            ballcolor[i, j] = Ball.ToRandomColor(ballaxis[i, j]);
                            FindAxisBall(i, j).BallLabel.ForeColor = ballcolor[i, j];
                        }

                    }

                }
            }
            for (i = 0; i < ballaxis.GetLength(0); i++)
            {
                for (j = 0; j < ballaxis.GetLength(1); j++)
                {
                    if (i > 0)
                    {
                        while (ballaxis[i, j].BallColor.Equals(ballaxis[i - 1, j].BallColor) || ballaxis[i, j].BallColor.Equals(ballcolor[i, j]))
                        {
                            FindAxisBall(i, j).BallLabel.ForeColor = Ball.ToRandomColor(ballaxis[i, j]);
                        }
                    }
                }
            }
        }
    }
}
