using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using HWLibrary.Domain;
using Newtonsoft.Json;
using TaskManager;

namespace HW
{
    public partial class GameFieldWindow : Window
    {
        public GameFieldWindow(User mainUser, GameField gameField, TaskManager.TaskManager tm)
        {
            InitializeComponent();
            Title = $"Крестики-нолики({mainUser.Name})";
            _buttons = new List<Button>();
            GetLogicalChildCollection(this, _buttons);
            LockAllfields();
            _mainUser = mainUser;
            _gameField = gameField;
            _tm = tm;
        }

        private GameField _gameField;
        private readonly User _mainUser;
        private readonly List<Button> _buttons;
        private readonly TaskManager.TaskManager _tm;

        private void ChangeField(int fieldnumber, string motionMaker)
        {
            Dispatcher.Invoke(() => {
                var img = new Image();
                switch (_gameField.IsHostX)
                {
                    case false when motionMaker == _gameField.Host.Name:
                        img.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Images\O.png"));
                        _gameField.MakeMotion(fieldnumber, TypeofCell.O);
                        break;
                    case false when motionMaker != _gameField.Host.Name:
                        img.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Images\X.png"));
                        _gameField.MakeMotion(fieldnumber, TypeofCell.X);
                        break;
                    case true when motionMaker == _gameField.Host.Name:
                        img.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Images\X.png"));
                        _gameField.MakeMotion(fieldnumber, TypeofCell.X);
                        break;
                    case true when _mainUser.Name != _gameField.Host.Name:
                        img.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\Images\O.png"));
                        _gameField.MakeMotion(fieldnumber, TypeofCell.O);
                        break;
                }
                var stackPnl = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(10)

                };
                stackPnl.Children.Add(img);
                _buttons.First(i=>i.Name==$"Field{fieldnumber+1}").Content = stackPnl;
                LockAllfields();
                Dispatcher.Invoke(() => {
                    _buttons.RemoveAll(i => i.Name == $"Field{fieldnumber + 1}");                       
                });
                if (motionMaker != _mainUser.Name)
                {
                    UnlockAllfields();
                    return;
                }
                var motion = new Motion(_gameField.GameId, fieldnumber, _mainUser.Name);
               _tm.Send("CURSserver", "gstep" + JsonConvert.SerializeObject(motion));
            });        
        }
    
        internal void LockAllfields()
        {
            foreach (var button in _buttons)
            {
                Dispatcher.Invoke(() => { button.IsEnabled = false; });
            }
        }
        internal void UnlockAllfields()
        {
            foreach (var button in _buttons)
            {             
                Dispatcher.Invoke(() => { button.IsEnabled = true; });
            }
        }
        internal void GetMotion(string json)
        {
            var motion = JsonConvert.DeserializeObject<Motion>(json);
            if (motion.GameId==_gameField.GameId)
                ChangeField(motion.CellId, motion.MotionMaker);
        }

        internal void GamerCome(string json)
        {
            _gameField = JsonConvert.DeserializeObject<GameField>(json);
        }

        private void Field1_Click(object sender, RoutedEventArgs e)
        {
            ChangeField(0, _mainUser.Name);
            LockAllfields();;
        }

        private void Field2_Click(object sender, RoutedEventArgs e)
        {
            ChangeField(1,_mainUser.Name);
            LockAllfields();
        }


        private void Field3_Click(object sender, RoutedEventArgs e)
        {
            ChangeField(2,_mainUser.Name);
            LockAllfields();
        }

        private void Field4_Click(object sender, RoutedEventArgs e)
        {
            ChangeField(3, _mainUser.Name);
            LockAllfields();
        }

        private void Field5_Click(object sender, RoutedEventArgs e)
        {
            ChangeField(4, _mainUser.Name);
            LockAllfields();
        }

        private void Field6_Click(object sender, RoutedEventArgs e)
        {
            ChangeField(5, _mainUser.Name);
            LockAllfields();
            Dispatcher.Invoke(() => {
                _buttons.RemoveAll(i => i.Name == "Field6");
            });
        }

        private void Field7_Click(object sender, RoutedEventArgs e)
        {
            ChangeField(6, _mainUser.Name);
            LockAllfields();
        }

        private void Field8_Click(object sender, RoutedEventArgs e)
        {
            ChangeField(7, _mainUser.Name);
            LockAllfields();
        }

        private void Field9_Click(object sender, RoutedEventArgs e)
        {
            ChangeField(8, _mainUser.Name);
            LockAllfields();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
          _tm.Send("CURSserver", "meout" + _gameField.GameId);       
        }

        private static void GetLogicalChildCollection<T>(DependencyObject parent, ICollection<T> logicalCollection) where T : DependencyObject
        {
            var children = LogicalTreeHelper.GetChildren(parent);
            foreach (var child in children)
            {
                if (!(child is DependencyObject)) continue;
                var depChild = child as DependencyObject;
                if (child is T)
                {
                    logicalCollection.Add(child as T);
                }
                GetLogicalChildCollection(depChild, logicalCollection);
            }
        }
    }
}
