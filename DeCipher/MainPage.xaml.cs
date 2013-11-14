using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DeCipher
{
    public sealed partial class MainPage : Page
    {
        int nlin = 8;  // quotes are limited to nlin number of lines and 
        // 78 characters per line. 
        int nalph = 26;
        string alph = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string code = getCode();      // on program initialization, quote, is selected at random 
        string[] quote = getQuote();  // and, code, a random cipher, is selected 
        string dcod = "                          ";
        string win = "YOU WIN - YOU WIN - YOU WIN - YOU WIN - YOU WIN - YOU WIN - YOU WIN - YOU WIN ";

        private const int columns = 40;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Create cryptogram character controls programmatically and add them as children to the "cryptogramLines"
            // StackPanel defined in the XAML file
            string text = "THIS IS A TEST MESSAGE THAT IS LONG ENOUGH THAT WE HAVE TO WRAP AROUND (BECAUSE IT'S MORE THAN FORTY COLUMNS LONG)";
            this.cryptogramLines.Children.Clear();
            int columnIndex = 0;
            StackPanel cryptogramLine = null;

            foreach (char c in text)
            {
                // TODO: Wrap at word boundaries or insert a hyphen
                if (columnIndex >= MainPage.columns)
                {
                    columnIndex = 0;
                }

                if (columnIndex == 0)
                {
                    cryptogramLine = new StackPanel();
                    cryptogramLine.Orientation = Orientation.Horizontal;
                    this.cryptogramLines.Children.Add(cryptogramLine);
                }

                cryptogramLine.Children.Add(new CryptogramCharacter(c));
                ++columnIndex;
            }
        }

        // This method is called by a button click 
        // On first click, a quote is selected at random from the quote database, 
        // that quote is enciphered into ciphr and displayed. 
        // Letter frequency count is also displayed. 
        // On Further clicks, this method takes two letters from the input text box, subAB, 
        // and displays B in dcode at every location where A appeared in ciphr. 
        private void buttonClick(object sender, RoutedEventArgs e)
        {
            string[] ciphr = encipherQuote(quote, code);
            string[] dcode = new string[nlin];
            int[] cnt = cntLetters(ciphr);
            string alphText = showAlph(alph);
            string cntText = showCnt(cnt);
            //
            if (String.Equals(button.Content, "Display Cipher")) { }
            else
            {
                string a2b = subAB.Text;
                string dcod2 = subBwhereAoccurs(dcod, a2b);
                dcod = dcod2;
            }
            dcode = showDcode(ciphr, dcod);
            displayOutput(ciphr, dcode);
            button.Content = "Substitute B for A";

            if (haveIWonYet(quote, dcode))
            {
                alphText = win;
                cntText = "";
            }
            alpha.Text = alphText;
            count.Text = cntText;
        }

        private bool haveIWonYet(string[] quote, string[] dcode)
        {
            // throw new NotImplementedException();
            int l, nl = 0;
            bool val;
            val = false;
            for (l = 0; l < nlin; l++)
            {
                if (String.Equals(dcode[l], quote[l])) nl++;
            }
            if (nl == nlin)
            {

                button.Content = "EXIT";
                subab.Text = "";
                subAB.Text = "";
                val = true;
            }
            return val;
        }

        private void displayOutput(string[] ciphr, string[] dcode)
        {
            // throw new NotImplementedException();
            ciphr0.Text = ciphr[0];
            ciphr1.Text = ciphr[1];
            ciphr2.Text = ciphr[2];
            ciphr3.Text = ciphr[3];
            ciphr4.Text = ciphr[4];
            ciphr5.Text = ciphr[5];
            ciphr6.Text = ciphr[6];
            ciphr7.Text = ciphr[7];
            dcode0.Text = dcode[0];
            dcode1.Text = dcode[1];
            dcode2.Text = dcode[2];
            dcode3.Text = dcode[3];
            dcode4.Text = dcode[4];
            dcode5.Text = dcode[5];
            dcode6.Text = dcode[6];
            dcode7.Text = dcode[7];
        }

        private string[] showDcode(string[] ciphr, string dcod)  // copy solved characters into dcode lines 
        {
            // throw new NotImplementedException();
            int ic, j, l;
            string[] dcode = new string[nlin];
            for (l = 0; l < nlin; l++)
            {
                dcode[l] = "";
                for (j = 0; j < ciphr[l].Length; j++)
                {
                    ic = alph.IndexOf(ciphr[l][j]);
                    if (ic > -1)
                        dcode[l] = dcode[l] + dcod[ic];
                    else
                        dcode[l] = dcode[l] + ciphr[l][j];
                }
            }
            return dcode;
        }

        private string subBwhereAoccurs(string dcod, string a2b)
        {
            // throw new NotImplementedException();
            string dcod2 = "";
            if (a2b.Length != 3)
            {
                dcod2 = dcod;
                return dcod2;
            }
            char cA = Char.ToUpper(a2b[0]);
            char cB = Char.ToUpper(a2b[2]);
            int ia = alph.IndexOf(cA);  // I need some more error checking here, what if the three
            int ib = alph.IndexOf(cB);  // input characters are not letters of the alphabet?
            string s1 = "";
            if (ia > 0) s1 = dcod.Substring(0, ia);
            string s2 = dcod.Substring(ia + 1, nalph - 1 - s1.Length);
            dcod2 = s1 + cB + s2;
            return dcod2;
        }

        private string showCnt(int[] cnt)
        {
            // throw new NotImplementedException();
            int i;
            string cntText = "";
            for (i = 0; i < nalph; i++)   // construct text for alphabet and for letter frequency count 
            {
                if (cnt[i] < 100)
                    cntText = cntText + " ";
                if (cnt[i] < 10)
                    cntText = cntText + " ";
                cntText = cntText + cnt[i].ToString();
            }
            return cntText;
        }

        private string showAlph(string alph)  // construct text for alphabet display 
        {
            // throw new NotImplementedException();
            int i;
            string alphText = "";
            for (i = 0; i < nalph; i++)
            {
                alphText = alphText + "  " + alph[i];
            }
            return alphText;
        }

        private int[] cntLetters(string[] ciphr)
        {
            // throw new NotImplementedException();
            int i, j, l;
            int[] cnt = new int[nalph];
            for (i = 0; i < nalph; i++)  //initialize cnt to zero 
                cnt[i] = 0;

            for (l = 0; l < nlin; l++)   // count letter frequency within ciphr 
            {
                for (i = 0; i < nalph; i++)
                {
                    for (j = 0; j < ciphr[l].Length; j++)
                    {
                        if (alph[i] == ciphr[l][j])
                            cnt[i]++;
                    }
                }
            }
            return cnt;
        }

        private string[] encipherQuote(string[] quote, string code)
        {
            // throw new NotImplementedException();
            int ic, j, l;
            // string[] quote = getQuote();
            string[] ciphr;
            ciphr = new string[nlin];
            for (l = 0; l < nlin; l++)  // encipher the quote
            {
                ciphr[l] = "";
                for (j = 0; j < quote[l].Length; j++)
                {
                    ic = alph.IndexOf(quote[l][j]);
                    if (ic > -1)
                        ciphr[l] = ciphr[l] + code[ic];
                    else
                        ciphr[l] = ciphr[l] + quote[l][j];
                }
            }
            return ciphr;
        }

        static string[] getQuote()
        {
            int i, l, nlin = 8;
            int quot = 6;
            Random rnd = new Random();
            string[] quote; quote = new string[nlin];
            string[] quotes = 
            {
            //        1         2         3         4         5         6         7
            //23456789012345678901234567890123456789012345678901234567890123456789012345678
            "ONCE UPON A MIDNIGHT DREARY, WHILE I PONDERED, WEAK AND WEARY, ", 
            "OVER MANY A QUAINT AND CURIOUS VOLUME OF FORGOTTON LORE, ", 
            "WHILE I NODDED, NEARLY NAPPING, SUDDENLY THERE CANE A TAPPING, ", 
            "AS OF SOME ONE GENTLY RAPPING, RAPPING AT MY CHAMBER DOOR. ", 
            "TIS SOME VISITOR, I MUTTERED, TAPPING AT MY CHAMBER DOOR-", 
            "ONLY THIS, AND NOTHING MORE. ", 
            "EDGAR ALLAN POE - THE RAVEN", 
            " ",
            //        1         2         3         4         5         6         7
            //23456789012345678901234567890123456789012345678901234567890123456789012345678
            "FOUR SCORE AND SEVEN YEARS AGO OUR FATHERS BROUGHT FORTH ON THIS CONTINENT, ",
            "A NEW NATION, CONCEIVED IN LIBERTY, AND DEDICATED TO THE PROPOSITION THAT ALL ",
            "MEN ARE CREATED EQUAL. NOW WE ARE ENGAGED IN A GREAT CIVIL WAR, TESTING ",
            "WHETHER THAT NATION, OR ANY NATION SO CONCEIVED AND SO DEDICATED, CAN LONG ",
            "ENDURE. WE ARE MET ON A GREAT BATTLE-FIELD OF THAT WAR. WE HAVE COME TO ", 
            "DEDICATE A PORTION OF THAT FIELD, AS A FINAL RESTING PLACE FOR THOSE WHO HERE ",
            "GAVE THEIR LIVES THAT THAT NATION MIGHT LIVE. IT IS ALTOGETHER FITTING AND ", 
            "PROPER THAT WE SHOULD DO THIS. BUT, IN A LARGER SENSE - ABRAHAM LINCOLN",
            //
            //23456789012345678901234567890123456789012345678901234567890123456789012345678
            "THERE WAS LIFE ON THIS WORLD, EXTRAVAGANT IN ITS NUMBERS AND VARIETY. THERE ", 
            "WERE JUMPING SPIDERS AT THE CHILLY TOPS OF THE HIGHEST MOUNTAINS AND SULFUR-", 
            "EATING WORMS IN HOT VENTS GUSHING UP THROUTH RIDGES ON THE OCEAN FLOORS. THERE", 
            "WERE BEINGS THAT COULD LIVE ONLY IN CONCENTRATED SULFURIC ACID, AND BEINGS ", 
            "THAT WERE DESTROYED BY CONCENTRATED SULFURIC ACID; ORGANISMS THAT WERE ", 
            "POISONED BY OXYGEN, AND ORGANISMS THAT COULD SURVIVE ONLY IN OXYGEN, THAT ", 
            "ACTUALLY BREATHED THE STUFF. CARL SAGAN - CONTACT", 
            "", 
            //
            //23456789012345678901234567890123456789012345678901234567890123456789012345678
            "HOW SHALL I EVER FORGET THAT DREADFUL VIGIL? I COULD NOT HEAR A SOUND, NOT ", 
            "EVEN DRAWING OF A BREATH, AND YET I KNEW THAT MY COMPANION SAT OPEN-EYED, ", 
            "WITHIN A FEW FEET OF ME, IN THE SAME STATE OF NERVOUS TENSION IN WHICH I WAS ", 
            "MYSELF. THE SHUTTERS CUT OFF THE LEAST RAY OF LIGHT, AND WE WAITED IN ABSOLUTE", 
            "DARKNESS. FROM OUTSIDE CAME THE OCCASIONAL CRY OF A NIGHT-BIRD, AND ONCE AT ", 
            "OUR VERY WINDOW A LONG DRAWN, CAT-LIKE WHINE, WHICH TOLD US THAT THE CHEETAH ", 
            "WAS INDEED AT LIBERTY. SIR ARTHUR CONAN DOYLE - THE SPECKLED BAND", 
            "", 
            //
            //23456789012345678901234567890123456789012345678901234567890123456789012345678
            "IT WAS THEN TEN IN THE MORNING; THE RAYS OF THE SUN STRUCK THE SURFACE OF THE ", 
            "WAVES AT RATHER AN OBLIQUE ANGLE, AND AT THE TOUCH OF THEIR LIGHT DECOMPOSED ", 
            "BY REFRACTION AS THOUGH THROUGH A PRISM, FLOWERS, ROCKS, PLANTS, SHELLS AND ", 
            "POLYPI WERE SHADED AT THE EDGES OF THE SEVEN SOLAR CONSTANTS. IT WAS MARVELOUS ", 
            "A FEAST FOR THE EYES, THIS COMPLICATION OF COLOURED TINTS A PERFECT KALEIDO-", 
            "SCOPE OF GREEN, YELLOW, ORANGE, VIOLET, INDIGO AND BLUE; IN ONE WORD THE WHOLE ", 
            "PALETTE OF AN ENTHUSIASTIC COLOURIST! JULES VERNE - TWENTY THOUSAND LEAGUES ", 
            "UNDER THE SEA", 
            //
            //23456789012345678901234567890123456789012345678901234567890123456789012345678
            "EAGERLY SHE POURED OVER THE RIDDLE UNTIL SHE SOLVED IT. THE SOLUTION POINTED ", 
            "HER TO ANOTHER PART OF THE HOUSE, WHERE SHE FOUND ANOTHER CARD AND ANOTHER ", 
            "RIDDLE. SHE SOLVED THIS ONE TOO, RACING ON TO THE NEXT CARD. RUNNING WILDLY, ", 
            "SHE DARTED BACK AND FORTH ACROSS THE HOUSE, FROM CLUE TO CLUE, UNTIL AT LAST ", 
            "SHE FOUND A CLUE THAT DIRECTED HER BACK TO HER OWN BEDROOM. SOPHIE DASHED ", 
            "UP THE STAIRS, RUSHED INTO HER ROOM, AND STOPPED IN HER TRACKS. THERE IN THE ", 
            "MIDDLE OF THE ROOM SAT A SHINING RED BICYCLE WITH A RIBBON TIED TO THE ", 
            "HANDLEBARS. SOPHIE SHRIEKED WITH DELIGHT. DAN BROWN - THE DAVINCI CODE" 
            };

            i = (int)(quot * rnd.NextDouble());

            for (l = 0; l < nlin; l++)
            {
                quote[l] = quotes[i * nlin + l];
            }
            return quote;
        }

        static string getCode()
        {
            int ia, nalph = 26;
            string alph = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            // string code = "BCDEFGHIJKLMNOPQRSTUVWXYZA";
            string code = "";
            bool[] used; used = new bool[nalph];
            Random rnd = new Random();
            double x;

            for (int i = 0; i < nalph; i++)
            {
                used[i] = false;
            }
            for (int na = 0; na < nalph; na++)
            {
                ia = (int)(nalph * rnd.NextDouble());
                while (used[ia])       // poor coding, but easy to write...  
                {
                    ia = (int)(nalph * rnd.NextDouble());
                }
                used[ia] = true;
                code = code + alph[ia];
            }
            return code;
        }
    }
}
