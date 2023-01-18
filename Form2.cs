using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaiPower
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            Debug.WriteLine("Form2 created ... ");
            this.WindowState = FormWindowState.Maximized;
            Debug.WriteLine("Form2 Maximised");
            this.Visible = false;
            Debug.WriteLine("Form 2 Set visible: false" );
            this.Opacity = 0.01d;
            Debug.WriteLine("Opacity set");
            //this.setCur
            //this
            this.Hide();
            Debug.WriteLine("Form 2 hidden.");

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            this.Visible = true;
            Debug.WriteLine("Form2 set Visible ... ");
            Cursor = new Cursor(Application.StartupPath + "\\dvd_pixels.cur");   //("Resources/Logo.cur");
            Debug.WriteLine("Cursor Applied");
            //this.Visible = false;
            this.TopMost = true;
            Debug.WriteLine("Topmost Set True");
            

        }
        //void OnActivated(EventArgs e)
        //{
            
        //}

    }
}
