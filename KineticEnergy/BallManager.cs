using System;
using System.Timers;
using System.Windows;

namespace KineticEnergy
{
    class BallManager
    {
        public BounceBall[] ball = new BounceBall[12];
        BounceBall selectedBall;
        const int SELECTED_THICKNESS = 5;
        readonly Random rand = new Random();

        private static BallManager instance = null;
        public static BallManager Instance
        { get { return instance ?? (instance = new BallManager()); } }

        readonly Timer timer = new Timer();
        const float UPDATE_SPEED = 5f;

        private BallManager()
        {
            for (int i = 0; i < ball.Length; i++)
            {
                ball[i] = new BounceBall(rand);
            }

            //Starts the timer
            timer.Elapsed += new ElapsedEventHandler(Update);
            timer.Interval = UPDATE_SPEED;
            timer.Enabled = true;
        }

        //Changes the border of selected ball
        public void SelectBall(int index)
        {
            if (selectedBall != null)
                selectedBall.SetStroke(0);

            selectedBall = ball[index];
            ViewModel.Instance.MassValue = Math.Round(selectedBall.mass, 2);
            ViewModel.Instance.VelocityValue = Math.Round(selectedBall.Velocity.Length, 2);
            selectedBall.SetStroke(SELECTED_THICKNESS);
        }

        //Is called once each "frame"
        private void Update(object source, ElapsedEventArgs e)
        {
            try { Application.Current.Dispatcher.Invoke(UpdateBalls); }
            catch { }
        }

        public void UpdateBalls()
        {
            //Updates upperlimit based on window size
            try
            {
                BounceBall.UpperLimitX = Application.Current.MainWindow.ActualWidth;
                BounceBall.UpperLimitY = Application.Current.MainWindow.ActualHeight;
            }
            catch { }

            //Check if each ball is colliding with any other ball
            for (int i = 0; i < ball.Length; i++)
            {
                for (int j = i + 1; j < ball.Length; j++)
                {
                    if (CheckForCollision(ball[i], ball[j]))
                    {
                        CollisionResponse(ball[i], ball[j]);
                    }
                }
            }

            //Adds each balls velocity to its position
            foreach (BounceBall b in ball)
            {
                b.UpdatePosition();
            }
        }

        bool CheckForCollision(BounceBall a, BounceBall b)
        {
            return (a.Position - b.Position).Length <= a.radius + b.radius;
        }

        public void CollisionResponse(BounceBall a, BounceBall b)
        {
            double xV1 = a.Velocity.X;
            double yV1 = a.Velocity.Y;

            double xV2 = b.Velocity.X;
            double yV2 = b.Velocity.Y;

            a.Velocity.X = (xV1 * (a.mass - b.mass) + (2 * b.mass * xV2)) / (a.mass + b.mass);
            a.Velocity.Y = (yV1 * (a.mass - b.mass) + (2 * b.mass * yV2)) / (a.mass + b.mass);

            b.Velocity.X = (xV2 * (b.mass - a.mass) + (2 * a.mass * xV1)) / (a.mass + b.mass);
            b.Velocity.Y = (yV2 * (b.mass - a.mass) + (2 * a.mass * yV1)) / (a.mass + b.mass);

            //Stops balls from being stuck together
            //Similar to while(CheckForCollision(a, b)) 
            //Except this gives up if its taking to long
            for (int i = 0; i < 100; i++)
            {
                if (!CheckForCollision(a, b))
                    break;

                a.UpdatePosition();
                b.UpdatePosition();
            }
        }
    }
}
