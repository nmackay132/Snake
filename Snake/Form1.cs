using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        List<Circle> Snake = new List<Circle>();
        Circle Food;

        public Form1()
        {
            InitializeComponent();

            //Set settings to default
            new Settings();

            //Set game speed and start timer
            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            //Start New game
            StartGame();
        }

        private void SelectOption()
        {

        }

        private void StartGame()
        {
            lblGameOver.Visible = false;

            new Settings();

            Snake.Clear();
            Circle head = new Circle(10, 5);
            Snake.Add(head);

            lblScore.Text = Settings.Score.ToString();
            CreateFood();
        }

        private void CreateFood()
        {
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            Random random = new Random();
            int x = random.Next(0, maxXPos);
            int y = random.Next(0, maxYPos);
            Food = new Circle(x, y);
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            if (Settings.GameOver)
            {
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;

                MovePlayer();
            }

            pbCanvas.Invalidate(); //deletes all graphics on canvas and then refreshes  
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!Settings.GameOver)
            {
                for (int i = 0; i < Snake.Count; i++)
                {
                    Brush snakeColor = Brushes.Black;
                    switch (i%7)
                    {
                        case 0:
                            snakeColor = Brushes.Red;
                            break;
                        case 1:
                            snakeColor = Brushes.Orange;
                            break;
                        case 2:
                            snakeColor = Brushes.Yellow;
                            break;
                        case 3:
                            snakeColor = Brushes.Green;
                            break;
                        case 4:
                            snakeColor = Brushes.Blue;
                            break;
                        case 5:
                            snakeColor = Brushes.Indigo;
                            break;
                        case 6:
                            snakeColor = Brushes.Violet;
                            break;
                    }

                    //make snake body
                    canvas.FillRectangle(snakeColor,
                        new Rectangle(Snake[i].X * Settings.Width, Snake[i].Y * Settings.Height,
                                        Settings.Width, Settings.Height));

                    //make food
                    canvas.FillEllipse(Brushes.Black,
                            new Rectangle(Food.X * Settings.Width, Food.Y * Settings.Height,
                                        Settings.Width, Settings.Height));
                }
            }
            else
            {
                lblGameOver.Text = "Game Over:\nFinal Score: " + Settings.Score;
                lblGameOver.Visible = true;
            }

        }

        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }

                    //Get maximum X and Y Pos
                    int maxXPos = pbCanvas.Size.Width / Settings.Width;
                    int maxYPos = pbCanvas.Size.Height / Settings.Height;

                    //Detect collission with game borders.
                    if (Snake[i].X < 0 || Snake[i].Y < 0
                        || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                    {
                        Die();
                    }

                    // Detect head collisions with body
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }

                    //Detect collision with food piece
                    if (Snake[0].X == Food.X && Snake[0].Y == Food.Y)
                    {
                        Eat();
                    }
                }
                else
                {
                    //Move body piece into position of last body piece
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
                
            }
        }

        private void Eat()
        {
            Snake.Add(new Circle(Snake[Snake.Count - 1].X, Snake[Snake.Count - 1].Y));

            //Update Score
            Settings.Score += Settings.Points;
            lblScore.Text = Settings.Score.ToString();

            CreateFood();
        }

        private void Die()
        {
            Settings.GameOver = true;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pbCanvas_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void lblNewGame_Click(object sender, EventArgs e)
        {

        }
    }
}
