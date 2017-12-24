using System.Collections.Generic;
using System.Linq;
using HWLibrary.Domain;
using HWLibrary.Infrastucture;
using Newtonsoft.Json;

namespace ServerWinForm.Repository
{
    public class Users
    {
        private static readonly UserRepository UserRepository=new UserRepository();

        public static bool CheckLogin(TaskManager.WTask sender)
        {        
           return UserRepository.LoadUser(sender.Description.Substring(5));
        }

        public static string ReturnAnswerToUser(bool checker)
        {
            return checker ? "logintrue"+GetCreatedGamesJson(Games.GetCreatedGames()) : "loginfalse";
        }

        public static void DeleteGame(string gameName,TaskManager.TaskManager _tm)
        {
            //REVIEW: NRE?
            foreach (var user in UserRepository.Users)
            {
                _tm.Send(user.Name, "delgm" + gameName);
            }
        }

        public static void HostCreateGame(string gameName, TaskManager.TaskManager _tm)
        {
            //REVIEW: NRE?
            foreach (var user in UserRepository.Users)
            {
                _tm.Send(user.Name, "newgm" + gameName);
            }
        }

        private static string GetCreatedGamesJson(IEnumerable<GameField> gamesList)
        {
            //REVIEW: NRE?
            var gameshost = gamesList.Select(i => i.Host.Name).ToList();
            //REVIEW: NRE?
            return JsonConvert.SerializeObject(gameshost);
        }


    }
}
