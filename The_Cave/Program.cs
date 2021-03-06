﻿using System;

namespace The_Cave
{
    class Program
    {
        //Main code is run in here.
        static void Main(string[] args)
        {
            do
            {
                Console.Clear();
                PrintIntro();
                PlayGame();

            } while (AskToPlayAgain());

        }

        //Print intro.
        public static void PrintIntro()
        {
            //Have some ASCII art
            Console.WriteLine("       THE CAVE");
            Console.WriteLine();
            Console.WriteLine("   ----------------");
            Console.WriteLine("  /  ------------  \\");
            Console.WriteLine(" (  (############)  )");
            Console.WriteLine("(   (############)   )");
            Console.WriteLine("(   (############)   )");
            Console.WriteLine("(   (############)   )");
            Console.WriteLine("----------------------");

            //Tell user what the game is
            //Explain rules and mechanics to user

            Console.WriteLine();
            Console.WriteLine("-*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*-");
            Console.WriteLine("| -This is a game of adventure!                                              #");
            Console.WriteLine("# -Choose your profession! Each one gives you different abilities and stats  |");
            Console.WriteLine("| -Find your way out of the cave.                                            #");
            Console.WriteLine("# -Use items to help you defeat the enemies that stand in your way           |");
            Console.WriteLine("| -Find treasures that you can take back to the surface!                     #");
            Console.WriteLine("# -Use words to navigate. Type help for the keywords!                        |");
            Console.WriteLine("|                                                                            #");
            Console.WriteLine("#                           Press enter to begin!                            |");
            Console.WriteLine("-*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*-");
            Console.ReadLine();
        }

        //Code that runs through the game.
        public static void PlayGame()
        {
            //Objects Declaration

            Random rand = new Random(DateTime.Now.Millisecond);
            TheCave game = new TheCave();

            //Creates arrays that an RNG randomly chooses from

            Enemies[] enemArr = game.InitEnemies();
            Weapons[] wepArr = game.InitWeapons();
            Armours[] armArr = game.InitArmours();

            //Get User Profession and put the stats into variables to be used later

            Professions userProf = game.GetProfession();

            //Get the max amount of turns based on difficulty

            int maxTurns = game.GetTurns(game.GetDifficulty());

            Console.Clear();

            Console.WriteLine("-*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*-");
            Console.WriteLine("|                       You have {0} turns to escape!                        |", maxTurns);
            Console.WriteLine("|            You start with a Dagger that has {0} atk and {1} def            |", wepArr[0].GetAtk(), wepArr[0].GetDef());
            Console.WriteLine("|                You start with Wool Armour that has {0} def                 |", armArr[0].GetDef());
            Console.WriteLine("|                    Press enter to continue. Good Luck!                     |");
            Console.WriteLine("-*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*--*-");
            Console.ReadLine();

            Console.Clear();

            //Variable declarations

            // Can only hold a certain amount of items(Inventory)

            Object[] inventory = new Object[10]; 

            int randomNum = rand.Next(1, 30);
            int genMonster = 0;
            //int genItem = 0;
            int fwd = 0;
            int right = 0;

            bool isValid = false;

            string input = "";

            for (int i = 0; i <= maxTurns; i++)
            {
                int userHP = userProf.GetHealth();
                int userAtk = wepArr[0].GetAtk();
                int userDef = wepArr[0].GetDef() + armArr[0].GetDef();

                //Asks the user the direction they want to go in

                do
                {
                    Console.Write("Which direction would you like to go in? (f, b, l, r): ");
                    input = Console.ReadLine();

                    input = input.ToLower();

                    //Validates user input

                    if (input[0] == 'f' || input[0] == 'b' || input[0] == 'l' || input[0] == 'r')
                    {
                        isValid = true;
                    }
                    else
                    {
                        Console.WriteLine("Input invalid. Please try again.");
                        Console.WriteLine();
                        isValid = false;
                    }
                } while (isValid == false);

                //Allow the user to make decision as to where they go

                switch (input[0])
                {
                    case 'f':

                        Console.WriteLine("You moved forward!");
                        fwd++;
                        break;

                    case 'b':

                        if (fwd == 0)
                        {
                            Console.WriteLine("You cant move back");
                        }
                        else
                        {
                            Console.WriteLine("You moved backwards");
                            fwd--;
                        }
                        break;

                    case 'l':

                        if (right == 0)
                        {
                            Console.WriteLine("You cant move left");
                        }
                        else
                        {
                            Console.WriteLine("You moved left");
                            right++;
                        }
                        break;

                    case 'r':

                        Console.WriteLine("You moved right");
                        right--;
                        break;
                }

            

                genMonster += randomNum;

                if (genMonster >= 100)
                {
                    int randNum = rand.Next(0, 5);

                   
                    //Generate enemies
                    Enemies enem = enemArr[randNum];

                    //Declares the chosen enemies battle stats

                    int enemAtk = enem.GetAtk();
                    int enemDef = enem.GetDef();
                    int enemHP = enem.GetHP();
                    int enemMP = enem.GetMP();
                    string enemName = enem.GetName();

                    Console.WriteLine("You have to battle {0}", enemName);

                    for (int j = 0; j < 999; i++)
                    {
                        string userInput = "";

                        Console.WriteLine("What would you like to do? (Attack, Defend, Use Item, Run Away)");
                        userInput = Console.ReadLine().ToLower();

                        int userStatus = game.UserTurn(enemAtk, enemDef, enemHP, userAtk, userDef, userHP, userInput[0]);
                        int enemStatus = game.EnemTurn(enemAtk, enemDef, enemHP, userAtk, userDef, userHP);

                        userHP = userStatus;
                        enemHP = enemStatus; 

                        if (userStatus == -1)
                        {
                            genMonster = 0;
                            break;
                        }
              
                        if (enemHP <= 0)
                        {
                            Console.WriteLine("You have successfully beaten the {0}", enemArr[0].GetName());
                            genMonster = 0;
                            break;
                        }
                        else if (userHP <= 0)
                        {
                            PrintSummary("dead");
                        }
                    }
                }
            }

            //TODO Assign objects where necessary

            //TODO Generate items around the area

            //Allow the use of objects where necessary
                //Give items certain boosts
                //Can drop item if necessary

            //Generate attack and defense of enemies
                //Compare enemies stats to user
                //Depending on stats, depends on damage
                //DEF ATK HP

            //TODO Have an end goal
                //Collected the right amount of quest items?
                //Make it out of the cave?
                //Kill a certain amount of enemies?

            //FUTURE REF: Perhaps have different quests that the user can choose from for replayability
            //FUTURE REF: Generate items in random places
            //FUTURE REF: Generate different maps each time

        }

        //Prints summary of the game.
        public static void PrintSummary(string ending)
        {
            if (ending == "esc")
            {
                Console.WriteLine("Congratulations! You made it out of the cave and you found all the items!");
            }
            else if (ending == "esc<100")
            {
                Console.WriteLine("Congratulations! You made it out but you didn't find everything. Try again!");
            }
            else if (ending == "dead")
            {
                Console.WriteLine("You have died.");
            }
        }

        //Asks the user to play again.
        public static bool AskToPlayAgain()
        {
            string opt = "";

            Console.WriteLine("Play again if you dare! (Y/N)");
            opt = Console.ReadLine();

            //Returns a boolean that tells the program if the user wants to play again or not
            return (opt == "y" || opt == "Y");
        }
    }
}
