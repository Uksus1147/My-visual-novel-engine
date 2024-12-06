using Newtonsoft.Json;
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
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;




namespace MyVisNovel
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Scene currentScene;
        private int dialogueIndex = 0;
        private MediaPlayer mediaPlayer = new MediaPlayer(); // Создаём плеер для музыки
        public List<Scene> Scenes { get; set; } = new List<Scene>();
        public string BGPath;
        public string CharacterPath;
        public string MusicPath;



        public MainWindow()
        {
            InitializeComponent();

            // Тестовая загрузка сцены
            currentScene = new Scene
            {
                BackgroundImagePath = "Images/background.jpg", // Убедись, что этот путь верен
                Dialogue = new List<string>
                {
                    "Привет! Добро пожаловать в нашу новеллу.",
                    "Это будет увлекательное приключение.",
                    "Надеюсь, тебе понравится!"
                }
            };

            LoadScene(currentScene);
        }

        private void LoadScene(Scene scene)
        {
            // Загрузка фонового изображения
            BackgroundImage.Source = new BitmapImage(new Uri(scene.BackgroundImagePath, UriKind.Relative));

            // Сброс индекса диалога
            dialogueIndex = 0;
            DisplayNextDialogue();

            // Загрузка персонажа (пример)
            Character character = new Character { Name = "Персонаж", ImagePath = "Images/character.png", IsVisible = true };
            if (character.IsVisible)
            {
                CharacterImage.Source = new BitmapImage(new Uri(character.ImagePath, UriKind.Relative));
                CharacterImage.Visibility = Visibility.Visible; // Показываем персонажа
            }
        }


        private void DisplayNextDialogue()
        {
            if (dialogueIndex < currentScene.Dialogue.Count)
            {
                DialogueText.Text = currentScene.Dialogue[dialogueIndex];
                dialogueIndex++;
            }
            else
            {
                DialogueText.Text = "Конец сцены.";
            }
        }

        // Обработчик клика для перехода к следующей реплике
        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DisplayNextDialogue();
        }
        private void ChangeBackground_Click(object sender, RoutedEventArgs e)
        {
            // Открываем диалог для выбора изображения
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp", // Разрешенные форматы
                Title = "Выберите изображение для фона"
            };

            // Если пользователь выбрал файл, обновляем фон
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImagePath = openFileDialog.FileName; // Путь к выбранному изображению
                BGPath = selectedImagePath;
                BackgroundImage.Source = new BitmapImage(new Uri(selectedImagePath, UriKind.Absolute)); // Обновляем фон
            }
        }

        private void ChangeText_Click(object sender, RoutedEventArgs e)
        {
            // Здесь должен быть код для изменения текста
            string newText = TextInput.Text; // Получаем текст из TextBox
            if (!string.IsNullOrWhiteSpace(newText))
            {
                DialogueText.Text = newText; // Изменяем текст на экране
                if (dialogueIndex < currentScene.Dialogue.Count)
                {
                    currentScene.Dialogue[dialogueIndex] = newText; // Обновляем текст в сцене
                }
            }
        }

        private void ChangeCharacter_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Images|*.png;*.jpg;*.jpeg", // Позволяем выбирать только изображения
                Title = "Выберите изображение персонажа"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string newCharacterPath = openFileDialog.FileName; // Получаем путь к выбранному изображению
                CharacterPath = newCharacterPath;
                CharacterImage.Source = new BitmapImage(new Uri(newCharacterPath)); // Отображаем новое изображение
                                                                                    // Здесь также можешь обновить объект Scene, чтобы сохранить этот путь
            }
        }


        private void TextInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TextInput.Text == "Введите текст...")
            {
                TextInput.Text = "";
                TextInput.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TextInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextInput.Text))
            {
                TextInput.Text = "Введите текст...";
                TextInput.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void AddMusic_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Audio Files|*.mp3;*.wav|All Files|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string musicPath = openFileDialog.FileName;
                MusicPath = musicPath;
                mediaPlayer.Open(new Uri(musicPath));
                mediaPlayer.MediaEnded += MediaPlayer_MediaEnded; // Подписываемся на событие окончания воспроизведения
                mediaPlayer.Play();
            }
        }

        // Событие повторного воспроизведения
        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            mediaPlayer.Position = TimeSpan.Zero; // Сбрасываем позицию воспроизведения в начало
            mediaPlayer.Play(); // Повторно запускаем музыку
        }
        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = e.NewValue; // Изменяем громкость
        }

        private void TextInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void WidthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (CharacterImage != null)
            {
                CharacterImage.Width = e.NewValue; // Устанавливаем новое значение ширины
            }
        }

        private void HeightSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (CharacterImage != null)
            {
                CharacterImage.Height = e.NewValue; // Устанавливаем новое значение высоты
            }
        }

        // Обработчик для добавления новой сцены
        private void AddSceneButton_Click(object sender, RoutedEventArgs e)
        {
            var newScene = new Scene
            {
                BackgroundImagePath = "default_background.jpg",  // Путь к фону
                CharacterImagePath = "default_character.jpg",    // Путь к персонажу
                MusicPath = "default_music.mp3",                  // Путь к музыке
                Text = "Текст сцены"                               // Текст сцены
            };

            Scenes.Add(newScene);

            // Обновляем ComboBox с новыми сценами
            SceneComboBox.ItemsSource = null;
            SceneComboBox.ItemsSource = Scenes;
            SceneComboBox.SelectedIndex = Scenes.Count - 1; // Выбираем последнюю добавленную сцену
        }

        // Обработчик для удаления выбранной сцены
        private void DeleteSceneButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedScene = SceneComboBox.SelectedItem as Scene;
            if (selectedScene != null)
            {
                Scenes.Remove(selectedScene);

                // Обновляем ComboBox с оставшимися сценами
                SceneComboBox.ItemsSource = null;
                SceneComboBox.ItemsSource = Scenes;
                SceneComboBox.SelectedIndex = 0; // Выбираем первую сцену (или другую логику)
            }
        }

        // Обработчик для сохранения сцены
        private void SaveSceneButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedScene = SceneComboBox.SelectedItem as Scene;
            if (selectedScene != null)
            {
                // Сохраняем текст сцены
                selectedScene.Text = DialogueText.Text;

                selectedScene.BackgroundImagePath = BGPath;
                selectedScene.CharacterImagePath = CharacterPath;
                selectedScene.MusicPath = MusicPath;

                MessageBox.Show("Сцена сохранена!");
            }
        }
        public Scene previousScene;
        // Обработчик для выбора сцены из ComboBox
        private void SceneComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedScene = SceneComboBox.SelectedItem as Scene;
            if (selectedScene != null)
            {
                // Отображаем данные сцены в интерфейсе
                DialogueText.Text = selectedScene.Text;

                // Загружаем фоновое изображение
                if (File.Exists(selectedScene.BackgroundImagePath))
                {
                    BackgroundImage.Source = new BitmapImage(new Uri(selectedScene.BackgroundImagePath, UriKind.Absolute));
                    Debug.WriteLine("Background: " + selectedScene.BackgroundImagePath);
                }

                // Загружаем изображение персонажа
                if (File.Exists(selectedScene.CharacterImagePath))
                {
                    CharacterImage.Source = new BitmapImage(new Uri(selectedScene.CharacterImagePath, UriKind.Absolute));
                }

                // Логика для воспроизведения музыки
                if (File.Exists(selectedScene.MusicPath))
                {
                    MediaPlayer mediaPlayer = new MediaPlayer();
                    // Проверка музыки
                    if (previousScene == null || selectedScene.MusicPath != previousScene.MusicPath)
                    {
                        if (!string.IsNullOrEmpty(selectedScene.MusicPath))
                        {
                            // Музыка отличается, переключаем трек
                            mediaPlayer.Stop();
                            mediaPlayer.Open(new Uri(selectedScene.MusicPath, UriKind.Absolute));
                            mediaPlayer.Play();
                        }
                        else
                        {
                            // Если музыки в новой сцене нет, останавливаем воспроизведение
                            mediaPlayer.Stop();
                        }
                    }
                    // Обновляем ссылку на предыдущую сцену
                    previousScene = selectedScene;

                    mediaPlayer.Open(new Uri(selectedScene.MusicPath, UriKind.Absolute));
                    mediaPlayer.Play();
                }
            }
        }

    }
}

