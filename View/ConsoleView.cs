using System;
using System.Globalization;
using System.Resources;
using System.Text.RegularExpressions;
using Easysave.Controller;
using Easysave.Logger;
using Easysave.Model;
using Newtonsoft.Json;


namespace Easysave.View
{
    public class ConsoleView
    {
        private readonly BackupController backupController;
        private ResourceManager resourceManager;
        private CultureInfo cultureInfo;

        private const int MinTask = 1;
        private const int MaxTask = 5;


        public ConsoleView(string stateTrackPath, string dailyPath)
        {
            resourceManager = new ResourceManager("EasySave.View.Messages", typeof(ConsoleView).Assembly);
            cultureInfo = new CultureInfo("en");

            DailyLogger dailyLogger;
            StateTrackLogger stateTrackLogger;

            if (dailyPath.Length == 0)
            {
                Console.WriteLine($"1. {resourceManager.GetString("dailyPath", cultureInfo)}");
                string dailyFolderPath = Console.ReadLine();
                dailyLogger = new DailyLogger(dailyFolderPath, DateTime.Today.ToString("yyyy-MM-dd")+".json");
            }
            else
            {
                dailyLogger = new DailyLogger(dailyPath);

            }
            if(stateTrackPath.Length == 0)
            {
                Console.WriteLine($"1. {resourceManager.GetString("statePath", cultureInfo)}");
                string stateTrackFolderPath = Console.ReadLine();
                stateTrackLogger = new StateTrackLogger(stateTrackFolderPath, "state.json");
            }
            else
            {
                stateTrackLogger = new StateTrackLogger(stateTrackPath);
            }

            /// Update Config
            string solutionDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            string jsonFilePath = Path.Combine(solutionDir, "AppConfig.json");
            AppConfigData appConfig = new AppConfigData(stateTrackLogger.FilePath, dailyLogger.FilePath);
            string updatedJson = JsonConvert.SerializeObject(appConfig, Formatting.Indented);
            File.WriteAllText(jsonFilePath, updatedJson);


            backupController = new BackupController(dailyLogger, stateTrackLogger);
        }


        private void ChangeLanguage()
        {
            Console.WriteLine();
            Console.WriteLine("1. English");
            Console.WriteLine("2. French");
            Console.Write("Select language: ");
            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    cultureInfo = new CultureInfo("en");
                    break;
                case "2":
                    cultureInfo = new CultureInfo("fr");
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
            Console.WriteLine();

        }

        public void DisplayMenu()
        {
            bool keepRunning = true;
            while (keepRunning)
            {
                Console.WriteLine($"\n--------- {resourceManager.GetString("MenuTitle", cultureInfo)} ---------");
                Console.WriteLine($"1. {resourceManager.GetString("AddBackupJob", cultureInfo)}");
                Console.WriteLine($"2. {resourceManager.GetString("ListBackupJobs", cultureInfo)}");
                Console.WriteLine($"3. {resourceManager.GetString("ExecuteBackupJob", cultureInfo)}");
                Console.WriteLine($"4. {resourceManager.GetString("ExecuteAllBackupJobs", cultureInfo)}");
                Console.WriteLine($"5. {resourceManager.GetString("ChangeLanguage", cultureInfo)}");
                Console.WriteLine($"6. {resourceManager.GetString("Exit", cultureInfo)}");

                Console.Write($"{resourceManager.GetString("SelectOption", cultureInfo)}: ");
                var option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        AddBackupTask();
                        break;
                    case "2":
                        ListBackupTasks();
                        break;
                    case "3":
                        ExecuteBackupJob();
                        break;
                    case "4":
                        //backupController.ExecuteAllBackupTask();
                        Console.WriteLine(resourceManager.GetString("AllJobsExecuted", cultureInfo));
                        break;
                    case "5":
                        ChangeLanguage();
                        break;
                    case "6":
                        keepRunning = false;
                        break;
                    default:
                        Console.WriteLine(resourceManager.GetString("InvalidOption", cultureInfo));
                        break;
                }

            }
        }


        private void AddBackupTask()
        {
            Console.WriteLine();

            Console.WriteLine($"\n------ {resourceManager.GetString("MenuAddBackupJob", cultureInfo)} ------");

            Console.Write(resourceManager.GetString("EnterBackupJobName", cultureInfo));
            var name = Console.ReadLine();

            string sourceDirectory;
            do
            {
                Console.Write(resourceManager.GetString("EnterSourceDirectoryPath", cultureInfo));
                sourceDirectory = Console.ReadLine();
                if (!Directory.Exists(sourceDirectory))
                {
                    Console.WriteLine(resourceManager.GetString("InvalidPath", cultureInfo));
                }
            } while (!Directory.Exists(sourceDirectory));

            string targetDirectory;
            bool isValidPath = false;
            do
            {
                Console.Write(resourceManager.GetString("EnterTargetDirectoryPath", cultureInfo));
                targetDirectory = Console.ReadLine();

                try
                {
                    var root = Path.GetPathRoot(targetDirectory);
                    if (Directory.Exists(root))
                    {
                        isValidPath = true;
                        if (!Directory.Exists(targetDirectory))
                        {
                            Console.WriteLine(resourceManager.GetString("CreatingTargetPath", cultureInfo));
                            Directory.CreateDirectory(targetDirectory);
                        }
                    }
                    else
                    {
                        Console.WriteLine(resourceManager.GetString("InvalidPath", cultureInfo));
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine(resourceManager.GetString("InvalidPath", cultureInfo));
                }
            } while (!isValidPath);

            Console.Write(resourceManager.GetString("SelectBackupType", cultureInfo));
            var typeInput = Console.ReadLine();
            BackupType type = (typeInput == "1") ? BackupType.Full : BackupType.Differential;

            backupController.AddBackupTask(new BackupModel(name, sourceDirectory, targetDirectory, type));
            Console.WriteLine(resourceManager.GetString("BackupJobAddedSuccess", cultureInfo));
            Console.WriteLine();
        }


        private void ListBackupTasks()
        {
            Console.WriteLine();
            Console.WriteLine(resourceManager.GetString("ListBackup", cultureInfo));
            for (int i = 1; i <= 5; i++)
            {
                if (backupController.BackupTasks.ContainsKey(i))
                {
                    Console.WriteLine($"{i}. {backupController.BackupTasks[i].Name}");
                }
                else
                {
                    Console.WriteLine($"{i}");
                }
            }
        }

        private void ExecuteBackupJob()
        {
            Console.Write(resourceManager.GetString("Index", cultureInfo));
            var userInput = Console.ReadLine();

            Regex uniqueJob = new Regex($@"[{MinTask-MaxTask}]", RegexOptions.IgnoreCase);
            Regex toJob = new Regex($@"[{MinTask-MaxTask}]-[{MinTask-MaxTask}]", RegexOptions.IgnoreCase);
            Regex andJob = new Regex($@"[{MinTask-MaxTask}];[{MinTask-MaxTask}]", RegexOptions.IgnoreCase);

            bool validInput = false;
            do
            {
                if (uniqueJob.IsMatch(userInput) || toJob.IsMatch(userInput) || andJob.IsMatch(userInput))
                {
                    validInput = true;
                    this.backupController.ExecuteTasks(userInput);
                }
                else
                {
                    Console.WriteLine($"{resourceManager.GetString("InvalidInput", cultureInfo)}, {resourceManager.GetString("PleaseEnterValidRange", cultureInfo)} (e.g., 1-3).");
                }
            } while (!validInput);


            /* var segments = userInput.Split(';');

            foreach (var segment in segments)
            {
                if (segment.Contains("-"))
                {
                    var parts = segment.Split('-');
                    if (parts.Length == 2 && int.TryParse(parts[0], out int start) && int.TryParse(parts[1], out int end) && start <= end)
                    {
                        for (int i = start; i <= end; i++)
                        {
                            //ExecuteSingleBackupJob(i - 1);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{resourceManager.GetString("InvalidRange", cultureInfo)} '{segment}', {resourceManager.GetString("PleaseEnterValidRange", cultureInfo)} (e.g., 1-3).");

                    }
                }
                else if (int.TryParse(segment, out int index))
                {
                    //ExecuteSingleBackupJob(index - 1);
                }
                else
                {
                    Console.WriteLine($"Invalid input in segment '{segment}', please enter a number or a range (e.g., 1 or 1-3).");
                }
            } */
        }
        //private void ExecuteSingleBackupJob(int jobIndex){}


        //     private void ExecuteAllBackupJobs()
        //   {
        //     backupController.ExecuteAllBackupJobs();
        //    Console.WriteLine(resourceManager.GetString("AllJobsExecuted", cultureInfo));
        //   }

    }
}
