//IFIN-5/3_Čeikauskas_Benjaminas_U3_23
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFIN53_Čeikauskas_Benjaminas_U3_23
{
    /// <summary>
    /// Klasė žaidėjo duomenims saugoti
    /// </summary>
    public class Player
    {
        private string team;    //Komandos pavadinimas
        private string surname; //Pavardė
        private string name;    //Vardas
        private int height;     //Ūgis centimetrais
        private int year;       //Gimimo metai
        private string role;    //Žaidimo pozicija
        private int games;      //Žaista rungtynių
        private int score;      //Įmesta taškų
        private double avarageScore; //Taškų vidurkis
        /// <summary>
        /// Tuščias konstruktorius
        /// </summary>
        public Player()
        {

        }
        /// <summary>
        /// Konstruktorius su parametrais
        /// </summary>
        /// <param name="team"></param>
        /// <param name="surname"></param>
        /// <param name="name"></param>
        /// <param name="height"></param>
        /// <param name="year"></param>
        /// <param name="role"></param>
        /// <param name="games"></param>
        /// <param name="score"></param>
        public Player(string team, string surname, string name, int height, int year,
                      string role, int games, int score)
        {
            this.team = team;
            this.surname = surname;
            this.name = name;
            this.height = height;
            this.year = year;
            this.role = role;
            this.games = games;
            this.score = score;
            avarageScore = 0;
            if (games > 0) avarageScore = (double)score / games;

        }
        /// <summary>
        /// Sąsajos metodai
        /// </summary>
        public string GetTeam() {return team;}
        public string GetSurname() {return surname;}
        public string GetName() {return name;}
        public int GetHeight() { return  height;}
        public int GetYear() { return year;}
        public string GetRole() {return role;}
        public int GetGames() {return games;}
        public int GetScore() {return score;}
        public double GetAverageScore()
        {
            if (games == 0)
                return 0.0;
            return (double)score / games;
        }
        //Užklotas ToString operatorius
        public override string ToString()
        {
            string line;
            line = string.Format("{0, -18}| {1, -14}| {2, -10}| {3, -5}| {4, -11}| {5, -9}" +
                                 "| {6, -9}| {7, -13} | {8, -8:f2} |", 
                                 team, surname, name, height, year,
                                 role, games, score, avarageScore);
            return line;
        }
        //Užklotas Equals operatorius
        public override bool Equals(object obj)
        {
            return obj is Player player &&
                   team == player.team &&
                   surname == player.surname &&
                   name == player.name &&
                   height == player.height &&
                   year == player.year &&
                   role == player.role &&
                   games == player.games &&
                   score == player.score;
        }
        //Užklotas GetHashCode operatorius
        public override int GetHashCode()
        {
            int hashCode = 133963872;
            hashCode = hashCode * -1521134295 +
                EqualityComparer<string>.Default.GetHashCode(team);
            hashCode = hashCode * -1521134295 +
                EqualityComparer<string>.Default.GetHashCode(surname);
            hashCode = hashCode * -1521134295 +
                EqualityComparer<string>.Default.GetHashCode(name);
            hashCode = hashCode * -1521134295 + height.GetHashCode();
            hashCode = hashCode * -1521134295 + year.GetHashCode();
            hashCode = hashCode * -1521134295 +
                EqualityComparer<string>.Default.GetHashCode(role);
            hashCode = hashCode * -1521134295 + games.GetHashCode();
            hashCode = hashCode * -1521134295 + score.GetHashCode();
            return hashCode;
        }
        //Užklotas >= operatorius
        public static bool operator >=(Player player1, Player Player2)
        {
            int score = 0;
            if (player1.avarageScore < Player2.avarageScore)
                score = -1;
            else if (player1.avarageScore > Player2.avarageScore)
                score = 1;
            else
                score = 0;

            int poz = String.Compare(player1.surname, Player2.surname,
                                         StringComparison.CurrentCulture);
            return (score > 0 || (score == 0 && poz < 0));
        }
        //Užklotas <= operatorius

        public static bool operator <=(Player player1, Player Player2)
        {
            int score = 0;
            if (player1.avarageScore > Player2.avarageScore)
                score = -1;
            else if (player1.avarageScore < Player2.avarageScore)
                score = 1;
            else
                score = 0;

            int poz = String.Compare(player1.surname, Player2.surname,
                                         StringComparison.CurrentCulture);
            return (score < 0 || (score == 0 && poz > 0));
        }
    }
    /// <summary>
    /// Žaidėjų konteinerinė klasė
    /// </summary>
    public class PlayerArray
    {
        const int cMax = 100; //Žaidėjų masyvo dydis
        private Player[] Players;  //Žaidėjų objektų masyvas
        private int count;    //Žaidėjų skaičius
        public PlayerArray()
        {
            count = 0;
            Players = new Player[cMax];
        }
        //Sąsajos metodai
        public Player GetPlayer(int i) { return Players[i]; }
        public int GetCount() { return count; }
        /// <summary>
        /// Žaidėjo pridėjimo metodas
        /// </summary>
        /// <param name="obj"></param>
        public void AddPlayer(Player obj) { Players[count++] = obj; }
        /// <summary>
        /// Rikiavimo metodas
        /// </summary>
        public void Sort()
        {
            int maxInd;
            for (int i = 0; i < count - 1; i++)
            {
                maxInd = i;
                for (int j = i + 1; j < count; j++)
                {
                    if (Players[j] >= Players[maxInd])
                        maxInd = j;
                }
                Player temp = Players[i];
                Players[i] = Players[maxInd];
                Players[maxInd] = temp;
            }
        }
        /// <summary>
        /// Metodas išimantis žaidėjus su ne daugiau nei vienomis rungtynėmis
        /// </summary>
        public void RemovePlayers()
        {
            int m = 0;
            for (int i = 0; i < count; i++)
            {
                if (Players[i].GetGames() > 1)
                    Players[m++] = Players[i];
            }
            count = m;
        }
    }
//-------------------------------------------------------------------------------------------
    internal class Program
    {
        const int Cn = 100;
        const string CFd = "Data1.txt";
        const string CFr = "Results.txt";
        static void Main(string[] args)
        {
            if (File.Exists(CFr))
                File.Delete(CFr);

            using (var fr = File.AppendText(CFr))
            {



                string errorLine =
                    "\t\t\t\t\t" +
                    "*******************************************************************\n" +
                    "\t\t\t\t\t" +
                    "*                         TOKIŲ ŽAIDĖJŲ NĖRA                      *\n" +
                    "\t\t\t\t\t" +
                    "*******************************************************************\n";

                PlayerArray Players = new PlayerArray();
                string PlayersHeader =
                    "\t\t\t\t\t" +
                    "*******************************************************************\n" +
                    "\t\t\t\t\t" +
                    "*                              ŽAIDĖJAI                           *\n" +
                    "\t\t\t\t\t" +
                    "*******************************************************************";

                Read(CFd, Players);
                if (Players.GetCount() > 0)
                    Print(fr, Players, PlayersHeader);
                else
                {
                    fr.WriteLine(errorLine);
                }

                PlayerArray PlayersP = new PlayerArray();
                string PlayersPHeader =
                    "\t\t\t\t\t" +
                    "*******************************************************************\n" +
                    "\t\t\t\t\t" +
                    "*                          GERIAUSI PUOLĖJAI                      *\n" +
                    "\t\t\t\t\t" +
                    "*******************************************************************";

                Filter(Players, "Puolėjas", out PlayersP);
                PlayersP.Sort();
                PlayersP.RemovePlayers();
                if (PlayersP.GetCount() > 0)
                    Print(fr, PlayersP, PlayersPHeader);
                else
                {
                    fr.WriteLine(errorLine);
                }

                PlayerArray PlayersC = new PlayerArray();
                string PlayersCHeader =
                    "\t\t\t\t\t" +
                    "*******************************************************************\n" +
                    "\t\t\t\t\t" +
                    "*                          GERIAUSI CENTRAI                       *\n" +
                    "\t\t\t\t\t" +
                    "*******************************************************************";

                Filter(Players, "Centras", out PlayersC);
                PlayersC.Sort();
                PlayersC.RemovePlayers();
                if (PlayersC.GetCount() > 0)
                    Print(fr, PlayersC, PlayersCHeader);
                else
                {
                    fr.WriteLine(errorLine);
                }

                PlayerArray PlayersG = new PlayerArray();
                string PlayersGHeader =
                    "\t\t\t\t\t" +
                    "*******************************************************************\n" +
                    "\t\t\t\t\t" +
                    "*                          GERIAUSI GYNĖJAI                       *\n" +
                    "\t\t\t\t\t" +
                    "*******************************************************************";

                Filter(Players, "Gynėjas", out PlayersG);
                PlayersG.Sort();
                PlayersG.RemovePlayers();
                if (PlayersG.GetCount() > 0)
                    Print(fr, PlayersG, PlayersGHeader);
                else
                {
                    fr.WriteLine(errorLine);
                }
            }

            Console.WriteLine("PROGRAMA BAIGĖ DARBĄ");
        }
//-------------------------------------------------------------------------------------------
/// <summary>
/// Metodas nuskaitanti duomenis iš duomenų failo ir priskirianti žaidėjų konteineriui
/// </summary>
/// <param name="fn">failas iš kurio skaitomi duomenys</param>
/// <param name="Players">žaidėjų konteineris</param>
        static void Read(string fn, PlayerArray Players)
        {
            using (StreamReader reader = new StreamReader(fn))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(';');
                    string team = parts[0];
                    string surname = parts[1];
                    string name = parts[2];
                    int height = int.Parse(parts[3]);
                    int year = int.Parse((parts[4]));
                    string role = parts[5];
                    int games = int.Parse(parts[6]);
                    int score = int.Parse(parts[7]);                       

                    Player player = new Player(team, surname, name, height, year,
                                                   role, games, score);
                    Players.AddPlayer(player);
                }
            }
        }
        /// <summary>
        /// Metodas spausdinantis duomenis
        /// </summary>
        /// <param name="fn">Failas į kurį spausdinama</param>
        /// <param name="Players">Žaidėjų konteineris</param>
        /// <param name="header">antraštė</param>
        static void Print(StreamWriter fr, PlayerArray Players, string header)
        {
            const string upperSection =
                "-----------------------------------------------------------------------" +
                "---------------------------------------------------\n" +
                "Nr.\t|\t\tKomanda\t\t|\tPavardė\t\t|\tVardas\t| Ūgis | Gim. Metai | " +
                "Pozicija | Ž. rung. | Pelnyta taškų | Vidurkis |\n" +
                "-----------------------------------------------------------------------" +
                "---------------------------------------------------";

                fr.WriteLine(header);
                fr.WriteLine(upperSection);
                for (int i = 0; i < Players.GetCount(); i++) 
                {
                    fr.WriteLine("{0,-3}\t| {1, -20}",
                                 i + 1, Players.GetPlayer(i).ToString());
                }
                fr.WriteLine(
                "-----------------------------------------------------------------------" +
                "---------------------------------------------------"
                    );
                fr.WriteLine();
        }
        /// <summary>
        /// Metodas filtruojantis duomenis
        /// </summary>
        /// <param name="Players">Žaidėjų konteineris iš kurio filtruojame</param>
        /// <param name="position">Pozicija kurią atrenkame</param>
        /// <param name="Filtered">Gražiname filtruotą masyvą su norima pozicija</param>
        static void Filter(PlayerArray Players, string position, out PlayerArray Filtered)
        {
            Filtered = new PlayerArray();

            for (int i = 0; i < Players.GetCount() ; i++)
            {
                Player player = Players.GetPlayer(i);
                if (Players.GetPlayer(i).GetRole().Trim().Equals(position, 
                    StringComparison.OrdinalIgnoreCase))
                {
                    Filtered.AddPlayer(player);
                }
            }
        }
    }
}
