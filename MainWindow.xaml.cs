using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;

namespace KSP_Setup
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        internal Setup Setup { get; }

        //생성자
        public MainWindow()
        {
            Setup = new Setup(this);

            //창 띄우기
            InitializeComponent();
        }

        //프로그램을 종료시키는 이벤트 메소드
        private void Btn_Exit_Click(object sender, RoutedEventArgs e) => Close();

        //KSP가 설치된 디렉토리를 탐색하는 이벤트 메소드
        private void Btn_kspDir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDlg = new OpenFileDialog())
                {
                    openFileDlg.Multiselect = false;
                    openFileDlg.Filter = "KSP_x64.exe|KSP_x64.exe";
                    openFileDlg.Title = "KSP_x64.exe를 선택해주세요.";

                    DialogResult result = openFileDlg.ShowDialog();
                    if (result.ToString() == "OK")
                    {
                        string filePath = openFileDlg.FileName;
                        string directory = filePath.Substring(0, filePath.LastIndexOf("\\", StringComparison.InvariantCulture));
                        Setup.KspDirectory = directory;
                        txtbox_kspDir.Text = directory;

                        btn_Setup.IsEnabled = true;
                        btn_OpenKspDir.IsEnabled = true;
                    }
                }
            }
            catch (Exception exception)
            {
                WriteLine("오류: " + exception);
            }
        }

        //KSP가 설치된 디렉토리를 여는 이벤트 메소드
        private void Btn_OpenKspDir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //KSP 디렉토리를 연다.
                Process.Start(Setup.KspDirectory);
            }
            catch (Exception exception)
            {
                WriteLine("오류: " + exception);
            }
        }

        //설정을 시작하는 이벤트 메소드
        private void Btn_Setup_Click(object sender, RoutedEventArgs e)
        {
            using (BackgroundWorker worker = new BackgroundWorker())
            {
                worker.DoWork += delegate
                {
                    //컨트롤을 비활성화한다.
                    ControlEnabled(false);

                    //현지화를 한다.
                    Setup.Localize();

                    //칸 띄우기
                    WriteLine("");

                    //CKAN 설치 유무를 확인한다.
                    if (Setup.IsInstallCkan == true)
                    {
                        Setup.InstallCkan();
                    }

                    //컨트롤을 활성화한다.
                    ControlEnabled(true);
                };
                worker.RunWorkerAsync();
            }
        }

        //CKAN 설치 체크박스의 체크상태를 저장하는 이벤트 메소드
        private void Chkbox_ckan_CheckedUnchecked(object sender, RoutedEventArgs e)
        {
            Setup.IsInstallCkan = (bool)chkbox_ckan.IsChecked;
        }

        //KSP의 언어를 저장하는 이벤트 메소드
        private void Ksp_language_selector_DropDownClosed(object sender, EventArgs e)
        {
            if (localization_korean.IsSelected == true)
            {
                Setup.KspLanguage = 0;
            }
            else if (localization_english.IsSelected == true)
            {
                Setup.KspLanguage = 1;
            }
            else
            {
                WriteLine("잘못된 언어를 선택했습니다.");
            }
        }

        //KSP의 버전을 저장하는 이벤트 메소드
        private void Ksp_version_selector_DropDownClosed(object sender, EventArgs e)
        {
            //KSP 버전 선택에 따라 필드의 값을 변경한다.
            if (ksp_version_1100.IsSelected == true)
            {
                Setup.KspVersion = 3;
            }
            else if (ksp_version_190.IsSelected == true)
            {
                Setup.KspVersion = 2;
            }
            else if (ksp_version_180.IsSelected == true)
            {
                Setup.KspVersion = 1;
            }
            else if (ksp_version_173.IsSelected == true)
            {
                Setup.KspVersion = 0;
            }
            else
            {
                WriteLine("잘못된 버전을 선택했습니다.");
            }
        }

        //필드를 초기화하는 이벤트 메소드
        private void Window_Initialized(object sender, EventArgs e)
        {
            //다운로드 디렉토리를 초기화한다.
            Setup.DownloadDir = ".\\Download";

            //KSP 언어와 버전을 초기화한다.
            Chkbox_ckan_CheckedUnchecked(sender, null);
            Ksp_language_selector_DropDownClosed(sender, null);
            Ksp_version_selector_DropDownClosed(sender, null);
        }

        //컨트롤을 활성화/비활성화하는 메소드
        internal void ControlEnabled(bool status)
        {
            if (Dispatcher.CheckAccess())
            {
                btn_kspDir.IsEnabled = status;
                ksp_version_selector.IsEnabled = status;
                ksp_language_selector.IsEnabled = status;
                chkbox_ckan.IsEnabled = status;
                btn_OpenKspDir.IsEnabled = status;
                btn_Setup.IsEnabled = status;
            }
            else
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    btn_kspDir.IsEnabled = status;
                    ksp_version_selector.IsEnabled = status;
                    ksp_language_selector.IsEnabled = status;
                    chkbox_ckan.IsEnabled = status;
                    btn_OpenKspDir.IsEnabled = status;
                    btn_Setup.IsEnabled = status;
                }));
            }
        }

        //로그를 기록하는 메소드
        internal void WriteLine(string message)
        {
            if (Dispatcher.CheckAccess())
            {
                txtbox_log.AppendText(message + "\n");
                txtbox_log.ScrollToEnd();
            }
            else
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    txtbox_log.AppendText(message + "\n");
                    txtbox_log.ScrollToEnd();
                }));
            }
        }
    }
}