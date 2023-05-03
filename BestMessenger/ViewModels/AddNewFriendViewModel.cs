using BestMessenger.Infrastructure.Commands;
using BestMessenger.Infrastructure.Services;
using BestMessenger.Models;
using BestMessenger.ModelShells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BestMessenger.ViewModels
{
    public class AddNewFriendViewModel : BaseViewModel
    {
        #region Properties
        private readonly UserService _userService;

        private ObservableCollection<UserShellForAddFriend> allUsers;
        public ObservableCollection<UserShellForAddFriend> AllUsers 
        {
            get => allUsers;
            set
            {
                UpdateValue<ObservableCollection<UserShellForAddFriend>>(ref allUsers, value);
            }
        }

        private UserShellForAddFriend selectedUser;
        public UserShellForAddFriend SelectedUser
        {
            get => selectedUser;
            set => UpdateValue<UserShellForAddFriend>(ref selectedUser, value);
        }

        private User mainUser;
        #endregion

        #region Commands
        public ActionCommand AddNewMemberCommand => new ActionCommand(x => AddMemberToGroup(), x => CanAddNewFriend());
        #endregion

        public AddNewFriendViewModel()
        {
            _userService = new UserService();
            LoadInfo().GetAwaiter();
          
        }

        private async void AddMemberToGroup()
        {
            if (selectedUser == null) return;
            await _userService.AddNewMember(mainUser, SelectedUser.Id);
        }
        private bool CanAddNewFriend()
        {
            return SelectedUser == null ? true : !SelectedUser.IsFriend;
        }
        private async Task LoadInfo()
        {
            mainUser = await _userService.GetUser(8);
            AllUsers = await _userService.GetAllUsersForAddFriendList(mainUser);
        }

    }
}
