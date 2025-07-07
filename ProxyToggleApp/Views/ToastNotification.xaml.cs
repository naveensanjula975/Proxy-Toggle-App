using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ProxyToggleApp.Views
{
    public partial class ToastNotification : UserControl
    {
        private System.Windows.Threading.DispatcherTimer? _autoHideTimer;

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(ToastNotification), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty NotificationTypeProperty =
            DependencyProperty.Register("NotificationType", typeof(NotificationType), typeof(ToastNotification), new PropertyMetadata(NotificationType.Info));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public NotificationType NotificationType
        {
            get { return (NotificationType)GetValue(NotificationTypeProperty); }
            set { SetValue(NotificationTypeProperty, value); }
        }

        public ToastNotification()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object? sender, RoutedEventArgs? e)
        {
            CloseToast();
        }

        public void CloseToast()
        {
            // Stop the auto-hide timer if it's running
            _autoHideTimer?.Stop();
            _autoHideTimer = null;
            
            var slideOut = new DoubleAnimation(0, -ActualHeight, TimeSpan.FromMilliseconds(300));
            slideOut.EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn };
            slideOut.Completed += (s, args) => { Visibility = Visibility.Collapsed; };
            BeginAnimation(Canvas.TopProperty, slideOut);
        }

        public void ShowToast()
        {
            // Stop any existing timer
            _autoHideTimer?.Stop();
            
            Visibility = Visibility.Visible;
            var slideIn = new DoubleAnimation(-ActualHeight, 20, TimeSpan.FromMilliseconds(300));
            slideIn.EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut };
            BeginAnimation(Canvas.TopProperty, slideIn);

            // Auto-hide after 5 seconds
            _autoHideTimer = new System.Windows.Threading.DispatcherTimer();
            _autoHideTimer.Interval = TimeSpan.FromSeconds(5);
            _autoHideTimer.Tick += (s, e) => { 
                CloseToast(); 
                _autoHideTimer.Stop(); 
                _autoHideTimer = null; 
            };
            _autoHideTimer.Start();
        }
    }

    public enum NotificationType
    {
        Info,
        Success,
        Warning,
        Error
    }
}
