﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Windows.Input;
//using Cursor = System.Windows.Input.Cursor;
//using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

//using Utilities;





///TODO
///FIX FORM 2 to be the complete screen saver and form 1 only the trayicon & settings. 

///2.) Play sound on wall hit
///3.) play hooray! on corner
///
/// 4.) Add Pong elements to the screen (user controls the paddles by shift (up) and ctr. (down) keys (both left and right, respectively)  
/// 5.) write simple ai for poing element 
/// 
///last
/// SET THE SPEED VARIABLE TO 1



///COMPLETED
/// Make a form for settings, 
///     start in minimized mode
///     hide from alt+tab
///     hide from taskbar
///     show in systray
///     have a trackbar to set the time waitable
///     
/// Track User input (User32.dll) (count the amount of time the user has not been using the keyboard or mouse.
///     using predefined variables, and 
///     adjusted from and to trackbar
///     calculate the time the program needs to wait before activating, compare with 'time inactive' 
///     
/// Using a seperate thread to the UI thread, simulate user input by moving the mouse, as if a human is doing it (ie by using user input)
///     Extra points for doing it in the old dvd screensaver motion. 
/// Auto Minimize on start 
///     bonus points for adding a miniature time delay on the minimize to tray function
/// Add a speed function to speed up the testing of the functions (int speed)
/// Incorporate custom graphics for the smal settings pannel
///     FORCE THE SETTINGS PANNEL bottom right on any screen resolution. 
/// Invoke method on
/// ///
///1.) Change Icon to custom DVD icon
///     can either change cursor ( wants 32x32)
///     will need to reconfigure the border of the screen **















namespace TaiPower
{
    public partial class Form1 : Form
    {
        int timeDelaybeforeClose = 5000;//5 seconds before open window closes after the epic magic began,
        private bool appRunning = false;
        private bool quit = false;
        int screenWidth;
        int screenHeight;
        //float aspectRatio;
        Thread thread;
        GetLastUserInput gi;
        int waittime = 300000;//default 5 mins 300000
        int speed = 100;// Default =1 ( the mathematics *milliseconds/speed is used for the compare   (CTR+F -> #101 -> Enter)
        int buffer = 20;// buffer from the edge bottom of the screen.
        int taskbarbuffer = 10;
        Form2 f2;
       // Cursor cursor;
       private void SaveToStartup()
        {
            //test if it already exist
            //then  save there

            RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            try
            {
                reg.GetValue("TaiPower", Application.ExecutablePath.ToString());
                //if(reg.TaiPowe)
                //Debug.WriteLine("ITS WRITTEN IN THE WIND: "+reg.GetValue("TaiPower"));
               // reg.SetValue("TaiPower", Application.ExecutablePath.ToString());
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
                /*
            string[] registryList = reg.GetSubKeyNames();

            foreach (string registryEntry in registryList)
            {
                Debug.WriteLine(registryEntry);
                //reg.SetValue("TaiPower", Application.ExecutablePath.ToString());

            }
//            reg.SetValue("TaiPower",`Application.ExecutablePath.ToString());
            */
            //MessageBox.Show("Tai Power is now a part of your system forever","AWESOME NEWS",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
        }
        
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
            //Cursor cur =new Cursor(new MemoryStream(Properties.Resources.Logo));

            f2 = new Form2();
            //setDVDLogo();
            //deprecated - moved to form 2 control

            gi = new GetLastUserInput();

            
            thread = new Thread(new ThreadStart(WorkThreadFunction));

            // cursor = new Cursor(Cursor.Current.Handle);
            this.ShowInTaskbar = false;//hide from task bar. 
            this.Visible = false; //this doens't do as intended, but ey, it's here and not create any problems.
            //this.FormBorderStyle = FormBorderStyle.None; //take away the border and closable to0lbar
            this.ShowInTaskbar = false;//hide from task bar. 
            this.WindowState = FormWindowState.Normal; //minimize the Infosheet
                                                       //
            this.trackBarWaitTime.Value = waittime/1000;
            //this.trackBarWaitTime.Enabled = true;

            setTimerlabel();
            //label4.Text = "Current Setting: "; - Hardcoding Deprecated.
            //thread = new Thread(new ThreadStart(WorkThreadFunction));
            //thread.
            //Debug.WriteLine("Thread Made");
            this.TopMost = true;
            
            //this.Visible = false;
            //this.WindowState = FormWindowState.Minimized;
            screenWidth = Screen.PrimaryScreen.Bounds.Width;
            screenHeight = Screen.PrimaryScreen.Bounds.Height;



            this.Location = new Point(screenWidth - this.Width -buffer, screenHeight-this.Height-buffer-taskbarbuffer);
            //this.Location = new Point(screenWidth-500, screenHeight-500);
            
            
            
            labelTitle.Text = ("Resolution: " + screenWidth + "x" + screenHeight);
            Task.Delay(timeDelaybeforeClose).ContinueWith(t =>
            {
                thread.Start();
            });
            ////this.WindowState = FormWindowState.Minimized;
            ///
            //SaveToStartup();
            //Thread.Sleep(1000);
            buttonClicked();
        }
        
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        public void RelativeMove(int relx, int rely)
        {
            mouse_event(0x0001, relx, rely, 0, 0);
            //mouse_event()
                
        }


        //private void setDVDLogo()
        //{
        //    ///TODO
        //    //cursorScopeElementOnly = false;
        //    //System.Windows.Input.Set
        //    Debug.WriteLine("Attempting to override cursor: ");
        //    //Console.WriteLine("HERE WE TRY");
        //    //cursorScopeElementOnly = false;
        //    //Mouse.OverrideCursor = new System.Windows.Input.Cursor(Application.StartupPath + "\\dvd_pixels.cur");
        //    //Mouse.OverrideCursor = DisplayArea.Cursor;
            
            
    

        //    // Forcing the cursor for all elements. 
        //    //Mouse.OverrideCursor = DisplayArea.Cursor;
        //}
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
        private void buttonClicked()
        {
            //appRunning = !appRunning;
            this.Visible = false;
            this.WindowState = FormWindowState.Minimized;
            this.Visible = false;
            this.TopMost = false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        /*
         [DllImport("user32.dll")]
         static extern IntPtr SetWindowsHookEx(int idHook, keyboardHookCallback callback, IntPtr hInstance, uint threadId);
        */

        //public static int IdleTime() //In seconds
        //{
        //    LASTINPUTINFO lastinputinfo = new LASTINPUTINFO();
        //    lastinputinfo.cbSize = Marshal.SizeOf(lastinputinfo);
        //    GetLastInputInfo(ref lastinputinfo);
        //    return (((Environment.TickCount & int.MaxValue) - (lastinputinfo.dwTime & int.MaxValue)) & int.MaxValue) / 1000;
        //}
        private void button1_Click(object sender, EventArgs e)
        {
            buttonClicked();
        }
        //public static void tickCount()
        //{
        //    //you can do what you want. but not set something outside of the thread.   
        //    //Form1.labelText.Text = "TIME IIS TICKING: " + GetLastUserInput.GetIdleTickCount();
        //}
        // "boring Spaceholdertext " + i;
        public void WorkThreadFunction()
        {
            Thread.Sleep(50);//four second start delay (allow app to load.)
            bool right = true;
            bool down = true;
            int dirX = 1;
            int dirY = 1;
            int x = -1;
            int y = -1;
            //this is hardcoded (because it's one use program only...)
            //index data
            //0     current (old x position)
            //1     current (old y position)
            //2     new (expected x position)
            //3     new (expected y position)

            //Label l = Form1.;   
            ///TODO: 
            /// constantly test if the user was inactive for more than 30 seconds if it is, turn on appRunning)
            /// 
            //Debug.WriteLine("thret 1");
            do
            {
                //int i = 0; //this is internal counter for this thread.
                while (!appRunning)
                {

                    string settingUpdate = "Idle Time: ";
                    int uhm = (int)GetLastUserInput.GetIdleTickCount() / 1000;
                    int mins = (int)(uhm/ 60);
                    int seconds = (int)(uhm % 60);

                    if (mins != 0)
                        settingUpdate += mins + " Min ";
                    if (seconds != 0)
                        settingUpdate += seconds + " Sec";

                    //if (labelText!=null)
                    try
                    {
                        labelText.Invoke((MethodInvoker)(() =>
                        {
                            labelText.Text = settingUpdate;
                        }));
                    }catch (Exception e) { /* do nothing: labelText.Text = e.Message;*/ }


                    // COMPARING IDLE TIME TO THE TIMER #101
                    if (GetLastUserInput.GetIdleTickCount() >= waittime*(1000/speed))//realtime is *1000 (speed 5 = 1000/5 = 200ms) ie 5 times shorted waitime!
                    {
                        
                        Debug.WriteLine("User didn't give input within time to prevent this...");
                        appRunning = !appRunning;
                        //Form2.Show();
                        //try
                        //{

                        //f2.Invoke((MethodInvoker)(() =>
                        // {
                        //     f2.Show();
                        // }));
                        f2.Show();
                        //}catch (Exception e) { /*do nothing*/}
                        //set starting position.
                        x = System.Windows.Forms.Cursor.Position.X;
                        //Cursor.Position.
                        
                        y = System.Windows.Forms.Cursor.Position.Y;
                        Debug.WriteLine($"you need dr phills help: {x}, {y}");

                        try
                        {
                            if (WindowState == FormWindowState.Normal)
                            {
                                
                                
                                    //This works. 
                                    Task.Delay(timeDelaybeforeClose).ContinueWith(t =>
                                    {
                                        this.Invoke((MethodInvoker)(() =>
                                        {
                                            this.WindowState = FormWindowState.Minimized;
                                        }));
                                    });
                               
                            }


                        }
                        catch (Exception e) {
                            Debug.WriteLine("Eception e in line 344 foprm1");
                            /* do nothing: labelText.Text = e.Message;*/
                        }
                        //set the start position.-
                        //calculate the next position
                    }
                    //Thread.Sleep(1000);
                    

                }
                //when the go ahead is given (please find a way out of this loop via TESTING for alternative user input)
                while (appRunning)
                {
                    
                    
                    try
                    {
                        Thread.Sleep(3); //trying to make me see the problem unfold
                        if (x== System.Windows.Forms.Cursor.Position.X && y == System.Windows.Forms.Cursor.Position.Y) // (ie, the user has not moved the mouse after starting
                        {


                            this.Invoke((MethodInvoker)(() =>
                            {
                                this.RelativeMove(dirX, dirY);// = FormWindowState.Minimized;
                                Debug.WriteLine("Invoked relative move...");
                            }));
                            //Form1.RelativeMove(dirX, dirY); //set position
                            
                            x = System.Windows.Forms.Cursor.Position.X;
                            y = System.Windows.Forms.Cursor.Position.Y;

                            Debug.WriteLine($"Stats (after Rel Move {x}, {y}");

                            if ((x == 0 || x == screenWidth - 1) && (y == screenHeight - 1 || y == 0))
                            {
                                Debug.WriteLine($"WINNER WINNER CHICKEN DINNER: {x},{y}"); //fun corner win*
                                ///play CHEERING sound
                            }

                            if (x == (screenWidth - 1)&&right)
                            {
                                dirX *= -1;
                                right = !right;
                                ///play sound
                            }
                            if (x == 0 && !right)
                            {
                                dirX *= -1;
                                right = !right;
                                ///play sound
                            }
                            if (y == (screenHeight - 1)&&down)
                            {
                                dirY *= -1;
                                down = !down;
                                ///play sound
                            }
                            if (y == 0 && !down)
                            {
                                dirY *= -1;
                                down = !down;
                                ///play sound
                            }
                            //if (i > 100)
                            //{
                            //    dirX *= -1;
                            //    dirY*= -1;
                            //    i = 0;
                            //}
                        }
                        else
                        {
                            Debug.WriteLine(" we believe the app should terminate");
                            Debug.WriteLine($"X {x} {Cursor.Position.X}");
                            Debug.WriteLine($"Y {y} {Cursor.Position.Y}");

                            ///play sound
                            appRunning = !appRunning;//the program reset to wait for inactive time
                            f2.Invoke((MethodInvoker)(() =>
                            {
                                f2.Hide();
                            }));
                        }


                        
                        //labelText.Invoke((MethodInvoker)(() =>
                        //{
                        //    labelText.Text = $"The App is working perfectly well, \n {x},{y}";
                        //}));
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("ERROR!!!: " + ex.Message);

                        // log errors
                        break;
                    }
                    //Debug.WriteLine(GetLastUserInput.GetIdleTickCount());
                }
            } while (!quit); //infinitely.

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Visible = true;
            this.TopMost = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            thread.Abort();
            Application.Exit();
        }

        private void trackBarWaitTime_Scroll(object sender, EventArgs e)
        {
            setTimerlabel();
        }
        private void setTimerlabel()
        {
            string settingUpdate = "Start after: ";
            int mins = (int)this.trackBarWaitTime.Value / 60;
            int seconds = (int)this.trackBarWaitTime.Value % 60;

            if (mins != 0)
                settingUpdate += mins + " Min ";
            if (seconds != 0)
                settingUpdate += seconds + " Sec";

            label4.Text = settingUpdate;

            waittime = (int)this.trackBarWaitTime.Value;
            //Debug.WriteLine($"WAIT TIME NOW: {waittime}");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void labelText_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }

    public class GetLastUserInput
    {
        private struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }
        private static LASTINPUTINFO lastInPutNfo;
        static GetLastUserInput()
        {
            lastInPutNfo = new LASTINPUTINFO();
            lastInPutNfo.cbSize = (uint)Marshal.SizeOf(lastInPutNfo);
        }
        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        /// <summary>
        /// Idle time in ticks
        /// </summary>
        /// <returns></returns>
        public static uint GetIdleTickCount()
        {
            return ((uint)Environment.TickCount - GetLastInputTime());
        }
        /// <summary>
        /// Last input time in ticks
        /// </summary>
        /// <returns></returns>
        public static uint GetLastInputTime()
        {
            if (!GetLastInputInfo(ref lastInPutNfo))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return lastInPutNfo.dwTime;
        }
    }
}
