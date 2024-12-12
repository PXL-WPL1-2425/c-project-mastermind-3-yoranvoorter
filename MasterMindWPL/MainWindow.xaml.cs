using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MasterMindWPL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<Color, string> _availableColors = new Dictionary<Color, string>();
        List<string> _code = new List<string>();
        bool _isDebugMode = false;
        int _attempts;
        int _score;
        string _name;
        string[] _highscores = new string[15];
        int _maxAttempts;
        List<string> _playerList = new List<string>();
        string _currentPlayer;
        string _currentCode;

        public MainWindow()
        {
            _score = 100;
            _attempts = 1;
            _maxAttempts = 10;
            InitializeComponent();
            AddColorsToDictionary();
            FillComboBoxes();
            GenerateRandomCode();
            foreach (string color in _code)
            {
                _currentCode = _currentCode + $"{color} ";

                this.Title = this.Title + " " + color;
                TxtCode.Text = TxtCode.Text + $" {color}";
            }
            StartGame();
        }

        public void AddColorsToDictionary()
        {
            _availableColors.Clear();
            _availableColors.Add(Colors.Red, "Red");
            _availableColors.Add(Colors.Blue, "Blue");
            _availableColors.Add(Colors.Green, "Green");
            _availableColors.Add(Colors.Yellow, "Yellow");
            _availableColors.Add(Colors.Orange, "Orange");
            _availableColors.Add(Colors.White, "White");
        }

        public void GenerateRandomCode()
        {
            _code.Clear();
            Random rand = new Random();
            for (int i = 0; i < 4; i++)
            {
                int j = rand.Next(0,5);
                _code.Add(_availableColors.ElementAt(j).Value);
            }
        }

        public void FillComboBoxes()
        {
            foreach (KeyValuePair<Color, string> color in _availableColors)
            {
                cboColors1.Items.Add(color.Value);
                cboColors2.Items.Add(color.Value);
                cboColors3.Items.Add(color.Value);
                cboColors4.Items.Add(color.Value);
            }
        }

        private void cboColors1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Color color = _availableColors.FirstOrDefault(x => x.Value == cboColors1.SelectedItem).Key;
            ellipseColor1.Fill = new SolidColorBrush(color);
        }

        private void cboColors2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Color color = _availableColors.FirstOrDefault(x => x.Value == cboColors2.SelectedItem).Key;
            ellipseColor2.Fill = new SolidColorBrush(color);
        }

        private void cboColors3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Color color = _availableColors.FirstOrDefault(x => x.Value == cboColors3.SelectedItem).Key;
            ellipseColor3.Fill = new SolidColorBrush(color);
        }

        private void cboColors4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Color color = _availableColors.FirstOrDefault(x => x.Value == cboColors4.SelectedItem).Key;
            ellipseColor4.Fill = new SolidColorBrush(color);
        }

        private void btnCheckCode_Click(object sender, RoutedEventArgs e)
        {
            int rightGuesses = 0;
            if (_code[0] == cboColors1.SelectedItem)
            {
                ellipseColor1.Stroke = new SolidColorBrush(Colors.DarkRed);
                rightGuesses++;
            }
            else if (_code.Contains(cboColors1.SelectedItem))
            {
                ellipseColor1.Stroke = new SolidColorBrush(Colors.Wheat);
                _score--;
            }
            else
            {
                ellipseColor1.Stroke = null;
                _score -= 2;
            }

            if (_code[1] == cboColors2.SelectedItem)
            {
                ellipseColor2.Stroke = new SolidColorBrush(Colors.DarkRed);
                rightGuesses++;
            }
            else if (_code.Contains(cboColors2.SelectedItem))
            {
                ellipseColor2.Stroke = new SolidColorBrush(Colors.Wheat);
                _score--;
            }
            else
            {
                ellipseColor2.Stroke = null;
                _score -= 2;
            }

            if (_code[2] == cboColors3.SelectedItem)
            {
                ellipseColor3.Stroke = new SolidColorBrush(Colors.DarkRed);
                rightGuesses++;
            }
            else if (_code.Contains(cboColors3.SelectedItem))
            {
                ellipseColor3.Stroke = new SolidColorBrush(Colors.Wheat);
                _score--;
            }
            else
            {
                ellipseColor3.Stroke = null;
                _score -= 2;
            }

            if (_code[3] == cboColors4.SelectedItem)
            {
                ellipseColor4.Stroke = new SolidColorBrush(Colors.DarkRed);
                rightGuesses++;
            }
            else if (_code.Contains(cboColors4.SelectedItem))
            {
                ellipseColor4.Stroke = new SolidColorBrush(Colors.Wheat);
                _score--;
            }
            else
            {
                ellipseColor4.Stroke = null;
                _score -= 2;
            }
            CheckAttempt(rightGuesses);
            AddToHistory(ellipseColor1, ellipseColor2, ellipseColor3, ellipseColor4);
        }

        public void ToggleDebug()
        {
            _isDebugMode = !_isDebugMode;
            TxtCode.Visibility = _isDebugMode ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TxtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.F12)
            {
                ToggleDebug();
            }
        }

        public void CheckAttempt(int rightGuesses)
        {
            string codeString = "";

            foreach(string color in _code)
            {
                codeString = codeString + $" {color}";
            }
            if (rightGuesses == 4)
            {
                int nextPlayer = _playerList.IndexOf(_currentPlayer) + 1;

                if (nextPlayer >= _playerList.Count)
                {
                    MessageBox.Show($"Gewonnen! In {_attempts} pogingen.\n Alle spelers zijn geweest.", $"{_currentPlayer}", MessageBoxButton.OK, MessageBoxImage.Information);
                    EndGame();
                    RestartGame();
                }
                else
                {
                    MessageBox.Show($"Gewonnen! In {_attempts} pogingen.\n Nu is speler {_playerList[nextPlayer]} aan de beurt.", $"{_currentPlayer}",MessageBoxButton.OK, MessageBoxImage.Information);
                    string[] highscoreDetails = new string[1] { $"{_currentPlayer} - {_attempts} pogingen - {_score}/100" };
                    _highscores.Concat(highscoreDetails);
                    _currentPlayer = _playerList[nextPlayer];
                    RestartGame();
                }

            }

            if (_attempts == _maxAttempts)
            {
                int nextPlayer = _playerList.IndexOf(_currentPlayer) + 1;

                if (nextPlayer >= _playerList.Count)
                {
                    MessageBox.Show($"You failed de correcte code was {_currentCode}\n Alle spelers zijn geweest.", $"{_currentPlayer}", MessageBoxButton.OK, MessageBoxImage.Information);
                    EndGame();
                    RestartGame();
                }
                else
                {
                    MessageBox.Show($"You failed de correcte code was {_currentCode}\n Nu is {_playerList[nextPlayer]} aan de beurt.", $"{_currentPlayer}", MessageBoxButton.OK, MessageBoxImage.Information);
                    _currentPlayer = _playerList[nextPlayer];
                    RestartGame();
                }

            }
            _attempts++;
            TxtPogingen.Text = $"Current player: {_currentPlayer}\nPoging: {_attempts} / {_maxAttempts}\nScore: {_score}";
        }

        public void AddToHistory(Ellipse ellipse1, Ellipse ellipse2, Ellipse ellipse3, Ellipse ellipse4)
        {
            StackPanel ellipsePanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(5)
            };

            Ellipse copyEllipse1 = CreateEllipse(ellipse1.Fill, ellipse1.Stroke);
            Ellipse copyEllipse2 = CreateEllipse(ellipse2.Fill, ellipse2.Stroke);
            Ellipse copyEllipse3 = CreateEllipse(ellipse3.Fill, ellipse3.Stroke);
            Ellipse copyEllipse4 = CreateEllipse(ellipse4.Fill, ellipse4.Stroke);

            ellipsePanel.Children.Add(copyEllipse1);
            ellipsePanel.Children.Add(copyEllipse2);
            ellipsePanel.Children.Add(copyEllipse3);
            ellipsePanel.Children.Add(copyEllipse4);

            ListBoxHistoriek.Items.Add(ellipsePanel);
        }

        //ISSUE: Ellipses get their own tooltip, but it checks the stackpanel tooltip instead which is always the wrongcolortt.
        public Ellipse CreateEllipse(Brush fillColor, Brush strokeColor)
        {
            ToolTip CorrectPositionTT = new ToolTip { Content = "Juiste kleur, juiste positie" };
            ToolTip CorrectColorTT = new ToolTip { Content = "Juiste kleur, foute positie" };
            ToolTip WrongColorTT = new ToolTip { Content = "Foute kleur" };

            Ellipse ellipse = new Ellipse
            {
                Width = 40,
                Height = 40,
                Fill = fillColor,
                Stroke = strokeColor,
                StrokeThickness = 4
            };

            if (strokeColor == new SolidColorBrush(Colors.Wheat))
            {
                ellipse.ToolTip = CorrectColorTT;
            }
            else if (strokeColor == new SolidColorBrush(Colors.DarkRed))
            {
                ellipse.ToolTip = CorrectPositionTT;
            }
            else
            {
                ellipse.ToolTip = WrongColorTT;
            }

            return ellipse;
        }

        public void EndGame()
        {
            _playerList.Clear();
            StartGame();
        }

        public void RestartGame() 
        {
            _attempts = 0;
            _score = 100;
            ListBoxHistoriek.Items.Clear();
            ellipseColor1.Fill = null;
            ellipseColor1.Stroke = null;
            ellipseColor2.Fill = null;
            ellipseColor2.Stroke = null;
            ellipseColor3.Fill = null;
            ellipseColor3.Stroke = null;
            ellipseColor4.Fill = null;
            ellipseColor4.Stroke = null;
            GenerateRandomCode();

            this.Title = "";
            _currentCode = "";

            foreach (string color in _code)
            {
                _currentCode = _currentCode + $"{color} ";

                this.Title = this.Title + " " + color;
                TxtCode.Text = TxtCode.Text + $" {color}";
            }
        }

        public void StartGame()
        {
            _name = AskPlayerName();
            _playerList.Add(_name);

            MessageBoxResult result;

            do
            {
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxImage img = MessageBoxImage.Question;
                result = MessageBox.Show("Wilt u nog een speler toevoegen?", "MULTIPLAYER", buttons, img);

                if (result == MessageBoxResult.No)
                {
                    break;
                }

                string extraPlayer = AskPlayerName();
                _playerList.Add(extraPlayer);
            } while (true);

            _currentPlayer = _playerList[0];
            TxtPogingen.Text = $"Current player: {_currentPlayer}\nPoging: {_attempts} / 10\nScore: {_score}";
        }

        public string AskPlayerName()
        {
            string name = Interaction.InputBox("Wat is jouw naam?", "START GAME", null);
            while (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Geef een naam!", "Warn");
                name = Interaction.InputBox("Wat is jouw naam?", "START GAME", null);
            }
            return name;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage messageBoxImage = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show("Wilt u het spel vroegtijdig beëindigen?", $"Poging: {_attempts}/10", buttons, messageBoxImage);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
            else if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            } 
        }

        private void menuNieuwSpel_Click(object sender, RoutedEventArgs e)
        {
            RestartGame();
        }

        private void menuHighScores_Click(object sender, RoutedEventArgs e)
        {
            string highScoreRanking = "";

            foreach(string highscore in _highscores)
            {
                highScoreRanking = highScoreRanking + $"{highscore}\n";
            }
            MessageBox.Show(highScoreRanking, "Mastermind highscores", MessageBoxButton.OK);
        }

        private void menuAfsluiten_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void menuPogingen_Click(object sender, RoutedEventArgs e)
        {
            string numOfAttempts = Interaction.InputBox("Maximum aantal pogingen (3 - 20)", "10");
            while (string.IsNullOrEmpty(numOfAttempts))
            {
                MessageBox.Show("Geef een nummer");
                numOfAttempts = Interaction.InputBox("Maximum aantal pogingen (3 - 20)", "10");
            }
            if (Convert.ToInt32(numOfAttempts) < 3 || Convert.ToInt32(numOfAttempts) > 20)
            {
                MessageBox.Show("Geef een nummer tussen 3 & 20", "FOUTIEVE INVOER");
                numOfAttempts = Interaction.InputBox("Maximum aantal pogingen (3 - 20)", "10");
                while (string.IsNullOrEmpty(numOfAttempts))
                {
                    MessageBox.Show("Geef een nummer");
                    numOfAttempts = Interaction.InputBox("Maximum aantal pogingen (3 - 20)", "10");
                }
            }
            _maxAttempts = Convert.ToInt32(numOfAttempts);
        }


        private void btnGetHint_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Wat voor hint wilt u gebruiken?\nYes = Juiste plaats\nNo = Juiste kleur\nCancel = Nog geen hint gebruiken", "HINT", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _score -= 25;
                Random rand = new Random();
                int codeIndex = rand.Next(0, 4);

                string color = _code[codeIndex];
                MessageBox.Show($"De code bevat de kleur {color}, deze staat op plaats {codeIndex+1}", "Juiste plaats");

                TxtPogingen.Text = $"Current player: {_currentPlayer}\nPoging: {_attempts} / 10\nScore: {_score}";
                btnGetHint.Visibility = Visibility.Collapsed;
            }
            else if (result == MessageBoxResult.No)
            {
                _score -= 15;
                Random rand = new Random();
                int codeIndex = rand.Next(0, 4);

                string color = _code[codeIndex];
                MessageBox.Show($"De code bevat de kleur {color}", "Juiste kleur");

                TxtPogingen.Text = $"Current player: {_currentPlayer}\nPoging: {_attempts} / 10\nScore: {_score}";
                btnGetHint.Visibility = Visibility.Collapsed;
            }
        }
    }
}
