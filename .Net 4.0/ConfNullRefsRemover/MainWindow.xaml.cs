using System;
using System.IO;
using System.Windows;
using Intecom.Configuration;
using Microsoft.Win32;
using System.Linq;
using Intecom.Common;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnSelectConf(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Xml file|*.xml"
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            var path = openFileDialog.FileName;
            var configuration = Configuration.Load(path, null);

            var pipeIds = configuration.topology.pipeGroup.pipes.Select(x => x.id).ToList();
            var sensorIds = configuration.topology.sensorGroup.sensors.Select(x => x.id).ToList();
            var stationIds = configuration.systems.assembly.stations.Select(x => x.id).ToList();
            var diagnosticGroups = configuration.systems.dss.dssassembly.diagnosticGroups.diagnosticGroups;

            foreach (var diagnosticGroup in diagnosticGroups)
            {
                diagnosticGroup.pipesId.RemoveIf(x => !pipeIds.Contains(x));
                diagnosticGroup.npssId.RemoveIf(x => !stationIds.Contains(x));
                diagnosticGroup.pSensorsId.RemoveIf(x => !sensorIds.Contains(x));
                diagnosticGroup.qSensorsId.RemoveIf(x => !sensorIds.Contains(x));
            }

            /*var directory = Directory.GetCurrentDirectory();
            var newName = string.Format("{0} ({1}).xml", Path.GetFileNameWithoutExtension(path), DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss"));
            var newPath = string.Format("{0}\\{1}", directory, newName);
            configuration.saveXML(newPath);*/
            configuration.Save();

            OutputBox.AppendText("Готово.\n");
        }
    }
}
