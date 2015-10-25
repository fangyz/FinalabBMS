using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.IO;

namespace Login
{
    public class VCode
    {
        /// <summary>
        /// 生成验证码图片 字节数组
        /// </summary>
        /// <returns></returns>
        public byte[] GetVCode()
        {
            using (Image img = new Bitmap(80, 30))
            {
                string strCode = GetRandomStr();
                HttpContext.Current.Session["vcode"] = strCode;
                using (Graphics g = Graphics.FromImage(img))
                {
                    g.Clear(Color.White);
                    g.DrawRectangle(Pens.Blue, 0, 0, img.Width - 1, img.Height - 1);
                    DrawPoint(g);
                    g.DrawString(strCode, new Font("微软雅黑", 15), Brushes.Blue, new PointF(5, 2));
                    DrawPoint(g);
                    using (System.IO.MemoryStream ms = new MemoryStream())
                    {
                        //将图片 保存到内存流中
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        //将内存流 里的 数据  转成 byte 数组 返回
                        return ms.ToArray();
                    }
                }
            }
        }

        //拿到图片里面的内容
        Random random = new Random();
        string GetRandomStr()
        {
            string str = string.Empty;
            string[] strArr = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0","a","b","c","d"};
            for (int i = 0; i < 4; i++)
            {
                int index = random.Next(strArr.Length);
                str += strArr[index];
            }
            return str;
        }
        //为图片画线
        void DrawPoint(Graphics g)
        {
            Pen[] pens = { Pens.Blue, Pens.Black, Pens.Red, Pens.Green };
            Point p1;
            Point p2;
            int length = 1;
            for (int i = 0; i < 50; i++)
            {
                p1 = new Point(random.Next(79), random.Next(29));
                p2 = new Point(p1.X - length, p1.Y - length);
                length = random.Next(5);
                g.DrawLine(pens[random.Next(pens.Length)], p1, p2);
            }
        }
    }
}