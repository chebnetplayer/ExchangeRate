using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CG.Web.MegaApiClient;
using HWLibrary.Domain;
using Newtonsoft.Json;

namespace ServerWinForm.Repository
{
    internal class Games
    {
        private static readonly List<GameField> StartedGameFields = new List<GameField>();
        private static readonly List<GameField> CreatedGameFields = new List<GameField>();

        public static List<GameField> GetCreatedGames()
        {
            return CreatedGameFields;
        }

        public static string CreateGame(User host, TaskManager.TaskManager _tm)
        {
            var cells = new Cell[9];
            for (int i = 0; i < 9; i++)
                cells[i] = new Cell(i);
            var isHostX = new Random().Next(100) < 49;
            var newgame = new GameField(host, Guid.NewGuid(), cells, isHostX);
            //REVIEW: А если эти Fiels==null?
            CreatedGameFields.Add(newgame);
            Users.HostCreateGame(newgame.Name, _tm);
            //REVIEW: NRE
            return "crgme" + JsonConvert.SerializeObject(newgame);
        }

        public static void OutFromGame(Guid gameId, TaskManager.TaskManager _tm, string maker)
        {
            try
            {
                
                var gamefield = CreatedGameFields.FirstOrDefault(i => i.GameId == gameId);
                if (gamefield != null)
                {
                    Users.DeleteGame(gamefield.Name, _tm);
                    return;
                }
                gamefield = StartedGameFields.First(i => i.GameId == gameId);
                if (maker == gamefield.Gamer.Name)
                {
                    GamerOutFromGame(gameId, _tm);
                    StartedGameFields.Remove(gamefield);
                    return;
                }
                _tm.Send(gamefield.Gamer.Name, "meout");
                
                StartedGameFields.Remove(gamefield);
            }
            catch
            {
                //REVIEW: Т.е., проглотили исключение, проигнорили его и дальше пошли? А смысл?
                //ignored
            }
        }

        public static void GamerOutFromGame(Guid gameId, TaskManager.TaskManager _tm)
        {
            var gamefield = StartedGameFields.First(i => i.GameId == gameId);
            //REVIEW: NRE?
            StartedGameFields.Remove(gamefield);
            _tm.Send(gamefield.Host.Name, "meout");
        }

        public static void ComeGamerinGame(User gamer, User host, TaskManager.TaskManager tm)
        {
            try
            {
                var gamefield = CreatedGameFields.First(i => i.Host.Name == host.Name);
                gamefield.ComeGamerInGame(gamer);
                StartedGameFields.Add(gamefield);
                CreatedGameFields.Remove(gamefield);
                Users.DeleteGame(gamefield.Name, tm);
                var json = JsonConvert.SerializeObject(gamefield);
                tm.Send(gamefield.Host.Name, "Gcome" + json);
                tm.Send(gamer.Name, "ycome" + json);
                if (gamefield.IsHostX) tm.Send(host.Name, "ystep");
                if (!gamefield.IsHostX) tm.Send(gamer.Name, "ystep");
            }
            catch
            {
                tm.Send(gamer.Name, "ncome");
            }

        }

        public static void MakeMotion(string json, TaskManager.TaskManager tm)
        {
            try
            {
                var motion = JsonConvert.DeserializeObject<Motion>(json);
                var game = StartedGameFields.First(i => i.GameId == motion.GameId);
                if (game.IsHostX && game.Host.Name == motion.MotionMaker)
                    game.MakeMotion(motion.CellId, TypeofCell.X);
                if (game.IsHostX == false && game.Gamer.Name == motion.MotionMaker)
                    game.MakeMotion(motion.CellId, TypeofCell.X);
                else game.MakeMotion(motion.CellId, TypeofCell.O);
                if (game.CheckOnFinishing())
                {
                    CheckGameOnFinish(game, tm);
                    return;
                }
                if (game.Cells.FirstOrDefault(i => i.TypeofCell == TypeofCell.Null) == null)
                {
                    tm.Send(game.Gamer.Name, "drow1");
                    tm.Send(game.Host.Name, "drow1");
                }
                if (motion.MotionMaker == game.Host.Name)
                {
                    tm.Send(game.Gamer.Name, "gstep" + json);
                    tm.Send(game.Gamer.Name, "ystep");
                }
                if (motion.MotionMaker != game.Gamer.Name) return;
                tm.Send(game.Host.Name, "gstep" + json);
                tm.Send(game.Host.Name, "ystep");
            }
            catch
            {
                var motion = JsonConvert.DeserializeObject<Motion>(json);
                tm.Send(motion.MotionMaker, "fails");
            }
        }

        public static void CheckGameOnFinish(GameField game, TaskManager.TaskManager tm)
        {
            try
            {
                FinishGame(game);
                if (game.IsXwins)
                {
                    tm.Send(game.Host.Name, "Xwinr");
                    tm.Send(game.Gamer.Name, "Xwinr");
                }
                if (!game.IsOwins) return;
                tm.Send(game.Host.Name, "Owinr");
                tm.Send(game.Gamer.Name, "Owinr");
            }
            catch
            {//ignored
                //REVIEW: Опять глотаем исключения?
            }
        }

        private static void FinishGame(GameField gamefield)
        {
            try
            {
                var game = new Game(gamefield.IsXwins, gamefield.GameId, gamefield.Host.Name,
                    gamefield.Gamer.Name);
                //REVIEW: А почему не использовать DataContractJsonSerializer?
                //REVIEW: И название файла - в константы или в настройки
                File.AppendAllText("gamesrep.json", JsonConvert.SerializeObject(game));
                DownloadFile();
                StartedGameFields.Remove(gamefield);
            }
            catch
            {
                //REVIEW: Опять игнорим исключения
                // ignored
            }
        }

        public static void DownloadFile()
        {
            var client = new MegaApiClient();

            //REVIEW: Нарываемся на NRE в нескольких местах сразу
            //REVIEW: И до кучи - почта и какие-то буковки, название файла - в константы или настройки
            client.Login("chebnetplayer@yandex.ru", "340119Ff");
            var nodes = client.GetNodes();
            var enumerable = nodes as INode[] ?? nodes.ToArray();
            var folders = enumerable.Where(n => n.Type == NodeType.Directory).ToList();
            var folder = folders.First(f => f.Name == "Upload");
            var myFile = client.UploadFile("gamesrep.json", folder);
        }
    }
}
