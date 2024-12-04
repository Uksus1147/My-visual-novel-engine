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



namespace MyVisNovel
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Scene currentScene;
        private int dialogueIndex = 0;

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
                CharacterImage.Source = new BitmapImage(new Uri(newCharacterPath)); // Отображаем новое изображение
                                                                                    // Здесь также можешь обновить объект Scene, чтобы сохранить этот путь
            }
        }


        private void SaveScene_Click(object sender, RoutedEventArgs e)
        {
            // Сохранение текущей сцены в файл
            string filePath = "Scenes/scene1.json"; // Путь к файлу сцены
            File.WriteAllText(filePath, JsonConvert.SerializeObject(currentScene, Formatting.Indented));
            MessageBox.Show("Сцена сохранена!");
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

        private void TextInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
