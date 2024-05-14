using System.Diagnostics;
using System.ServiceProcess;
using System.Windows.Forms;
using System;
using System.IO;

namespace FileLicence
{
    public class FunctionService
    {
        private static string _serviceName = "ServiceFile";
        private static string _serviceInstallFileName = Path.GetFullPath("ServiceFile.exe");

        public void InstallService()
        {
            bool result;

            try
            {
                Process install = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        Arguments = $"create {_serviceName} binPath= \"{_serviceInstallFileName}\" start=auto",
                        FileName = "sc.exe",
                        Verb = "runas",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    }
                };
                install.Start();
                install.WaitForExit();
                if (_serviceName != null)
                    result = CheckServiceIsInstalled();
                else
                    result = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu uma exceção ao instalar o serviço: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Save(DateTime date_,string txt_path)
        {
            try
            {
                string endFilePath = Path.GetFullPath("EndDate.txt");
                string locationFilePath = Path.GetFullPath("LocationLic.txt");


                DirectoryInfo endDirectory = Directory.CreateDirectory(Path.GetDirectoryName(endFilePath));
                DirectoryInfo locationDirectory = Directory.CreateDirectory(Path.GetDirectoryName(locationFilePath));


                if (!File.Exists(endFilePath))
                {
                    File.Create(endFilePath).Close(); 
                }
                if (!File.Exists(locationFilePath))
                {
                    File.Create(locationFilePath).Close();
                }

                File.WriteAllText(endFilePath, string.Empty);
                File.WriteAllText(locationFilePath, string.Empty);

                File.WriteAllText(endFilePath, date_.ToString());

                File.WriteAllText(locationFilePath, txt_path);

                MessageBox.Show("Informações salvas com sucesso!", "Confirmação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Acesso negado. Por favor, execute o programa como administrador.", "Erro de Permissão", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        bool CheckServiceIsInstalled()
        {
            ServiceController[] services = ServiceController.GetServices();

            foreach (ServiceController service in services)
            {
                if (service.ServiceName.Equals(_serviceName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public void UninstallService()
        {
            try
            {
                Process uninstall = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        Arguments = $"delete {_serviceName}",
                        FileName = "sc.exe",
                        Verb = "runas",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    }
                };
                uninstall.Start();
                uninstall.WaitForExit();



                if (uninstall.ExitCode == 0)
                {
                    MessageBox.Show($"O serviço {_serviceName} foi desinstalado com sucesso.", "Confirmação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Ocorreu um erro ao desinstalar o serviço {_serviceName}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao iniciar o processo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu uma exceção ao desinstalar o serviço: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void StartService()
        {
            try
            {
                using (ServiceController serviceController = new ServiceController(_serviceName))
                {
                    if (serviceController.Status != ServiceControllerStatus.Running)
                    {
                        ProcessStartInfo processInfo = new ProcessStartInfo
                        {
                            FileName = "sc.exe",
                            Arguments = $"start {_serviceName}",
                            Verb = "runas",
                            CreateNoWindow = true,
                            WindowStyle = ProcessWindowStyle.Hidden
                        };

                        Process.Start(processInfo)?.WaitForExit();

                        serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30)); // Espera até que o serviço esteja no estado "Running" ou até 30 segundos
                        MessageBox.Show($"O serviço {_serviceName} foi iniciado com sucesso.", "Confirmação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"O serviço {_serviceName} já está em execução.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show($"Ocorreu um erro ao tentar iniciar o serviço: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (System.ServiceProcess.TimeoutException ex)
            {
                MessageBox.Show($"Tempo esgotado ao aguardar o serviço iniciar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu uma exceção ao iniciar o serviço: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
