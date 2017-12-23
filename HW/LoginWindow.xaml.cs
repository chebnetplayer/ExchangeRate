using System;
using System.Collections.Generic;
using System.Windows;
using HWLibrary.Domain;
using Newtonsoft.Json;


namespace GameClient
{

    public partial class MainWindow : Window
    {
        private TaskManager.TaskManager _tm;

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private string GetNickname()
        {
            return NicknameTextBox.Text;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            NicknameTextBox.Text = GetNickname();
            if (_tm == null)
            {
                _tm = new TaskManager.TaskManager("settings1.txt", GetNickname()) {Enabled = true};
                _tm.Accept += Processing;
            }
            _tm.Send("CURSserver", "login" + GetNickname());            
        }

        private void Processing(object sender, EventArgs e)
        {
            var senderWTask = (TaskManager.WTask) sender;
            Dispatcher.Invoke(() =>
            {
                if (senderWTask.Description.Remove(9) == "logintrue")
                {
                    var games = JsonConvert.DeserializeObject<List<string>>(senderWTask.Description.Remove(0, 9));
                    new GameWindow(new User(GetNickname()),_tm, games).Show();
                    Hide();
                }
                else NicknameTextBox.Text = "Введите другое имя";
            });
        }
    }
}
