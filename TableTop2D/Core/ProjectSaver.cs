using System;
using System.IO;
using System.Text.Json;
using TableTop2D.Core.WorkTable;

namespace TableTop2D.Core
{
    internal static class ProjectSaver
    {
        /// <summary> Путь до папки Temp, автоматически создаваемой в папке приложения </summary>
        public static readonly string ProjectsDirectoryPath = $@"{Environment.CurrentDirectory}/Projects";

        /// <summary> Имя текущего проекта </summary>
        public static string? ProjectName { get; private set; }

        /// <returns> FileInfo текущего проекта </returns>
        public static FileInfo GetProjectFile()
        {
            if (string.IsNullOrEmpty(ProjectName)) throw new ArgumentNullException(nameof(ProjectName));

            if (!File.Exists($"{ProjectsDirectoryPath}/{ProjectName}.json")) throw new Exception("Отутствует проект с таким именем");

            return new FileInfo($"{ProjectsDirectoryPath}/{ProjectName}.json");
        }

        public static void CreateNewProject(string projectName, ProjectTable project)
        {
            if (!string.IsNullOrEmpty(ProjectName)) throw new Exception("Проект с таким именем уже существует");

            ProjectName = projectName;

            using var fs = new FileStream($@"{ProjectsDirectoryPath}/{ProjectName}.json", FileMode.Create);
            JsonSerializer.Serialize(fs, project, new JsonSerializerOptions() { WriteIndented = true });
        }

        public static ProjectTable GetProject()
        {
            if (string.IsNullOrEmpty(ProjectName)) throw new ArgumentNullException(nameof(ProjectName));

            var path = $@"{ProjectsDirectoryPath}/{ProjectName}.json";

            if (!File.Exists(path)) throw new Exception("Файл проекта не найден!");

            using var fs = new FileStream($@"{ProjectsDirectoryPath}/{ProjectName}.json", FileMode.Open);
            return JsonSerializer.Deserialize<ProjectTable>(fs) ?? throw new Exception("Не удалось десериализировать ProjectSaver!");
        }
    }
}