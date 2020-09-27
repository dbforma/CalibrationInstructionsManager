using System.Windows;

namespace CalibrationInstructionsManager.Core.Behaviors
{
    public class ResponsivenessBehavior
    {
        #region Attached Properties
        /// <summary>
        /// Registering an attached property, so we can use it on any WPF element
        /// In xaml add to StackPanel element after importing the namespace: behaviors:ResponsivenessBehavior.IsResponsive="True"
        /// </summary>
        public static DependencyProperty IsResponsiveProperty = DependencyProperty.RegisterAttached(
            "IsResponsive", 
            typeof(bool), 
            typeof(ResponsivenessBehavior), 
            new PropertyMetadata(false, OnIsResponsiveChanged));

        public static bool GetIsResponsive(DependencyObject obj)
        {
            return (bool) obj.GetValue(IsResponsiveProperty);
        }

        public static void SetIsResponsive(DependencyObject obj, bool value)
        {
            obj.SetValue(IsResponsiveProperty, value);
        }
        /// <summary>
        /// Registering an attached property
        /// Representing the breakpoint to switch from horizontal to vertical on StackPanel 'behaviors:ResponsivenessBehavior.HorizontalBreakpoint="XXX"' 
        /// </summary>
        public static DependencyProperty HorizontalBreakpointProperty = DependencyProperty.RegisterAttached(
            "HorizontalBreakpoint",
            typeof(double),
            typeof(ResponsivenessBehavior),
            new PropertyMetadata(double.MaxValue));

        public static double GetHorizontalBreakpoint(DependencyObject obj)
        {
            return (double)obj.GetValue(HorizontalBreakpointProperty);
        }

        public static void SetHorizontalBreakpoint(DependencyObject obj, double value)
        {
            obj.SetValue(HorizontalBreakpointProperty, value);
        }

        /// <summary>
        /// Registering an attached property
        /// Whenever the breakpoint hits we will apply setters to the IsResponsive element 
        /// </summary>
        public static DependencyProperty HorizontalBreakpointSettersProperty = DependencyProperty.RegisterAttached(
        "HorizontalBreakpointSetters",
        typeof(SetterBaseCollection),
        typeof(ResponsivenessBehavior),
        new PropertyMetadata(new SetterBaseCollection()));

        public static SetterBaseCollection GetHorizontalBreakpointSetters(DependencyObject obj)
        {
            return (SetterBaseCollection)obj.GetValue(HorizontalBreakpointSettersProperty);
        }

        public static void SetHorizontalBreakpointSetters(DependencyObject obj, SetterBaseCollection value)
        {
            obj.SetValue(HorizontalBreakpointSettersProperty, value);
        }

        /// <summary>
        /// Registering an attached property
        /// Tracks if setters are active or inactive
        /// </summary>
        public static DependencyProperty IsHorizontalBreakpointSettersActiveProperty = DependencyProperty.RegisterAttached(
            "IsHorizontalBreakpointSettersActive",
            typeof(bool),
            typeof(ResponsivenessBehavior),
            new PropertyMetadata(false));

        public static bool GetIsHorizontalBreakpointSettersActive(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsHorizontalBreakpointSettersActiveProperty);
        }

        public static void SetIsHorizontalBreakpointSettersActive(DependencyObject obj, bool value)
        {
            obj.SetValue(IsHorizontalBreakpointSettersActiveProperty, value);
        }

        #endregion // Attached Properties

        #region Methods

        private static void OnIsResponsiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs dpe)
        {
            if (d is FrameworkElement element)
            {
                Window window = Application.Current.MainWindow;

                if (GetIsResponsive(element))
                {
                    window.SizeChanged += (s, e) => UpdateElement(window, element);
                }
                else
                {
                    window.SizeChanged -= (s, e) => UpdateElement(window, element);
                }
            }
        }

        private static void UpdateElement(Window window, FrameworkElement element)
        {
            double windowWidth = window.Width;
            double breakpointWidth = GetHorizontalBreakpoint(element);

            if (windowWidth >= breakpointWidth && !GetIsHorizontalBreakpointSettersActive(element))
            {
                SetIsHorizontalBreakpointSettersActive(element, true);
                element.Style = CreateResponsivenessStyle(element);
            }

            else if (windowWidth < breakpointWidth && GetIsHorizontalBreakpointSettersActive(element))
            {
                SetIsHorizontalBreakpointSettersActive(element, false);
                element.Style = element.Style.BasedOn;
            }
        }

        private static Style CreateResponsivenessStyle(FrameworkElement element)
        {
            Style responsivenessStyle = new Style(element.GetType(), element.Style);

            foreach (Setter setter in GetHorizontalBreakpointSetters(element))
            {
                responsivenessStyle.Setters.Add(setter);
            }

            return responsivenessStyle;
        }

        #endregion // Methods

    }
}
