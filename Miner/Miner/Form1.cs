using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Miner
{
    public partial class Form1 : Form
    {
        protected static List<FieldButton> fb = new List<FieldButton>();
        protected static TextBox tb = new TextBox();
        protected static int FieldCount = 0;
        protected static int bWidth = 30;
        protected static int bHight = 30;
        protected static int x = 0;
        protected static int y = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            newGame();
        }

        protected void newGame()
        {
            //Запуск игры
            FieldButton.bombCount = 0;
            createField();
            Focus();
        }

        protected int setBombs()
        {
            int maxBombs = 48;
            Random rng = new Random();
            while (FieldButton.bombCount < maxBombs)
                foreach (FieldButton bt in fb)
                {
                    int bombsInLine = 3;
                    if (rng.Next(0, 101) < FieldButton.bombPercent && FieldButton.bombCount < maxBombs && bombsInLine > 0)
                    {
                        bt.isBomb = true;
                       // bt.Text = "*";
                        bombsInLine--;
                        FieldButton.bombCount++;
                    }
                }
            return FieldButton.bombCount;
        }

        protected void createField()
        {
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    x = 15 + bWidth * j;
                    y = 45 + bHight * i;
                    FieldButton bt = new FieldButton();
                    bt.Size = new Size(bWidth, bHight);
                    bt.Location = new Point(x, y);
                    bt.Name = $"{x} {y}";
                    bt.Text = "";
                    tb.TabIndex = 1;

                    fb.Add(bt);
                    Controls.Add(bt);

                    bt.MouseUp += new Core().Bt_Click;
                    bt.GotFocus += new Core().Bt_GotFocus;
                }
            }

            FieldCount = fb.Count();

            tb.Size = new Size(bWidth, bHight);
            tb.Location = new Point(15, 15);
            tb.Text = setBombs().ToString();
            tb.ReadOnly = true;
            tb.Enabled = false;
            tb.TabIndex = 0;
            Controls.Add(tb);
        }
    }

    public class FieldButton: Button
    {
        public bool isBomb = false;
        public static double bombPercent = 20;
        public static int bombCount = 0;
        public bool isClicable = true;
        public bool isFlag = false;

        public bool Bomb()
        {
            return isBomb;
        }
    }
}
