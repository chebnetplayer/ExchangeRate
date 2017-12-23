using System;
using System.Windows;
using HWLibrary.Domain;
using ServerWinForm.Repository;

namespace ServerWinForm
{
    public partial class MainWindow : Window
    {
        private readonly TaskManager.TaskManager _tm;
        public MainWindow()
        {
            InitializeComponent();
            _tm = new TaskManager.TaskManager("settings1.txt", "CURSserver") {Enabled = true};
            _tm.Accept += Processing;  
        }

        private void Processing(object sender, EventArgs e)
        {
            var senderWTask = (TaskManager.WTask) sender;
            switch (senderWTask.Description.Substring(0,5))
            {
                case "login":
                   _tm.Send(senderWTask.From,Users.ReturnAnswerToUser(Users.CheckLogin(senderWTask)));
                   break;
                case "crgme":
                    _tm.Send(senderWTask.From, Games.CreateGame(new User(senderWTask.From), _tm));
                    break;
                case "icome":
                    Games.ComeGamerinGame(new User(senderWTask.From), new User(senderWTask.Description.Remove(0, 5)),_tm);
                    break;
                case "meout":
                    Games.OutFromGame(Guid.Parse(senderWTask.Description.Remove(0, 5)), _tm, senderWTask.From);
                    break;
                case "gstep":
                    Games.MakeMotion(senderWTask.Description.Remove(0, 5),_tm);
                    break;            }         
        }
    }
}
