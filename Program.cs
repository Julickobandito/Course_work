using System;
using System.Collections.Generic;

/**Made by Yuliia Koziuk, IS-03
 * Social Network. Relations between users/ user and net**/


namespace Course_work
{
    class Program
    {

        static void Main(string[] args)
        {
            SocialNet Net = new SocialNet("IS", "FICT@KPI");

            int R = -1;
            do
            {
                //outputting main menu
                PrintMainMenu();
                Console.WriteLine("Please enter from 1 to 5");
                try
                {
                    R = Convert.ToInt32(Console.ReadLine());
                    switch (R)
                    {
                        case 1:
                            Registration(Net);
                            break;
                        case 2:
                            User U = Authorisation(Net);
                            if (U != null)
                            {
                                Console.WriteLine("---------");
                                MyAccount(U, Net);
                            }
                            break;
                        case 3:
                            Groups(Net);
                            break;
                        case 4:
                            SocialNetInfo(Net);
                            break;
                        case 5:
                            break;
                        default:
                            Console.WriteLine("Enter 1 to 5");
                            break;
                    }
                    if (R < 0 | R > 5)
                    { throw new ArithmeticException("Enter 1 to 5"); }

                }
                catch (FormatException)
                {
                    Console.WriteLine("Enter 1 to 5");
                    Console.ReadKey();
                }
                catch (ArithmeticException)
                {
                    Console.WriteLine("Enter 1 to 5");
                    Console.ReadKey();
                }
            } while (R != 5);


            static void PrintMainMenu()
            {
                Console.Clear();
                Console.WriteLine("Choose item:\n" +
                    "1. Registration \n" +
                    "2. My account \n" +
                    "3. Groups \n" +
                    "4. Info\n" +
                    "5. Exit\n");
            }


            static void Registration(SocialNet Net)
            {
                Console.Clear();
                Console.WriteLine("Input Nick");
                string nick = Console.ReadLine();
                Console.WriteLine("Input Password");
                string pass = Console.ReadLine();
                if (Net.registry(nick, pass))
                {
                    Console.WriteLine("Congratulations,you are registered in the IS-FICT.net!");
                }
                else
                {
                    Console.WriteLine("Sorry, Nick is empty or occupied or password is empty");
                }
                Console.ReadKey();
            }

            static User Authorisation(SocialNet Net)
            {
                Console.Clear();
                Console.WriteLine("In order to enter the social network, enter your nickname and password");
                Console.WriteLine("Input Nick");
                string nick = Console.ReadLine();
                Console.WriteLine("Input Password");
                string pass = Console.ReadLine();
                User U = Net.myAccount(nick, pass);
                if (U == null)
                    Console.WriteLine("User doesn`t exist");
                Console.ReadKey();
                return U;


            }
            static void PrintUserInfo(User U)
            {
                if (U != null)
                {
                    Console.Clear();
                    Console.WriteLine("Nick: " + U.Nick);
                    Console.WriteLine("Name: " + U.Name);
                    Console.WriteLine("Age: " + U.Age.ToString());
                    Console.WriteLine("About me: " + U.Additional);
                    Console.WriteLine("My groups: ");
                    for (int i = 0; i < U.MyGroups.Count; i++)
                        Console.WriteLine("\t" + U.MyGroups[i]);
                    Console.WriteLine("My friends: " + U.Friends);
                    Console.WriteLine("My friend requests: " + U.Requests);
                    Console.WriteLine("-------------");
                }
            }

            static void PrintAccountMenu()
            {
                Console.WriteLine("Choose item:\n" +
                    "1. Change info \n" +
                    "2. Follow group \n" +
                    "3. Unfollow group \n" +
                    "4. Send request\n" +
                    "5. Add to friend\n" +
                    "6. Decline request\n" +
                    "7. Delete account\n" +
                    "8. Return to main menu"
                    );
            }

            static void MyAccount(User U, SocialNet Net)
            {
                int R = -1;
                do
                {
                    PrintUserInfo(U);
                    PrintAccountMenu();
                    try
                    {
                        R = Convert.ToInt32(Console.ReadLine());
                        switch (R)
                        {
                            case 1:
                                AddUserInfo(U);

                                break;
                            case 2:
                                FollowGroup(Net, U);
                                break;
                            case 3:
                                UnFollowGroup(U);
                                break;
                            case 4:
                                SendRequest(U);
                                break;
                            case 5:
                                AddToFriend(U);
                                break;
                            case 6:
                                DeclineRequest(U);
                                break;
                            case 7:
                                if (DeleteAccount(U, Net))
                                    R = 8;
                                break;
                            case 8:
                                break;
                            default:
                                Console.WriteLine("Enter 1 to 8");
                                break;
                        }
                    }

                    catch (FormatException)
                    {
                        Console.WriteLine("Enter 1 to 8");
                        Console.ReadKey();
                    }

                } while (R != 8);

            }

            static void AddUserInfo(User U)
            {
                string name = "";
                int Age = 0;
                string Additional = "";
                if (U != null)
                {
                    Console.Clear();
                    Console.WriteLine("Name: ");
                    name = Console.ReadLine();
                    try
                    {
                        Console.WriteLine("Age: ");
                        Age = Convert.ToInt32(Console.ReadLine());
                        if ((Age < 0) | (Age > 100))
                        {
                            throw new LogicalException("Incorrect age");
                        }
                        Console.WriteLine("About me: ");
                        Additional = Console.ReadLine();
                        U.addInfo(name, Age);
                        U.addMore(Additional);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Enter integer number");
                    }
                    catch (LogicalException)
                    {
                        Console.WriteLine("Enter correct age");
                    }
                    Console.ReadKey();
                }
            }
            static void FollowGroup(SocialNet Net, User U)
            {
                Console.WriteLine("Choose from groups:");
                PrintGroups(Net.getGroupNames());
                Console.WriteLine("Input name:");
                string name = Console.ReadLine();
                Group G = Net.getGroupbyName(name);
                if ((G != null) && (G.addMember(U.Nick, U.Password)))
                { }
                else
                { Console.WriteLine("Group not found"); }
                Console.ReadKey();

            }

            static void UnFollowGroup(User U)
            {
                Console.WriteLine("Choose from groups:");
                PrintGroups(U.MyGroups);
                Console.WriteLine("Input name:");
                string name = Console.ReadLine();
                if (!(U.delete_from_group(name)))
                {
                    Console.WriteLine("Group not found");
                }
            }

            //Net gets information about User you want to be friend with if he exists
            static void SendRequest(User U)
            {
                Console.Clear();
                Console.WriteLine("Enter Username:");
                string name = Console.ReadLine();
                if (U.send_request(name))
                { Console.WriteLine("Request has been sent!"); }
                else { Console.WriteLine("User wasn`t found. Request hasn`t been sent!"); }
                Console.ReadKey();
            }

            //you can choose users from the request list
            static void AddToFriend(User U)
            {
                Console.Clear();
                Console.WriteLine("Requests: " + U.Requests);
                Console.WriteLine("Choose user:");
                string name = Console.ReadLine();
                if (!(U.acceptRequest(name)))
                {
                    Console.WriteLine("User not found");
                }
                Console.ReadKey();

            }
            static void DeclineRequest(User U)
            {
                Console.Clear();
                Console.WriteLine("Requests: " + U.Requests);
                Console.WriteLine("Choose user:");
                string name = Console.ReadLine();
                if (!(U.declineRequest(name)))
                {
                    Console.WriteLine("User not found");

                }
                Console.ReadKey();

            }
            static bool DeleteAccount(User U, SocialNet Net)
            {
                Console.Clear();
                Console.WriteLine("Your account will be deleted.");

                bool Res = Net.delete_account(U.Nick, U.Password);
                if (Res)
                    Console.WriteLine("Your account was deleted");
                Console.ReadKey();
                return Res;
            }

            //operations with groups in network
            static void PrintGroupMenu()
            {
                Console.WriteLine("Choose item:\n" +
                "1. Add group \n" +
                "2. Delete group \n" +
                "3. Group info \n" +
                "4. Return\n"
                );
            }

            static void Groups(SocialNet Net)
            {
                int R = -1;
                do
                {
                    Console.Clear();
                    PrintGroups(Net.getGroupNames());
                    PrintGroupMenu();
                    try
                    {
                        R = Convert.ToInt32(Console.ReadLine());
                        switch (R)
                        {
                            case 1:
                                AddGroup(Net);
                                break;
                            case 2:
                                DeleteGroup(Net);
                                break;
                            case 3:
                                GroupInfo(Net);
                                break;
                            case 4:

                                break;
                            default:
                                Console.WriteLine("Enter 1 to 4");
                                break;
                        }
                    }

                    catch (FormatException)
                    {
                        Console.WriteLine("Enter 1 to 4");
                        Console.ReadKey();
                    }
                } while (R != 4);
            }

            //outputting all groups in network
            static void PrintGroups(List<string> groups)
            {

                Console.WriteLine("Groups:");
                if (groups != null)
                {
                    for (int i = 0; i < groups.Count; i++)
                    {
                        Console.WriteLine(groups[i]);
                    }
                }
                Console.WriteLine("-------------------");
            }
            //only admin can create a group
            static void AddGroup(SocialNet Net)
            {
                Console.Clear();
                Console.WriteLine("Input Nick");
                string nick = Console.ReadLine();
                Console.WriteLine("Input Password");
                string pass = Console.ReadLine();
                if ((Net.Admin.Nick == nick) && (Net.Admin.Password == pass))
                {
                    Console.WriteLine("Input name of group:");
                    string name = Console.ReadLine();
                    Console.WriteLine("Input information about group:");
                    string info = Console.ReadLine();
                    if (!(Net.addGroup(name, info)))
                        Console.WriteLine("Group already exists");
                    else { Console.WriteLine("Group is created!"); }
                }
                else { Console.WriteLine("You are not admin. Only administrator can add and delete groups"); }
                Console.ReadKey();
            }
            //only admin can delete group
            static void DeleteGroup(SocialNet Net)
            {
                Console.Clear();
                Console.WriteLine("Input Nick");
                string nick = Console.ReadLine();
                Console.WriteLine("Input Password");
                string pass = Console.ReadLine();
                if ((Net.Admin.Nick == nick) && (Net.Admin.Password == pass))
                {

                    PrintGroups(Net.getGroupNames());
                    Console.WriteLine("Input name of group:");
                    string name = Console.ReadLine();
                    if (!(Net.deleteGroup(name)))
                    {
                        Console.WriteLine("There is no such group");
                    }
                    else { Console.WriteLine("Group is deleted!"); }

                }
                else { Console.WriteLine("You are not admin. Only administrator can add and delete groups!"); }
                Console.ReadKey();


            }

            static void GroupInfo(SocialNet Net)
            {
                Console.Clear();
                PrintGroups(Net.getGroupNames());
                Console.WriteLine("Input name of group:");
                string name = Console.ReadLine();
                Group G = Net.getGroupbyName(name);
                if (G != null)
                {
                    Console.Clear();
                    Console.WriteLine(G.Name);
                    Console.WriteLine(G.Info);
                    Console.WriteLine("Amount of users: " + G.Count.ToString());

                    Console.WriteLine("Users:");
                    List<string> L = G.getUsers();
                    for (int i = 0; i < L.Count; i++)
                        Console.WriteLine("\t" + L[i]);
                }
                else
                {
                    Console.WriteLine("There is no such group");
                }
                Console.ReadKey();
            }
            //info about groups and amount of users
            static void SocialNetInfo(SocialNet Net)
            {
                Console.Clear();
                Console.WriteLine("Name: " + Net.Name);
                Console.WriteLine("Adress: " + Net.Adress);
                PrintGroups(Net.getGroupNames());
                Console.WriteLine("Amount of users: " + Net.Count.ToString());
                Console.ReadKey();
            }
        }
        //generating own exception
        class LogicalException : Exception
        {
            public LogicalException(string message)
                : base(message)
            { }
        }
    }
}   
