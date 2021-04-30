using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    //界面、游戏画面绘制和游戏操作输入
    public partial class Form1 : Form
    {


         
        //[DllImport("testdll.dll", EntryPoint = "Add", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        //extern static int Add(int x, int y);




        //struct testS
        //{
        //    int[,] a;
        //    //    int a;
        //    double b;
        //};



        Timer fresh; //画面刷新计时
        //Timer fresh2; 
        Game game;
        //Graphics gra;
        Boolean ifSuggest = false;
        //public Boolean ifAI = false;

        int map_cell_width;
        int map_cell_height;

        public Form1(Boolean ifAi)
        {

            InitializeComponent();
            Text = "俄罗斯方块";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.BackColor = Color.Black;
            //gra = pictureBox1.CreateGraphics();
            game = new Game(this);


            if (ifAi == true) game.speed2.Enabled = true;
            else
            {
                game.speed2.Enabled = false;
            }



            map_cell_width = 30;
            map_cell_height = 30;

            KeyDown += new KeyEventHandler(Form1_KeyDown);
            fresh = new Timer();
            fresh.Tick += new System.EventHandler(fresh_Tick);
            fresh.Interval = 50 / 3;    //60帧
            fresh.Enabled = true;


            //fresh2 = new Timer();
            //fresh2.Tick += new System.EventHandler(display);
            //fresh2.Interval = 50 / 3;    //60帧
            //fresh2.Enabled = true;
        }
        //public void NewGame()
        //{
        //    game = new Game();
        //}
        private void fresh_Tick(object sender, EventArgs e)
        {
            Draw();
        }

        private void fresh_Tick2(object sender, EventArgs e)
        {
            Draw2();
        }









        private void Draw() //游戏画面绘制
        {
            //初始化画布
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            SolidBrush s;

            g.DrawRectangle(Pens.White, map_cell_width * 4 - 3 - 80, map_cell_height * 4 - 3 - 80, map_cell_width * game.map.Width + 3, map_cell_height * game.map.Height + 3);
            //g.DrawRectangle(Pens.White, map_cell_width * 4 - 3 , map_cell_height * 4 - 3 , map_cell_width * game.map.Width + 3, map_cell_height * game.map.Height + 3);
            //画地图
            for (int i = 4; i < game.map.Height + 4; i++) //(int i = 0; i < game.map.Height + 8; i++)
            {
                for (int j = 4; j < game.map.Width + 4; j++)
                {
                    if (game.map.Mod[i, j].p == 1)
                    {
                        s = new SolidBrush(game.map.Mod[i, j].c);
                        g.FillRectangle(s, map_cell_width * j - 80, map_cell_height * i - 80, map_cell_width - 2, map_cell_height - 2);
                    }
                }
            }


            s = new SolidBrush(game.curBrick.col);
            //画方块
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (game.curBrick.Mod[i, j] == 1)
                    {
                        if (i + game.curBrick.Pos.Y > 3)
                        {
                            //(i,j)方块内的点阵位置 Pos是方块左上角点的地图位置
                            g.FillRectangle(s, map_cell_width * (j + game.curBrick.Pos.X) - 80, map_cell_height * (i + game.curBrick.Pos.Y) - 80, map_cell_width - 2, map_cell_height - 2);
                        }

                    }
                }
            }
            //画下一个
            s = new SolidBrush(game.nextBrick.col);
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (game.nextBrick.Mod[i, j] == 1)
                    {
                        //g.FillRectangle(s, map_cell_width * j + 380, map_cell_height * i + 50, map_cell_width - 2, map_cell_height - 2);
                        g.FillRectangle(s, map_cell_width * j + 380, map_cell_height * i + 80, map_cell_width - 2, map_cell_height - 2);
                    }
                }
            }
            //绘制文字
            g.DrawString("NEXT：", Font, Brushes.White, 380, 40);
            //g.DrawString("游戏说明：\n\n   通过方向键控制砖块。\n   向上键：变形\n   向左键：左移\n   向下键：下移\n   向右键：右移\n\n当前分数：", Font, Brushes.White, 380, 200);
            g.DrawString("游戏说明：\n\n   通过方向键控制砖块。\n   向上键：变形\n   向左键：左移\n   向下键：下移\n   向右键：右移\n      N键：下落减速\n      M键：下落加速\n      X键：开启提示\n    \n当前分数：", Font, Brushes.White, 380, 160);
            Font f = new Font("微软雅黑", 32, Font.Style | FontStyle.Bold);
            g.DrawString(game.score.ToString(), f, Brushes.Snow, 370, 320);
            g.DrawString("当前速度", Font, Brushes.White, 380, 380);
            g.DrawString((1000-game.speed.Interval).ToString(), f, Brushes.Snow, 370, 390);
            f = new Font("微软雅黑", 25, Font.Style | FontStyle.Bold);
            if (ifSuggest==true)
            {
                g.DrawString("提示已开", f, Brushes.YellowGreen, 370, 445);
            }
            else
            {
                g.DrawString("提示关闭", f, Brushes.YellowGreen, 370, 445);
            }
            
            //g.DrawString(game.score.ToString(), f, Brushes.Snow, 420, 340);
            pictureBox1.BackgroundImage = bmp;
            pictureBox1.Refresh();
        }
        //按键部分
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (game.speed.Enabled == true)
            {
                return false;
            }
            return base.ProcessDialogKey(keyData);
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Keys key = e.KeyCode;
            if (key == Keys.Up)
            {
                pictureBox1.Refresh();
                //game.Move(Towards.UP);
                game.Rotate();
                Draw();
            }
            if (key == Keys.Left)
            {
                pictureBox1.Refresh();
                game.Move(Towards.LETF);
                Draw();
            }
            if (key == Keys.Right)
            {
                pictureBox1.Refresh();
                game.Move(Towards.RIGHT);
                Draw();
            }
            if (key == Keys.Down)
            {
                pictureBox1.Refresh();
                game.Move(Towards.DOWN);
                Draw();
            }
            if (key == Keys.Space)
            {
                game.Rotate();
            }
            if (key == Keys.P)
            {
                game.Pause();
            }

            if (key == Keys.Q)
            {
                if(game.speed2.Enabled==true) game.speed2.Enabled = false;
                else
                {
                    game.speed2.Enabled = true;
                }
            }


            if (key == Keys.N)
            {
                game.speed.Interval += 2;
            }

            if (key == Keys.M)
            {
                if(game.speed.Interval - 5>0)
                    game.speed.Interval -= 5;
                else if (game.speed.Interval - 1 > 0)
                    game.speed.Interval -= 1;
            }


            if (key == Keys.X)
            {
                if (ifSuggest == false)
                {
                    fresh.Tick -= new System.EventHandler(fresh_Tick);
                    fresh.Tick += new System.EventHandler(fresh_Tick2);
                    ifSuggest = true;
                }
                else
                {
                    fresh.Tick -= new System.EventHandler(fresh_Tick2);
                    fresh.Tick += new System.EventHandler(fresh_Tick);
                    ifSuggest = false;
                }
            }


            
            //if (key == Keys.Q)
            //{
            //    display1();
            //}
            //label3.Text = game.curBrick.Pos.X.ToString() + "," + game.curBrick.Pos.Y.ToString();
        }





        private void Draw2()
        {
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            SolidBrush s;
            SolidBrush s2;

            g.DrawRectangle(Pens.White, map_cell_width * 4 - 3 - 80, map_cell_height * 4 - 3 - 80, map_cell_width * game.map.Width + 3, map_cell_height * game.map.Height + 3);
            //g.DrawRectangle(Pens.White, map_cell_width * 4 - 3 , map_cell_height * 4 - 3 , map_cell_width * game.map.Width + 3, map_cell_height * game.map.Height + 3);
            //画地图
            for (int i = 4; i < game.map.Height + 4; i++) //(int i = 0; i < game.map.Height + 8; i++)
            {
                for (int j = 4; j < game.map.Width + 4; j++)
                {
                    if (game.map.Mod[i, j].p == 1)
                    {
                        s = new SolidBrush(game.map.Mod[i, j].c);
                        g.FillRectangle(s, map_cell_width * j - 80, map_cell_height * i - 80, map_cell_width - 2, map_cell_height - 2);
                    }
                }
            }


            s = new SolidBrush(game.curBrick.col);
            //画方块
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (game.curBrick.Mod[i, j] == 1)
                    {
                        if (i + game.curBrick.Pos.Y > 3)
                        {
                            //(i,j)方块内的点阵位置 Pos是方块左上角点的地图位置
                            g.FillRectangle(s, map_cell_width * (j + game.curBrick.Pos.X) - 80, map_cell_height * (i + game.curBrick.Pos.Y) - 80, map_cell_width - 2, map_cell_height - 2);
                        }

                    }
                }
            }
            //画下一个
            s = new SolidBrush(game.nextBrick.col);
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (game.nextBrick.Mod[i, j] == 1)
                    {
                        //g.FillRectangle(s, map_cell_width * j + 380, map_cell_height * i + 50, map_cell_width - 2, map_cell_height - 2);
                        g.FillRectangle(s, map_cell_width * j + 380, map_cell_height * i + 80, map_cell_width - 2, map_cell_height - 2);
                    }
                }
            }


            //画建议位置
            s = new SolidBrush(Color.White);
            int x = 6 + game.moveCopy;
            int[,] brick = game.curBrick.assignShapeValue(game.curBrick.shape1, game.rotateCopy);
            int y = (Intersect1(brick)-1);
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (brick[i, j] == 1)
                    {
                        if (i + y > 3)
                        {
                            //(i,j)方块内的点阵位置 Pos是方块左上角点的地图位置
                            g.FillRectangle(s, map_cell_width * (j + x) - 80, map_cell_height * (i + y) - 80, map_cell_width - 2, map_cell_height - 2);
                        }

                    }
                }
            }



            //绘制文字
            g.DrawString("NEXT：", Font, Brushes.White, 380, 40);
            g.DrawString("游戏说明：\n\n   通过方向键控制砖块。\n   向上键：变形\n   向左键：左移\n   向下键：下移\n   向右键：右移\n      N键：下落减速\n      M键：下落加速\n      X键：开启提示\n    \n当前分数：", Font, Brushes.White, 380, 160);
            Font f = new Font("微软雅黑", 32, Font.Style | FontStyle.Bold);
            g.DrawString(game.score.ToString(), f, Brushes.Snow, 370, 320);
            g.DrawString("当前速度", Font, Brushes.White, 380, 380);
            g.DrawString((1000-game.speed.Interval).ToString(), f, Brushes.Snow, 370, 390);
            f = new Font("微软雅黑", 25, Font.Style | FontStyle.Bold);
            if (ifSuggest == true)
            {
                g.DrawString("提示已开", f, Brushes.YellowGreen, 370, 445);
            }
            else
            {
                g.DrawString("提示关闭", f, Brushes.YellowGreen, 370, 445);
            }

            //g.DrawString(game.score.ToString(), f, Brushes.Snow, 420, 340);
            pictureBox1.BackgroundImage = bmp;
            pictureBox1.Refresh();
        }



        public int Intersect1(int[,] brick) //相交测试
        {
            int x = 6 + game.moveCopy;
            int y = 0;
            for (int z = 1; z <= 20; z++)
            {
                for (int i = 0; i < 5; i++)
                    for (int j = 0; j < 5; j++)
                        if (brick[i, j] + game.map.Mod[z + i, x + j].p == 2)
                        {
                            y = z;
                            return y;
                        }
            }
            return -10;
        }



        //private void display1()
        //{
        //    //获取当前MAP数组的值

        //    //int count = EvaluateShape(gameMap, brickMap);
        //    //label6.Text = count.ToString();
        //    //for (int i = 0; i < 15; i++) //(int i = 0; i < game.map.Height + 8; i++)
        //    //{
        //    //    for (int j = 0; j < 10; j++)
        //    //    {
        //    //        if (gameMap1[i, j] == 1)
        //    //        {
        //    //            label1.Text += "1";
        //    //        }
        //    //        else
        //    //        {
        //    //            label1.Text += "0";
        //    //        }
        //    //    }
        //    //    label1.Text += "\n";
        //    //}


        //    label1.Text = "";
        //    for (int i = 4; i < game.map.Height + 4; i++) //(int i = 0; i < game.map.Height + 8; i++)
        //    {
        //        for (int j = 4; j < game.map.Width + 4; j++)
        //        {
        //            if (game.map.Mod[i, j].p == 1)
        //            {
        //                label1.Text += "1";
        //            }
        //            else
        //            {
        //                label1.Text += "0";
        //            }
        //        }
        //        label1.Text += "\n";
        //    }
        //    //获取当前方块数组的值
        //    label2.Text = "";
        //    //for (int i = 0; i < 5; i++)
        //    //{
        //    //    for (int j = 0; j < 5; j++)
        //    //    {
        //    //        if (game.curBrick.Mod[i, j] == 1)
        //    //        {
        //    //            if (i + game.curBrick.Pos.Y > 3)
        //    //            {
        //    //                label2.Text += "1";
        //    //            }
        //    //        }
        //    //        else
        //    //        {
        //    //            label2.Text += "0";
        //    //        }
        //    //    }
        //    //    label2.Text += "\n";
        //    //}

        //    //int[,] brick = game.curBrick.assignShapeValue(game.curBrick.shape1, (game.curBrick.shape2 + 1) % 4);
        //    //for (int i = 0; i < 5; i++)
        //    //{
        //    //    for (int j = 0; j < 5; j++)
        //    //    {
        //    //        if (brick[i, j] == 1)
        //    //        {
        //    //            label2.Text += "1";
        //    //        }
        //    //        else
        //    //        {
        //    //            label2.Text += "0";
        //    //        }
        //    //    }
        //    //    label2.Text += "\n";
        //    //}


        //    int[,] brick = game.curBrick.Mod;
        //    for (int i = 0; i < 5; i++)
        //    {
        //        for (int j = 0; j < 5; j++)
        //        {
        //            if (brick[i, j] == 1)
        //            {
        //                label2.Text += "1";
        //            }
        //            else
        //            {
        //                label2.Text += "0";
        //            }
        //        }
        //        label2.Text += "\n";
        //    }


        //    //int[,] b1 = game.curBrick.assignShapeValue(game.curBrick.shape1, (game.curBrick.shape2 + 1) % 4);
        //    //int[,] b2 = game.curBrick.assignShapeValue(game.curBrick.shape1, (game.curBrick.shape2 + 2) % 4);
        //    //int[,] b3 = game.curBrick.assignShapeValue(game.curBrick.shape1, (game.curBrick.shape2 + 3) % 4);
        //    //    label5.Text = "";
        //    //    for (int i = 0; i < 5; i++)
        //    //    {
        //    //        for (int j = 0; j < 5; j++)
        //    //        {
        //    //            //if (b1[i, j] == 1)
        //    //            if (game.curBrick.shape[game.curBrick.shape1, game.curBrick.shape2+1,i, j] == 1)
        //    //            {
        //    //                label5.Text += "1";
        //    //            }
        //    //            else
        //    //            {
        //    //                label5.Text += "0";
        //    //            }
        //    //        }
        //    //        label5.Text += "\n";
        //    //    }


        //    //    label6.Text = "";
        //    //    for (int i = 0; i < 5; i++)
        //    //    {
        //    //        for (int j = 0; j < 5; j++)
        //    //        {
        //    //            //if (b2[i, j] == 1)
        //    //            if (game.curBrick.shape[game.curBrick.shape1, game.curBrick.shape2 + 2, i, j] == 1)
        //    //            {
        //    //                label6.Text += "1";
        //    //            }
        //    //            else
        //    //            {
        //    //                label6.Text += "0";
        //    //            }
        //    //        }
        //    //        label6.Text += "\n";
        //    //    }

        //    //    label7.Text = "";
        //    //    for (int i = 0; i < 5; i++)
        //    //    {
        //    //        for (int j = 0; j < 5; j++)
        //    //        {
        //    //            //if (b3[i, j] == 1)
        //    //            if (game.curBrick.shape[game.curBrick.shape1, game.curBrick.shape2 + 3, i, j] == 1)
        //    //            {
        //    //                label7.Text += "1";
        //    //            }
        //    //            else
        //    //            {
        //    //                label7.Text += "0";
        //    //            }
        //    //        }
        //    //        label7.Text += "\n";
        //    //    }

        //    //}

        //    //当前坐标、格局、方块数据显示
        //    //private void display(object sender, EventArgs e)
        //    //{
        //    //    //显示当前方块的XY坐标
        //    //    label3.Text = game.curBrick.Pos.X.ToString() + "," + game.curBrick.Pos.Y.ToString();
        //    //}





        //    //测试DLL函数
        //////private void button1_Click_1()
        //////{
        //////    label5.Text = Add(10, 20).ToString();
        //////}


        //}

    }
}
