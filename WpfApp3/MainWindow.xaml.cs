using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Streaming;
using FireSharp;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
namespace WpfApp3
{


    public partial class MainWindow : Window
    {
        public double x;
        public double y;
        ObservableCollection<string> messages = new ObservableCollection<string>();
        public MainWindow()
        {
            InitializeComponent();
            ChatListView.ItemsSource = messages;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentPosition = e.GetPosition(this);
            x = currentPosition.X;
            y = currentPosition.Y;


        }
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            ListenToFirebase();

        }

        private void RecordCoordinates_Click(object sender, RoutedEventArgs e)
        {

            while (true)

            {
                if (1 == 1)
                {

                    Firebase.Database.FirebaseClient FirebaseClient = new Firebase.Database.FirebaseClient("https://coursework-d0ee6-default-rtdb.firebaseio.com");
                    lab.Content = x.ToString();
                    lab1.Content = y.ToString();

                    FirebaseClient.Child("coordinates").PostAsync(new Table { X = x.ToString(), Y = y.ToString() });
                }
            }
        }
        private void ListenToFirebase()
        {
            var FirebaseClient = new Firebase.Database.FirebaseClient("https://coursework-d0ee6-default-rtdb.firebaseio.com/");

            var allMessages = FirebaseClient.Child("coordinates").OnceAsync<Table>();

            FirebaseClient
                .Child("coordinates")
                .AsObservable<Table>()
                .Subscribe(d =>
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (d != null && d.EventType == FirebaseEventType.InsertOrUpdate)
                        {
                            messages.Add($"X:{d.Object.X}\n Y:{d.Object.Y}");
                        }
                    }));
                });
        }

    }

    public class Table
    {
        public string X { get; set; }
        public string Y { get; set; }
    }
}
