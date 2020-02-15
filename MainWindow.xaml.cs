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

            //한글파일 다운로드 링크를 초기화한다.
            Setup.downloadURL[0, 2, 0] = "https://k.kakaocdn.net/dn/c5LcD5/btqB1GK0uae/YCX5EZI3RptwMMSYHgk3uk/dictionary.cfg?attach=1&knm=tfile.cfg";
            Setup.downloadURL[0, 2, 1] = "https://k.kakaocdn.net/dn/cIgAPG/btqBZMrTKu2/sOuIKbyUtzlGUDzxH8r6b0/dictionary.cfg?attach=1&knm=tfile.cfg";
            Setup.downloadURL[0, 2, 2] = "https://k.kakaocdn.net/dn/biAwcf/btqB1dvM4yf/N7k2bFcOgvYwBU2YvOPXG0/dictionary.cfg?attach=1&knm=tfile.cfg";

            Setup.downloadURL[0, 1, 0] = "https://k.kakaocdn.net/dn/l4DWp/btqziF9SnQ3/ZpGyluTiGCvldIQvdPfTHK/dictionary.cfg?attach=1&knm=tfile.cfg";
            Setup.downloadURL[0, 1, 1] = "https://k.kakaocdn.net/dn/bESaSu/btqzi3JpPZe/a4pCiosirM533YvkPNsLzK/dictionary.cfg?attach=1&knm=tfile.cfg";
            Setup.downloadURL[0, 1, 2] = "https://k.kakaocdn.net/dn/kTLyS/btqzi3byopv/6PIPIlsDGEBetMaAHS0HY0/dictionary.cfg?attach=1&knm=tfile.cfg";

            Setup.downloadURL[0, 0, 0] = "https://k.kakaocdn.net/dn/b1IqZR/btqxBQkdeoT/k7iLsYs27mAEohjQT4Ivu1/dictionary.cfg?attach=1&knm=tfile.cfg";
            Setup.downloadURL[0, 0, 1] = "https://k.kakaocdn.net/dn/lAC54/btqxwD1qwcQ/dUVIYn3hmKk7MasrsnwkXK/dictionary.cfg?attach=1&knm=tfile.cfg";
            Setup.downloadURL[0, 0, 2] = "https://k.kakaocdn.net/dn/bt6okj/btqxygRJi7l/zDtOTl5k4pN98ee6dBkask/dictionary.cfg?attach=1&knm=tfile.cfg";
            //---------------------------------------------------------------------------------

            //영문파일 다운로드 링크를 초기화한다.
            Setup.downloadURL[1, 2, 0] = "https://k.kakaocdn.net/dn/nhVnG/btqBYfhwdlk/X4MEwg6cMnJRzVQQH0AkVk/dictionary.cfg?attach=1&knm=tfile.cfg";
            Setup.downloadURL[1, 2, 1] = "https://k.kakaocdn.net/dn/De20I/btqB1ZXSv98/NM7UZzw5wUE7TLjKkDOkdK/dictionary.cfg?attach=1&knm=tfile.cfg";
            Setup.downloadURL[1, 2, 2] = "https://k.kakaocdn.net/dn/ckozS0/btqB1HiQVqa/pKtx35iQGV7Y9BH3Axs1Rk/dictionary.cfg?attach=1&knm=tfile.cfg";

            Setup.downloadURL[1, 1, 0] = "https://k.kakaocdn.net/dn/M10EJ/btqzhg4eWJj/vMG2BmZNWycLU1ShONXTuk/dictionary.cfg?attach=1&knm=tfile.cfg";
            Setup.downloadURL[1, 1, 1] = "https://k.kakaocdn.net/dn/bk6JaF/btqzhJrmQEb/Ws5pgaUk2EgVtXP1fUuwd1/dictionary.cfg?attach=1&knm=tfile.cfg";
            Setup.downloadURL[1, 1, 2] = "https://k.kakaocdn.net/dn/bPIQUL/btqzibVut1Z/hD9JKO7q0EPJ7Aw6GVlVok/dictionary.cfg?attach=1&knm=tfile.cfg";

            Setup.downloadURL[1, 0, 0] = "https://k.kakaocdn.net/dn/bTFvoQ/btqxx1ArNTU/fsDXKYgKUkDTVHgDkAEcV0/dictionary.cfg?attach=1&knm=tfile.cfg";
            Setup.downloadURL[1, 0, 1] = "https://k.kakaocdn.net/dn/xhA70/btqxx1mYepI/UnrCt2nEqwOaOqOkJO8iT0/dictionary.cfg?attach=1&knm=tfile.cfg";
            Setup.downloadURL[1, 0, 2] = "https://k.kakaocdn.net/dn/P7Y7N/btqxB7zgw34/ZYyd7M5u3DPjx4voKVtIF1/dictionary.cfg?attach=1&knm=tfile.cfg";
            //---------------------------------------------------------------------------------

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
            if (ksp_version_190.IsSelected == true)
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