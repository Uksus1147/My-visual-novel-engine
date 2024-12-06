using System;
using System.Collections.Generic;

public class Scene
{
    public string BackgroundImagePath { get; set; } // Путь к изображению фона
    public string Text { get; set; } // Свойство для текста сцены
    public List<string> Dialogue { get; set; } = new List<string>(); // Реплики диалога
    public string CharacterImagePath { get; set; }   // Путь к изображению персонажа
    public string MusicPath { get; set; }

}
