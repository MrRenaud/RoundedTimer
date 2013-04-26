using System;
using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace RoundedTimer
{
    public sealed partial class RoundedTimer : INotifyPropertyChanged
    {
        #region Depedency properties
        public static readonly DependencyProperty BackgroundFillProperty =
           DependencyProperty.Register("BackgroundFill", typeof(Brush), typeof(RoundedTimer), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 48, 146, 213))));

        public Brush BackgroundFill
        {
            get { return (Brush)GetValue(BackgroundFillProperty); }
            set { SetValue(BackgroundFillProperty, value); }
        }

        public static readonly DependencyProperty ForegroundFillProperty =
            DependencyProperty.Register("ForegroundFill", typeof(Brush), typeof(RoundedTimer), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 245, 96, 71))));

        public Brush ForegroundFill
        {
            get { return (Brush)GetValue(ForegroundFillProperty); }
            set { SetValue(ForegroundFillProperty, value); }
        }

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(RoundedTimer), new PropertyMetadata(default(TimeSpan)));

        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(RoundedTimer), new PropertyMetadata(default(double)));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty StrokeBrushProperty =
            DependencyProperty.Register("StrokeBrush", typeof(Brush), typeof(RoundedTimer), new PropertyMetadata(default(Brush)));

        public Brush StrokeBrush
        {
            get { return (Brush)GetValue(StrokeBrushProperty); }
            set { SetValue(StrokeBrushProperty, value); }
        }

        public static readonly DependencyProperty TextForegroundProperty =
            DependencyProperty.Register("TextForeground", typeof(Brush), typeof(RoundedTimer), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        public Brush TextForeground
        {
            get { return (Brush)GetValue(TextForegroundProperty); }
            set { SetValue(TextForegroundProperty, value); }
        }
        #endregion

        public RoundedTimer()
        {
            InitializeComponent();

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += timer_Tick;
        }

        private readonly DispatcherTimer _timer;

        public TimeSpan TimeElapsed { get; set; }

        public Visibility FirstQuarterVisibility { get; set; }
        public double FirstQuarterAngle { get; set; }
        public Visibility SecondQuarterVisibility { get; set; }
        public double SecondQuarterAngle { get; set; }
        public Visibility ThirdQuarterVisibility { get; set; }
        public double ThirdQuarterAngle { get; set; }
        public Visibility FourthQuarterVisibility { get; set; }
        public double FourthQuarterAngle { get; set; }

        void timer_Tick(object sender, object e)
        {
            if (Math.Abs(Duration.TotalSeconds - 0) < 0.01)
                return;
            TimeElapsed = TimeElapsed.Add(TimeSpan.FromSeconds(1));
            if (Math.Abs(TimeElapsed.TotalSeconds - Duration.TotalSeconds) < 0.01)
            {
                TimeElapsed = TimeSpan.FromSeconds(0);
                _timer.Stop();
            }
            var ratio = TimeElapsed.TotalSeconds / Duration.TotalSeconds;
            if (ratio < 0.25)
            {
                FirstQuarterVisibility = Visibility.Visible;
                SecondQuarterVisibility = Visibility.Visible;
                ThirdQuarterVisibility = Visibility.Visible;
                FourthQuarterVisibility = Visibility.Visible;
                FirstQuarterAngle = -90 * (ratio * 4);
                SecondQuarterAngle = 0;
                ThirdQuarterAngle = 0;
                FourthQuarterAngle = 0;
            }
            else if (ratio < 0.5)
            {
                FirstQuarterVisibility = Visibility.Collapsed;
                SecondQuarterVisibility = Visibility.Visible;
                ThirdQuarterVisibility = Visibility.Visible;
                FourthQuarterVisibility = Visibility.Visible;
                FirstQuarterAngle = 0;
                SecondQuarterAngle = 90 * ((ratio - 0.25) * 4);
                ThirdQuarterAngle = 0;
                FourthQuarterAngle = 0;
            }
            else if (ratio < 0.75)
            {
                FirstQuarterVisibility = Visibility.Collapsed;
                SecondQuarterVisibility = Visibility.Collapsed;
                ThirdQuarterVisibility = Visibility.Visible;
                FourthQuarterVisibility = Visibility.Visible;
                FirstQuarterAngle = 0;
                SecondQuarterAngle = 0;
                ThirdQuarterAngle = -90 * ((ratio - 0.5) * 4);
                FourthQuarterAngle = 0;
            }
            else
            {
                FirstQuarterVisibility = Visibility.Collapsed;
                SecondQuarterVisibility = Visibility.Collapsed;
                ThirdQuarterVisibility = Visibility.Collapsed;
                FourthQuarterVisibility = Visibility.Visible;
                FirstQuarterAngle = 0;
                SecondQuarterAngle = 0;
                ThirdQuarterAngle = 0;
                FourthQuarterAngle = 90 * ((ratio - 0.75) * 4);
            }


            RaisePropertyChanged("FirstQuarterAngle");
            RaisePropertyChanged("SecondQuarterAngle");
            RaisePropertyChanged("ThirdQuarterAngle");
            RaisePropertyChanged("FourthQuarterAngle");
            RaisePropertyChanged("FirstQuarterVisibility");
            RaisePropertyChanged("SecondQuarterVisibility");
            RaisePropertyChanged("ThirdQuarterVisibility");
            RaisePropertyChanged("FourthQuarterVisibility");
            RaisePropertyChanged("Countdown");
        }

        public string Countdown
        {
            get
            {
                var diff = (Duration - TimeElapsed);
                return String.Format("{0:00}:{1:00}", diff.Minutes, diff.Seconds);
            }
        }

        public void Start()
        {
            TimeElapsed = TimeSpan.FromSeconds(0);
            _timer.Start();
        }

        public void Stop()
        {
            if (_timer.IsEnabled)
                _timer.Stop();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

        public void Dispose()
        {
            if (_timer != null)
                _timer.Tick -= timer_Tick;
        }
    }
}
