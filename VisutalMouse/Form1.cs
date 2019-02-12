using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices;//调用WINDOWS API函数时要用到
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Win32;  //写入注册表时要用到

namespace VisutalMouse
{
    public partial class Form1 : Form
    {
        
        [DllImport("user32", CharSet = CharSet.Unicode)] 
          private static extern int mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        [DllImport("user32.dll", EntryPoint = "ShowCursor", CharSet = CharSet.Auto)]
        public static extern void ShowCursor(int status);
        //移動滑鼠 
        const int MOUSEEVENTF_MOVE = 0x0001;      
          //模擬滑鼠左鍵按下 
          const int MOUSEEVENTF_LEFTDOWN = 0x0002; 
          //模擬滑鼠左鍵抬起 
          const int MOUSEEVENTF_LEFTUP = 0x0004; 
          //模擬滑鼠右鍵按下 
          const int MOUSEEVENTF_RIGHTDOWN = 0x0008; 
          //模擬滑鼠右鍵抬起 
          const int MOUSEEVENTF_RIGHTUP = 0x0010; 
          //模擬滑鼠中鍵按下 
          const int MOUSEEVENTF_MIDDLEDOWN = 0x0020; 
          //模擬滑鼠中鍵抬起 
          const int MOUSEEVENTF_MIDDLEUP = 0x0040; 
          //標示是否採用絕對座標 
          const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        [DllImport("User32.dll")]  
        public extern static bool GetCursorPos(ref Point pot); 
        
        [DllImport("User32.dll")] 
        public extern static void SetCursorPos(int x, int y);
        Point point = new Point();
        Bitmap bit;
        ResourceManager rm;
        //const int WH_KEYBOARD_LL = 13;
       KeyboardHook k_hook = new KeyboardHook();




        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bit = new Bitmap(Properties.Resources.star);  //載入png圖檔

            bit.MakeTransparent(Color.Blue);  //設成透明

            notifyIcon1.ShowBalloonTip(3000);

            /*  kbv.HookedKeys.Add(Keys.Up);
              kbv.HookedKeys.Add(Keys.Clear);
              kbv.HookedKeys.Add(Keys.Left);
              kbv.HookedKeys.Add(Keys.Right);
              kbv.HookedKeys.Add(Keys.Home);
              kbv.HookedKeys.Add(Keys.PageUp);

              kbv.KeyDown += new KeyEventHandler(gkh_KeyDown);
              kbv.KeyUp += new KeyEventHandler(gkh_KeyUp);
              kbv.hook();*/
           // k_hook = new KeyboardHook();
            k_hook.KeyDownEvent += new KeyEventHandler(gkh_KeyDown);//钩住键按下
            k_hook.Start();//安装键盘钩子


        }
        
        void gkh_KeyUp(object sender, KeyEventArgs e)
        {
           
           /* this.Invoke(new Action(() => {  listBox1.Items.Add("Up\t" + e.KeyCode.ToString());
            }));*/
            e.Handled = true;
        }

        void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            GetCursorPos(ref point);
            this.Location= new Point(point.X,point.Y);
            Debug.Print(e.KeyCode.ToString());

             if (e.KeyCode == Keys.Up)
             {
                 this.Location = new Point(this.Location.X, this.Location.Y - 10);
                 SetCursorPos(point.X, point.Y - 10);
             }else if(e.KeyCode == Keys.Down)
             {
                 this.Location = new Point(this.Location.X, this.Location.Y + 10);
                   SetCursorPos(point.X, point.Y + 10);
             }
             else if(e.KeyCode == Keys.Left)
             {
                 this.Location = new Point(this.Location.X-10, this.Location.Y);
                 SetCursorPos(point.X-10, point.Y);
             }
             else if (e.KeyCode == Keys.Right)
             {
                 this.Location = new Point(this.Location.X+10, this.Location.Y);
                 SetCursorPos(point.X + 10, point.Y);
             }else if(e.KeyCode == Keys.Clear)
             {
                 mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, this.Location.X, this.Location.Y, 0, 0);
             }else if(e.KeyCode == Keys.PageUp)
             {
                 mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, this.Location.X, this.Location.Y, 0, 0);
             }
             /*this.Invoke(new Action(() => {
                 listBox1.Items.Add("Down\t" + e.KeyCode.ToString());
             }));*/
            /*if (e.KeyValue == (int)Keys.Up && (int)Control.ModifierKeys == (int)Keys.Alt)
            {
                System.Windows.Forms.MessageBox.Show("按下了指定快捷键组合");
            }*/
            e.Handled = true;
        }




        private void button1_Click(object sender, EventArgs e)
        {
            SetCursorPos(571, 323);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
          //  GetCursorPos(ref point);
            /*//滑鼠左鍵
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            //滑鼠右鍵
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
            //mouse_event(MOUSEEVENTF_MOVE, i, i, 0, 0);
            Point point = new Point();
            GetCursorPos(ref point);
            label1.Text = "X:" + point.X + "  Y:" + point.Y;*/

        }
        struct MousePoint
        {
            public int x;
            public int y;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
         //   kbv.unhook();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage((Image)bit, new Point(0, 0)); ////在視窗上繪製圖片
        }
    }
}
