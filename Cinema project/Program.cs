using System;
using System.IO;
using System.Linq;

namespace Cinema_project
{
    internal class Program
    {
        public static string username, fName, lName, eMail, password, movieTitle;
        public static double money, price;
        public static bool staff;
        public static int roomID, seatTotal;
        public static string[] userMovies, bookedSeats, roomSeats1, roomSeats2, roomSeats3;
        public static string[][] allUserSeats = new string[3][];
        public static string[] path = { "Account", "Room", "Movie" };
        public static string[] accountList = GetFiles(0);
        public static string[] roomList = GetFiles(1);
        public static string[] movieList = GetFiles(2);
        static void Main(string[] args)
        {
            bool verified = false;
            int choice = Menu();
            if (choice == 1)
            {
                verified = CreateAccount();
            }
            else
            {
                verified = SignIn();
            }
            while (verified == true)
            {
                int choice2 = AccountInfo();
                if (staff == false)
                {
                    if (choice2 == 1)
                    {
                        AccountBalance();
                    }
                    else if (choice2 == 2)
                    {
                        DepositMoney();
                    }
                    else if (choice2 == 3)
                    {
                        WithdrawMoney();
                    }
                    else if (choice2 == 4)
                    {
                        MovieOptions();
                    }
                    else if (choice2 == 5)
                    {
                        SignOut();
                        verified = false;
                    }
                }
                if (staff == true)
                {
                    if (choice2 == 1)
                    {
                        StaffMovieOptions();
                    }
                    else if (choice2 == 2)
                    {
                        SignOut();
                    }
                }
            }
        }
        static string GetStringInput(string inputMessage)
        {
            string userInput = "";
            while (true)
            {
                Console.WriteLine($"{inputMessage}\n");
                string rawInput = Console.ReadLine();
                try
                {
                    userInput = rawInput;
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("invalid input");
                }
            }
            return userInput;
        }
        static double GetDoubleInput(string inputMessage)
        {
            double userInput = 0.0;
            while (true)
            {
                Console.WriteLine($"{inputMessage}\n");
                string rawInput = Console.ReadLine();
                try
                {
                    double userInput1 = double.Parse(rawInput);
                    userInput = Math.Round(userInput1, 2);
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("invalid input");
                }
            }
            return userInput;
        }
        static int GetIntInput(string inputMessage)
        {
            int userInput = 0;
            while (true)
            {
                Console.WriteLine($"{inputMessage}\n");
                string rawInput = Console.ReadLine();
                try
                {
                    userInput = int.Parse(rawInput);
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("invalid input");
                }
            }
            return userInput;
        }
        static int GetChoice(string inputMessage, string[] choices)
        {
            int userInput = 0;
            while (true)
            {
                int x = 1;
                foreach (string choice in choices)
                {
                    Console.WriteLine($"\n{x} --> {choice}\n");
                    x++;
                }
                Console.WriteLine($"{inputMessage}\n");
                string rawInput = Console.ReadLine();
                try
                {
                    userInput = int.Parse(rawInput);
                    if (userInput < 1 || userInput > choices.Length)
                    {
                        throw new FormatException();
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("invalid input");
                }
            }
            return userInput;
        }
        static string[] GetFiles(int folderIndex)
        {
            string[] files = Directory.GetFiles(path[folderIndex]);
            string[] filesList = new string[files.Length];
            int x = 0;
            foreach (string file in files)
            {
                string newFile = file.Replace(".txt", null).Replace($"{path[folderIndex]}\\", null).Trim();
                filesList[x] = newFile;
                x++;
            }
            return filesList;
        }
        static string FormatList(string[] list)
        {
            string strList = null;
            int x = 0;
            if (list != null)
            {
                foreach (string item in list)
                {
                    if (item == "")
                    {
                        x++;
                    }
                }
                string[] newList = new string[list.Length - x];
                int i = 0;
                for (int y = 0; y < list.Length; y++)
                {
                    if (i >= newList.Length)
                    {
                        break;
                    }
                    if (list[y] != "" && list[y] != null)
                    {
                        newList[i] = list[y];
                        i++;
                    }
                }
                strList = string.Join(", ", newList);
            }
            else
            {
                strList = "";
            }
            return strList;
        }
        static string[] OpenFile(int folderIndex, string fileName)
        {
            string[] linesList = File.ReadAllLines($"{path[folderIndex]}\\{fileName}.txt");
            string[] lines = new string[linesList.Length];
            for (int x = 0; x < linesList.Length; x++)
            {
                string newLine = linesList[x].Trim();
                lines[x] = newLine;
            }
            if (folderIndex == 0)
            {
                fName = lines[0];
                lName = lines[1];
                eMail = lines[2];
                password = lines[3];
                money = double.Parse(lines[4]);
                staff = bool.Parse(lines[5]);
                userMovies = lines[6].Split(", ");
                roomSeats1 = lines[7].Split(", ");
                roomSeats2 = lines[8].Split(", ");
                roomSeats3 = lines[9].Split(", ");
                allUserSeats[0] = roomSeats1;
                allUserSeats[1] = roomSeats2;
                allUserSeats[2] = roomSeats3;
            }
            if (folderIndex == 1)
            {
                movieTitle = lines[0];
                bookedSeats = lines[1].Split(", ");
                seatTotal = int.Parse(lines[2]);
            }
            else if (folderIndex == 2)
            {
                movieTitle = lines[0];
                price = double.Parse(lines[1]);
                roomID = int.Parse(lines[2]);
            }
            return lines;
        }
        static void SaveChanges(dynamic[] credentials, int folderindex, string fileName)
        {
            FileStream newFile = File.Create($"{path[folderindex]}\\{fileName}.txt");
            StreamWriter writeFile = new(newFile);
            foreach (dynamic cred in credentials)
            {
                writeFile.WriteLine(cred);
            }
            writeFile.Dispose();
            writeFile.Close();
        }
        static int Menu()
        {
            string[] options = { "Create Account", "Sign-In" };
            int choice = GetChoice("What would you like to do?", options);
            return choice;
        }
        static bool CreateAccount()
        {
            bool verified = false;
            bool x = true;
            while (x == true)
            {
                string usernameTemp = GetStringInput("Please enter a username");
                if (accountList.Length == 0)
                {
                    username = usernameTemp;
                    x = false;
                }
                else
                {
                    foreach (string account in accountList)
                    {
                        if (account != usernameTemp)
                        {
                            username = usernameTemp;
                            x = false;
                        }
                        else
                        {
                            Console.WriteLine("This username already exists\n");
                            x = true;
                            break;
                        }
                    }
                }
            }
            fName = GetStringInput("What is your first name?");
            lName = GetStringInput("What is your last name?");
            while (true)
            {
                eMail = GetStringInput("Please enter your E-mail");
                if (eMail.Contains("@") == true && eMail.Contains(".") == true)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("E-Mail does not have a valid domain\n");
                }
            }
            while (true)
            {
                int digits = 0;
                int special = 0;
                int capital = 0;
                Console.WriteLine("Password must contain:\n  - 12 Characters\n  - At least 2 special charcaters (e.g. @, %, *, <)\n  - At least 2 numbers\n  - A capital Letter");
                password = GetStringInput("Please enter new password");
                foreach (char c in password)
                {
                    if (Char.IsDigit(c) == true)
                    {
                        digits++;
                    }
                    if (Char.IsLetterOrDigit(c) == false)
                    {
                        special++;
                    }
                    if (Char.IsUpper(c) == true)
                    {
                        capital++;
                    }
                }
                if (special >= 2 && digits >= 2 && password.Length >= 12 && capital >= 1)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Password is invalid, Please try again\n");
                }
            }
            money = 0.00;
            staff = false;
            userMovies = Array.Empty<string>();
            allUserSeats[0] = Array.Empty<string>();
            allUserSeats[1] = Array.Empty<string>();
            allUserSeats[2] = Array.Empty<string>();
            string newuserMovies = "";
            string userSeats1 = "";
            string userSeats2 = "";
            string userSeats3 = "";
            dynamic[] credentials = { fName, lName, eMail, password, money, staff, newuserMovies, userSeats1, userSeats2, userSeats3 };
            SaveChanges(credentials, 0, username);
            verified = true;
            return verified;
        }
        static bool SignIn()
        {
            bool verified;
            bool x = true;
            while (x == true)
            {
                string tempUsername = GetStringInput("Please enter username:");
                foreach (string file in accountList)
                {
                    if (tempUsername == file)
                    {
                        x = false;
                        username = tempUsername;
                        OpenFile(0, username);
                        break;
                    }
                }
                if (x == true)
                {
                    Console.WriteLine("The account could not be found, please try again.\n");
                }
            }
            while (true)
            {
                string passwordTemp = GetStringInput("Please enter your password");
                if (passwordTemp == password)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Your password wrong, please try again\n");
                }
            }
            verified = true;
            return verified;
        }
        static bool SignOut()
        {
            bool signOut = false;
            string[] options = { "Yes", "No" };
            int choice = GetChoice("Are you sure you want to exit?", options);
            if (choice == 1)
            {
                string newuserMovies = FormatList(userMovies);
                string userSeats1 = FormatList(allUserSeats[0]);
                string userSeats2 = FormatList(allUserSeats[1]);
                string userSeats3 = FormatList(allUserSeats[2]);
                signOut = true;
                dynamic[] credentials = { fName, lName, eMail, password, money, staff, newuserMovies, userSeats1, userSeats2, userSeats3 };
                SaveChanges(credentials, 0, username);
            }
            return signOut;
        }
        static int AccountInfo()
        {
            string[] options;
            Console.WriteLine($"Welcome, {fName}");
            if (staff == false)
            {
                options = new string[5] { "View Balance", "Deposit Money", "Withdraw Money", "Movies", "Sign-Out" };
            }
            else
            {
                options = new string[2] { "Movies", "Sign-Out" };
            }
            int choice = GetChoice("What would you like to do?", options);
            return choice;
        }
        static void AccountBalance()
        {
            bool x = true;
            while (x == true)
            {
                Console.WriteLine($"Your account balance is {money}");
                x = Finish();
            }
        }
        static void DepositMoney()
        {
            bool x = true;
            while (x == true)
            {
                double moneyAdd = GetDoubleInput("How much money would you like to add?");
                money += moneyAdd;
                x = Finish();
            }
        }
        static void WithdrawMoney()
        {
            bool x = true;
            while (x == true)
            {
                double moneyRemove = GetDoubleInput("How much money would you like to remove?");
                while (true)
                {

                    if (moneyRemove > money)
                    {
                        Console.WriteLine("You cannot withdraw that amount");
                    }
                    else
                    {
                        money -= moneyRemove;
                        break;
                    }
                }
                x = Finish();
            }
        }
        static void MovieOptions()
        {
            bool exit = false;
            while (exit == false)
            {
                string[] options = { "View Bookings", "Make New Booking", "Remove Booking", "Back to Home" };
                int choice = GetChoice("What would you like to do?", options);
                if (choice == 1)
                {
                    ViewBookings();
                }
                else if (choice == 2)
                {
                    AddBooking();
                }
                else if (choice == 3)
                {
                    RemoveBooking();
                }
                else if (choice == 4)
                {
                    exit = true;
                }
            }
        }
        static void GetRoomID(int choice)
        {
            movieTitle = movieList[choice - 1];
            OpenFile(2, movieTitle);
            OpenFile(1, roomID.ToString());
        }
        static void ViewBookings()
        {
            int choice = GetChoice("What booking would you like to view?", movieList);
            GetRoomID(choice);
            RoomAscii();
        }
        static void AddBooking()
        {
            bool x = true;
            while (x == true)
            {
                bool bookSeat = true;
                bool movieExist = false;
                int choice = GetChoice("What booking would you like to add?", movieList);
                GetRoomID(choice);
                foreach (string film in userMovies)
                {
                    if (movieTitle == film)
                    {
                        movieExist = true;
                        Console.WriteLine($"You have already booked this movie, {movieTitle}, before...");
                        string[] options = { "Yes", "No" };
                        int bookAgain = GetChoice("Would you like to book more seats?", options);
                        if (bookAgain == 2)
                        {
                            bookSeat = false;
                        }
                        break;
                    }
                    else
                    {
                        movieExist = false;
                    }
                }
                if (movieExist == false)
                {
                    string[] newUserMovies = new string[userMovies.Length + 1];
                    for (int i = 0; i < newUserMovies.Length; i++)
                    {
                        if (i < userMovies.Length)
                        {
                            newUserMovies[i] = userMovies[i];
                        }
                        else newUserMovies[i] = movieTitle;
                    }
                    userMovies = newUserMovies;
                }
                if (bookSeat == true)
                {
                    int seatAmount = 0;
                    OpenFile(1, roomID.ToString());
                    while (true)
                    {
                        RoomAscii();
                        seatAmount = GetIntInput("Input the number of seats you would like to add");
                        if (seatAmount > 100 - seatTotal)
                        {
                            Console.WriteLine("That is too many seats\n");
                        }
                        else
                        {
                            double totalCost = seatAmount * price;
                            if (totalCost > money)
                            {
                                Console.WriteLine("You do not have enough money for this\n");
                            }
                            else
                            {
                                money -= totalCost;
                                break;
                            }
                        }
                    }
                    string[] seatsqueue = MakeSeatQueue(seatAmount);
                    AddSeats(seatsqueue);
                }
                x = Finish();
            }
        }
        static void RemoveBooking()
        {
            bool x = true;
            while (x == true)
            {
                bool removeSeat = true;
                int choice = GetChoice("What booking would you like to remove?", movieList);
                GetRoomID(choice);
                foreach (string film in userMovies)
                {
                    if (movieTitle == film)
                    {
                        string[] options = { "Yes", "No" };
                        int bookAgain = GetChoice($"Are you sure you wish to remove bookings for {movieTitle}?", options);
                        if (bookAgain == 2)
                        {
                            removeSeat = false;
                        }
                        break;
                    }
                }
                if (removeSeat == true)
                {
                    string[] options = { "Whole Movie", "Specific Seats" };
                    int removeScale = GetChoice("What would you like to remove?", options);
                    if (removeScale == 1)
                    {
                        RemoveSeats(allUserSeats[roomID - 1]);
                        string[] newUserMovies = new string[userMovies.Length - 1];
                        int j = 0;
                        for (int i = 0; i < userMovies.Length; i++)
                        {
                            if (userMovies[i] != movieTitle)
                            {
                                newUserMovies[j] = userMovies[i];
                                j++;
                            }
                        }
                        userMovies = newUserMovies;
                    }
                    else
                    {
                        int seatAmount = 0;
                        OpenFile(1, roomID.ToString());
                        while (true)
                        {
                            RoomAscii();
                            seatAmount = GetIntInput("Input the number of seats you would like to remove");
                            if (seatAmount > allUserSeats[roomID - 1].Length)
                            {
                                Console.WriteLine("That is too many seats");
                            }
                            else
                            {
                                seatTotal -= seatAmount;
                                double totalCost = seatAmount * price;
                                money += totalCost;
                                break;
                            }
                        }
                        string[] seatsQueue = MakeSeatQueue(seatAmount);
                        RemoveSeats(seatsQueue);
                    }
                }
                if (allUserSeats[choice - 1] == null)
                {
                    string[] newUserMovies = new string[userMovies.Length - 1];
                    for (int i = 0; i < userMovies.Length; i++)
                    {
                        if (userMovies[i] == movieTitle)
                        {
                            userMovies[i] = null;
                        }
                    }
                    for (int j = 0; j < newUserMovies.Length; j++)
                    {
                        if (String.IsNullOrEmpty(userMovies[j]) == false)
                        {
                            newUserMovies[j] = userMovies[j];
                        }
                    }
                    userMovies = newUserMovies;
                }
                x = Finish();
            }
        }
        static void StaffMovieOptions()
        {
            bool exit = false;
            while (exit == false)
            {
                string[] options = { "Add Movie", "Remove Movie", "Back to Home" };
                int choice = GetChoice("What would you like to do?", options);
                if (choice == 1)
                {
                    AddMovie();
                }
                else if (choice == 2)
                {
                    RemoveMovie();
                }
                else if (choice == 3)
                {
                    exit = true;
                }
            }
        }
        static void AddMovie()
        {
            bool x = true;
            while (x == true)
            {
                bool exist = true;
                bool roomTaken = true;
                string tempMovieTitle = "";
                while (exist == true)
                {
                    tempMovieTitle = GetStringInput("What is the name of the movie?");
                    foreach (string movie in movieList)
                    {
                        if (movie != tempMovieTitle)
                        {
                            exist = false;
                        }
                        else
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (movieList.Length == 0)
                    {
                        exist = false;
                    }
                    if (exist == true)
                    {
                        Console.WriteLine($"This movie {tempMovieTitle} already exists\n");
                    }
                }
                double tempPrice = GetDoubleInput("How much will a seat cost?");
                int roomTemp = 0;
                while (roomTaken == true)
                {
                    while (true)
                    {
                        roomTemp = GetIntInput("What room would you like?");
                        if (roomTemp > roomList.Length || roomTemp <= 0)
                        {
                            Console.WriteLine("this room does not exist\n");
                        }
                        else
                        {
                            break;
                        }
                    }
                    foreach (string movie in movieList)
                    {
                        OpenFile(2, movie);
                        if (roomTemp == roomID)
                        {
                            Console.WriteLine($"The room {roomTemp} is already taken\n");
                            roomTaken = true;
                            break;
                        }
                        else
                        {
                            roomTaken = false;
                        }
                    }
                    if (movieList.Length == 0)
                    {
                        roomTaken = false;
                    }
                }
                roomID = roomTemp;
                movieTitle = tempMovieTitle;
                price = tempPrice;
                string[] tempBookedSeats = Array.Empty<string>();
                int tempSeatTotal = 0;
                dynamic[] roomInfo = { movieTitle, FormatList(tempBookedSeats), tempSeatTotal };
                dynamic[] movieInfo = { movieTitle, price, roomID };
                SaveChanges(movieInfo, 2, movieTitle);
                SaveChanges(roomInfo, 1, roomID.ToString());
                x = Finish();
            }
        }
        static void RemoveMovie()
        {
            bool x = true;
            while (x == true)
            {
                int choice = GetChoice("What movie would you like to remove?", movieList);
                roomID = choice;
                for (int y = 0; y < movieList.Length; y++)
                {
                    if (choice - 1 == y)
                    {
                        File.Delete($"{path[2]}\\{movieList[y]}.txt");
                        OpenFile(1, roomID.ToString());
                        movieTitle = String.Empty;
                        foreach (string account in accountList)
                        {
                            OpenFile(0, account);
                            bookedSeats = Array.Empty<string>();
                            allUserSeats[roomID - 1] = Array.Empty<string>();
                            dynamic[] accountCreds = { fName, lName, eMail, password, money, staff, userMovies, roomSeats1, roomSeats2, roomSeats3 };
                            SaveChanges(accountCreds, 0, account);
                        }
                        seatTotal = 0;
                        Console.WriteLine($"This movie {movieTitle} has been removed\n");
                        break;
                    }
                }
                dynamic[] roomInfo = { movieTitle, FormatList(bookedSeats), seatTotal };
                SaveChanges(roomInfo, 1, roomID.ToString());
                x = Finish();
            }
        }
        static string[] MakeLine(string[] list, int start, int end)
        {
            string[] line = new string[10];
            int y = 0;
            for (int x = start; x < end; x++)
            {
                line[y] = list[x];
                y++;
            }
            return line;
        }
        static void RoomAscii()
        {
            string[] seats = new string[100];
            string[] asciiSeats = new string[100];
            int i = 0;
            for (char x = 'A'; x < 'K'; x++)
            {
                for (int y = 1; y <= 10; y++)
                {
                    string coord = $"{x.ToString() + y.ToString()}";
                    seats[i] = coord;
                    i++;
                }
            }
            for (int y = 0; y < seats.Length; y++)
            {
                asciiSeats[y] = $"[ {seats[y]} ]";
            }
            string[][] lines = new string[10][];
            for (int x = 0; x < seats.Length; x++)
            {
                if (bookedSeats.Contains(seats[x]) == true && allUserSeats[roomID - 1].Contains(seats[x]) == false)
                {
                    asciiSeats[x] = $"[={seats[x]}=]";
                }
                else if (bookedSeats.Contains(seats[x]) == true && allUserSeats[roomID - 1].Contains(seats[x]) == true)
                {
                    asciiSeats[x] = $"[#{seats[x]}#]";
                }
                for (int value = 0; value < 100; value += 10)
                {
                    lines[value / 10] = MakeLine(asciiSeats, value, value+10);
                }
            }
            Console.WriteLine("Your bookings - '#'");
            Console.WriteLine("Other bookings - '='");
            Console.WriteLine($"------------------------{movieTitle}-------------------------");
            foreach (string[] line in lines)
            {
                string newLine = String.Join(", ", line);
                Console.WriteLine(newLine);
            }
        }
        static string[] MakeSeatQueue(int seatAmount)
        {
            string[] seatQueue = new string[seatAmount];
            for (int i = 0; i < seatAmount; i++)
            {
                while (true)
                {
                    string rawSeatCoord = GetStringInput("Input the coordinate of the seat");
                    string seatCoord = rawSeatCoord.Trim().ToUpper().Replace(" ", "").Replace(",", "");
                    if (bookedSeats.Contains(seatCoord) == true)
                    {
                        Console.WriteLine("This seat has already been booked\n");
                    }
                    else if (seatQueue != null && allUserSeats[roomID - 1] != null && allUserSeats[roomID - 1].Contains(seatCoord) == true || seatQueue.Contains(seatCoord) == true)
                    {
                        Console.WriteLine("You have already booked this seat\n");
                    }
                    else if (seatQueue == null || allUserSeats[roomID - 1] == null || seatQueue.Contains(seatCoord) == false && allUserSeats[roomID - 1].Contains(seatCoord) == false && bookedSeats.Contains(seatCoord) == false)
                    {
                        seatQueue[i] = seatCoord;
                        break;
                    }
                }
            }
            return seatQueue;
        }
        static void AddSeats(string[] seatQueue)
        {
            int userSeatAmount;
            if (allUserSeats[roomID - 1] == null)
            {
                userSeatAmount = 0;
            }
            else
            {
                userSeatAmount = allUserSeats[roomID - 1].Length;
            }
            string[] newUserSeats = new string[seatQueue.Length + userSeatAmount];
            string[] newBookedSeats = new string[seatQueue.Length + bookedSeats.Length];
            for (int x = 0; x < newUserSeats.Length; x++)
            {
                if (x < userSeatAmount)
                {
                    newUserSeats[x] = allUserSeats[roomID - 1][x];
                }
                else if (x >= userSeatAmount)
                {
                    newUserSeats[x] = seatQueue[x - userSeatAmount];
                }
            }
            allUserSeats[roomID - 1] = newUserSeats;
            Console.WriteLine(bookedSeats.Length);
            for (int y = 0; y < newBookedSeats.Length; y++)
            {
                if (y < bookedSeats.Length)
                {
                    newBookedSeats[y] = bookedSeats[y];
                }
                else if (y >= bookedSeats.Length)
                {
                    newBookedSeats[y] = seatQueue[y - bookedSeats.Length];
                }

            }
            bookedSeats = newBookedSeats;
            dynamic[] roomCreds = { movieTitle, FormatList(bookedSeats), roomID };
            SaveChanges(roomCreds, 1, roomID.ToString());
        }
        static void RemoveSeats(string[] seatQueue)
        {
            int userSeatAmount;
            if (allUserSeats[roomID - 1] == null)
            {
                userSeatAmount = 0;
            }
            else
            {
                userSeatAmount = allUserSeats[roomID - 1].Length;
            }
            string[] newUserSeats = new string[userSeatAmount - seatQueue.Length];
            string[] newBookedSeats = new string[bookedSeats.Length - seatQueue.Length];
            for (int x = 0; x < newUserSeats.Length; x++)
            {
                if (allUserSeats[roomID - 1].Contains(seatQueue[x]) == false)
                {
                    newUserSeats[x] = allUserSeats[roomID - 1][x];
                }
            }
            allUserSeats[roomID - 1] = newUserSeats;
            for (int y = 0; y < newBookedSeats.Length; y++)
            {
                bool bookedContain = bookedSeats.Contains(seatQueue[y]);
                if (bookedSeats.Contains(seatQueue[y]) == false)
                {
                    newBookedSeats[y] = bookedSeats[y];
                }
            }
            bookedSeats = newBookedSeats;
            dynamic[] roomCreds = { movieTitle, FormatList(bookedSeats), seatTotal };
            SaveChanges(roomCreds, 1, roomID.ToString());
        }
        static bool Finish()
        {
            bool x = true;
            string[] options = { "Yes", "No" };
            int choice = GetChoice("Are you finished in this section?", options);
            if (choice == 1)
            {
                x = false;
            }
            return x;
        }
    }
}