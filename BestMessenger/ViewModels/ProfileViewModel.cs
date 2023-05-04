using BestMessenger.Infrastructure.Commands;
using BestMessenger.Infrastructure.Services;
using BestMessenger.Models;
using BestMessenger.ModelShells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestMessenger.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        int userId;
        public int UserId
        { 
            get => userId;
            set 
            {
                userId = value;
                GetMainUser(userId).GetAwaiter();               
            }
        }
        User mainUser;
        UserService _userService;

        private string firstName;
        public string FirstName 
        {
            get => firstName;
            set => UpdateValue<string>(ref firstName, value);
        } 
        private string lastName;
        public string LastName 
        {
            get => lastName;
            set => UpdateValue<string>(ref lastName, value);
        }
        public ProfileViewModel() 
        {
            _userService = new UserService();
        }
        public ActionCommand SaveChangesCommand => new ActionCommand(async x => await SaveChanges());
        private async Task GetMainUser(int userId)
        {
            mainUser = await _userService.GetUser(userId);
            FirstName = mainUser.FirstName == null ? "" : mainUser.FirstName;
            LastName = mainUser.LastName == null ? "" : mainUser.LastName;
        }
        private async Task SaveChanges()
        {
            mainUser.FirstName = FirstName;
            mainUser.LastName = LastName;
            await _userService.ChangeUserAsync(mainUser);
        }
        
    }
}
