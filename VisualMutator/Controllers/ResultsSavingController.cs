﻿namespace VisualMutator.Controllers
{
    #region

    using System.Diagnostics;
    using System.IO;
    using System.Xml.Linq;
    using Microsoft.Win32;
    using Model;
    using UsefulTools.Core;
    using UsefulTools.FileSystem;
    using UsefulTools.Wpf;
    using ViewModels;

    #endregion

    public class ResultsSavingController : Controller
    {
        private readonly ResultsSavingViewModel _viewModel;

        private readonly IFileSystem _fs;

        private readonly CommonServices _svc;

        private readonly XmlResultsGenerator _generator;

        private MutationTestingSession _currentSession;

        public ResultsSavingController(
            ResultsSavingViewModel viewModel, 
            IFileSystem fs,
            CommonServices svc,
            XmlResultsGenerator generator)
        {
            _viewModel = viewModel;
            _fs = fs;
            _svc = svc;
            _generator = generator;

            _viewModel.CommandSaveResults = new SmartCommand(SaveResults);

            _viewModel.CommandClose = new SmartCommand(Close);
            _viewModel.CommandBrowse = new SmartCommand(BrowsePath);

            if (_svc.Settings.ContainsKey("MutationResultsFilePath"))
            {
                _viewModel.TargetPath = _svc.Settings["MutationResultsFilePath"];
            }
           
        }

        
        public void Run(MutationTestingSession currentSession)
        {
            _currentSession = currentSession;
            _viewModel.Show();

        }
        public void BrowsePath()
        {

            SaveFileDialog dlg = new SaveFileDialog
            {
                FileName = "MutationResults",
                DefaultExt = ".xml",
                Filter = "XML documents (.xml)|*.xml"
            };


            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                _viewModel.TargetPath = dlg.FileName;

            }


        }
        public void SaveResults()
        {
            if (string.IsNullOrEmpty(_viewModel.TargetPath)
                || !Path.IsPathRooted(_viewModel.TargetPath))
            {
                _svc.Logging.ShowError("Invalid path");
                return;
            }

            XDocument document = _generator.GenerateResults(_currentSession, 
                _viewModel.IncludeDetailedTestResults, _viewModel.IncludeCodeDifferenceListings);

            try
            {
                using (var writer = _fs.File.CreateText(_viewModel.TargetPath))
                {
                    writer.Write(document.ToString());
                }
                _svc.Settings["MutationResultsFilePath"] = _viewModel.TargetPath;

                _viewModel.Close();


                var p = new Process();

                p.StartInfo.FileName = _viewModel.TargetPath;
                p.Start();
            }
            catch (IOException)
            {
                _svc.Logging.ShowError("Cannot write file: " + _viewModel.TargetPath);
            }
            
        }
        public void Close()
        {
            _viewModel.Close();
        }

       







    }
}