using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;

namespace RollBallGame
{
    /// <summary>
    /// 轉珠顏色列舉
    /// </summary>
    public enum BallColorType : int
    {
        red = 1,
        green = 2,
        blue = 3,
        yellow = 4,
        purple = 5,
        pink = 6
    };

    /// <summary>
    /// 轉珠標籤控制項型別
    /// </summary>
    public class CustomBallLabel : Label
    {
        /// <summary>
        /// 取得此標籤之轉珠型別
        /// </summary>
        public Ball ball { get; set; }

        // static Construction
        public CustomBallLabel(Ball ball)
        {
            this.ball = ball;

            this.Text = Ball.Img.ToString();
            this.Size = Ball.Origin.Size;
            this.Font = Ball.Origin.Font;
            this.BackColor = Ball.Origin.BackColor;

            this.ForeColor = this.ball.BallColor;

            // Label樣式調整
            this.Padding = new Padding(7, 1, 0, 0);
            this.TextAlign = ContentAlignment.MiddleCenter;
        }
    }

    /// <summary>
    /// 轉珠型別
    /// </summary>
    public class Ball
    {
        #region public static variable

        /// <summary>
        /// 取得字元(圖像)'●'
        /// </summary>
        public static char Img { get { return '●'; } }

        /// <summary>
        /// 轉珠控制項之基底樣式
        /// </summary>
        public static Control Origin { get; }

        /// <summary>
        /// 取得轉珠模版之尺寸
        /// </summary>
        public static Size AxisSize { get; }

        /// <summary>
        /// 轉珠區之控制項
        /// </summary>
        public static Control PlayArea { get; set; }

        #endregion

        #region public local variable

        /// <summary>
        /// 取得或設定轉珠之顏色
        /// </summary>
        public Color BallColor { get; set; }

        /// <summary>
        /// 轉珠之標籤控制項
        /// </summary>
        public CustomBallLabel BallLabel { get; set; }

        /// <summary>
        /// 轉珠位於轉珠區的位置
        /// </summary>
        public Point Axis { get; set; }

        /// <summary>
        /// 取得轉珠是否正在執行
        /// </summary>
        public bool IsActive { get; set; }

        #endregion

        #region private static variable

        // Random實體物件,用於產生隨機顏色
        private static Random ran = new Random();
        

        #endregion

        #region private local variable

        public bool IsClear;

        // 用於處理消除轉珠的資料
        //暫時P

        // 用於處理消除轉珠之資料的結構
        public struct ClearData
        {
            public bool Horz;
            public bool Vert;
            public bool Linked;
            public int Number;
            public Queue<Ball> queue;
        }

        public ClearData cleardata;

        #endregion

        // static Construction
        static Ball()
        {
            // 建立實體物件
            PlayArea = new Control();
            Origin = new Control();
            // 初始尺寸
            Origin.Size = new Size(70, 60);
            // 初始字型
            Origin.Font = new Font("新細明體", 44F);
            // 初始背景顏色
            Origin.BackColor = SystemColors.Control;

            // 座標尺寸
            AxisSize = new Size(6, 5);
        }

        #region Construction

        /// <summary>
        /// 基本建構式, 
        /// 無引數則顏色隨機
        /// </summary>
        public Ball()
        {
            Ball.ToRandomColor(this);
            //this.BallLabel = new CustomBallLabel(this);
            IsClear = false;
            IsActive = true;
            this.ResetClearData();
        }
        /// <summary>
        /// 設定初始轉珠顏色
        /// </summary>
        public Ball(BallColorType color) : this()
        {
            this.BallColor = IntToBallColor((int)color);
        }
        /// <summary>
        /// 設定初始轉珠顏色
        /// </summary>
        public Ball(Color color) : this()
        {
            this.BallColor = color;
        }
        /// <summary>
        /// 設定初始轉珠顏色
        /// </summary>
        public Ball(int color) : this()
        {
            this.BallColor = IntToBallColor(color);
        }
        /// <summary>
        /// 設定初始建構為引數之Ball
        /// </summary>
        public Ball(Ball ball)
        {
            this.BallColor = ball.BallColor;
            //this.BallLabel = new CustomBallLabel(ball);
            this.IsClear = ball.IsClear;
            this.IsActive = ball.IsActive;
            this.ResetClearData();
        }
        #endregion

        #region public static Method

        /// <summary>
        /// 將整數轉換為轉珠用顏色
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color IntToBallColor(int color)
        {
            switch (color)
            {
                case (int)BallColorType.blue:
                    return Color.Blue;
                case (int)BallColorType.green:
                    return Color.Green;
                case (int)BallColorType.pink:
                    return Color.Pink;
                case (int)BallColorType.purple:
                    return Color.Purple;
                case (int)BallColorType.red:
                    return Color.Red;
                case (int)BallColorType.yellow:
                    return Color.Yellow;
            }
            return Color.Black;
        }

        /// <summary>
        /// 隨機設定轉珠顏色值
        /// </summary>
        /// <param name="ball"></param>
        public static Color ToRandomColor(Ball ball)
        {
            ball.BallColor = IntToBallColor(ran.Next(1, 7));
            return ball.BallColor;
        }

        /// <summary>
        /// 返回相對於轉珠區之螢幕座標值
        /// </summary>
        /// /// <returns></returns>
        public static Point GetBallLocation(int x, int y)
        {
            return new Point(PlayArea.Location.X + (x * PlayArea.Width / AxisSize.Width) + ((PlayArea.Width / AxisSize.Width - Origin.Width) / 2), PlayArea.Location.Y + y * PlayArea.Height / AxisSize.Height + ((PlayArea.Height / AxisSize.Height - Origin.Height) / 2));
        }
        /// <summary>
        /// 返回相對於轉珠區之螢幕座標值
        /// </summary>
        /// /// <returns></returns>
        public static Point GetBallLocation(Point axis)
        {
            return GetBallLocation(axis.X, axis.Y);
        }         

        #endregion

        #region public local Method

        /// <summary>
        /// 返回相對於轉珠區座標之螢幕座標值
        /// </summary>
        /// <returns></returns>
        public Point GetBallLocation()
        {
            return GetBallLocation(this.Axis);
        }

        /// <summary>
        /// 設定轉珠之標籤位置為Axis變數之螢幕座標值, 
        /// </summary>
        public void SetBallLocation()
        {
            this.BallLabel.Location = this.GetBallLocation();
        }
        /// <summary>
        /// 設定轉珠之標籤位置為螢幕座標值, 
        /// 預設為轉珠內之Axis變數
        /// </summary>
        public void SetBallLocation(int x, int y)
        {
            this.BallLabel.Location = GetBallLocation(x, y);
        }
        /// <summary>
        /// 設定轉珠之標籤位置為螢幕座標值, 
        /// 預設為轉珠內之Axis變數
        /// </summary>
        public void SetBallLocation(Point axis)
        {
            this.BallLabel.Location = GetBallLocation(axis);
        }

        #endregion

        #region private local Method

        // 初始化ClearData
        public void ResetClearData()
        {
            cleardata.Horz = false;
            cleardata.Vert = false;
            cleardata.Linked = false;
            cleardata.Number = 0;
            cleardata.queue = null;
        }

        #endregion

    }
}
