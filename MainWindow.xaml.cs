using System;
using System.Windows;
using NAudio.Wave;

namespace RadioApp
{
    public partial class MainWindow : Window
    {
        private WaveOutEvent? _waveOut;
        private MediaFoundationReader? _mediaReader;

        public MainWindow()
        {
            InitializeComponent();
            
            // Назначаем обработчики кнопок
            PlayButton.Click += PlayButton_Click;
            StopButton.Click += StopButton_Click;
            
            // Пример ссылки для теста
            UrlTextBox.Text = "http://cdn.energyfm.ru:9000/energy";
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, введена ли ссылка
            if (string.IsNullOrWhiteSpace(UrlTextBox.Text))
            {
                MessageBox.Show("Введите ссылку на радиостанцию!", "Ошибка", 
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string url = UrlTextBox.Text.Trim();

            try
            {
                // Останавливаем текущее воспроизведение
                StopPlayback();
                
                // Создаем новый плеер
                _waveOut = new WaveOutEvent();
                _mediaReader = new MediaFoundationReader(url);
                _waveOut.Init(_mediaReader);
                _waveOut.Play();
                
                // Обновляем статус
                StatusText.Text = "Статус: Играет";
                StatusText.Foreground = System.Windows.Media.Brushes.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка воспроизведения:\n{ex.Message}", 
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusText.Text = "Статус: Ошибка";
                StatusText.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            StopPlayback();
            StatusText.Text = "Статус: Остановлено";
            StatusText.Foreground = System.Windows.Media.Brushes.Gray;
        }

        private void StopPlayback()
        {
            if (_waveOut != null)
            {
                _waveOut.Stop();
                _waveOut.Dispose();
                _waveOut = null;
            }

            if (_mediaReader != null)
            {
                _mediaReader.Dispose();
                _mediaReader = null;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            StopPlayback();
            base.OnClosed(e);
        }
    }
}