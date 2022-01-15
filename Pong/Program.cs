using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Pong
{
    interface Drawer
    {
        Shape GetSurface();
    }
    static class Draw
    {
        static public void draw(Drawer _object)
        {
            Screen.Win.Draw(_object.GetSurface());
        }
    }
    class Desk : Drawer
    {
        private static readonly int Speed = 10;
        public static readonly int Size_X = 30;
        public static readonly int Size_Y = 100;
        private static RectangleShape Rect = new RectangleShape
        {
            Size = new Vector2f(Size_X, Size_Y),
            FillColor = Color.Green,
        };
        private static int CountDesks = 0;

        public Desk()
        {
            if (CountDesks == 2) { return; }

            if (CountDesks == 1)
            {
                this.X = (int)Screen.Width / 100 * 95;
                this.Y = (int)Screen.Height / 2 - Size_Y / 2;
                CountDesks += 1;
            }
            if (CountDesks == 0)
            {
                this.X = (int)Screen.Width / 100 * 5;
                this.Y = (int)Screen.Height / 2 - Size_Y / 2;
                CountDesks += 1;
            }
        }
        public int X { get; private set; }
        public int Y { get; private set; }
        public void Up()
        {
            if (this.Y <= 0) return;
            this.Y -= Speed;
        }
        public void Down()
        {
            if (this.Y + Size_Y >= Screen.Height) return;
            this.Y += Speed;
        }
        Shape Drawer.GetSurface()
        {
            Rect.Position = new Vector2f(this.X, this.Y);
            return Rect;
        }
    }
    class Ball : Drawer
    {
        private static readonly int Speed = 10;
        private static readonly int Radius = 10;
        private static readonly CircleShape Rect = new CircleShape
        {
            Radius = Radius,
            FillColor = Color.Red,
        };

        public int X { get; private set; }
        public int Y { get; private set; }

        private int dx = 1;
        private int dy = 1;

        public Ball()
        {
            this.X = Screen.Width / 2;
            this.Y = Screen.Height / 2;
        }
        public void Update(Desk leftDesk, Desk rightDesk)
        {
            this.X += this.dx * Ball.Speed;
            this.Y += this.dy * Ball.Speed;

            this.CheckCollisionWithWall();

            this.CheckCollisionWithDesk(leftDesk, rightDesk);
        }
        private void CheckCollisionWithWall()
        {
            if (this.Y < 0) { this.dy *= -1; }
            if (this.Y > Screen.Height) { this.dy *= -1; }

            if (this.X < 0) { this.Respawn(1); Screen.ScoreUpdate(1); }
            if (this.X > Screen.Width) { this.Respawn(2); Screen.ScoreUpdate(2); }
        }
        private void CheckCollisionWithDesk(Desk leftDesk, Desk rightDesk)
        {
            void CollisionWithDesk(Desk desk)
            {
                if(this.X > desk.X && this.X < desk.X + Desk.Size_X && this.Y > desk.Y && this.Y < desk.Y + Desk.Size_Y)
                {
                    this.dx *= -1;
                }
            }

            if (this.X > Screen.Width / 2)
            {
                CollisionWithDesk(rightDesk);
            }
            else
            {
                CollisionWithDesk(leftDesk);
            }
        }
        private void Respawn(int c)
        {
            this.X = Screen.Width / 2;
            this.Y = Screen.Height / 2;
            if (c == 1)
            {
                this.dx = -1;
            }
            if (c == 2)
            {
                this.dx = 1;
            }
        }
        Shape Drawer.GetSurface()
        {
            Rect.Position = new Vector2f(this.X - Ball.Radius, this.Y - Ball.Radius);
            return Rect;
        }
    }
    static class Screen
    {
        public static readonly int Width = 1800, Height = 900;
        public static RenderWindow Win;
        public static SFML.System.Clock Clock;
        public static Random Rand = new Random();
        private static Vector2f Score = new Vector2f(0, 0);
        static public void ScoreUpdate(int c)
        {
            if (c == 1)
            {
                Score.X += 1;
            }
            if (c == 2)
            {
                Score.Y += 1;
            }
        }
        public static string GetScore()
        {
            return Score.ToString();
        }
    }
    class Program
    {
        static void Main()
        {
            Screen.Win = new RenderWindow(new SFML.Window.VideoMode((uint)Screen.Width, (uint)Screen.Height), "snake game");
            Screen.Win.SetVerticalSyncEnabled(true);
            Screen.Win.Closed += WinClosed;
            Screen.Clock = new SFML.System.Clock();

            RectangleShape rect = new RectangleShape
            {
                FillColor = new Color(70, 70, 70),
            };

            Desk LeftDesk = new Desk();
            Desk RightDesk = new Desk();

            Ball ball = new Ball();

            // main loop
            while (Screen.Win.IsOpen)
            {
                Screen.Clock.Restart();
                Screen.Win.DispatchEvents();
                Screen.Win.Clear(Color.Black);

                if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.Up))    { RightDesk.Up();   }
                if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.Down))  { RightDesk.Down(); }

                if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.W))  { LeftDesk.Up(); }
                if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.S))  { LeftDesk.Down(); }

                Draw.draw(ball);
                ball.Update(LeftDesk, RightDesk);

                Draw.draw(LeftDesk);
                Draw.draw(RightDesk);

                Screen.Win.Display();
                Screen.Win.SetTitle(Convert.ToString(1000 / Math.Max(1, Screen.Clock.ElapsedTime.AsMilliseconds())) + "  " + Screen.GetScore());
            }
        }
        private static void WinClosed(object sender, EventArgs e)
        {
            Screen.Win.Close();
        }
    }
}
