using BestMessenger.Infrastructure.Commands;
using BestMessenger.Infrastructure.Services;
using BestMessenger.ModelShells;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace BestMessenger.ViewModels
{
    public class AuthorizationViewModel : BaseViewModel
    {

        private UserService _userService;
        private UserShellForAuthorization mainUser;
        public UserShellForAuthorization MainUser
        { 
            get { return mainUser; }
            set { UpdateValue<UserShellForAuthorization>(ref mainUser, value); }
        }

        private UserShellForAuthorization userForAuth;
        public UserShellForAuthorization UserForAuth
        {
            get => userForAuth;
            set => UpdateValue<UserShellForAuthorization>(ref userForAuth, value);
        }
        private string email;
        public string Email
        {
            get => email;
            set 
            { 
                UpdateValue(ref email, value);
                canEnter = email == null || email == "" ? false : true;
                WrongEmail = false;
            }
        }
        private string password;
        public string Password
        {
            get => password;
            set 
            {               
                UpdateValue(ref password, value);
                canEnter = password == null || password == "" ? false : true;
                WrongPassword = false;
            } 
        }
        private bool canEnter = false;
        public bool CanEnter { get => canEnter; set => UpdateValue(ref canEnter, value); }
        private bool wrongEmail;
        public bool WrongEmail { get => wrongEmail; set => UpdateValue(ref wrongEmail, value); }
        private bool wrongPassword;
        public bool WrongPassword { get => wrongPassword; set => UpdateValue(ref wrongPassword, value); }
        public AuthorizationViewModel()
        {
            _userService = new UserService();
        }
        public async Task CreateUserAndFindItInDB()
        {
            UserShellForAuthorization authUser = null;
            if(await _userService.UserForAuthorizationIsExist(new UserShellForAuthorization() { Email = this.Email, Password = this.Password }) == false)
            {
                WrongEmail = true;        
                return; 
            }
            else
            {
                WrongEmail = false;
            }
            authUser = await _userService.GetUserForAuthorization(new UserShellForAuthorization() { Email = this.Email, Password = this.Password });
            if(authUser == null)
            {
                WrongPassword = true;
                return;
            }
            else
            {
                WrongPassword = false;
            }
            MainUser = authUser;
        }     
    }
}
