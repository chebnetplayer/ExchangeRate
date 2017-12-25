using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
            Games.ItemsSource = _createdGames;
            DataContext = this;
        }

        private readonly User _mainUser;
        private readonly TaskManager.TaskManager _tm;
        private readonly ObservableCollection<String> _createdGames;
        private GameFieldWindow _gameFieldWindow;
        public bool TakePartInGameButtonIsEnabled { get; set; }

        private void CreateGameButton_Click(object sender, RoutedEventArgs e)
        {
            _tm.Send("CURSserver", "crgme");
        }

        public new static DependencyProperty IsEnabledProperty = DependencyProperty.Register(
            "IsEnabled",
            typeof(bool),
            typeof(Button),
            new System.Windows.PropertyMetadata(PropertyChanged));

        public bool IsEnabledBool
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        private static void PropertyChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            var control = depObj as Button;
            if (e.Property != IsEnabledProperty) return;
            if (control != null) control.IsEnabled = (bool)e.NewValue;
        }

        private void Processing(object sender, EventArgs e)
        {
            var senderWTask = (TaskManager.WTask) sender;
            switch (Enum.Parse(typeof(Commands), senderWTask.Description.Substring(0, 5)))
            {
                case Commands.crgme:
                    CreateGameField(senderWTask.Description.Remove(0, 5));
                    break;
                case Commands.ycome:
                    ComeInGame(senderWTask.Description.Remove(0, 5));
                    break;
                case Commands.ncome:
                    MessageBox.Show("Ошибка! Не удалось войти в игру");
                    break;
                case Commands.fails:
                    MessageBox.Show("Ошибка сервера");
                    break;
                case Commands.newgm:
                    Dispatcher.Invoke(
                        () =>
                        {
                            _createdGames.Add(senderWTask.Description.Remove(0, 5));
                        });
                    break;
                case Commands.delgm:
                    Dispatcher.Invoke(
                        () =>
                        {
                            _createdGames.Remove(senderWTask.Description.Remove(0, 5));
                        });
                    break;
                case Commands.Gcome:
                    _gameFieldWindow.GamerCome(senderWTask.Description.Remove(0, 5));
                    break;
                case Commands.meout:
                    MessageBox.Show("Противник вышел");
                    Dispatcher.Invoke(_gameFieldWindow.Close);
                    UnLockButtons(default, new EventArgs());
                    break;
                case Commands.gstep:
                    _gameFieldWindow.GetMotion(senderWTask.Description.Remove(0, 5));
                    break;
                case Commands.Xwinr:
                    Dispatcher.Invoke(_gameFieldWindow.Close);
                    MessageBox.Show("Крестики победили!!!");
                    UnLockButtons(default, new EventArgs());
                    break;
                case Commands.Owinr:
                    Dispatcher.Invoke(_gameFieldWindow.Close);
                    MessageBox.Show("Нолики победили!!!");
                    UnLockButtons(default, new EventArgs());
                    break;
                case Commands.ystep:
                    _gameFieldWindow.UnlockAllfields();
                    break;
                case Commands.drow1:
                    Dispatcher.Invoke(_gameFieldWindow.Close);
                    MessageBox.Show("Ничья!!!!");
                    UnLockButtons(default, new EventArgs());
                    break;
            }
        }

        
        private void CreateGameField(string json)
        {
            var gameField = JsonConvert.DeserializeObject<GameField>(json);
            Dispatcher.Invoke(
                () =>
                {
                    _gameFieldWindow = new GameFieldWindow(_mainUser, gameField, _tm);
                    LockButtons();
                    _gameFieldWindow.Show();
                    _gameFieldWindow.Closed += UnLockButtons;
                });
        }

        private void LockButtons()
        {
            Dispatcher.Invoke(() =>
            {
                CreateGameButton.IsEnabled = false;
                TakePartInGameButton.IsEnabled = false;
            });
        }

        private void UnLockButtons(object o, EventArgs e)
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
            _tm.Send("CURSserver", "icome" + game);
        }

        private void ComeInGame(string json)
        {
            var gameField = JsonConvert.DeserializeObject<GameField>(json);
            Dispatcher.Invoke(
                () =>
                {
                    _gameFieldWindow = new GameFieldWindow(_mainUser, gameField, _tm);
                    LockButtons();
                    _gameFieldWindow.Show();
                    _gameFieldWindow.Closed += UnLockButtons;
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
