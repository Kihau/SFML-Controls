using System;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace SFML.Controls
{
    public class Button : Drawable
    {
        /// <summary>
        /// Updatable members outside of the class
        /// </summary>

        public Vector2f Position
        {
            get => Rect.Position;
            set
            {
                // (???) Check if size and position changing works normally
                Rect.Position = value; //+ new Vector2f(BorderHover, BorderHover)
                BasePosition = value;
            }
        }

        public Vector2f Size
        {
            // Add border thickness to returned value
            get => Rect.Size;
            set
            {
                Rect.Size = value;
                BaseSize = value;
            }
        }

        public string Caption
        {
            get
            {
                return "YO";
            }
            set
            {

            }
        }

        // Border Color
        public float BorderBaseColor { get; set; }
        public float BorderHoverColor { get; set; }
        public float BorderPressedColor { get; set; }

        // Button colors
        public Color BaseColor { get; set; }
        public Color HoverColor { get; set; }
        public Color PressedColor { get; set; }

        public bool Enabled { get; set; }

        /// <summary>
        /// Non-updatable members outside of the class
        /// </summary>

        // Border thickness
        private const float BorderBase = 2.0f;
        private const float BorderHover = 1.0f;

        private RectangleShape Rect { get; set; }
        private Vector2f BaseSize { get; set; }
        private Vector2f BasePosition { get; set; }

        public event EventHandler Clicked;

        MouseMoveEvent moveEvent;
        MouseButtonEvent buttonEvent;

        private bool IsHeld { get; set; }
        private bool Released { get; set; }
        private bool IsMouseInside { get; set; }
        private bool PressedInside { get; set; }
        private bool Pressed { get; set; }

        public Button(Vector2f size, Vector2f position) => Initialize(size, position);
        public Button(Button b) => Initialize(b.Size, b.Position);

        private void Initialize(Vector2f size, Vector2f position)
        {
            Rect = new RectangleShape();
            this.Size = size - new Vector2f(BorderBase, BorderBase);
            this.Position = position + new Vector2f(BorderHover, BorderHover);

            Rect.FillColor = new Color(230, 230, 230);
            Rect.OutlineColor = new Color(150, 150, 150);
            Rect.OutlineThickness = 2.0f;

            BaseColor = new Color(230, 230, 230);
            PressedColor = new Color(180, 230, 230);
            HoverColor = new Color(200, 230, 230);

            Enabled = true;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            UpdateEvents(target);
            UpdateState(target);
            UpdateButton();
            target.Draw(Rect);
        }

        private void UpdateEvents(RenderTarget target)
        {
            moveEvent.X = buttonEvent.X = Mouse.GetPosition(target as RenderWindow).X;
            moveEvent.Y = buttonEvent.Y = Mouse.GetPosition(target as RenderWindow).Y;
            buttonEvent.Button = GetButtonClick();

            OnClicked();
        }

        private Mouse.Button GetButtonClick()
        {
            IsHeld = true;
            if (Mouse.IsButtonPressed(Mouse.Button.Left)) return Mouse.Button.Left;
            else if (Mouse.IsButtonPressed(Mouse.Button.Right)) return Mouse.Button.Right;
            else if (Mouse.IsButtonPressed(Mouse.Button.Middle)) return Mouse.Button.Middle;
            else if (Mouse.IsButtonPressed(Mouse.Button.XButton1)) return Mouse.Button.XButton1;
            else if (Mouse.IsButtonPressed(Mouse.Button.XButton2)) return Mouse.Button.XButton2;
            else IsHeld = false;
            // something
            return Mouse.Button.ButtonCount;
        }

        private void OnClicked()
        {
            if (Released && !IsHeld && Enabled)
                Clicked?.Invoke(this, new MouseButtonEventArgs(buttonEvent));
        }

        private void UpdateState(RenderTarget target)
        {
            // Checking if button was pressed inside the window
            if (IsHeld && !Pressed)
            {
                if (Rect.GetGlobalBounds().Contains(Mouse.GetPosition(target as RenderWindow).X, Mouse.GetPosition(target as RenderWindow).Y))
                    PressedInside = true;
                else Pressed = false;
                Pressed = true;
            }
            else if (!IsHeld) Pressed = false;

            // Checking if mouse is inside the window
            if (Rect.GetGlobalBounds().Contains(buttonEvent.X, buttonEvent.Y))
                IsMouseInside = true;
            else IsMouseInside = false;

            // Checking if pressed button was released
            if (PressedInside && IsHeld && IsMouseInside)
                Released = true;
            else Released = false;
        }

        private void UpdateButton()
        {
            // Button is pressed
            if (IsMouseInside && IsHeld && PressedInside && Enabled)
                SetButtonStyle(PressedColor, BorderBase, BorderHover);
            // Mouse is hovering over button
            else if (IsMouseInside && Enabled)
                SetButtonStyle(HoverColor, BorderBase, BorderHover);
            // Default button style
            else SetButtonStyle(BaseColor, BorderHover, BorderBase);
        }

        private void SetButtonStyle(Color color, float offSet, float borderSize)
        {
            Rect.FillColor = color;

            Rect.Size = BaseSize + new Vector2f(2 * offSet, 2 * offSet);
            Rect.Position = BasePosition - new Vector2f(offSet, offSet);
            Rect.OutlineThickness = borderSize;
        }
    }
}
