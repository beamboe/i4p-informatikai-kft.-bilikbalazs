namespace i4p_informatikai_kft_feladat_megoldas
{
    class CribResult
    {
        public string CribWord { get; set; }
        public string KeyFragment { get; set; }
        public string SecondWord { get; set; }

        public CribResult(string cribWord, string keyFragment, string secondWord)
        {
            CribWord = cribWord;
            KeyFragment = keyFragment;
            SecondWord = secondWord;
        }
    }
    internal class Program
    {
        static List<string> wordList = new List<string>();
        static int CharToNum(char c)
        {
            if (c == ' ')
            {
                return 26;
            }
            else
            {
                return c - 'a';
            }
        }
        static char NumToChar(int n)
        {
            if (n == 26)
            {
                return ' ';
            }
            else
            {
                return (char)(n + 'a');
            }
        }
        


        // Titkosítás
        static string Encrypt(string message, string key)
        {


            if (key.Length < message.Length)
                throw new ArgumentException("A kulcs túl rövid!");

            char[] result = new char[message.Length];
            for (int i = 0; i < message.Length; i++)//iteralas az uzeneten
            {
                int mVal = CharToNum(message[i]);
                int kVal = CharToNum(key[i]);
                int cVal = (mVal + kVal) % 27;
                result[i] = NumToChar(cVal);
            }
            return new string(result);
        }

        // Visszafejtés
        static string Decrypt(string cipher, string key)
        {
            if (key.Length < cipher.Length)
                throw new ArgumentException("A kulcs túl rövid!");

            char[] result = new char[cipher.Length];
            for (int i = 0; i < cipher.Length; i++)//iteralas a titkositott uzeneten
            {
                int cVal = CharToNum(cipher[i]);
                int kVal = CharToNum(key[i]);
                int mVal = (cVal - kVal + 27) % 27; 
                result[i] = NumToChar(mVal);
            }
            return new string(result);
        }
        static List<CribResult> FindInitialCribs(string cipher1, string cipher2)
        {
            var results = new List<CribResult>();

            foreach (var word in wordList)
            {
                if (word.Length > cipher1.Length || word.Length > cipher2.Length)
                    continue;

                string keyFragment = "";
                for (int i = 0; i < word.Length; i++)
                {
                    int cipherVal = CharToNum(cipher1[i]);
                    int messageVal = CharToNum(word[i]);
                    int keyVal = (cipherVal - messageVal + 27) % 27;
                    keyFragment += NumToChar(keyVal);
                }

                string secondWord = "";
                for (int i = 0; i < word.Length; i++)
                {
                    int cipherVal2 = CharToNum(cipher2[i]);
                    int keyVal = CharToNum(keyFragment[i]);
                    int messageVal2 = (cipherVal2 - keyVal + 27) % 27;
                    secondWord += NumToChar(messageVal2);
                }

                if (wordList.Contains(secondWord))
                {
                    results.Add(new CribResult(word, keyFragment, secondWord));
                }
            }

            return results;
        }
        static void Main(string[] args)
        {
            /* NumToChar Teszt
            Console.Write(NumToChar(0)); //Output : a
            Console.Write(NumToChar(26));//Output : ''
            Console.Write($"{NumToChar(25)}\n"); //Output : z
            
            string message = "helloworld";
            string key = "abcdefgijklw";

            string cipher = Encrypt(message, key);
            string decoded = Decrypt(cipher, key);

            Console.WriteLine("Eredeti üzenet: " + message);
            Console.WriteLine("Kulcs: " + key);
            Console.WriteLine("Titkosított: " + cipher);
            Console.WriteLine("Visszafejtve: " + decoded);*/
            string wordFilePath = "words.txt";

            try
            {
                using (StreamReader reader = new StreamReader(wordFilePath))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim().ToLower();
                        if (line.Length > 0)
                            wordList.Add(line);
                    }
                }
                Console.WriteLine($"Betöltött szavak száma: {wordList.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HIBA a szólista betöltésekor: {ex.Message}");
                return;
            }

            Console.WriteLine("Add meg az első titkosított üzenetet:");
            string cipher1 = Console.ReadLine()!.Trim().ToLower();

            Console.WriteLine("Add meg a második titkosított üzenetet:");
            string cipher2 = Console.ReadLine()!.Trim().ToLower();

            var cribs = FindInitialCribs(cipher1, cipher2);

            Console.WriteLine("\nLehetséges kezdőszavak, kulcsrészletek és megfejtések:");
            if (cribs.Count == 0)
            {
                Console.WriteLine("Nem találtunk értelmes szópárt a szólistával.");
            }
            else
            {
                foreach (var res in cribs)
                    Console.WriteLine($"1. {res.CribWord} (kulcsrészlet: {res.KeyFragment})  --> 2. {res.SecondWord}");
            }
        }
    }
}
