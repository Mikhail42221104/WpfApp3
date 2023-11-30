using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Firebase.Database;
using Firebase.Database.Query;

namespace MouseCoordinatesApp
{
    public partial class MainWindow : Window
    {
        private FirebaseClient firebaseClient;
        private int lastX = -1;
        private int lastY = -1;

        public MainWindow()
        {
            InitializeComponent();
            InitializeFirebaseClient();
            MouseMove += MainWindow_MouseMove;
            UpdateDataGrid(); 
        }

        private void InitializeFirebaseClient()
        {
            
            firebaseClient = new FirebaseClient("https://coursework-d0ee6-default-rtdb.firebaseio.com/");
        }

        private async void StartRecording_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private async void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);

            if (Math.Abs(position.X - lastX) >= 5 || Math.Abs(position.Y - lastY) >= 5)
            {
                await firebaseClient.Child("mouseCoordinates").PostAsync(new { X = (int)position.X, Y = (int)position.Y });
                lastX = (int)position.X;
                lastY = (int)position.Y;
                UpdateDataGrid();
            }
        }

        private async void UpdateDataGrid()
        {
            var data = await firebaseClient.Child("mouseCoordinates").OnceAsync<MouseCoordinate>();
            coordinatesDataGrid.ItemsSource = null; 
            coordinatesDataGrid.ItemsSource = data.Select(d => new { X = d.Object.X, Y = d.Object.Y });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MouseMove -= MainWindow_MouseMove;
        }
    }

    public class MouseCoordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}