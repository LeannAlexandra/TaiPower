using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Utilities;

namespace TaiPower
{
    public partial class Form1 : Form
    {
        private bool f12toggle = true;
       Cursor cursor;
        public Form1()
        {
            InitializeComponent();
            cursor = new Cursor(Cursor.Current.Handle);
            this.ShowInTaskbar = false;//hide from task bar. 
            //this.Visible = false; //this doens't do as intended, but ey, it's here and not create any problems.
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None; //take away the border and closable to0lbar
            this.ShowInTaskbar = false;//hide from task bar. 
            //this.WindowState = FormWindowState.Minimized; //minimize the Infosheet*
            Thread thread = new Thread(new ThreadStart(WorkThreadFunction));
            //thread.
            //Debug.WriteLine("Thread Made");
            thread.Start();
        }
        private void MoveCursor()
        {
            // Set the Current cursor, move the cursor's Position,
            // and set its clipping rectangle to the form. 

            this.Cursor = new Cursor(Cursor.Current.Handle);
            //Cursor.Current = Cursors.WaitCursor;
            RelativeMove(Cursor.Position.X - 10, Cursor.Position.Y - 10);
            //Cursor.Position = new Point(Cursor.Position.X - 50, Cursor.Position.Y - 50);
            //button2.Cursor = System.Windows.Forms.Cursors.Hand;
            //Cursor.Clip = new Rectangle(this.Location, this.Size);
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        public static void RelativeMove(int relx, int rely)
        {
            mouse_event(0x0001, relx, rely, 0, 0);
                
        }

        //then to move mouse Relativemove(whereeveruwantx, y);

        private void ChangePosition() 
            // This is used to change the trajectory once one/ or both values reach 0
        { 
        
        
        
        }

         protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // turn on WS_EX_TOOLWINDOW style bit
                cp.ExStyle |= 0x80;
                return cp;
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                //DisplayText, Asuming you have a textbox do something like this:
                Debug.WriteLine("The power of f12 activated");
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MoveCursor();
            //Debug.WriteLine("Clicked");
            
            //Debug.WriteLine("fret started");
        }
        public void WorkThreadFunction()
        {
            //Debug.WriteLine("thret 1");
            try
            {
                Debug.WriteLine("thret try");

                // do any background work
                if (f12toggle)
                {
                    Debug.WriteLine("f12 true");
                    for (int i = 0; i < 10; i++) { 
                    Thread.Sleep(1000);
                    MoveCursor(); 
                }
                } /*else
                {
                    Debug.WriteLine("f12 true");
                }*/
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR!!!: " + ex.Message);
                // log errors
            }
        }
    }
}
