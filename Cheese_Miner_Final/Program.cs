using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheese_miner_REMASTERED
{

    public class PlayerInformation
    {

        public string name;
        public int cheese;
        public int xPosition;
        public int yPosition;
        public ConsoleColor colour;

    }

    class Program
    {

        //Stores position of cheeses.
        static bool[,] cheesePositions = new bool[8, 8];

        //List to store players.
        static List<PlayerInformation> players = new List<PlayerInformation>();

        //Enables special features.
        static bool adminMode = false;

        //Enables special features.
        static bool defaultTestSequence = false;

        //Stores the turn number to display.
        static int currentTurn = 0;

        //Method to take in y/n replies.
        public static bool yesOrNoInput(string messageUponYesReply, string messageUponNoReply)
        {

            char yesNoResponse;

            do
            {

                yesNoResponse = Console.ReadKey().KeyChar;

                //Checking for valid response.
                if (yesNoResponse == 'y')
                {

                    if (messageUponYesReply != "")
                    {

                        Console.WriteLine(messageUponYesReply);

                    }

                    return true;

                }

                else if (yesNoResponse == 'n')
                {

                    if (messageUponNoReply != "")
                    {

                        Console.WriteLine(messageUponNoReply);

                    }

                    return false;

                }

                else
                {

                    Console.WriteLine("Invalid reply, please use \"y\" or \"n\".");

                }

            }
            while (yesNoResponse != 'y' && yesNoResponse != 'n');

            return true;

        }

        //Method to take in values and return them as integers, takes a string that is to be displayed when receiving the input, a minimum acceptable value and a maximum acceptable value.
        public static int safeIntInput(int lowerLimit, int upperLimit)
        {

            //What the person enters.
            string parseString;

            //Int to return, initial state is an unacceptable value to ensure the player gives a new correct value.
            int returnInt = lowerLimit - 1;

            do
            {

                parseString = Console.ReadLine();

                //Checking the entered value is parseable.
                try
                {

                    returnInt = int.Parse(parseString);

                }
                catch (Exception)
                {

                }
                finally
                {

                    //Checking the entered value is within the range.
                    if (returnInt < lowerLimit || returnInt > upperLimit)
                    {

                        Console.WriteLine("Incorrect integer value, please make sure your value is between " + lowerLimit + " and " + upperLimit + ":");

                    }

                }

            }
            while (returnInt < lowerLimit || returnInt > upperLimit);

            return returnInt;

        }

        //Method to count the amount of players on a given tile.
        public static int playersOnATile(int xPosition, int yPosition)
        {

            int playerCount = 0;

            for (int i = 0; i < players.Count(); i++)
            {

                //If player being checked is on the tile being checked.
                if (players[i].xPosition == xPosition && players[i].yPosition == yPosition)
                {

                    playerCount++;

                }

            }

            return playerCount;

        }

        //Checks if there is a winner and returns their ID if there is, else returns -1.
        public static int winCheck()
        {

            //Checking through each player.
            for (int i = 0; i < players.Count(); i++)
            {

                //If given player could currently be the winner.
                if (players[i].cheese >= 6)
                {

                    return i;

                }

            }

            return -1;

        }

        //Returns the player to within board limits.
        public static int playerPositionCorrection(int position)
        {

            if (position < 0)
            {

                return position + 8;

            }

            else if (position > 7)
            {

                return position - 8;

            }

            else
            {

                return position;

            }

        }

        //Method to display the positions of cheese and players.
        public static void renderBoard()
        {

            Console.Clear();

            //Creates top axis numbers with either test scenario 0-7 numbering or more inuitive 1-8 numbering.
            if (defaultTestSequence)
            {

                Console.WriteLine("  0 1 2 3 4 5 6 7");

            }

            else
            {

                Console.WriteLine("  1 2 3 4 5 6 7 8");

            }

            //Current positio on y axis.
            for (int y = 0; y < 8; y++)
            {

                //Creates left axis numbers with either test scenario 0-7 numbering or more inuitive 1-8 numbering.
                if (defaultTestSequence)
                {

                    Console.Write((y) + " ");

                }

                else
                {

                    Console.Write((y + 1) + " ");

                }

                //Current position on x axis.
                for (int x = 0; x < 8; x++)
                {

                    //If no players on tile.
                    if (playersOnATile(xPosition: x, yPosition: y) == 0)
                    {

                        //If there's a cheese
                        if (cheesePositions[x, y])
                        {

                            //Render some cheese!
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write("x ");
                            Console.ResetColor();

                        }

                        //If empty space.
                        else
                        {

                            //Render (???) some space.
                            Console.Write("  ");

                        }

                    }

                    //If there's a single player (done to render their custom colours).
                    else if (playersOnATile(xPosition: x, yPosition: y) == 1)
                    {

                        //Check which player is on the tile by checking each player.
                        for (int i = 0; i < players.Count(); i++)
                        {

                            if (players[i].xPosition == x && players[i].yPosition == y)
                            {

                                //Draw a player.
                                Console.ForegroundColor = players[i].colour;
                                Console.Write("1 ");
                                Console.ResetColor();

                            }

                        }

                    }

                    //Other case, multiple players on one tile.
                    else
                    {

                        //Draw multiple players.
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.Write(playersOnATile(xPosition: x, yPosition: y) + " ");
                        Console.ResetColor();

                    }

                }

                Console.WriteLine();

            }

            Console.WriteLine("\nTurn: " + currentTurn);

            //Showing scoreboard.
            for (int i = 0; i < players.Count(); i++)
            {

                Console.ForegroundColor = players[i].colour;
                Console.Write("\n" + players[i].name + ": ");
                Console.ResetColor();
                Console.Write(players[i].cheese);

            }

            Console.WriteLine();

            //If testing, display the stuff rob wants in the form he wants!
            if (defaultTestSequence)
            {

                for (int i = 0; i < players.Count(); i++)
                {

                    Console.WriteLine(players[i].name + " is on " + (players[i].xPosition) + "," + (players[i].yPosition) + " with a score of " + players[i].cheese);

                }

            }

        }

        //Returns the character (w,a,s,d) that was entered.
        public static char takePlayerInput()
        {

            char playerDirection;

            //Loop until valid direction is given.
            do
            {

                playerDirection = Console.ReadKey().KeyChar;

                //Valid key check.
                if (playerDirection != 'w' && playerDirection != 'a' && playerDirection != 's' && playerDirection != 'd')
                {

                    Console.WriteLine("Invalid input, please use w, a, s or d to specify a direction.");

                }

            }
            while (playerDirection != 'w' && playerDirection != 'a' && playerDirection != 's' && playerDirection != 'd');

            return playerDirection;

        }

        //Offers the player the ability to move to an adjacent tile.
        public static void optionalMove(int rollWinnerID)
        {

            if (yesOrNoInput(messageUponNoReply: "", messageUponYesReply: "Please enter a direction to move:"))
            {

                movePlayer(amountToMove: 1, playerID: rollWinnerID, allowedToLandOnPlayer: false, allowedToTeleport: true);

            }

        }

        //Checks if a player is able to take a cheese, if they can: take the cheese for them.
        public static void cheeseCheck(int playerID)
        {

            //If cheese on players location.
            if (cheesePositions[players[playerID].xPosition, players[playerID].yPosition])
            {

                //Remove cheese from board, add to players score.
                cheesePositions[players[playerID].xPosition, players[playerID].yPosition] = false;
                players[playerID].cheese++;

            }

        }

        //Allows players to 'fight' eachother.
        public static void duel(int playerID)
        {

            renderBoard();

            List<int> targetablePlayersIDs = new List<int>();

            int targetPlayerID;

            Random diceRoll = new Random();

            int player1Roll;
            int player2Roll;

            int rollWinnerID;
            int rollLoserID;

            int highRoll;

            //Propegates list of targetable players.
            for (int i = 0; i < players.Count(); i++)
            {

                if (i != playerID)
                {

                    if ((players[playerID].xPosition == players[i].xPosition) && (players[playerID].yPosition == players[i].yPosition))
                    {

                        targetablePlayersIDs.Add(i);

                    }

                }

            }

            //Asks the player to choose their target if there are multiple.
            if (targetablePlayersIDs.Count() > 1)
            {

                Console.WriteLine(players[playerID].name + ", please select a target player:");

                //Prints list of targets.
                for (int i = 0; i < targetablePlayersIDs.Count(); i++)
                {

                    Console.WriteLine((i + 1) + ". " + players[targetablePlayersIDs[i]].name);

                }

                //Sets target to whichever target the player selects.
                targetPlayerID = targetablePlayersIDs[safeIntInput(lowerLimit: 1, upperLimit: targetablePlayersIDs.Count()) - 1];

            }

            else //If only 1 option.
            {

                //Target player auto assigned.
                targetPlayerID = targetablePlayersIDs[0];

            }

            //Coke to roll and find a duel winner + their score. Loops until a winner is found from dice rolls.
            if (defaultTestSequence)
            {

                //If cheese can be stolen, take one.
                if (players[targetPlayerID].cheese > 0)
                {

                    Console.WriteLine("[DEFAULT TEST SEQUENCE] " + players[playerID].name + " steals the cheese from " + players[targetPlayerID].name + ".\nPress return to continue.");
                    players[playerID].cheese++;
                    players[targetPlayerID].cheese--;
                    Console.ReadLine();

                }

                else //No cheese to take.
                {

                    Console.WriteLine("[DEFAULT TEST SEQUENCE] " + players[targetPlayerID].name + " had no cheese to steal.");

                }

            }

            else
            {

                do
                {

                    player1Roll = diceRoll.Next(1, 7);
                    player2Roll = diceRoll.Next(1, 7);

                    //Allows admins to enter their roll.
                    if (adminMode)
                    {

                        Console.WriteLine("(ADMIN) [DUEL]: " + players[playerID].name + ", please enter your roll:");
                        player1Roll = safeIntInput(lowerLimit: 1, upperLimit: 6);

                        Console.WriteLine("(ADMIN) [DUEL]: " + players[targetPlayerID].name + ", please enter your roll:");
                        player2Roll = safeIntInput(lowerLimit: 1, upperLimit: 6);

                    }

                    Console.WriteLine(players[playerID].name + " rolled a " + player1Roll);
                    Console.WriteLine(players[targetPlayerID].name + " rolled a " + player2Roll);

                    //Assigning winner/loser and storing the winning roll.
                    if (player1Roll > player2Roll)
                    {

                        rollWinnerID = playerID;
                        rollLoserID = targetPlayerID;
                        highRoll = player1Roll;
                        break;

                    }

                    else if (player1Roll < player2Roll)
                    {

                        rollWinnerID = targetPlayerID;
                        rollLoserID = playerID;
                        highRoll = player2Roll;
                        break;

                    }

                    else
                    {

                        Console.WriteLine("No winner, press return to re-roll.");
                        Console.ReadLine();

                    }

                }
                while (true);

                //Generating results based on given rule set.
                //In this section what happens is fully explained by writelines.
                //Any checks denoted with a '*' are checking how much the player losing the cheese can possibly lose.
                if (highRoll == 2)
                {

                    if (players[rollWinnerID].cheese > 0) //*
                    {

                        Console.WriteLine(players[rollWinnerID].name + "Fails in his attempt to steal cheese from " + players[rollLoserID].name + " and loses one cheese to " + players[rollLoserID].name + " in the process!");
                        players[rollWinnerID].cheese--;
                        players[rollLoserID].cheese++;

                    }

                    else //*
                    {

                        Console.WriteLine(players[rollWinnerID].name + "Fails in his attempt to steal cheese from " + players[rollLoserID].name + " and their cheese cargo doors springs open!\nLuckily for them, " + players[rollWinnerID].name + " didn't have any cheese to lose!");

                    }

                }

                else if (highRoll == 3)
                {

                    Console.WriteLine(players[rollWinnerID].name + " manages to drive " + players[rollLoserID].name + " away.\nPlease select a direction to move " + players[rollLoserID].name);
                    movePlayer(amountToMove: 1, playerID: rollLoserID, allowedToLandOnPlayer: false, allowedToTeleport: true);

                }

                else if (highRoll == 4)
                {

                    Console.WriteLine(players[rollWinnerID].name + " successfully flies into " + players[rollLoserID].name + "'s vacinity.\n" + players[rollWinnerID].name + ", would you like to move to an adjacent tile? y/n");
                    optionalMove(rollWinnerID: rollWinnerID);

                }

                else if (highRoll == 5)
                {

                    if (players[rollLoserID].cheese > 0) //*
                    {

                        Console.WriteLine(players[rollWinnerID].name + " successfully flies into " + players[rollLoserID].name + "'s vacinity and manages to steal a cheese.\n" + players[rollWinnerID].name + ", would you like to move to continue moving to an adjacent tile? y/n");
                        players[rollLoserID].cheese--;
                        players[rollWinnerID].cheese++;
                        optionalMove(rollWinnerID: rollWinnerID);

                    }

                    else //*
                    {

                        Console.WriteLine(players[rollWinnerID].name + " successfully flies into " + players[rollLoserID].name + "'s vacinity and manges to raid their cargo bay.\n Sadly for them, " + players[rollLoserID].name + " had no cheese to steal.\n" + players[rollWinnerID].name + ", would you like to move to continue moving to an adjacent tile? y/n");
                        optionalMove(rollWinnerID: rollWinnerID);

                    }

                }

                else //if (highRoll == 6)
                {

                    if (players[rollLoserID].cheese == 1) //*
                    {

                        Console.WriteLine(players[rollWinnerID].name + " successfully flies into " + players[rollLoserID].name + "'s vacinity and manages to steal a crate of cheese.\nUnfortunately the crate only contains one cheese wedge.\n" + players[rollWinnerID].name + ", would you like to move to continue moving to an adjacent tile? y/n");
                        players[rollLoserID].cheese--;
                        players[rollWinnerID].cheese++;
                        optionalMove(rollWinnerID: rollWinnerID);

                    }

                    else if (players[rollLoserID].cheese > 1) //*
                    {

                        Console.WriteLine(players[rollWinnerID].name + " successfully flies into " + players[rollLoserID].name + "'s vacinity and manages to steal a crate containing two cheese!\n" + players[rollWinnerID].name + ", would you like to move to continue moving to an adjacent tile? y/n");
                        players[rollLoserID].cheese -= 2;
                        players[rollWinnerID].cheese += 2;
                        optionalMove(rollWinnerID: rollWinnerID);

                    }

                    else //*
                    {

                        Console.WriteLine(players[rollWinnerID].name + " successfully flies into " + players[rollLoserID].name + "'s vacinity and manges to raid their cargo bay.\n Sadly for them, " + players[rollLoserID].name + " had no cheese to steal.\n" + players[rollWinnerID].name + ", would you like to move to continue moving to an adjacent tile? y/n");
                        optionalMove(rollWinnerID: rollWinnerID);

                    }

                    Console.WriteLine("Press return to continue.");
                    Console.ReadLine();

                }

            }

        }

        //Method to move the players around the board as requested.
        public static void movePlayer(int amountToMove, int playerID, bool allowedToLandOnPlayer, bool allowedToTeleport)
        {

            //If not special telport case.
            if (amountToMove != 6 || !allowedToTeleport)
            {
                Char playerDirection;

                //Loop until succseful input.
                do
                {

                    //Taking player directional input.
                    playerDirection = takePlayerInput();

                    //Printing movement roll.
                    if (playerDirection == 'w' || playerDirection == 'a' || playerDirection == 's' || playerDirection == 'd')
                    {

                        Console.WriteLine(players[playerID].name + " moves " + amountToMove + " tiles.");

                    }

                    //Executing the movement.
                    if (playerDirection == 's')
                    {

                        if (allowedToLandOnPlayer)
                        {

                            //Movement will always happen so assigned at the start.
                            players[playerID].yPosition = playerPositionCorrection(players[playerID].yPosition + amountToMove);

                            //If there's another player where the current moving player lands, duel them.
                            if (playersOnATile(xPosition: players[playerID].xPosition, yPosition: players[playerID].yPosition) > 1)
                            {

                                duel(playerID: playerID);

                            }

                            cheeseCheck(playerID: playerID);

                            break;

                        }

                        else //If not allowed to land on another player.
                        {

                            //If there's no one present on the target tile, move the player.
                            if (playersOnATile(xPosition: players[playerID].xPosition, yPosition: playerPositionCorrection(position: players[playerID].yPosition + amountToMove)) == 0)
                            {

                                players[playerID].yPosition = playerPositionCorrection(position: players[playerID].yPosition + amountToMove);

                                cheeseCheck(playerID: playerID);

                                break;

                            }

                            else
                            {

                                Console.WriteLine("Invalid direction, a ship already occupies that space.");

                            }

                        }

                    }

                    else if (playerDirection == 'a')
                    {

                        if (allowedToLandOnPlayer)
                        {

                            //Movement will always happen so assigned at the start.
                            players[playerID].xPosition = playerPositionCorrection(position: players[playerID].xPosition - amountToMove);

                            //If there's another player where the current moving player lands, duel them.
                            if (playersOnATile(xPosition: players[playerID].xPosition, yPosition: players[playerID].yPosition) > 1)
                            {

                                duel(playerID: playerID);

                            }

                            cheeseCheck(playerID: playerID);

                            break;

                        }

                        else //If not allowed to land on another player.
                        {

                            //If there's no one present on the taret tile, move the player.
                            if (playersOnATile(xPosition: playerPositionCorrection(position: players[playerID].xPosition - amountToMove), yPosition: players[playerID].yPosition) == 0)
                            {

                                //Correct position if off-screen.
                                players[playerID].xPosition = playerPositionCorrection(position: players[playerID].xPosition - amountToMove);

                                cheeseCheck(playerID: playerID);

                                break;

                            }

                            else
                            {

                                Console.WriteLine("Invalid direction, a ship already occupies that space.");

                            }

                        }

                    }

                    else if (playerDirection == 'w')
                    {

                        if (allowedToLandOnPlayer)
                        {

                            //Movement will always happen so assigned at the start.
                            players[playerID].yPosition = playerPositionCorrection(position: players[playerID].yPosition - amountToMove);

                            //If there's another player where the current moving player lands, duel them.
                            if (playersOnATile(xPosition: players[playerID].xPosition, yPosition: players[playerID].yPosition) > 1)
                            {

                                duel(playerID: playerID);

                            }

                            cheeseCheck(playerID: playerID);

                            break;

                        }

                        else //If not allowed to land on another player.
                        {

                            //If there's no one present on the taret tile, move the player.
                            if (playersOnATile(xPosition: players[playerID].xPosition, yPosition: playerPositionCorrection(position: players[playerID].yPosition - amountToMove)) == 0)
                            {

                                players[playerID].yPosition = playerPositionCorrection(position: players[playerID].yPosition - amountToMove);

                                cheeseCheck(playerID: playerID);

                                break;

                            }

                            else
                            {

                                Console.WriteLine("Invalid direction, a ship already occupies that space.");

                            }

                        }

                    }

                    else //if (playerDirection == 'd')
                    {

                        if (allowedToLandOnPlayer)
                        {

                            //Movement will always happen so assigned at the start.
                            players[playerID].xPosition = playerPositionCorrection(position: players[playerID].xPosition + amountToMove);

                            //If there's another player where the current moving player lands, duel them.
                            if (playersOnATile(xPosition: players[playerID].xPosition, yPosition: players[playerID].yPosition) > 1)
                            {

                                duel(playerID: playerID);

                            }

                            cheeseCheck(playerID: playerID);

                            break;

                        }

                        else //If not allowed to land on another player.
                        {

                            //If there's no one present on the target tile, move the player.
                            if (playersOnATile(xPosition: playerPositionCorrection(position: players[playerID].xPosition + amountToMove), yPosition: players[playerID].yPosition) == 0)
                            {

                                players[playerID].xPosition = playerPositionCorrection(position: players[playerID].xPosition + amountToMove);

                                cheeseCheck(playerID: playerID);

                                break;

                            }

                            else
                            {

                                Console.WriteLine("Invalid direction, a ship already occupies that space.");

                            }

                        }

                    }

                }
                while (true);

            }

            else //If rolled 6 and can teleport.
            {

                Console.WriteLine("Congratulations " + players[playerID].name + ", you rolled a 6!\nDo you wish to teleport to another player? y/n:");

                bool wishToTeleport;

                wishToTeleport = yesOrNoInput(messageUponNoReply: "", messageUponYesReply: "");

                if (wishToTeleport)
                {

                    //Loop until correct tile given.
                    do
                    {

                        Console.WriteLine(players[playerID].name + ", please enter the x co-ordinate which you wish to teleport to:");

                        //Using 0-7 test sequence numbering.
                        if (defaultTestSequence)
                        {

                            players[playerID].xPosition = (safeIntInput(lowerLimit: 0, upperLimit: 7));

                        }

                        else //Using more inuitive 1-8 numbering.
                        {

                            players[playerID].xPosition = (safeIntInput(lowerLimit: 1, upperLimit: 8) - 1);

                        }

                        Console.WriteLine(players[playerID].name + ", please enter the y co-ordinate which you wish to teleport to:");

                        if (defaultTestSequence)
                        {

                            players[playerID].yPosition = (safeIntInput(lowerLimit: 0, upperLimit: 7));

                        }

                        else //Using more inuitive 1-8 numbering.
                        {

                            players[playerID].yPosition = (safeIntInput(lowerLimit: 1, upperLimit: 8) - 1);

                        }

                        //If there's another player on target tile.
                        if (playersOnATile(players[playerID].xPosition, players[playerID].yPosition) > 1)
                        {

                            duel(playerID);
                            break;

                        }

                        else
                        {

                            Console.WriteLine("That tiles does not contain a player, please try again.");

                        }

                    }
                    while (true);

                }

                else
                {

                    //Move without teleport.
                    Console.WriteLine("Please choose a direction using w,a,s or d:");
                    movePlayer(amountToMove: 6, playerID: playerID, allowedToLandOnPlayer: true, allowedToTeleport: false);
                    renderBoard();

                }

            }

        }

        static void Main(string[] args)
        {

            Console.SetWindowSize(70, 70);

            //Main game code inside a do - while - true, ensures multiple games can be played without re-loading game.
            do
            {

                players.Clear();

                //Inputting amount of players.
                Console.WriteLine("Please enter the amount of players:");
                int playerCount = safeIntInput(lowerLimit: 2, upperLimit: 4);
                int CHEESE_TO_DISTRIBUTE = 16;

                //Used below to store whether a name is valid or not.
                bool nameTaken = false;

                //Takes in player information.
                for (int i = 0; i < playerCount; i++)
                {

                    players.Add(new PlayerInformation());

                    Console.WriteLine("Please enter your name, player " + (i + 1) + ":");

                    do
                    {

                        players[i].name = Console.ReadLine();

                        nameTaken = false;

                        //Checking if name is taken or an empty string.
                        for (int j = 0; j < players.Count(); j++)
                        {

                            if (players[i].name == "" || (players[i].name == players[j].name && i != j))
                            {

                                Console.WriteLine("Name is either null or already taken. Please enter a different name:");
                                nameTaken = true;
                                break;

                            }

                        }

                    }
                    while (nameTaken);

                    //Set of if statements which assign initial positions and colours.
                    if (i == 0)
                    {

                        players[i].xPosition = 0;
                        players[i].yPosition = 0;
                        Console.WriteLine(players[i].name + ", you are green. Press any key to continue.");
                        players[i].colour = ConsoleColor.Green;
                        Console.ReadKey();
                        Console.Clear();

                        Console.WriteLine("(ADMIN) Admin mode enabled.");

                        //If admin mode enabled.
                        if (players[i].name == "admin".ToLower())
                        {

                            //Enable/disable test sequence - only for use by Rob and co.
                            Console.WriteLine("(ADMIN) Would you like to use the default cheese test sequence and award blue an extra three cheese? y/n");
                            if (yesOrNoInput(messageUponNoReply: "", messageUponYesReply: ""))
                            {

                                Console.WriteLine("\n(ADMIN) Dueling mode disabled for test sequence, one cheese will be stolen in every scenario.\n\n(ADMIN) For the purpose of the test sequence, written co-ordinates will be displayed between 0 and 7, rather than 1 and 8 to ensure a quick and proper demonstration.\n");
                                defaultTestSequence = true;

                                cheesePositions[1, 0] = true;
                                cheesePositions[0, 1] = true;
                                cheesePositions[1, 3] = true;
                                cheesePositions[1, 0] = true;
                                cheesePositions[2, 4] = true;
                                cheesePositions[0, 5] = true;
                                cheesePositions[2, 7] = true;
                                cheesePositions[3, 6] = true;
                                cheesePositions[4, 5] = true;
                                cheesePositions[5, 0] = true;
                                cheesePositions[5, 3] = true;
                                cheesePositions[5, 5] = true;
                                cheesePositions[6, 0] = true;
                                cheesePositions[6, 1] = true;
                                cheesePositions[6, 2] = true;
                                cheesePositions[6, 7] = true;
                                cheesePositions[7, 6] = true;

                            }

                            else
                            {

                                Console.WriteLine("(ADMIN) Please enter the amount of cheese to distribute (default 16):");
                                CHEESE_TO_DISTRIBUTE = safeIntInput(lowerLimit: 0, upperLimit: 64 - playerCount);

                            }

                            Console.WriteLine("(ADMIN) You will now be able to select your rolls in every scenario.\nPress any key to continue.");
                            adminMode = true;

                            Console.ReadKey();
                            Console.Clear();

                        }

                    }

                    else if (i == 1)
                    {

                        players[i].xPosition = 7;
                        players[i].yPosition = 0;
                        Console.WriteLine(players[i].name + ", you are blue. Press any key to continue.");
                        players[i].colour = ConsoleColor.Yellow;
                        Console.ReadKey();
                        Console.Clear();



                    }

                    else if (i == 2)
                    {

                        players[i].xPosition = 7;
                        players[i].yPosition = 7;
                        Console.WriteLine(players[i].name + ", you are yellow. Press any key to continue.");
                        players[i].colour = ConsoleColor.Red;
                        Console.ReadKey();
                        Console.Clear();

                    }

                    else if (i == 3)
                    {

                        players[i].xPosition = 0;
                        players[i].yPosition = 7;
                        Console.WriteLine(players[i].name + ", you are red. Press any key to continue.");
                        players[i].colour = ConsoleColor.DarkCyan;
                        Console.ReadKey();
                        Console.Clear();

                        //If using test sequence, start the player with the required score.
                        if (defaultTestSequence)
                        {

                            players[i].cheese = 3;

                        }

                    }

                }

                //Cheese distributing position information.
                int cheeseDistributionXPosition = 0;
                int cheeseDistributionYPosition = 0;

                //Cheese distribution code.
                //Runs while there are cheese available.
                if (!defaultTestSequence)
                {

                    while (CHEESE_TO_DISTRIBUTE > 0)
                    {

                        //Allows players to distribute cheese in sequence.
                        for (int i = 0; i < playerCount; i++)
                        {


                            if (CHEESE_TO_DISTRIBUTE > 0)
                            {

                                renderBoard();
                                Console.WriteLine("Cheese left to place: " + CHEESE_TO_DISTRIBUTE);

                                //Taking in cheese co-ordinates.
                                Console.WriteLine(players[i].name + ", please enter the x co-ordinate on which you wish to place your cheese.");
                                cheeseDistributionXPosition = (safeIntInput(lowerLimit: 1, upperLimit: 8) - 1);

                                Console.WriteLine(players[i].name + ", please enter the y co-ordinate on which you wish to place your cheese.");
                                cheeseDistributionYPosition = (safeIntInput(lowerLimit: 1, upperLimit: 8) - 1);

                                //Checking if the chosen tile is a player home tile.
                                if ((cheeseDistributionXPosition == 0 && cheeseDistributionYPosition == 0) ||
                                    (cheeseDistributionXPosition == 0 && cheeseDistributionYPosition == 7) ||
                                    (cheeseDistributionXPosition == 7 && cheeseDistributionYPosition == 0) ||
                                    (cheeseDistributionXPosition == 7 && cheeseDistributionYPosition == 7))
                                {

                                    Console.WriteLine("Cannot place cheese on that tile as it is a player home tile. Press return to enter a different position.");
                                    i--;
                                    Console.ReadLine();

                                }
                                //Checking if tile already has a cheese on it.
                                else if (cheesePositions[cheeseDistributionXPosition, cheeseDistributionYPosition])
                                {

                                    Console.WriteLine("Cannot place cheese on that tile as it already contains cheese. Press return to enter a different position.");
                                    i--;
                                    Console.ReadLine();

                                }

                                //Confirming valid choice and making change.
                                else
                                {

                                    CHEESE_TO_DISTRIBUTE--;
                                    cheesePositions[cheeseDistributionXPosition, cheeseDistributionYPosition] = true;

                                }

                            }

                            else
                            {

                                break;

                            }

                        }

                    }

                }


                //Used to determine movement distance.
                Random diceRoll = new Random();

                //Loop until a winner is found.
                do
                {

                    currentTurn++;

                    renderBoard();

                    int rollValue = 0;

                    //Go through each player's turn.
                    for (int i = 0; i < playerCount; i++)
                    {

                        Console.WriteLine("It's your turn, " + players[i].name + ".");

                        //In test sequence mode, dice rolls are decided by the admin.
                        if (!adminMode)
                        {

                            //Deciding how far the player moves.
                            rollValue = diceRoll.Next(1, 7);

                            Console.WriteLine(players[i].name + ", you have rolled a " + rollValue + ".");

                        }

                        //If admin mode, let player select move distance.
                        else
                        {

                            Console.WriteLine("(ADMIN) [MOVEMENT]: " + players[i].name + ", Please select your movement roll amount.");
                            rollValue = safeIntInput(lowerLimit: 1, upperLimit: 6);

                        }

                        //Moving the player normally.
                        if (rollValue < 6)
                        {

                            Console.WriteLine("Please choose a direction using w,a,s or d:");

                        }

                        movePlayer(amountToMove: rollValue, playerID: i, allowedToLandOnPlayer: true, allowedToTeleport: true);
                        renderBoard();

                        Console.WriteLine(players[i].name + " moves " + rollValue + ".");

                        //End the game if there's a winner.
                        if (winCheck() >= 0)
                        {

                            break;

                        }

                    }

                }
                while (winCheck() < 0);


                Console.Clear();

                //Show final state if default test sequence - like rob wants!
                if (defaultTestSequence)
                {

                    Console.WriteLine("Final state:\n");

                    for (int i = 0; i < players.Count; i++)
                    {

                        renderBoard();

                    }

                }
                Console.WriteLine("Congratulations " + players[winCheck()].name + ", you win with " + players[winCheck()].cheese + " cheese!\nWould you like to play again? (y/n):");

                //Taking response to replaying.

                //Clear board and allow code to loop.
                if (yesOrNoInput(messageUponNoReply: "", messageUponYesReply: ""))
                {

                    //Reset variables.
                    Array.Clear(cheesePositions, 0, 64);
                    defaultTestSequence = false;
                    adminMode = false;
                    players = new List<PlayerInformation>();
                    Console.Clear();

                }

                //End game.
                else
                {

                    break;

                }

            }
            while (true);

        }

    }

}