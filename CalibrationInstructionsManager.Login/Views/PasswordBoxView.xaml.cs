using System.Windows;
using System.Windows.Controls;

namespace CalibrationInstructionsManager.Login.Views
{
    /// <summary>
    /// Interaction logic for PasswordBoxView.xaml
    /// </summary>
    /// <summary>
    /// Interaction logic for PasswordBoxView.xaml
    /// </summary>
    public partial class PasswordBoxView : UserControl
    {
        private bool _isPasswordChanging;
        public string Password { get { return (string)GetValue(PasswordProperty); } set { SetValue(PasswordProperty, value); } }
        public PasswordBoxView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for Password.
        /// This enables binding, etc...
        /// </summary>
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                "Password",
                typeof(string),
                typeof(PasswordBoxView),
                new PropertyMetadata(string.Empty, PasswordPropertyChanged));

        private static void PasswordPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is PasswordBoxView passwordBox)
            {
                passwordBox.UpdatePassword();
            }
        }

        /// <summary>
        /// Sending from passwordBox.Password to property Password
        /// </summary>
        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            _isPasswordChanging = true;
            Password = passwordBox.Password;
            _isPasswordChanging = false;
        }

        /// <summary>
        /// When binding changes, we set the Password value to our passwordBox.Password
        /// </summary>
        private void UpdatePassword()
        {
            if (!_isPasswordChanging)
            {
                passwordBox.Password = Password;
            }
        }
    }
}
