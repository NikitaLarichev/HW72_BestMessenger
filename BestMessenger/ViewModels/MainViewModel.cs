using BestMessenger.Infrastructure.Commands;
using BestMessenger.Infrastructure.Services;
using BestMessenger.Models;
using BestMessenger.ModelShells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BestMessenger.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Commands
        public ActionCommand CloseAppCommand => new ActionCommand(x => Application.Current.Shutdown());
        public ActionCommand WindowMinimizeCommand => new ActionCommand(x => Application.Current.MainWindow.WindowState = WindowState.Minimized);
        public ActionCommand WindowMaximizeCommand => new ActionCommand(x => MaximizeCommand());
        public ActionCommand SendMessageCommand => new ActionCommand(x => MessageBox.Show("test"));
        #endregion

        #region Property
        private User mainUser;//дальше никаких маниполуций с mainUser я не писал, но он точно нужен
        private UserShellForAuthorization authUser;
        public UserShellForAuthorization AuthUser
        {
            private get { return authUser; }
            set
            {
                authUser = value;
                MainUserSet().GetAwaiter();
            }
        }

        private event Func<User,Task> getLastMessageEvent;

        private string newText;

        public string NewText
        {
            get { return newText; }
            set 
            { 
                newText = value; 
                OnPropertyChanged();
            }
        }

        private ObservableCollection<User> members;

        public ObservableCollection<User> Members
        {
            get { return members; }
            set 
            { 
                members = value;
                OnPropertyChanged();
            }
        }

        private User selectedMember;

        public  User SelectedMember
        {
            get { return selectedMember; }
            set 
            {
                
                selectedMember = value;

                getLastMessageEvent(selectedMember);

                OnPropertyChanged();
            }
        }

        private ObservableCollection<Message> messagesWithSelectedUser;

        public ObservableCollection<Message> MessagesWithSelectedUser
        {
            get { return messagesWithSelectedUser; }
            set 
            { 
                messagesWithSelectedUser = value;
                OnPropertyChanged();
            }
        }


        private readonly UserService _userService;

        #endregion



        public  MainViewModel()
        {
            _userService = new UserService();
            
            LoadInfo().GetAwaiter();
            
            getLastMessageEvent += GetLastMessage;
        }


        private async Task GetLastMessage(User user)
        {                                                                   //mainUser.Id
            var res =  (await _userService.GetMessageWithUser(await _userService.GetUser(5), user));
            
            MessagesWithSelectedUser = res;

            user.LastMessage = res.LastOrDefault()?.Text;
        }

        private async Task LoadInfo()
        {

            var user1 = new User
            {
                FirstName = "user1"
            };
            var user2 = new User
            {
                FirstName = "user2"
            };
            var user3 = new User
            {
                FirstName = "user3"
            };
            var user4 = new User
            {
                FirstName = "user4"
            };
            var user5 = new User
            {
                FirstName = "user5"
            };
            //await _userService.AddNewUser(user1);
            //await _userService.AddNewUser(user2);
            //await _userService.AddNewUser(user3);
            //await _userService.AddNewUser(user4);
            //await _userService.AddNewUser(user5);

            //await _userService.AddNewMember(await _userService.GetUser(8), await _userService.GetUser(9));
            //ObservableCollection<User> tempMembers = await _userService.GetAllMembersAtGroup(mainUser, 4);
           
            //foreach (var item in tempMembers)
            //{
            //   //item.Id != mainUser.Id
            //    await GetLastMessage(item);

            //}
            //Members = tempMembers;
            #region
            //_userService.AddPhoto();

            //await _userService.SendMessage(new Message
            //{
            //    Sender = await _userService.GetUser(5),
            //    Receiver = await _userService.GetUser(6),
            //    DateOfSend = DateTime.Now,
            //    Text = "Test 6"
            //});
            //string res = "";
            //foreach (var item in await _userService.GetMessageWithUser(await _userService.GetUser(5), await _userService.GetUser(6)))
            //{
            //    res += item.Text + Environment.NewLine;
            //}
            //MessageBox.Show(res);
            //await _userService.AddNewUser(user3);
            ////await _userService.AddNewUser(user2);
            //await _userService.AddNewMember(await _userService.GetUser(5), user3);

            //string res = "";
            //foreach (var item in await _userService.GetAllMembersAtGroup(null, 3))
            //{
            //    res += item.FirstName + Environment.NewLine;
            //}
            //MessageBox.Show(res);
            #endregion
        }
        private async Task MainUserSet()
        {
            mainUser = await _userService.GetUser(authUser.Id);
            ObservableCollection<User> tempMembers = await _userService.GetAllMembersAtGroup(mainUser, 4);

            foreach (var item in tempMembers)
            {
                //item.Id != mainUser.Id
                await GetLastMessage(item);

            }
            Members = tempMembers;
        }

        private void MaximizeCommand()
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Normal)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }

        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
