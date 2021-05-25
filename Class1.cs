using System;
using System.Collections.Generic;
using System.Text;

namespace Course_work
{
    //Class describes Social-net User
    class User
    {
        private SocialNet net;
        private string password;
        private string nick;
        private string name;
        private int age;
        private string additional;
        private Groups myGroups;
        private Friends friends;
        private Friends friend_request;
        public User(SocialNet Net, string Nick, string Password)
        {
            this.net = Net;
            this.password = Password;
            this.nick = Nick;
            this.name = "";
            this.additional = "";
            this.age = 0;
            this.myGroups = new Groups();
            this.friends = new Friends();
            this.friend_request = new Friends();
        }
        ~User() { }
        //properties to get private fields
        public string Nick { get { return this.nick; } }
        public string Password { get { return this.password; } }
        public string Name { get { return this.name; } }
        public int Age { get { return this.age; } }
        public string Additional { get { return this.additional; } }
        public List<string> MyGroups { get { return this.myGroups.getGroups(); } }
        public string Requests { get { return this.friend_request.getUsers(); } }
        public string Friends { get { return this.friends.getUsers(); } }

        //add new information (function overloading)
        public void addInfo(string Name) { this.name = Name; }
        public void addInfo(string Name, int Age)
        { this.name = Name;
            this.age = Age;
        }
        public void addMore(string Additional) { this.additional = Additional; }

        public bool add_to_group(Group NewGroup)
        {
            return this.myGroups.addMember(NewGroup);
        
        }
        public bool delete_from_group(string Name) 
        {
            return this.myGroups.deleteMember(Name);
        }
        public bool send_request(string Name) 
        {
            return net.resend_request(this, Name);
        }
        public void get_Request(User Friend)
        {
            if(!(Friend is null) && this.friend_request.findbyNick(Friend.Nick) is null)
            {
                this.friend_request.addMember(Friend);
            }    
        
        }


        public bool acceptRequest(string Friend)
        {
            bool Res = false;
            if(!(this.friend_request.findbyName(Friend)is null) && this.friends.findbyName(Friend) is null)
            {
                User F = this.friend_request.findbyName(Friend);
                Res =  this.friends.addMember(F);
                if (Res)
                {
                    this.friend_request.deleteMember(F.Nick);
                    F.addFriend(this);
                }
            }
            return Res;

        }
        public bool addFriend(User U)
        {
           return this.friends.addMember(U);
        }
        public bool declineRequest(string Friend)
        {
            bool Res = false;
            if (!(this.friend_request.findbyName(Friend) is null))
            {
                User F = this.friend_request.findbyName(Friend);
                if (F != null)
                {
                    this.friend_request.deleteMember(F.Nick);
                    Res = true;
                }
            }
            return Res;
        }

        //deleting from friends in case of deleting account
        public void deletefromfriends()
        {
            for (int i = 0; i < friends.Count; i++)
            {
                friends[i].deletefriend(this.nick);
            }
        }
        //deleting from friends if friend deletes account
        public void deletefriend(string Friend)
        {
            friends.deleteMember(Friend);
        }
    }

    //cointainer of users
    class BaseGroup
    {
        private List<User> members;
        public BaseGroup() { this.members = new List<User>(); }
        ~BaseGroup() { }
        public int Count { get { return this.members.Count; } }
        //indexator that let work with members of the any group
        public User this[int index]
        {
            set
            {
                if (index < this.members.Count && index >= 0)
                { this.members[index] = value; }
            }
            get
            {
                if (index < this.members.Count && index >= 0)
                    return this.members[index];
                else { return null; }
            }
        }

        protected bool addMember(User U)
        {
            if (U != null)
            {
                if (findbyNick(U.Nick) == null)
                {
                    this.members.Add(U);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        protected User findbyNick(string Nick)
        {
            User res = null;
            for (int i = 0; i < this.members.Count; i++)
            {
                if (this.members[i].Nick == Nick)
                {
                    res = this.members[i];
                    break;
                }
            }
            return res;
        }

        protected User findbyName(string Name)
        {
            User res = null;
            for (int i = 0; i < this.members.Count; i++)
            {
                if (this.members[i].Name == Name)
                {
                    res = this.members[i];
                    break;
                }
            }
            return res;
        }

        protected bool deleteMember(string Nick)
        {
            User U = findbyNick(Nick);
            if (U != null)
            {
                 return this.members.Remove(U); }
            else { return false; }
        }

        protected List<string> getUsers()
        {
            List<string> Res = new List<string>();
            for (int i=0; i<members.Count; i++)
            {
                Res.Add(members[i].Name+"("+members[i].Nick+")");
            }
            return Res;
        }
    }

    //class describes social net
    class SocialNet : BaseGroup
    {

        private string name;
        private string adress;
        //SocialNet has a special user with more credentials than others
        private User superuser;
        private Groups groups;
        public SocialNet(string Name, string Adress)
        {
            this.name = Name;
            this.adress = Adress;
            this.superuser = new User(this,"root","123qweASD");
            this.groups = new Groups();
        }
        ~SocialNet() { }
        //properties to get private fields
        public string Name { get { return this.name; } }
        public string Adress { get { return this.adress; } }
        public User Admin { get { return this.superuser; } }
        //account verification
        public bool verify(string Nick, string Password)
        {
            User U = findbyNick(Nick);
            return !(U is null) && (U.Password == Password);
        }
        public new User findbyNick(string Nick)
        {
            return base.findbyNick(Nick);
        }
        
        public new List <string > getUsers()
        {
            return base.getUsers();
        }
        public User myAccount(string Nick, string Password)
        {
            if (verify(Nick, Password))
            {
                return findbyNick(Nick);
            }
            else
            {
                return null;
            }

        }
        
        public bool delete_account(string Nick, string Password)
        {
            if (verify(Nick, Password))
            {
                User U = findbyNick(Nick);
                U.deletefromfriends();
                for (int i=0;i<U.MyGroups.Count;i++)
                {
                    Group G = getGroupbyName(U.MyGroups[i]);
                    if (G != null)
                        G.deleteMember(Nick, Password);
                }
                return deleteMember(Nick);
            }
            else { return false; }

        }

        public bool registry(string Nick, string Password)
        {
            if (findbyNick(Nick) is null && Password != "")
            {
                User NewUser = new User(this, Nick, Password);
                return addMember(NewUser);
            }
            else { return false;}
        }

       
        public bool addGroup(string Name, string Info)
        {
            if (this.groups.findbyName(Name) is null)
            {
                Group NewGroup = new Group(this, Name, Info);
                return this.groups.addMember(NewGroup);
            }
            else { return false; }

        }

        public Group getGroupbyName(string Name)
        {
            return this.groups.findbyName(Name);
        }

        public bool deleteGroup(string Name)
        {
            return this.groups.deleteMember(Name);
        }
        public List<string> getGroupNames()
        {
            return this.groups.getGroups();
        }

        public bool resend_request(User Sender, string Receiver)
        {
            bool R = false;
            if(!(findbyNick(Sender.Nick) is null) &&(!(findbyName(Receiver) is null)))
            {
                User Res = findbyName(Receiver);
                Res.get_Request(Sender);
                R = true;
            }
            return R;
        }
    }

    class Group: BaseGroup
    {
        private SocialNet owner;
        private string name;
        private string info;
        public Group(SocialNet S,string Name,string Info)
        {
            this.owner = S;
            this.name = Name;
            this.info = Info;
        }
        ~Group() { }
        public string Name { get { return this.name; } }
        public string Info { get { return this.info; } }
        public bool addMember(string Nick, string Password)
        {
            bool res = owner.verify(Nick, Password) && (findbyNick(Nick) is null);
            if (res) 
            {
                User U = owner.findbyNick(Nick);
                res = addMember(U);
                if (res) U.add_to_group(this);
            }
            return res;
        }

        public bool deleteMember(string Nick, string Password)
        {
            bool res = owner.verify(Nick, Password) && (!(findbyNick(Nick) is null));
            if (res) 
            {
                User U = findbyNick(Nick);
                res = deleteMember(Nick);
                if(res)
                {
                    U.delete_from_group(this.Name);
                }
            }
            return res;
        }

        //delete members in case group is gonna be deleted
        public void deleteAllMembers()
        {
            List<string> Users = getUsers();
            for (int i=0; i<Users.Count;i++)
            {
                User U = findbyName(Users[i]);
                if (U != null)
                {
                    bool res = deleteMember(U.Nick);
                    if (res)
                    {
                        U.delete_from_group(this.Name);
                    }
                }
            }
        }
        public new List<string> getUsers(){return base.getUsers();}
    }

    class Friends: BaseGroup
    {
        public Friends() { }
        ~Friends() { }
        public new bool addMember(User U) { return base.addMember(U);}
        public new User findbyNick(string Nick) { return base.findbyNick(Nick); }
        public new User findbyName(string Name) { return base.findbyName(Name); }
        public new bool deleteMember(string Nick) { return base.deleteMember(Nick); }
        public new string getUsers()
        {
            List<string> Users = base.getUsers();
            string Res = "";
            if (Users.Count > 0)
            {
                for (int i = 0; i < Users.Count - 1; i++)
                    Res += Users[i] + ",";
                Res += Users[Users.Count - 1];            
            }
            return Res;
        }

    }

    //group container
    class Groups
    {
        private List<Group> members;
        public Groups() { this.members = new List<Group>(); }
        ~Groups() { }
        public int Count{ get { return this.members.Count; } }
        public Group this[int index]
        {
            set
            {
                if (index < this.members.Count && index >= 0)
                { this.members[index] = value; }
            }
            get
            {
                if (index < this.members.Count && index >= 0)
                    return this.members[index];
                else { return null; }
            }
        }

        public bool addMember(Group G)
        {
            if (G != null)
            {
                if (findbyName(G.Name) == null)
                {
                    this.members.Add(G);
                    return true;
                }
                else { return false; }
                
            }
            else { return false; }
        }


        public Group findbyName(string Name)
        {
            Group res = null;
            for (int i = 0; i < this.members.Count; i++)
            {
                if (this.members[i].Name == Name)
                {
                    res = this.members[i];
                    break;
                }
            }
            return res;
        }

        public bool deleteMember(string Name)
        {
            Group G = findbyName(Name);
            if (G != null)
            { 
                G.deleteAllMembers();
                return this.members.Remove(G); }
            else { return false; }
        }

        public List<string> getGroups()
        {
            List<string> Res = new List<string>();
            for (int i = 0; i < members.Count; i++)
            {
                Res.Add(members[i].Name);
            }
            return Res;
        }
    }

}



