using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFML.Controls;

namespace SfmlTestProj
{
    class Program
    {
        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new VideoMode(800, 500), "Test window");
            window.Closed += (sender, e) => (sender as RenderWindow).Close();

            var buttons = new List<Button>();
            var size = new Vector2f(50, 50);

            for (float row = 10; row < window.Size.X - size.X; row += size.Y + 10)
            {
                for (float col = 10; col < window.Size.Y - size.Y; col += size.X + 10)
                {
                    var b = new Button(size, new Vector2f(row, col));
                    b.Clicked += (sender, e) => { b.BaseColor = Color.Red; b.Enabled = false; };
                    buttons.Add(b);
                }
            }

            var testRect = new RectangleShape(new Vector2f(100, 100));
            testRect.Position = new Vector2f(100, 100);
            testRect.FillColor = Color.Red;

            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear(Color.White);
                //window.Draw(testRect);
                buttons.ForEach(x => window.Draw(x));
                window.Display();
            }
        }
    }
}
