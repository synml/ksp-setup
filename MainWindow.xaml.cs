using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace KSP_Setup
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public string DownloadDir { get; set; }     //다운로드한 파일이 저장될 디렉토리를 저장한다.
        public string KspDirectory { get; set; }    //KSP가 설치된 디렉토리를 저장한다.
        public int KspLanguage { get; set; }        //사용자가 설정한 ksp 언어를 저장한다.
        public int KspVersion { get; set; }         //사용자가 설정한 ksp 버전을 저장한다.
        public bool IsInstallCkan { get; set; }     //CKAN을 설치할 것인지를 저장한다.

        //언어 파일의 다운로드 URL을 저장하는 3차원 배열을 선언한다. (면: 언어, 행: 버전, 열: 항목)
        //0면: 한국어, 1면: 영어
        //4행: 1.7.3, 3행: 1.7.2, 2행: 1.7.1, 1행: 1.7.0, 0행: 1.6.1
        //0열: 바닐라, 1열: Making History DLC, 2열: Breaking Ground DLC
        private readonly string[,,] downloadURL = new string[2, 5, 3];


        //생성자
        public MainWindow()
        {
            //한글파일 다운로드 링크를 초기화한다.
            downloadURL[0, 4, 0] = "http://cfile229.uf.daum.net/attach/990B2F4C5D56496C2A2575";
            downloadURL[0, 4, 1] = "http://cfile201.uf.daum.net/attach/9946C64C5D56497134127D";
            downloadURL[0, 4, 2] = "http://cfile240.uf.daum.net/attach/9978914C5D56497531CDC3";

            downloadURL[0, 3, 0] = "http://cfile239.uf.daum.net/attach/998A94355D028BBA0109E9";
            downloadURL[0, 3, 1] = "http://cfile231.uf.daum.net/attach/998AF0355D028BC1011CCB";
            downloadURL[0, 3, 2] = "http://cfile224.uf.daum.net/attach/998AF4355D028BC6012A69";

            downloadURL[0, 2, 0] = "http://cfile203.uf.daum.net/attach/9980C4385CF6B215046670";
            downloadURL[0, 2, 1] = "http://cfile201.uf.daum.net/attach/9981EB385CF6B21A0424CB";
            downloadURL[0, 2, 2] = "http://cfile228.uf.daum.net/attach/997753385CF6B21E051470";

            downloadURL[0, 1, 0] = "http://cfile234.uf.daum.net/attach/9902DC505CBE00E60343A0";
            downloadURL[0, 1, 1] = "http://cfile229.uf.daum.net/attach/990378505CBE00EA0315DC";

            downloadURL[0, 0, 0] = "http://cfile234.uf.daum.net/attach/99B08C355C3DEAFA27BF73";
            downloadURL[0, 0, 1] = "http://cfile217.uf.daum.net/attach/9960B2355C3DEAFE36D20B";
            //---------------------------------------------------------------------------------

            //영문파일 다운로드 링크를 초기화한다.
            downloadURL[1, 4, 0] = "http://cfile218.uf.daum.net/attach/99E5E24C5D56497B2C455A";
            downloadURL[1, 4, 1] = "http://cfile230.uf.daum.net/attach/990C7B4C5D5649802ADB76";
            downloadURL[1, 4, 2] = "http://cfile203.uf.daum.net/attach/9923D94C5D56498431DF8D";

            downloadURL[1, 3, 0] = "http://cfile202.uf.daum.net/attach/99E4A0415D028D3202B879";
            downloadURL[1, 3, 1] = "http://cfile228.uf.daum.net/attach/99E4F3415D028D360221C4";
            downloadURL[1, 3, 2] = "http://cfile201.uf.daum.net/attach/99E55D415D028D3B02F25B";

            downloadURL[1, 2, 0] = "http://cfile202.uf.daum.net/attach/99691D405CF6B24304E219";
            downloadURL[1, 2, 1] = "http://cfile213.uf.daum.net/attach/996925405CF6B24B04C068";
            downloadURL[1, 2, 2] = "http://cfile227.uf.daum.net/attach/996935405CF6B25104636B";

            downloadURL[1, 1, 0] = "http://cfile219.uf.daum.net/attach/99039F505CBE011D039E03";
            downloadURL[1, 1, 1] = "http://cfile202.uf.daum.net/attach/990400505CBE012803C2C5";

            downloadURL[1, 0, 0] = "http://cfile215.uf.daum.net/attach/9915E2355C3DEB04188C58";
            downloadURL[1, 0, 1] = "http://cfile215.uf.daum.net/attach/99D69D355C3DEB0725BB68";
            //---------------------------------------------------------------------------------

            //창 띄우기
            InitializeComponent();
        }

        //언어 파일을 적용하는 메소드 (모드 0번: 바닐라, 1번: Making DLC, 2번: Breaking DLC)
        private int ApplyLanguageFile(int applyMode)
        {
            string name, sourceFileName, destFileName;

            try
            {
                //원본, 대상 파일의 경로를 설정한다.
                switch (applyMode)
                {
                    case 0:
                        name = "바닐라";
                        sourceFileName = Path.Combine(DownloadDir, "바닐라.cfg");
                        destFileName = Path.Combine(KspDirectory, "GameData\\Squad\\Localization\\dictionary.cfg");
                        break;
                    case 1:
                        name = "Making History DLC";
                        sourceFileName = Path.Combine(DownloadDir, "Making_History_DLC.cfg");
                        destFileName = Path.Combine(KspDirectory, "GameData\\SquadExpansion\\MakingHistory\\Localization\\dictionary.cfg");
                        break;
                    case 2:
                        name = "Breaking Ground DLC";
                        sourceFileName = Path.Combine(DownloadDir, "Breaking_Ground_DLC.cfg");
                        destFileName = Path.Combine(KspDirectory, "GameData\\SquadExpansion\\Serenity\\Localization\\dictionary.cfg");
                        break;
                    default:
                        WriteLine("잘못된 적용 모드입니다.");
                        return 1;
                }

                //파일이 존재하지 않으면 중단한다.
                if (!File.Exists(destFileName))
                {
                    WriteLine(name + "가 존재하지 않습니다.");
                    return 2;
                }

                //파일을 복사하여 덮어쓴다.
                File.Copy(sourceFileName, destFileName, true);

                //언어 파일의 적용을 완료했다고 알린다.
                if (KspLanguage == 0)
                    WriteLine(name + " 한글패치를 완료했습니다.");
                else if (KspLanguage == 1)
                    WriteLine(name + " 영문패치를 완료했습니다.");
            }
            catch (Exception e)
            {
                WriteLine("오류: " + e);
                return 3;
            }

            return 0;
        }

        //언어 파일을 다운로드하는 메소드 (모드 0번: 바닐라, 1번: Making DLC, 2번: Breaking DLC)
        private int DownloadLanguageFile(int downloadMode)
        {
            try
            {
                string address = downloadURL[KspLanguage, KspVersion, downloadMode];
                string fileName;

                switch (downloadMode)
                {
                    case 0:
                        fileName = Path.Combine(DownloadDir, "바닐라.cfg");
                        break;
                    case 1:
                        fileName = Path.Combine(DownloadDir, "Making_History_DLC.cfg");
                        break;
                    case 2:
                        fileName = Path.Combine(DownloadDir, "Breaking_Ground_DLC.cfg");
                        break;
                    default:
                        WriteLine("잘못된 다운로드 모드입니다.");
                        return 1;
                }

                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(address, fileName);
                }
            }
            catch (Exception e)
            {
                WriteLine("오류: " + e);
                return 2;
            }

            return 0;
        }

        //CKAN을 설치하는 메소드
        private void InstallCkan()
        {
            //스레드 동기화를 위한 변수를 선언한다.
            bool isFinished = false;

            //이벤트 로컬 메소드를 생성하고 이벤트를 등록한다.
            void p(object sender, System.Windows.Navigation.NavigationEventArgs e)
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += delegate
                {
                    try
                    {
                        //CKAN의 최신버전을 다운로드할 수 있는 URL을 만든다.
                        string url = string.Empty;
                        Dispatcher.Invoke(new Action(() =>
                        {
                            url = web.Source.ToString();
                        }));
                        string latestVersion = url.Substring(url.LastIndexOf('/') + 1);
                        string ckanUrl = "https://github.com/KSP-CKAN/CKAN/releases/download/" + latestVersion + "/ckan.exe";

                        //다운로드 디렉토리를 만든다.
                        Directory.CreateDirectory(DownloadDir);

                        //파일을 다운로드한다.
                        WriteLine("CKAN 다운로드 중. . .");
                        using (WebClient webClient = new WebClient())
                        {
                            webClient.DownloadFile(ckanUrl, Path.Combine(DownloadDir, "ckan.exe"));
                        }
                        WriteLine("CKAN 다운로드 완료.");

                        //파일을 KSP 디렉토리로 복사한다. (이미 파일이 존재하면 덮어씌운다.)
                        File.Copy(Path.Combine(DownloadDir, "ckan.exe"), Path.Combine(KspDirectory, "ckan.exe"), true);

                        //CKAN 설치를 완료했다고 알린다.
                        WriteLine("CKAN 설치를 완료했습니다.");
                        WriteLine("");

                        //다운로드 디렉토리를 삭제한다.
                        Directory.Delete(DownloadDir, true);
                    }
                    catch (Exception exception)
                    {
                        WriteLine("오류: " + exception);
                    }
                    finally
                    {
                        web.Navigated -= p;
                        isFinished = true;
                    }
                };
                worker.RunWorkerAsync();
            }
            web.Navigated += p;

            //CKAN의 최신버전이 릴리즈된 곳으로 이동한다.
            if (Dispatcher.CheckAccess())
            {
                web.Navigate("https://github.com/KSP-CKAN/CKAN/releases/latest");
            }
            else
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    web.Navigate("https://github.com/KSP-CKAN/CKAN/releases/latest");
                }));
            }

            //CKAN 설치가 완료될 때까지 기다린다.
            while (isFinished == false)
            {
                Thread.Sleep(100);
            }
        }

        //현지화를 하는 메소드
        private void Localize()
        {
            int retval;

            try
            {
                //다운로드 디렉토리를 만든다.
                Directory.CreateDirectory(DownloadDir);

                for (int i = 0; i <= 2; i++)
                {
                    //1.7.0과 1.6.1은 Breaking DLC 현지화 적용안함.
                    if ((KspVersion <= 1) && (i == 2))
                        break;

                    //언어 파일을 다운로드한다.
                    retval = DownloadLanguageFile(i);
                    if (retval != 0)
                        return;

                    //언어 파일을 적용한다.
                    retval = ApplyLanguageFile(i);
                    if (retval != 0)
                        return;
                }

                //다운로드 디렉토리를 삭제한다.
                Directory.Delete(DownloadDir, true);
            }
            catch (Exception e)
            {
                WriteLine("오류: " + e);
            }
        }


        //-----------------------------------------------------------------------------------------
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
                        KspDirectory = directory;
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
                Process.Start(KspDirectory);
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
                    Localize();

                    //칸 띄우기
                    WriteLine("");

                    //CKAN 설치 유무를 확인한다.
                    if (IsInstallCkan == true)
                    {
                        InstallCkan();
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
            IsInstallCkan = (bool)chkbox_ckan.IsChecked;
        }

        //KSP의 언어를 저장하는 이벤트 메소드
        private void Ksp_language_selector_DropDownClosed(object sender, EventArgs e)
        {
            if (localization_korean.IsSelected == true)
            {
                KspLanguage = 0;
            }
            else if (localization_english.IsSelected == true)
            {
                KspLanguage = 1;
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
            if (ksp_version_173.IsSelected == true)
            {
                KspVersion = 4;
            }
            else if (ksp_version_172.IsSelected == true)
            {
                KspVersion = 3;
            }
            else if (ksp_version_171.IsSelected == true)
            {
                KspVersion = 2;
            }
            else if (ksp_version_170.IsSelected == true)
            {
                KspVersion = 1;
            }
            else if (ksp_version_161.IsSelected == true)
            {
                KspVersion = 0;
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
            DownloadDir = ".\\Download";

            //KSP 언어와 버전을 초기화한다.
            Chkbox_ckan_CheckedUnchecked(sender, null);
            Ksp_language_selector_DropDownClosed(sender, null);
            Ksp_version_selector_DropDownClosed(sender, null);
        }

        //컨트롤을 활성화/비활성화하는 메소드
        private void ControlEnabled(bool status)
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
        private void WriteLine(string message)
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