using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;

namespace QSeeView.Controls
{
    public class PreviewSliderControl : Slider
    {
        public static readonly DependencyProperty PreviewControlProperty =
            DependencyProperty.Register(
                "PreviewControl",
                typeof(TextBox),
                typeof(PreviewSliderControl),
                new FrameworkPropertyMetadata(null));

        public TextBox PreviewControl
        {
            get { return (TextBox)GetValue(PreviewControlProperty); }
            set { SetValue(PreviewControlProperty, value); }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            PreviewControl.Visibility = Visibility.Visible;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            PreviewControl.Visibility = Visibility.Hidden;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (PreviewControl.IsVisible)
                PreviewControl.RenderTransform = new TranslateTransform(e.GetPosition(this).X + SystemParameters.CursorWidth, ActualHeight);
            base.OnMouseMove(e);
        }
    }
}
