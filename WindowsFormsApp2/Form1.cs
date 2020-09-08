using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        #region controls and panel1
        public Label lblPlayer = new Label
        {
            Name = "playerTurn",
            Text = string.Format("Current player is {0}", playerTwoTurn ? "Player TWO" : "Player ONE"),
            BackColor = Color.Red,
            AutoSize = true,

        };
        Panel panel1 = new Panel()
        {
            Dock = DockStyle.Fill
        };
        Button btn = new Button
        {
            Name = "restartButton",
            Text = "Restart",
            AutoSize = true,
            Location = new Point(15 * 30 + 30, 90)
        };
        #endregion
        public Form1()
        {
            InitializeComponent();
            Init(table);
            btn.Click += btnClick;
        }
        private void btnClick(object sender, EventArgs e)
        {
            Init(table);
            MessageBox.Show("Game has been restarted", "Restart");
        }

        public string[,] table = new string[15,15];
        public string[] players = { "x", "y" };
        static public bool playerTwoTurn = false;
        void Init(string[,] arr)
        {
            playerTwoTurn = false;
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    arr[i, j] = "_";
                }
            }
            DrawBoard(table);
            Controls.Add(lblPlayer);
            checkCurrPlayer(playerTwoTurn);
            Controls.Add(btn);
            Controls.Add(panel1);
        }
        void checkCurrPlayer(bool player)
        {
            lblPlayer.Text = string.Format("Current player is {0}", playerTwoTurn ? "Player TWO" : "Player ONE");

        }
        void DrawBoard(string[,] UItable)
        {
            panel1.Controls.Clear();
            for (int col = 0; col < UItable.GetLength(0); col++)
            {
                for (int row = 0; row < UItable.GetLength(1); row++)
                {
                    Label box = new Label()
                    {
                        Name = string.Format("{0},{1}", col, row),
                        BorderStyle = BorderStyle.FixedSingle,
                        Text = UItable[col, row].ToString(),
                        Size = new Size(30, 30),
                        Location = new Point(col * 30 + 30, row * 30 + 30),
                        TextAlign = ContentAlignment.TopCenter,
                        Font = new Font("Robotics", 18)

                    };
                    box.Click += Click;
                    panel1.Controls.Add(box);
                }
            }
        }
        new void Click(object sender, EventArgs e)
        {
            int indexPlayer = !playerTwoTurn ? 0 : 1;
            Label lbl = sender as Label;
            int[] indexes = lbl.Name.Split(',').Select(Int32.Parse).ToArray();
            if (lbl.Text == "_" && !playerTwoTurn)
            {
                lbl.Text = players[indexPlayer];
                table[indexes[0], indexes[1]] = players[indexPlayer];
                playerTwoTurn = !playerTwoTurn;
                CheckVictory(table, indexes[0], indexes[1], indexPlayer);
            }
            else if (lbl.Text == "_" && playerTwoTurn)
            {
                lbl.Text = players[indexPlayer];
                table[indexes[0], indexes[1]] = players[indexPlayer];
                playerTwoTurn = !playerTwoTurn;
                CheckVictory(table, indexes[0], indexes[1], indexPlayer);
            }
            else MessageBox.Show("You cannot click on opponent's box", "Oops");
            checkCurrPlayer(playerTwoTurn);
        }
        void Victory(int player)
        {
            EndGame();
            MessageBox.Show($"Player {player + 1} victory");
        }
        void CheckVictory(string[,] table, int col, int row, int player)
        {
            int col1, row1, count = 1;
            //horiz
            col1 = col - 1; row1 = row;
            while (col1 > table.GetLowerBound(0) && table[col1, row1] == players[player])
            {
                if (count != 4) { count++; col1--; }
                else if (count == 4)
                {
                    Victory(player); break;
                }
            }
            col1 = col + 1;
            while (col1 < table.GetUpperBound(0) && col1 > -1 && table[col1, row1] == players[player])
            {
                if (count != 4) { count++; col1++; }
                else if (count == 4)
                {
                    Victory(player);
                    break;
                }
            }
            //end horiz

            //vertical
            col1 = col; row1 = row - 1; count = 1;
            while (row1 > table.GetLowerBound(1) && row1 < table.GetUpperBound(1) && table[col1, row1] == players[player])
            {
                if (count != 4) { count++; row1--; }
                else if (count == 4)
                {
                    Victory(player); break;
                }
            }
            row1 = row + 1;
            while (row1 < table.GetUpperBound(0) && row1 > -1 && table[col1, row1] == players[player])
            {
                if (count != 4) { count++; row1++; }
                else if (count == 4)
                {
                    Victory(player);
                    break;
                }
            }
            //end vertic

            //diagonal
            //leftup->rightdown check
            col1 = col - 1; row1 = row - 1; count = 1;
            while ((col1 > table.GetLowerBound(0)) && (row1 > table.GetLowerBound(1) && table[col1, row1] == players[player]))
            {
                if (table[col1, row1] == players[player] && count != 4) { count++; col1--; row1--; }
                else if (count == 4)
                {
                    Victory(player); break;
                }
            }
            col1 = col + 1; row1 = row + 1;
            while (col1 <= table.GetUpperBound(0) && row1 <= table.GetUpperBound(1) && table[col1, row1] == players[player])
            {
                if (table[col1, row1] == players[player] && count != 4) { count++; col1++; row1++; }
                else if (count == 4)
                {
                    Victory(player); break;
                }
            }
            //leftdown->rightup check
            col1 = col + 1; row1 = row - 1; count = 1;
            while ((col1 > table.GetLowerBound(0)) && col1 < table.GetUpperBound(0) && (row1 > table.GetLowerBound(1) && table[col1, row1] == players[player]))
            {
                if (table[col1, row1] == players[player] && count != 4) { count++; col1++; row1--; }
                else if (count == 4)
                {
                    Victory(player); break;
                }
            }
            col1 = col - 1; row1 = row + 1;
            while (col1 <= table.GetUpperBound(0) && col1 > 0 && row1 <= table.GetUpperBound(1) && table[col1, row1] == players[player])
            {
                if (table[col1, row1] == players[player] && count != 4) { count++; col1--; row1++; }
                else if (count == 4)
                {
                    Victory(player); break;
                }
            }
            //end diag
            //
            if (DrawCheck(table))
            {
                EndGame();
                MessageBox.Show("HOW IS IT A DRAW");
            }
            //todo: draw check
            //
        }
        void EndGame()
        {
            panel1.Controls.Clear();
        }
        bool DrawCheck(string[,] table)
        {
            //Stopwatch timer = Stopwatch.StartNew();
            List<string> tableCheck = new List<string>();
            foreach (string s in table)
            {
                tableCheck.Add(s);
            }
            //timer.Stop();
           // MessageBox.Show(timer.Elapsed.ToString());
           //no need to fix, eta first check 1,25*10-5
            return !tableCheck.Contains("_");

        }
    }
}