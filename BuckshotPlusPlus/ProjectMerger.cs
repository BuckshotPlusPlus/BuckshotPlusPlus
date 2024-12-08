using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace BuckshotPlusPlus
{
    public class ProjectMerger
    {
        private readonly string _projectPath;
        private readonly HashSet<string> _processedFiles;
        private readonly StringBuilder _mergedContent;

        public ProjectMerger(string projectPath)
        {
            _projectPath = projectPath;
            _processedFiles = new HashSet<string>();
            _mergedContent = new StringBuilder();
        }

        public void MergeProject()
        {
            try
            {
                // Add header comment
                _mergedContent.AppendLine("## CompleteProject.BPP");
                _mergedContent.AppendLine($"## Generated on: {DateTime.Now}");
                _mergedContent.AppendLine("## This is an auto-generated file containing all BPP code from the project");
                _mergedContent.AppendLine();

                // Start with the main file
                ProcessFile(_projectPath);

                // Write the merged content to CompleteProject.BPP
                string outputPath = Path.Combine(Path.GetDirectoryName(_projectPath), "CompleteProject.BPP");
                File.WriteAllText(outputPath, _mergedContent.ToString());

                Formater.SuccessMessage($"Successfully created CompleteProject.BPP at {outputPath}");
            }
            catch (Exception ex)
            {
                Formater.CriticalError($"Failed to merge project: {ex.Message}");
            }
        }

        private void ProcessFile(string filePath)
        {
            // Avoid processing the same file twice
            if (_processedFiles.Contains(filePath))
                return;

            _processedFiles.Add(filePath);

            try
            {
                string content = File.ReadAllText(filePath);
                string formattedContent = Formater.FormatFileData(content);

                // Add file header
                _mergedContent.AppendLine($"## File: {filePath}");
                _mergedContent.AppendLine();

                // Process includes
                var lines = formattedContent.Split('\n');
                foreach (var line in lines)
                {
                    if (line.TrimStart().StartsWith("include"))
                    {
                        string includePath = ParseIncludePath(line);
                        if (!string.IsNullOrEmpty(includePath))
                        {
                            string fullPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(filePath), includePath));
                            ProcessFile(fullPath);
                        }
                    }
                    else
                    {
                        _mergedContent.AppendLine(line);
                    }
                }

                _mergedContent.AppendLine();
            }
            catch (Exception ex)
            {
                Formater.Warn($"Error processing file {filePath}: {ex.Message}");
            }
        }

        private string ParseIncludePath(string includeLine)
        {
            try
            {
                var parts = includeLine.Split('"');
                if (parts.Length >= 2)
                {
                    return parts[1];
                }
            }
            catch (Exception)
            {
                // Ignore parsing errors
            }
            return null;
        }
    }

    // Extension for Program.cs
    public static class ProgramExtensions
    {
        public static void GenerateCompleteProject(string filePath)
        {
            var merger = new ProjectMerger(filePath);
            merger.MergeProject();
        }
    }
}