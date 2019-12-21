using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KineticEnergy
{
    class BounceBall
    {
        public Vector Position;
        public Vector Velocity;
        public Brush ColorBrush;
        public TranslateTransform BallTransform = new TranslateTransform();
        const int SPEED_RANGE = 5;
        readonly Ellipse circle;
        readonly ViewModel VM = ViewModel.Instance;
        readonly Label label;

        public static double UpperLimitX = 620;
        public static double UpperLimitY = 400;
        const double LOWER_LIMIT_X = -20;
        const double LOWER_LIMIT_Y = -20;
        const double UPPER_PADDING_X = -190;
        const double UPPER_PADDING_Y = -70;

        public float radius;
        public float mass;
        const int MIN_RADIUS = 23;
        const int MAX_RADIUS = 36;
        const float PI = 3.14f;
        const float GRAMS_IN_KG = 1000;

        public BounceBall(Random rand)
        {
            //Random velocity
            Velocity = new Vector((rand.NextDouble() - 0.5) * SPEED_RANGE,
                (rand.NextDouble() - 0.5) * SPEED_RANGE);

            //Random position within bounds
            Position = new Vector(rand.Next((int)LOWER_LIMIT_X, (int)UpperLimitX),
                rand.Next((int)LOWER_LIMIT_Y, (int)UpperLimitY));

            //Random color
            ColorBrush = new SolidColorBrush(Color.FromRgb(
                (byte)rand.Next(0, 205),
                (byte)rand.Next(0, 205),
                (byte)rand.Next(0, 205)));

            //Random size
            radius = rand.Next(MIN_RADIUS, MAX_RADIUS);

            //Based on the volume of a sphere (to grams)
            mass = radius * radius * radius * PI * 4 / 3 / GRAMS_IN_KG;

            circle = new Ellipse
            {
                Stroke = Brushes.White,
                StrokeThickness = 0,
                RenderTransform = BallTransform,
                Width = radius * 2,
                Height = radius * 2,
                Fill = ColorBrush
            };

            VM.Balls.Add(circle);

            label = new Label
            {
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Background = ColorBrush
            };

            VM.Labels.Add(new ListBoxItem
            {
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                Content = label
            });
        }

        public void SetStroke(int thickness)
        {
            circle.StrokeThickness = thickness;
        }

        public void UpdatePosition()
        {
            //Movement
            Position += Velocity;

            BallTransform.X = Position.X;
            BallTransform.Y = Position.Y;

            //Update label
            label.Content = VM.CalcKineticEnergy(mass, Velocity.Length);

            //Checks if the ball is outside of bounds
            if (Position.X >= UpperLimitX + UPPER_PADDING_X - radius)
            {
                Position.X = UpperLimitX + UPPER_PADDING_X - radius - 1;
                Velocity.X *= -1;
            }
            else if (Position.X <= LOWER_LIMIT_X + radius)
            {
                Position.X = LOWER_LIMIT_X + radius + 1;
                Velocity.X *= -1;
            }

            if (Position.Y >= UpperLimitY + UPPER_PADDING_Y - radius)
            {
                Position.Y = UpperLimitY + UPPER_PADDING_Y - radius - 1;
                Velocity.Y *= -1;
            }
            else if (Position.Y <= LOWER_LIMIT_Y + radius)
            {
                Position.Y = LOWER_LIMIT_Y + radius + 1;
                Velocity.Y *= -1;
            }
        }
    }
}
