using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DeCipher
{
    public sealed partial class CryptogramCharacter : UserControl
    {
        public CryptogramCharacter()
        {
            this.InitializeComponent();
        }

        private void UserControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Focus(Windows.UI.Xaml.FocusState.Programmatic);
        }

        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            // TODO: Use a style or something
            this.grid.Background = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            // TODO: Use a style or something
            this.grid.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }

        private void UserControl_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            // TODO: I don't think this is getting called...
            Debug.Assert(false);
        }
    }
}
