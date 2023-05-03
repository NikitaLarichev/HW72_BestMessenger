using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestMessenger.ModelShells
{
    public class UserShellForAddFriend
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string PhotoSource { get; set; }
        public string LastName { get; set; }
        public bool IsFriend { get; set; }
        
    }
}
