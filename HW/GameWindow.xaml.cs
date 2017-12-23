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
                    CreateGameField(JsonConvert.DeserializeObject<GameField>(senderWTask.Description.Remove(0, 5)));
                    break;
                case "ycome":
                    ComeInGame(JsonConvert.DeserializeObject<GameField>(senderWTask.Description.Remove(0, 5)));
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

            }
        }


        private void CreateGameField(GameField gameField)
        {
            Dispatcher.Invoke(
                 () => { 
                 new GameFieldWindow(_mainUser, _tm, gameField).Show();
             });
        }


        private void TakePartInGameButton_Click(object sender, RoutedEventArgs e)
        {
            var game = Games.SelectedItems[0];
            _tm.Send("CURSserver", "icome"+game);
        }

        private void ComeInGame(GameField gameField)
        {
            Dispatcher.Invoke(
                () => {
                    new GameFieldWindow(_mainUser, _tm, gameField).Show();
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
