using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Miner
{
    class Core: Form1
    {
        protected void defeat()
        {
            //Поражение
            foreach (FieldButton button in fb)
            {
                if (button.Bomb())
                {
                    if (!button.isClicable)
                        button.Text = "▲*";
                    else
                        button.Text = "*";
                }
                else
                {
                    if (!button.isClicable && button.isFlag)
                        button.Text = "x▲";
                }
            }

            DialogResult res = MessageBox.Show("Вы проиграли!\nНовая игра?",
                                            "Поражение",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Exclamation);

            if (res == DialogResult.Yes)
            {
                Application.Restart();
            }
            else
            {
                Application.Exit();
            }
        }

        protected void victory()
        {
            //Победа
            DialogResult res = MessageBox.Show("Вы выиграли!\nНовая игра?",
                            "Победа",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Asterisk);

            if (res == DialogResult.Yes)
            {
                Application.Restart();
            }
            else
            {
                Application.Exit();
            }
        }

        protected void setFlag(FieldButton bt)
        {
            //поставить или снять флажок
            if (bt.isClicable)
            {
                if (FieldButton.bombCount > 0)
                {
                    bt.Text = "▲";
                    bt.isClicable = false;
                    bt.isFlag = true;
                    FieldButton.bombCount--;
                    FieldCount--;
                    tb.Text = FieldButton.bombCount.ToString();
                }
            }
            else
            {
                bt.Text = "";
                bt.isClicable = true;
                bt.isFlag = false;
                FieldButton.bombCount++;
                FieldCount++;
                tb.Text = FieldButton.bombCount.ToString();
            }
        }

        protected void openRegion(FieldButton bt)
        {
            //Открывает все 0 ячейки в округе, если нажата ячейка с 0
            var str = bt.Name.Split(' ');
            var x1 = Convert.ToInt32(str[0]) - bWidth;
            var x2 = x1 + bWidth * 3;
            var y1 = Convert.ToInt32(str[1]) - bHight;
            var y2 = y1 + bHight * 3;

            var buttons = new List<FieldButton>();

            for (int i = x1; i < x2; i += bWidth)
            {
                for (int j = y1; j < y2; j += bHight)
                {
                    FieldButton tempBt = fb.Find(button => button.Name == $"{i} {j}");
                    if (tempBt != null && tempBt.isClicable && !tempBt.Bomb())
                    {
                        int b = bombAround(tempBt);
                        openCell(tempBt, b);
                        if (b == 0)
                            buttons.Add(tempBt);
                    }
                }
            }

            foreach (FieldButton button in buttons)
            {
                openRegion(button);
            }
        }

        protected void openCell(FieldButton bt, int b)
        {
            //открыть ячейку
            bt.FlatStyle = FlatStyle.Popup;
            bt.Text = b.ToString();
            FieldCount--;
            bt.Enabled = false;
            bt.isClicable = false;
        }

        protected int bombAround(FieldButton bt)
        {
            //Считает бомбы вокруг нажатой кнопки
            int bombs = 0;
            var str = bt.Name.Split(' ');
            var x1 = Convert.ToInt32(str[0]) - bWidth;
            var x2 = x1 + bWidth * 3;
            var y1 = Convert.ToInt32(str[1]) - bHight;
            var y2 = y1 + bHight * 3;

            for (int i = x1; i < x2; i += bWidth)
            {
                for (int j = y1; j < y2; j += bHight)
                {
                    FieldButton tempBt = fb.Find(button => button.Name == $"{i} {j}");
                    if (tempBt != null && tempBt.Bomb())
                        bombs++;
                }
            }
            return bombs;
        }

        public void Bt_Click(object sender, MouseEventArgs e)
        {
            var button = sender as FieldButton;//нажатая кнопка
            if (e.Button == MouseButtons.Right)//флажки
            {
                setFlag(button);
            }

            if (e.Button == MouseButtons.Left)
            {
                if (button.isClicable)
                {
                    if (button.Bomb())
                        defeat();//поражение
                    else
                    {
                        int b = bombAround(button);
                        openCell(button, b);//открытие ячеек
                        if (b == 0)
                        {
                            openRegion(button);
                        }
                    }
                }
            }

            if (FieldCount == 0)
                victory();//победа
        }

        public void Bt_GotFocus(object sender, EventArgs e)
        {
            ActiveForm.Focus();
        }
    }
}
