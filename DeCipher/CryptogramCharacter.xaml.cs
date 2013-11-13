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
        // Prototype for event handlers for when the selected letter changes
        public delegate void SolutionCharacterChangedEventHandler(object sender, CryptogramCharacter.SolutionCharacterChangedEventArgs args);

        // Event that can be subscribed to that fires when the selected letter changes
        public event SolutionCharacterChangedEventHandler SolutionCharacterChanged;

        // Dictionary to map key presses (VirtualKey) to letters (Char)
        private Dictionary<VirtualKey, char> keyToChar;

        public CryptogramCharacter()
        {
            this.InitializeComponent();

            // Set up mapping from VirtualKey enum to Char
            this.keyToChar = new Dictionary<VirtualKey, char>();
            this.keyToChar[VirtualKey.A] = 'A';
            this.keyToChar[VirtualKey.B] = 'B';
            this.keyToChar[VirtualKey.C] = 'C';
            this.keyToChar[VirtualKey.D] = 'D';
            this.keyToChar[VirtualKey.E] = 'E';
            this.keyToChar[VirtualKey.F] = 'F';
            this.keyToChar[VirtualKey.G] = 'G';
            this.keyToChar[VirtualKey.H] = 'H';
            this.keyToChar[VirtualKey.I] = 'I';
            this.keyToChar[VirtualKey.J] = 'J';
            this.keyToChar[VirtualKey.K] = 'K';
            this.keyToChar[VirtualKey.L] = 'L';
            this.keyToChar[VirtualKey.M] = 'M';
            this.keyToChar[VirtualKey.N] = 'N';
            this.keyToChar[VirtualKey.O] = 'O';
            this.keyToChar[VirtualKey.P] = 'P';
            this.keyToChar[VirtualKey.Q] = 'Q';
            this.keyToChar[VirtualKey.R] = 'R';
            this.keyToChar[VirtualKey.S] = 'S';
            this.keyToChar[VirtualKey.T] = 'T';
            this.keyToChar[VirtualKey.U] = 'U';
            this.keyToChar[VirtualKey.V] = 'V';
            this.keyToChar[VirtualKey.W] = 'W';
            this.keyToChar[VirtualKey.X] = 'X';
            this.keyToChar[VirtualKey.Y] = 'Y';
            this.keyToChar[VirtualKey.Z] = 'Z';

            // Setup event handler to update the letter shown in the UI when the selected character changes
            this.SolutionCharacterChanged += new SolutionCharacterChangedEventHandler(this.UserControl_SolutionCharacterChanged);
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
            // Map the key to a character and raise an event to say the letter changed
            char character;
            if (this.keyToChar.TryGetValue(e.Key, out character))
            {
                this.SolutionCharacterChanged(this, new CryptogramCharacter.SolutionCharacterChangedEventArgs(character));
            }
        }

        private void UserControl_SolutionCharacterChanged(object sender, SolutionCharacterChangedEventArgs args)
        {
            // When the letter changes, update the UI
            this.solutionCharacter.Text = args.SolutionCharacter.ToString();
        }

        // Event arguments for when the selected letter changes
        public class SolutionCharacterChangedEventArgs : EventArgs
        {
            public char SolutionCharacter { get; private set; }

            public SolutionCharacterChangedEventArgs(char c)
            {
                this.SolutionCharacter = c;
            }
        }
    }
}
