using System;
using System.Collections.Generic;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DeCipher
{
    public sealed partial class CryptogramCharacter : UserControl
    {
        // Use a non-breaking space so that the white space is preserved
        private const char spaceLetter = '\u00a0';

        // Use a null character to represent a letter that has not been mapped in the cryptogram
        private const char emptyLetter = '\0';

        // These dependency properties expose the cryptogram/solution letters to XAML's data binding mechanism
        // TODO: The cryptogram letter probably doesn't need to be a dependency property since it doesn't change
        public static readonly DependencyProperty CryptogramLetterProperty = DependencyProperty.Register("CryptogramLetter", typeof(char), typeof(CryptogramCharacter), new PropertyMetadata('M'));
        public static readonly DependencyProperty SolutionLetterProperty = DependencyProperty.Register("SolutionLetter", typeof(char), typeof(CryptogramCharacter), new PropertyMetadata(CryptogramCharacter.emptyLetter));

        // Dictionary to map key presses (VirtualKey) to letters (Char)
        private static Dictionary<VirtualKey, char> keyToChar;

        public event EventHandler SolutionLetterChanged;

        // These C# properties just call into the XAML dependency property infrastructure (GetValue/SetValue) so that
        // the types can be conveniently accessed from C# code
        public char CryptogramLetter
        {
            get
            {
                return (char)this.GetValue(CryptogramCharacter.CryptogramLetterProperty);
            }
            set
            {
                this.SetValue(CryptogramCharacter.CryptogramLetterProperty, value);
            }
        }

        public char SolutionLetter
        {
            get
            {
                return (char)this.GetValue(CryptogramCharacter.SolutionLetterProperty);
            }
            set
            {
                this.SetValue(CryptogramCharacter.SolutionLetterProperty, value);
            }
        }

        static CryptogramCharacter()
        {
            // Set up mapping from VirtualKey enum to Char
            CryptogramCharacter.keyToChar = new Dictionary<VirtualKey, char>();
            CryptogramCharacter.keyToChar[VirtualKey.A] = 'A';
            CryptogramCharacter.keyToChar[VirtualKey.B] = 'B';
            CryptogramCharacter.keyToChar[VirtualKey.C] = 'C';
            CryptogramCharacter.keyToChar[VirtualKey.D] = 'D';
            CryptogramCharacter.keyToChar[VirtualKey.E] = 'E';
            CryptogramCharacter.keyToChar[VirtualKey.F] = 'F';
            CryptogramCharacter.keyToChar[VirtualKey.G] = 'G';
            CryptogramCharacter.keyToChar[VirtualKey.H] = 'H';
            CryptogramCharacter.keyToChar[VirtualKey.I] = 'I';
            CryptogramCharacter.keyToChar[VirtualKey.J] = 'J';
            CryptogramCharacter.keyToChar[VirtualKey.K] = 'K';
            CryptogramCharacter.keyToChar[VirtualKey.L] = 'L';
            CryptogramCharacter.keyToChar[VirtualKey.M] = 'M';
            CryptogramCharacter.keyToChar[VirtualKey.N] = 'N';
            CryptogramCharacter.keyToChar[VirtualKey.O] = 'O';
            CryptogramCharacter.keyToChar[VirtualKey.P] = 'P';
            CryptogramCharacter.keyToChar[VirtualKey.Q] = 'Q';
            CryptogramCharacter.keyToChar[VirtualKey.R] = 'R';
            CryptogramCharacter.keyToChar[VirtualKey.S] = 'S';
            CryptogramCharacter.keyToChar[VirtualKey.T] = 'T';
            CryptogramCharacter.keyToChar[VirtualKey.U] = 'U';
            CryptogramCharacter.keyToChar[VirtualKey.V] = 'V';
            CryptogramCharacter.keyToChar[VirtualKey.W] = 'W';
            CryptogramCharacter.keyToChar[VirtualKey.X] = 'X';
            CryptogramCharacter.keyToChar[VirtualKey.Y] = 'Y';
            CryptogramCharacter.keyToChar[VirtualKey.Z] = 'Z';
        }

        public CryptogramCharacter(char cryptogramLetter)
        {
            this.InitializeComponent();

            // Set the cryptogram letter accordingly
            if (Char.IsLetter(cryptogramLetter))
            {
                this.CryptogramLetter = cryptogramLetter;
            }
            else
            {
                if (cryptogramLetter == ' ')
                {
                    // Handle spaces specially; we want to use a non-breaking space so that the whitespace isn't simply
                    // discarded
                    this.CryptogramLetter = CryptogramCharacter.spaceLetter;
                }
                else
                {
                    this.CryptogramLetter = cryptogramLetter;
                }

                // Pass non-letters through to the solution
                this.SolutionLetter = this.CryptogramLetter;

                // Make it so that this non-letter character can't get focus
                this.IsTabStop = false;
            }
        }

        public void OnSolutionLetterChanged()
        {
            // TODO: Should this support deleting the solution letter? Right now it doesn't
            if (this.SolutionLetterChanged != null && Char.IsLetter(this.CryptogramLetter) && Char.IsLetter(this.SolutionLetter))
            {
                this.SolutionLetterChanged(this, null);
            }
        }

        private void UserControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Tapping/clicking the control will put focus on it
            this.Focus(Windows.UI.Xaml.FocusState.Programmatic);
        }

        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            // Highlight the control when it's focused
            // TODO: Use a style or something
            this.grid.Background = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            // Remove the highlight when the control loses focus
            // TODO: Use a style or something
            this.grid.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }

        // TODO: Handle touch-only input as well (e.g. pop up the touch keyboard)
        private void UserControl_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            // Map the key to a character and set the solution letter appropriately
            char character;
            if (CryptogramCharacter.keyToChar.TryGetValue(e.Key, out character))
            {
                this.SolutionLetter = character;
                this.OnSolutionLetterChanged();
            }
            // TODO: Support clearing characters elsewhere
            //else
            //{
            //    // Handle a few non-letter keys specially
            //    switch (e.Key)
            //    {
            //        case VirtualKey.Delete:
            //        case VirtualKey.Back:
            //        case VirtualKey.Clear:
            //            // Set this letter back to the "empty" state
            //            this.SolutionLetter = CryptogramCharacter.emptyLetter;
            //            break;
            //    }
            //}
        }
    }
}
