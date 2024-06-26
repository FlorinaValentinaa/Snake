using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;


namespace Snake
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food=new Circle();
        int maxWidth;
        int maxHeight;
        int score;
        int highScore;
        Random rand= new Random();
        bool goLeft, goRight, goUp, goDown;
        public Form1()
        {
            InitializeComponent();
            new Settings();
        }
       private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Left && Settings.directions!="right")
            {
                goLeft = true;
            }
            if(e.KeyCode==Keys.Right && Settings.directions!="left")
            {
                goRight = true;
            }
            if (e.KeyCode==Keys.Up && Settings.directions !="down")
            {
                goUp = true;
            }
            if (e.KeyCode==Keys.Down && Settings.directions!="up")
            {
                goDown = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Left )
            {
                goLeft = false;
            }
            if (e.KeyCode==Keys.Right)
            {
                goRight= false;
            }
            if(e.KeyCode==Keys.Up)
            {
                goUp=false
                    ;
            }
            if(e.KeyCode==Keys.Down)
            {
                goDown = false;
            }
        }
         private void StartGame(object sender, EventArgs e)
        {
            RestartGame();
        }

       

       /* public void TakeSnapShot(object sender, EventArgs e)
        {
            Label caption= new Label();
            caption.Text = "I scored: " + score + "and my Highscore is" + highScore;
            caption.Font = new Font("Ariel", 12, FontStyle.Bold);
            caption.ForeColor = Color.Pink;
            caption.AutoSize=false;
            caption.Width = pictureBox1.Width;
            caption.Height=30;
            caption.TextAlign=ContentAlignment.MiddleCenter;
            pictureBox1.Controls.Add(caption);

            SaveFileDialog dialog=new SaveFileDialog();
            dialog.FileName = "Snake Game";
            dialog.DefaultExt = "jpg";
            dialog.Filter = "JPG Image File | *.jpg";
            dialog.ValidateNames= true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int width=Convert.ToInt32(pictureBox1.Width);
                int height=Convert.ToInt32(pictureBox1.Height);
                Bitmap bmp = new Bitmap(width, height);
                pictureBox1.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));
                bmp.Save(dialog.FileName, ImageFormat.Jpeg);
                pictureBox1.Controls.Remove(caption);

            }

        }*/

        private void timer1Event(object sender, EventArgs e)
        {
            if (goLeft)
            {
                Settings.directions = "left";
            }
            if (goRight)
            {
                Settings.directions = "right";

            }
           
            if (goDown)
            {
                Settings.directions = "down";
            }
            if (goUp)
            {
                Settings.directions = "up";

            }
            for (int i=Snake.Count-1;i>=0;i--)
            {
                if (i == 0)
                {
                    switch(Settings.directions)
                    {
                        case "left":
                            Snake[i].X--;
                            break;
                        case "right":
                            Snake[i].X++;
                            break;
                        case "down":
                            Snake[i].Y++; 
                            break;
                        case "up":
                            Snake[i].Y--;
                            break;
                       

                    }
                    if (Snake[i].X<0)
                    {
                        Snake[i].X = maxWidth;
                    }
                    if (Snake[i].X>maxWidth)
                    {
                        Snake[i].X=0;
                    }
                    if (Snake[i].Y<0)
                    {
                        Snake[i].Y = maxHeight;
                    }
                    if (Snake[i].Y > maxHeight)
                    {
                        Snake[i].Y = 0;
                    }

                     if (Snake[i].X==food.X && Snake[i].Y==food.Y)
                     {
                         EatFood();
                     }

                    for (int j=1;j<Snake.Count;j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            GameOver();
                        }
                    }
                }

                else
                {
                    Snake[i].X = Snake[i-1].X;
                    Snake[i].Y = Snake[i-1].Y;
                }
            }

            pictureBox1.Invalidate();
        }

        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            Brush snakeColor;
            
            for (int i=0;i<Snake.Count;i++)
            {
                if (i==0)
                {
                    snakeColor = Brushes.Black;
                }
                else
                {
                    snakeColor = Brushes.DarkGreen;
                }

                canvas.FillEllipse(snakeColor, new Rectangle
                (
                    Snake[i].X * Settings.Width,
                    Snake[i].Y * Settings.Height,
                    Settings.Width,
                    Settings.Height
                    ));
            }

            canvas.FillEllipse(Brushes.DarkRed, new Rectangle
                (
                food.X * Settings.Width,
                food.Y * Settings.Height,
                Settings.Width,
                Settings.Height
                ));
          
        }
        private void RestartGame()
        {
            maxWidth = pictureBox1.Width / Settings.Width - 1;
            maxHeight = pictureBox1.Height / Settings.Height - 1;

            Snake.Clear();

            button1.Enabled=false;
           

            score = 0;
            label1.Text = "Score: " + score;

            Circle head = new Circle { X = 10, Y = 5 };
            Snake.Add(head);

            for(int i=0; i<10;i++)
            {
                Circle body = new Circle();
                Snake.Add(body);
            }

            food = new Circle
            {
                X = rand.Next(2, maxWidth),
                Y = rand.Next(2, maxHeight)
            };
            timer1.Start();
        }
        private void EatFood()
        {
            score += 1;
            label1.Text = "Score: " + score;

            Circle body = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y,
            };

            Snake.Add(body);
            food = new Circle { X = rand.Next(2, maxWidth),
                Y = rand.Next(2, maxHeight) };

        }
        private void GameOver()
        {
            timer1.Stop();
            button1.Enabled = true;
           
            if (score>highScore)
            {
                highScore=score;

                label2.Text = "High Score: " + Environment.NewLine + highScore;
                label2.ForeColor = Color.Maroon;
                label2.TextAlign=ContentAlignment.MiddleCenter;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //RestartGame();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
