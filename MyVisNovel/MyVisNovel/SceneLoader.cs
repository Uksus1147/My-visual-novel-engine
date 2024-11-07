using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json; 

public class SceneLoader
{
    public Scene LoadScene(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Файл не найден: {filePath}");

        string json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<Scene>(json);
    }
}
