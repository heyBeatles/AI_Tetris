using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;






namespace Tetris
{
    //游戏的核心算法
    //进行游戏的逻辑更新


    public enum Towards { UP = 0, RIGHT, DOWN, LETF }; //方向
    class Game
    {

        [DllImport("AI_for_Tetris.dll", EntryPoint = "EvaluateShape", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static int EvaluateShape(int[,] game, int[,] brick);
        [DllImport("AI_for_Tetris.dll", EntryPoint = "EvaluateShape2", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        extern static int EvaluateShape2(int[,] game, int[,] brick);

        static int a = (int)DateTime.Now.Ticks;
        //Random ran = new Random(401024353);
        
        public int move=0;
        public int moveCopy = 0;
        public int rotateCopy = 0;
        int rotate = 0;
        public Map map;
        public Brick curBrick;
        public Brick nextBrick;
        public Timer speed; //更新速度
        public Timer speed2; //更新速度
        public static Random random = new Random(488988057); //整个游戏只用random来产生随机数，避免出现同时调用多个随机数生成器由相同或相近的时间种子获得相同的随机数的情况
        public int score = 0;
        public Form1 frm1;
        public Game(Form1 frm1)
        {
            map = new Map(10,15);
            //map = new Map(15, 5);
            CreateBrick();
            nextBrick = new Brick(0);
            speed = new Timer();
            speed.Tick += new EventHandler(speed_Tick);
            speed.Interval = 250;
            speed.Enabled = true;


            speed2 = new Timer();
            speed2.Tick += new EventHandler(movHorz);
            speed2.Interval = 1;
            speed2.Enabled = true;

            System.Media.SoundPlayer sp = new System.Media.SoundPlayer();
            sp.SoundLocation = @".\Sound.wav";
            //if (System.IO.File.Exists(@".\Sound.wav")) sp.PlayLooping();
            this.frm1 = frm1;
        }
        private void speed_Tick(object sender, EventArgs e)
        {
            if (!Drop())
            {
                //Draw2();
                PutBrick2Map();
                ClearLine();
                if (!NewFall())
                {
                    speed.Enabled = false;
                    MessageBox.Show("GAME OVER", "提示");
                    //Application.Exit();
                }
            }
        }



        public void Pause()
        {
            if (speed.Enabled == true)
                speed.Enabled = false;
            else
                speed.Enabled = true;
        }
        public bool Intersect() //相交测试
        {
            bool flag = false;
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    if (curBrick.tMod[ i, j] + map.Mod[curBrick.tPos.Y + i, curBrick.tPos.X + j].p == 2)
                        flag = true;
            return flag;
        }
        public void PutBrick2Map()//将当前方块放入地图
        {
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    if (curBrick.Mod[i, j] == 1)
                    {
                        map.Mod[i + curBrick.Pos.Y, j + curBrick.Pos.X].p = curBrick.Mod[i, j];
                        map.Mod[i + curBrick.Pos.Y, j + curBrick.Pos.X].c = curBrick.col;
                    }
        }
        public void Rotate()//旋转
        {
            curBrick.RotateTestMod();//旋转测试模式
            if(!Intersect())
            {
                curBrick.ChangeMod();
                curBrick.shape2 = (curBrick.shape2 + 1) % 4;
            }

            //this.shape++;
            //shape = shape % 4;
        }
        public void CreateBrick()
        {
            //此处为了开始下落像AI固定开始方块和旋转
            curBrick = new Brick(6);
            curBrick.Mod = curBrick.assignShapeValue(6, 2);
            rotateCopy = 2;
            //curBrick.shape2 = 2;


            //curBrick = new Brick(random.Next(19));
            curBrick.Pos.X = (map.Width + 8 / 2) - 8;
            curBrick.Pos.Y = 2;
        }
        public bool NewFall()//新的方块下落
        {
            //move = 5;

            curBrick = nextBrick;
            curBrick.Pos.X = (map.Width + 8 / 2) - 8;
            curBrick.Pos.Y = 1;
            //nextBrick = new Brick(6);
            nextBrick = new Brick(random.Next(19));
            curBrick.MoveTestMod();//移动测试模式

            double[] p = new double[4];
            int[] m = new int[4];
            int[] mm = new int[4];
            //int p1,p2,p3,p4,m1, m2, m3,m4,m11,m22,m33,m44;
            int[,] gameMap = returnMapArray();
            int[,] brickMap = curBrick.Mod;
            int[,] b1 = curBrick.assignShapeValue(curBrick.shape1, (curBrick.shape2 + 1)%4);
            int[,] b2 = curBrick.assignShapeValue(curBrick.shape1, (curBrick.shape2 + 2)%4);
            int[,] b3 = curBrick.assignShapeValue(curBrick.shape1, (curBrick.shape2 + 3)%4);
            //int[,] brickMap = returnBrickArray();
            //int[,] b1 = Rotate2(brickMap);
            //int[,] b2 = Rotate2(b1);
            //int[,] b3 = Rotate2(b2);
            p[0] = EvaluateShape2(gameMap, brickMap);
            p[1] = EvaluateShape2(gameMap, b1);
            p[2] = EvaluateShape2(gameMap, b2);
            p[3] = EvaluateShape2(gameMap, b3);
            m[0] = EvaluateShape(gameMap, brickMap);
            m[1] = EvaluateShape(gameMap, b1);
            m[2] = EvaluateShape(gameMap, b2);
            m[3] = EvaluateShape(gameMap, b3);
            mm[0] = Math.Abs(m[0]);
            mm[1] = Math.Abs(m[1]);
            mm[2] = Math.Abs(m[2]);
            mm[3] = Math.Abs(m[3]);
            double maxP = p.Max();
            if (p[0]==maxP)
            {
                rotateCopy = 0;
                rotate = 0;
                move = m[0];
                moveCopy= m[0];
                
            }
            else if (p[1] == maxP)
            {
                rotateCopy = 1;
                rotate = 1;
                move = m[1];
                moveCopy = m[1];
            }
            else if (p[2] == maxP)
            {
                rotateCopy = 2;
                rotate = 2;
                move = m[2];
                moveCopy = m[2];
            }
            else
            {
                rotateCopy = 3;
                rotate = 3;
                move = m[3];
                moveCopy = m[3];
            }
            //if (m11<=m22&&m11<=m33&m11<=m44)
            //{
            //    move = m1;
            //    rotate = 0;
            //}
            //else if (m22 <= m11 && m22 <= m33 && m22 <= m44)
            //{
            //    move = m2;
            //    rotate = 1;
            //}
            //else if (m33 <= m11 && m33 <= m22 && m33 <= m44)
            //{
            //    move = m3;
            //    rotate = 2;
            //}
            //else
            //{
            //    move = m4;
            //    rotate = 3;
            //}
            //move = EvaluateShape(gameMap, brickMap);
            return !Intersect();
        }



        public bool Drop()
        {
            bool flag = false;
            curBrick.MoveTestMod();
            curBrick.MoveTestDown();


            


            if (!Intersect())
            {
                //显示当前方块的XY坐标
                //frm1.label3.Text = curBrick.Pos.X.ToString() + "," + curBrick.Pos.Y.ToString();
                flag = true;
                curBrick.ChangePos();//改变位置
            }
            else
            {
                curBrick.InitialtPos();
            }
            return flag;
        }


        public void movHorz(object sender, EventArgs e)
        {
            curBrick.MoveTestMod();
            if (rotate>0)
            {
                curBrick.RotateTestMod();
                curBrick.ChangeMod();
                rotate--;
                curBrick.shape2++;
            }
            if (move!=0)
            {
                if (move < 0)
                {
                    curBrick.MoveTestLeft();
                    move++;
                }
                else
                {
                    curBrick.MoveTestRight();
                    move--;
                }
            }
            


            if (!Intersect())
            {
                curBrick.ChangePos();//改变位置
            }
            else
            {
                curBrick.InitialtPos();
            }


            //if (!Intersect())
            //{
            //    curBrick.ChangePos();//改变位置
            //}
            //else
            //{
            //    curBrick.InitialtPos();
            //}
        }


        public void Move(Towards dir)//移动方块,返回值
        {
            curBrick.MoveTestMod();
            if (dir == Towards.LETF)
                curBrick.MoveTestLeft();
            if (dir == Towards.RIGHT)
                curBrick.MoveTestRight();
            if (dir == Towards.UP)//呵呵
                curBrick.MoveTestUp();
            if (dir == Towards.DOWN)
                curBrick.MoveTestDown();
            if (!Intersect())
            {
                curBrick.ChangePos();//改变位置
            }
            else
            {
                curBrick.InitialtPos();
            }
        }
        public void ClearLine()
        {
            //清除掉满行
            //这个函数实际上效率也很低的,为了简便它从头到尾作了测试
            //具体的算法为:
            //从游戏区域第0行开始到最后一行,测试地图点阵是否为满,如果是的话
            //从当前行算起,之上的地图向下掉一行
            int i, dx, dy;
            bool fullflag;
            for (i = 4; i < map.Height + 4; i++)//最后一行保留行
            {
                fullflag = true;//假设为满
                for (int j = 4; j < map.Width + 4; j++)//保留列
                { 
                    if (map.Mod[i,j].p == 0)
                    {
                        fullflag = false;
                        break;
                    }
                }//找出第i行为满
                if (fullflag)
                { 
                    score += 10;
                    for (dy = i; dy > 0; dy--)
                        for (dx = 4; dx < map.Width + 4; dx++)
                            map.Mod[dy, dx] = map.Mod[dy - 1, dx];//向下移动一行
                    for (dx = 4; dx < map.Width + 4; dx++)
                        map.Mod[0, dx].p = 0;//并清除掉第一行
                }
            }
        }




        //返回当前地图二维数组
        private int[,] returnMapArray()
        {
            int[,] outMap = new int[15, 10];
            int n = 0, m = 0;
            for (int i = 4; i < map.Height + 4; i++) //(int i = 0; i < game.map.Height + 8; i++)
            {
                m = 0;
                for (int j = 4; j < map.Width + 4; j++)
                {
                    if (map.Mod[i, j].p == 1)
                    {
                        outMap[n, m] = 1;
                    }
                    else
                    {
                        outMap[n, m] = 0;
                    }
                    m++;
                }
                n++;
            }
            return outMap;
        }




        //返回当前砖块二维数组
        private int[,] returnBrickArray()
        {
            int[,] outBrick = new int[5, 5];
            int e = 0, f = 0;
            for (int i = 0; i < 5; i++)
            {
                f = 0;
                for (int j = 0; j < 5; j++)
                {
                    if (curBrick.Mod[i, j] == 1)
                    {
                            outBrick[e, f] = 1;
                    }
                    else
                    {
                        outBrick[e, f] = 0;
                    }
                    f++;
                }
                e++;
            }

            



            //for (int i = 0; i < 5; i++)
            //{
            //    for (int j = 0; j < 5; j++)
            //    {
            //        if (curBrick.Mod[i, j] == 1)
            //        {
            //            if (i+curBrick.Pos.Y > 3)
            //            {
            //                label2.Text += "1";
            //            }
            //        }
            //        else
            //        {
            //            label2.Text += "0";
            //        }
            //    }
            //    label2.Text += "\n";
            //}
            return outBrick;
        }


        //返回旋转后的方块
        //public int[,] Rotate2(int[,] b)//旋转
        //{
        //    int[,] a = new int[5,5];
        //    for (int i = 0; i < 5; i++)
        //        for (int j = 0; j < 5; j++)
        //            a[j, 5 - i - 1] = b[i, j];
        //    return a;
        //}



        //返回方块string
        public string displayBrick(int[,] q)//旋转
        {
            String a = "";
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (q[i, j] == 1)
                    {
                            a += "1";
                    }
                    else
                    {
                        a += "0";
                    }
                }
                a += "\n";
            }
            return a;
        }


        //private void recommendDisplay()//旋转
        //{
        //    int x = 6 + moveCopy;
        //    int[,] brick = curBrick.assignShapeValue(curBrick.shape1, rotateCopy);
        //    int y = Intersect1(brick);


        //    Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        //    Graphics g = Graphics.FromImage(bmp);
        //    SolidBrush s;
        //    //画方块
        //    for (int i = 0; i < 5; i++)
        //    {
        //        for (int j = 0; j < 5; j++)
        //        {
        //            if (brick[i,j] == 1)
        //            {
        //                if (i + y > 3)
        //                {
        //                    //(i,j)方块内的点阵位置 Pos是方块左上角点的地图位置
        //                    g.FillRectangle(s, map_cell_width * (j + x) - 80, map_cell_height * (i + y) - 80, map_cell_width - 2, map_cell_height - 2);
        //                }

        //            }
        //        }
        //    }



        //    //tMod = assignShapeValue(shape1, (shape2 + 1) % 4);
        //}

        //public int Intersect1(int[,] brick) //相交测试
        //{
        //    int x = 6 + moveCopy;
        //    int y = 0;
        //    for (int z = 1; z <= 15; z++)
        //    {
        //        for (int i = 0; i < 5; i++)
        //            for (int j = 0; j < 5; j++)
        //                if (brick[i, j] + map.Mod[z + i, x + j].p == 2)
        //                {
        //                    y = z;
        //                    return y;
        //                }
        //    }
        //    return -15;
        //}
    }
}
