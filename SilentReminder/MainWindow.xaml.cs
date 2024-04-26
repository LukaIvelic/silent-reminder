using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;

namespace SilentReminder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void SetReminder(object sender, RoutedEventArgs e)
        {
            //parsing and adding time
            int timerMinutes = int.Parse(TimeField.Text);
            DateTime setTime = DateTime.Now.AddMinutes(timerMinutes);

            //confirming choice
            MessageBoxResult r =  MessageBox.Show($"A timer will be set for: {setTime.Hour.ToString("D2")}:{setTime.Minute.ToString("D2")} ", "Silent Reminder", MessageBoxButton.OKCancel);

            //evaluating choice
            if (r != MessageBoxResult.OK) return;

            //if okay -> execute
            string reminderName = NameField.Text;
            NameField.Text = string.Empty;
            TimeField.Text = string.Empty;

            Thread timerThread = new Thread(new ThreadStart(() =>{
                while (timerMinutes > 0){
                    --timerMinutes;
                    Thread.Sleep(1000*60);
                    if (timerMinutes <= 1)
                    {
                        Dispatcher.Invoke(new Action(() => { 
                            Topmost = true;
                            ReminderGrid.Visibility = Visibility.Visible;
                            ReminderName.Content = $"Reminder for: {reminderName}";
                            ReminderMessage.Text = $"You have 1 minute left before the reminder expires.";
                        }));
                        break;
                    }
                }
            }));

            timerThread.Start();
        }

        private void OKButtonClicked(object sender, RoutedEventArgs e)
        {
            ReminderGrid.Visibility = Visibility.Hidden;
            ReminderName.Content = string.Empty;
            ReminderMessage.Text = string.Empty;
        }
    }
}