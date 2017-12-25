using System;
using System.Windows;
using HWLibrary.Domain;
using ServerWinForm.Repository;
using ServerWPF;

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
            switch (Enum.Parse(typeof(Commands),senderWTask.Description.Substring(0,5)))
            {
               case Commands.login:
                   _tm.Send(senderWTask.From,Users.ReturnAnswerToUser(Users.CheckLogin(senderWTask)));
                   break;
                case Commands.crgme:
                    _tm.Send(senderWTask.From, Games.CreateGame(new User(senderWTask.From), _tm));
                    break;
                case Commands.icome:
                    Games.ComeGamerinGame(new User(senderWTask.From), new User(senderWTask.Description.Remove(0, 5)),_tm);
                    break;
                case Commands.meout:
                    Games.OutFromGame(Guid.Parse(senderWTask.Description.Remove(0, 5)), _tm, senderWTask.From);
                    break;
                case Commands.gstep:
                    Games.MakeMotion(senderWTask.Description.Remove(0, 5),_tm);
                    break;
                    
            }         
        }
    }
}
