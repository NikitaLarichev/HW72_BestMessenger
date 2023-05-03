using BestMessenger.Data;
using BestMessenger.Models;
using BestMessenger.ModelShells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BestMessenger.Infrastructure.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService()
        {
            _context = new AppDbContext();
        }


        public void AddPhoto()
        {
            foreach (var item in _context.Users)
            {
                item.PhotoSource = @"C:\Users\feday\Pictures\2.jpg";
            }
            _context.SaveChanges();
        }

        public async Task<bool> AddNewUser(User user)
        {
            int res = 0;
            try
            {
                user.Groups = new ObservableCollection<Group>();
                user.Groups.Add(new Group() { Name = "First" });
                _context.Users.Add(user);
                res = await _context.SaveChangesAsync();
            }
            catch (EntitySqlException ex)
            {

                throw new Exception("Exception with add user in DB");
            }
            catch
            {
                throw;
            }
            return res > 0;
        }

        public async Task<User> GetUser(int id)
        {
            try
            {
                return await _context.Users.Include("Groups").Include("Messages").FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> AddNewMember(User mainUser, User member)//GetUser(1), GetUser(2)
        {
            try
            {
                mainUser.Groups.FirstOrDefault().Members.Add(member);
                int res = await _context.SaveChangesAsync();

                return res > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> AddNewMember(User mainUser, int memberId)//GetUser(1), GetUser(2)
        {
            try
            {
                var member = await GetUser(memberId);
                mainUser.Groups.FirstOrDefault(g => g.Id == 4).Members.Add(member);
                int res = await _context.SaveChangesAsync();

                return res > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ObservableCollection<User>> GetAllMembersAtGroup(User mainUser, int groupId = 0)//GetUser(1)
        {
            if (groupId == 0)
            {
                groupId = mainUser.Groups.FirstOrDefault().Id;
            }
            IQueryable<User> result = null;

            await Task.Run(() =>
            {
                try
                {
                    result = _context.Users.Include("Messages").Include("Groups")
                    .Where(u => u.Id != mainUser.Id && u.Groups.Any(g => g.Id == groupId)).Select(s => s);
                }
                catch (Exception)
                {

                    throw;
                }
            });

            return result == null ? new ObservableCollection<User>() : new ObservableCollection<User>(result);

        }

        public async Task<bool> SendMessage(Message message)
        {
            _context.Messages.Add(message);
            var res = await _context.SaveChangesAsync();
            return res > 0;
        }
        public async Task<ObservableCollection<Message>> GetMessageWithUser(User mainUser, User member)
        {
            var res = await _context.Messages.ToListAsync();
            var temp = res.Where(m => m.Sender.Id == mainUser.Id && m.Receiver.Id == member.Id
            || m.Sender.Id == member.Id && m.Receiver.Id == mainUser.Id);
            foreach (var item in temp)
            {
                if (item.Sender.Id == mainUser.Id)
                {
                    item.Sender.IsMain = true;
                }
                else if (item.Receiver.Id == mainUser.Id)
                {
                    item.Receiver.IsMain = true;
                }
            }
            return new ObservableCollection<Message>(temp);
        }

        public async Task<ObservableCollection<UserShellForAddFriend>> GetAllUsersForAddFriendList(User mainUser)
        {
            List<UserShellForAddFriend> res = new List<UserShellForAddFriend>();
            var allUsers = await _context.Users.Include("Groups").Include("Messages").ToListAsync();
            allUsers.ForEach(u => res.Add(new UserShellForAddFriend
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Id = u.Id,
                IsFriend = mainUser.Groups.FirstOrDefault().Members.Any(m => m.Id == u.Id),
                PhotoSource = u.PhotoSource,
            }));

            return new ObservableCollection<UserShellForAddFriend>(res);
        }
        public async Task<bool> UserForAuthorizationIsExist(UserShellForAuthorization authUser)
        {
            bool res = await _context.Users.AnyAsync(u => u.Email == authUser.Email);
            return res;
        }
        public async Task<UserShellForAuthorization> GetUserForAuthorization(UserShellForAuthorization authUser)
        {
            UserShellForAuthorization authorizationUser = null;
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == authUser.Email && u.Password == authUser.Password);
            if (user != null)
            {
                authorizationUser = new UserShellForAuthorization()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Password = user.Password
                };
            }
            return authorizationUser;
        }
    }
}
