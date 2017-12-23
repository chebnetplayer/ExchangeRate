using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using HW;
using HWLibrary.Domain;
using Newtonsoft.Json;

namespace GameClient
{
    public partial class GameWindow : Window
    {

        public GameWindow(User mainUser, TaskManager.TaskManager tm, List<String> createdGames)
        {
            _mainUser = mainUser;
            _tm = tm;
            _createdGames = new ObservableCollection<string>(createdGames);
            _tm.Accept = null;
            _tm.Accept += Processing;
            InitializeComponent();
            Title = $"Крестики-нолики({_mainUser.Name})";
            Games.ItemsSource = _createdGames;
        }

        private readonly User _mainUser;
        private readonly TaskManager.TaskManager _tm;
        private readonly ObservableCollection<String> _createdGames;
        private GameFieldWindow _gameFieldWindow;
        

        private void CreateGameButton_Click(object sender, RoutedEventArgs e)
        {
            _tm.Send("CURSserver", "crgme");
        }

        private void Processing(object sender, EventArgs e)
        {
            var senderWTask = (TaskManager.WTask)sender;
            switch (senderWTask.Description.Substring(0, 5))
            {
                case "crgme":
                    CreateGameField(senderWTask.Description.Remove(0, 5));
                    break;
                case "ycome":
                    ComeInGame(senderWTask.Description.Remove(0, 5));
                    break;
                case "ncome":
                    MessageBox.Show("Ошибка! Не удалось войти в игру");
                    break;
                case "fails":
                    MessageBox.Show("Ошибка сервера");
                    break;
                case "newgm":
                    Dispatcher.Invoke(
                        () => {
                            _createdGames.Add(senderWTask.Description.Remove(0, 5));
                        });
                    break;
                case "delgm":
                    Dispatcher.Invoke(
                        () => {
                            _createdGames.Remove(senderWTask.Description.Remove(0, 5));
                        });
                    break;
                case "Gcome":
                    _gameFieldWindow.GamerCome(senderWTask.Description.Remove(0, 5));
                    break;
                case "meout":
                    MessageBox.Show("Противник вышел");
                    Dispatcher.Invoke(_gameFieldWindow.Close);
                    UnLockButtons();
                    break;
                case "gstep":
                    _gameFieldWindow.GetMotion(senderWTask.Description.Remove(0, 5));
                    break;
                case "Xwinr":
                    Dispatcher.Invoke(_gameFieldWindow.Close);
                    MessageBox.Show("Крестики победили!!!");
                    UnLockButtons();
                    break;
                case "Owinr":
                    Dispatcher.Invoke(_gameFieldWindow.Close);
                    MessageBox.Show("Нолики победили!!!");
                    UnLockButtons();
                    break;
                case "ystep":
                    _gameFieldWindow.UnlockAllfields();
                    break;
                case "drow1":
                    Dispatcher.Invoke(_gameFieldWindow.Close);
                    MessageBox.Show("Ничья!!!!");
                    break;
            }
        }


        private void CreateGameField(string json)
        {
            var gameField = JsonConvert.DeserializeObject<GameField>(json);
            Dispatcher.Invoke(
                 () => { 
                     _gameFieldWindow = new GameFieldWindow(_mainUser, gameField,_tm);
                     LockButtons();
                     _gameFieldWindow.Show();                     
                 });
        }

        private void LockButtons()
        {
            Dispatcher.Invoke(() => {
                CreateGameButton.IsEnabled = false;
                TakePartInGameButton.IsEnabled = false;
            });       
        }

        private void UnLockButtons()
        {
            Dispatcher.Invoke(() =>
            {
                CreateGameButton.IsEnabled = true;
                TakePartInGameButton.IsEnabled = true;
            });
        }
        private void TakePartInGameButton_Click(object sender, RoutedEventArgs e)
        {
            var game = Games.SelectedItems[0];
            _tm.Send("CURSserver", "icome"+game);
        }

        private void ComeInGame(string json)
        {
            var gameField = JsonConvert.DeserializeObject<GameField>(json);
            Dispatcher.Invoke(
                () => {
                    _gameFieldWindow = new GameFieldWindow(_mainUser, gameField, _tm);
                    LockButtons();
                    _gameFieldWindow.Show();
                });
        }

        private void Games_SelectionChanged_1(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
