namespace i4p_informatikai_kft_feladat_megoldas
{
    internal class Program
    {
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

        static void Main(string[] args)
        {
            /* NumToChar Teszt
            Console.Write(NumToChar(0)); //Output : a
            Console.Write(NumToChar(26));//Output : ''
            Console.Write($"{NumToChar(25)}\n"); //Output : z
            */
            
            string message = "helloworld";
            string key = "abcdefgijklw";

            string cipher = Encrypt(message, key);
            string decoded = Decrypt(cipher, key);

            Console.WriteLine("Eredeti üzenet: " + message);
            Console.WriteLine("Kulcs: " + key);
            Console.WriteLine("Titkosított: " + cipher);
            Console.WriteLine("Visszafejtve: " + decoded);
        }
    }
}
