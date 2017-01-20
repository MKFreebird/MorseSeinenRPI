using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorseSeinenRPI
{
    class MorseLibrary
    {
        /* Lookup array for translation */
        public string[,] morseTable = new string[38, 2] {
            {"a", ".-" },   // 0
            {"b", "-..."},
            {"c", "-.-."},
            {"d", "-.."},
            {"e", "."},     // 4
            {"f", "..-."},
            {"g", "--." },
            {"h", "...."},
            {"i", ".."},
            {"j", ".---" }, // 9
            {"k", "-.-" },
            {"l", ".-.." },
            {"m", "--"},
            {"n", "-."},
            {"o", "---"},   // 14
            {"p", ".--."},
            {"q", "--.-"},
            {"r", ".-."},
            {"s", "..." },
            {"t", "-" },    // 19
            {"u", "..-"},
            {"v", "...-"},
            {"w", ".--"},
            {"x", "-..-"},
            {"y", "-.--"},  // 24
            {"z", "--.."},
            {"0", "-----"},
            {"1", ".----"},
            {"2", "..---"},
            {"3", "...--"}, // 29
            {"4", "....-"},
            {"5", "....."},
            {"6", "-...."},
            {"7", "--..."},
            {"8", "---.."}, // 34
            {"9", "----."},
            {" ", "-...-"},
            {"eom", "...-.-"}
        };

        /* Translate every individual morse character and add it to translatedMessage */
        public string Translate(List<string> message)
        {
            string translatedMessage = "";
       //     List<string> morseMessageList = message.Split('/').ToList();

            foreach (string morseLetter in message)
            {
                for (int i = 0; i < morseTable.GetLength(0); i++)
                {
                    if (morseLetter == morseTable[i, 1])
                    {
                        translatedMessage += morseTable[i, 0];
                    }
                }    
            }       
            return translatedMessage;
        }

        /* Check if a given morse character exists */
        public bool IsValidChar(string character)
        {
            bool isValid = false;

            for (int i = 0; i < morseTable.GetLength(0); i++)
            {
                if (character == morseTable[i, 1])
                {
                    isValid = true;
                    break;
                }
            }
            return isValid;
        }
    }
}
