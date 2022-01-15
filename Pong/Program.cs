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
    static class Screen
    {
        public const int ScreenWidth = 1800, ScreenHeight = 900;

        public static RenderWindow Win;
        public static SFML.System.Clock Clock;
        public static Random Rand = new Random();
    }
    class Program
    {
        static void Main(string[] args)
        {

            Screen.Win = new RenderWindow(new SFML.Window.VideoMode(Screen.ScreenWidth, Screen.ScreenHeight), "snake game");
            Screen.Win.SetVerticalSyncEnabled(true);
            Screen.Win.Closed += WinClosed;
            Screen.Clock = new SFML.System.Clock();

            RectangleShape rect = new RectangleShape
            {
                FillColor = new Color(70, 70, 70),
            };

            int Sc = 0;

            // main loop
            while (Screen.Win.IsOpen)
            {
                Sc += 1;
                if (Sc == 6) Sc = 0;
                Screen.Clock.Restart();
                Screen.Win.DispatchEvents();
                Screen.Win.Clear(Color.Black);

                if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.Up))    {  }
                if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.Down))  {  }
                if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.Right)) {  }
                if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.Left))  {  }


                rect.Size = new SFML.System.Vector2f(1, 900);
                for (int i = 0; i < 1801; i += 30)
                {
                    rect.Position = new SFML.System.Vector2f(i, 0);
                    Screen.Win.Draw(rect);
                }
                rect.Size = new SFML.System.Vector2f(1800, 1);
                for (int i = 0; i < 901; i += 30)
                {
                    rect.Position = new SFML.System.Vector2f(0, i);
                    Screen.Win.Draw(rect);
                }

                Screen.Win.Display();
                Screen.Win.SetTitle(Convert.ToString(1000 / Math.Max(1, Screen.Clock.ElapsedTime.AsMilliseconds())));
            }
        }
        private static void WinClosed(object sender, EventArgs e)
        {
            Screen.Win.Close();
        }
    }
}
