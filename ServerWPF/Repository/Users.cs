using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
            try
            {
                foreach (var user in UserRepository.Users)
                {
                    _tm.Send(user.Name, "delgm" + gameName);
                }
            }
            catch (Exception e )
            {
                MessageBox.Show(e.Message);
            }
          
        }

        public static void HostCreateGame(string gameName, TaskManager.TaskManager _tm)
        {
            try
            {
                foreach (var user in UserRepository.Users)
                {
                    _tm.Send(user.Name, "newgm" + gameName);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }

        private static string GetCreatedGamesJson(IEnumerable<GameField> gamesList)
        {

            try
            {
                var gameshost = gamesList.Select(i => i.Host.Name).ToList();
                return JsonConvert.SerializeObject(gameshost);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return "fails";
            }

        }


    }
}
