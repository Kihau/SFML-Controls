using System;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace SFML.Controls
{
    public class Button : Drawable
    {
        /// <summary>
        /// Visual Proporties of the Button class
        /// </summary>
        public Vector2f Position
        {
            get => Rect.Position;
            set
            {
                Rect.Position = value;
                BasePosition = value;
            }
        }

        public Vector2f Size
        {
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

        private RectangleShape Rect { get; set; }
        private Vector2f BaseSize { get; set; }
        private Vector2f BasePosition { get; set; }

        // Border Color
        public float BorderBaseColor { get; set; }
        public float BorderHoverColor { get; set; }
        public float BorderPressedColor { get; set; }

        // Border thickness
        private float BorderBase { get; set; }
        private float BorderHover { get; set; }

        // Button colors
        public Color BaseColor { get; set; }
        public Color HoverColor { get; set; }
        public Color PressedColor { get; set; }

        /// <summary>
        /// Functionality of the Button class
        /// </summary>
        public event EventHandler Clicked;

        // change to nullable and remove IsHeld
        MouseMoveEvent moveEvent;
        MouseButtonEvent buttonEvent;

        public bool Enabled { get; set; }

        private bool IsHeld { get; set; }
        private bool Released { get; set; }

        private Vector2i MousePosition { get; set; }
        private bool IsMouseInside { get; set; }

        private Vector2i PressPosition { get; set; }
        private bool WasPressed { get; set; }

        public Button(Vector2f size, Vector2f position)
        {
            Rect = new RectangleShape();
            this.Size = size;

            Rect.FillColor = new Color(230, 230, 230);
            Rect.OutlineColor = new Color(150, 150, 150);
            Rect.OutlineThickness = 2.0f;

            BorderBase = Rect.OutlineThickness;
            BorderHover = Rect.OutlineThickness / 2;

            BaseSize = size;
            BasePosition = position;

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
            if (IsHeld && !WasPressed)
            {
                PressPosition = Mouse.GetPosition(target as RenderWindow);
                WasPressed = true;
            }
            else if (!IsHeld) WasPressed = false;

            // Updating mouse position state
            if (Rect.GetGlobalBounds().Contains(buttonEvent.X, buttonEvent.Y))
                IsMouseInside = true;
            else IsMouseInside = false;

            // Updating button state
            if (Rect.GetGlobalBounds().Contains(PressPosition.X, PressPosition.Y) && IsHeld && IsMouseInside)
                Released = true;
            else Released = false;
        }

        private void UpdateButton()
        {
            // WARNING!!
            // FIX: Size of the button changes the first loop program is executed
            if (IsMouseInside && IsHeld && Rect.GetGlobalBounds().Contains(PressPosition.X, PressPosition.Y) && Enabled)
                SetButtonStyle(PressedColor, BorderBase, BorderHover);
            else if (IsMouseInside && Enabled)
                SetButtonStyle(HoverColor, BorderBase, BorderHover);
            else SetButtonStyle(BaseColor, BorderHover, BorderBase);
        }

        private void SetButtonStyle(Color color, float offSet, float borderSize)
        {
            Rect.FillColor = color;

            Rect.Size = BaseSize + new Vector2f(2 * offSet, 2 * offSet);
            Rect.Position = BasePosition - new Vector2f(offSet, offSet);
            Rect.OutlineThickness = borderSize;
        }
        
        private Mouse.Button? GetButtonClickNullable()
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left)) return Mouse.Button.Left;
            else if (Mouse.IsButtonPressed(Mouse.Button.Right)) return Mouse.Button.Right;
            else if (Mouse.IsButtonPressed(Mouse.Button.Middle)) return Mouse.Button.Middle;
            else if (Mouse.IsButtonPressed(Mouse.Button.XButton1)) return Mouse.Button.XButton1;
            else if (Mouse.IsButtonPressed(Mouse.Button.XButton2)) return Mouse.Button.XButton2;
            else return null;
        }
    }
}
