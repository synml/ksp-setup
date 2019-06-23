using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Forms;

namespace KSP_Setup
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        //필드 목록
        public string CkanDownloadDir { get; set; }     //CKAN의 다운로드 파일이 저장된 디렉토리를 저장한다.
        public string HangulDownloadDir { get; set; }   //한글패치 파일이 저장된 디렉토리를 저장한다.
        public string KspDirectory { get; set; }    //KSP가 설치된 디렉토리를 저장한다.
        public int Ksp_version { get; set; }    //사용자가 설정한 ksp 버전을 저장한다.

        //각 버전별 다운로드 URL을 저장하는 배열을 선언한다. (행: 버전, 열: 항목)
        //3행: 1.7.2, 2행: 1.7.1, 1행: 1.7.0, 0행: 1.6.1
        //0열: 바닐라, 1열: Making History DLC, 2열: Breaking Ground DLC
        private readonly string[,] downloadURL = new string[4, 3];


        //메인 메소드
        public MainWindow()
        {
            //디렉토리 필드를 초기화한다.
            CkanDownloadDir = "./CKAN/";
            HangulDownloadDir = "./한글패치/";
            KspDirectory = null;

            //한글패치 다운로드 링크를 초기화한다.
            downloadURL[3, 0] = "http://cfile239.uf.daum.net/attach/998A94355D028BBA0109E9";
            downloadURL[3, 1] = "http://cfile231.uf.daum.net/attach/998AF0355D028BC1011CCB";
            downloadURL[3, 2] = "http://cfile224.uf.daum.net/attach/998AF4355D028BC6012A69";

            downloadURL[2, 0] = "http://cfile203.uf.daum.net/attach/9980C4385CF6B215046670";
            downloadURL[2, 1] = "http://cfile201.uf.daum.net/attach/9981EB385CF6B21A0424CB";
            downloadURL[2, 2] = "http://cfile228.uf.daum.net/attach/997753385CF6B21E051470";

            downloadURL[1, 0] = "http://cfile234.uf.daum.net/attach/9902DC505CBE00E60343A0";
            downloadURL[1, 1] = "http://cfile229.uf.daum.net/attach/990378505CBE00EA0315DC";

            downloadURL[0, 0] = "http://cfile234.uf.daum.net/attach/99B08C355C3DEAFA27BF73";
            downloadURL[0, 1] = "http://cfile217.uf.daum.net/attach/9960B2355C3DEAFE36D20B";

            //창 띄우기
            InitializeComponent();
        }

        //CKAN을 설치하는 메소드
        private void CkanInstall()
        {
            //웹 브라우저 객체를 생성한다.
            WebBrowser web = new WebBrowser();

            //이벤트를 등록한다.
            web.Navigated += delegate
            {
                try
                {
                    //다운로드 디렉토리를 만든다.
                    Directory.CreateDirectory(CkanDownloadDir);

                    //CKAN의 최신버전을 다운로드할 수 있는 URL을 만든다.
                    string url = web.Url.ToString();
                    string latestVersion = url.Substring(url.LastIndexOf('/') + 1);
                    string ckanUrl = "https://github.com/KSP-CKAN/CKAN/releases/download/" + latestVersion + "/ckan.exe";

                    //파일을 다운로드한다.
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFile(ckanUrl, CkanDownloadDir + "ckan.exe");
                    }
                    WriteLine("CKAN 다운로드 완료.");

                    //파일을 KSP 디렉토리로 이동한다. (이미 파일이 존재하면 덮어씌운다.)
                    if (File.Exists(KspDirectory + "/ckan.exe"))
                    {
                        File.Delete(KspDirectory + "/ckan.exe");
                    }
                    File.Move(CkanDownloadDir + "ckan.exe", KspDirectory + "/ckan.exe");

                    //CKAN 설치를 완료했다고 알린다.
                    WriteLine("CKAN 설치 완료.");
                    WriteLine("");

                    //다운로드 디렉토리를 삭제한다.
                    Directory.Delete(CkanDownloadDir, true);
                }
                catch (Exception e)
                {
                    WriteLine("오류: " + e.Message);
                }
                finally
                {
                    //웹 브라우저 객체의 리소스를 해제한다.
                    web.Dispose();
                }
            };

            //CKAN의 최신버전이 릴리즈된 곳으로 이동한다.
            web.Navigate("https://github.com/KSP-CKAN/CKAN/releases/latest");

            //CKAN 설치를 완료했다고 알린다.
            WriteLine("CKAN 다운로드 중. . .");
        }

        //한글파일을 적용하는 메소드 (모드 0번: 바닐라, 1번: Making DLC, 2번: Breaking DLC)
        private int HangulFileApply(int applyMode)
        {
            string sourceFileName, destFileName;

            switch (applyMode)
            {
                case 0:
                    sourceFileName = HangulDownloadDir + "바닐라.cfg";
                    destFileName = KspDirectory + "/GameData/Squad/Localization/dictionary.cfg";
                    break;
                case 1:
                    sourceFileName = HangulDownloadDir + "Making_History_DLC.cfg";
                    destFileName = KspDirectory + "/GameData/SquadExpansion/MakingHistory/Localization/dictionary.cfg";
                    break;
                case 2:
                    sourceFileName = HangulDownloadDir + "Breaking_Ground_DLC.cfg";
                    destFileName = KspDirectory + "/GameData/SquadExpansion/Serenity/Localization/dictionary.cfg";
                    break;
                default:
                    WriteLine("잘못된 적용 모드입니다.");
                    return 1;
            }

            try
            {
                //파일이 존재하지 않으면 중단한다.
                if (!File.Exists(destFileName))
                {
                    WriteLine(destFileName + "가 존재하지 않습니다.");
                    return 2;
                }
                File.Delete(destFileName);
                File.Move(sourceFileName, destFileName);
            }
            catch (Exception e)
            {
                WriteLine("오류: " + e.Message);
                return 3;
            }

            return 0;
        }

        //한글패치 파일을 다운로드하는 메소드 (모드 0번: 바닐라, 1번: Making DLC, 2번: Breaking DLC)
        private int HangulFileDownload(int downloadMode)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    switch (downloadMode)
                    {
                        case 0:
                            webClient.DownloadFile(downloadURL[Ksp_version, 0], HangulDownloadDir + "바닐라.cfg");
                            break;
                        case 1:
                            webClient.DownloadFile(downloadURL[Ksp_version, 1], HangulDownloadDir + "Making_History_DLC.cfg");
                            break;
                        case 2:
                            webClient.DownloadFile(downloadURL[Ksp_version, 2], HangulDownloadDir + "Breaking_Ground_DLC.cfg");
                            break;
                        default:
                            WriteLine("잘못된 다운로드 모드입니다.");
                            return 1;
                    }
                }
            }
            catch (Exception e)
            {
                WriteLine("오류: " + e.Message);
                return 2;
            }

            return 0;
        }

        //한글패치를 하는 메소드
        private int HangulPatch()
        {
            int retval;

            //어떤 버전의 한글패치를 적용하는지 알린다.
            

            //다운로드 디렉토리를 만든다.
            Directory.CreateDirectory(HangulDownloadDir);

            //체크박스 체크 유무에 따라 설치를 진행한다.
            if (chkbox_vanilla.IsChecked == true)
            {
                //한글파일을 다운로드한다.
                retval = HangulFileDownload(0);
                if (retval != 0)
                    return 1;

                //한글파일을 적용한다.
                retval = HangulFileApply(0);
                if (retval != 0)
                    return 1;

                //한글패치 적용을 완료했다고 알린다.
                WriteLine("바닐라 한글패치 완료.");
            }
            if (chkbox_dlc1.IsChecked == true)
            {
                //한글파일을 다운로드한다.
                retval = HangulFileDownload(1);
                if (retval != 0)
                    return 1;

                //한글파일을 적용한다.
                retval = HangulFileApply(1);
                if (retval != 0)
                    return 1;

                //한글패치 적용을 완료했다고 알린다.
                WriteLine("Making History DLC 한글패치 완료.");
            }
            if (chkbox_dlc2.IsChecked == true)
            {
                //한글파일을 다운로드한다.
                retval = HangulFileDownload(2);
                if (retval != 0)
                    return 1;

                //한글파일을 적용한다.
                retval = HangulFileApply(2);
                if (retval != 0)
                    return 1;

                //한글패치 적용을 완료했다고 알린다.
                WriteLine("Breaking Ground DLC 한글패치 완료.");
            }

            //다운로드 디렉토리를 삭제한다.
            Directory.Delete(HangulDownloadDir, true);

            return 0;
        }

        //로그를 기록하는 메소드
        private void WriteLine(string str)
        {
            txtbox_log.AppendText(str + "\n");
            txtbox_log.ScrollToEnd();
        }

        //-----------------------------------------------------------------------------------------

        //종료 버튼을 클릭한 경우의 이벤트 메소드
        private void Btn_Exit_Click(object sender, RoutedEventArgs e) => Close();

        //KSP가 설치된 디렉터리를 탐색하는 버튼을 클릭한 경우의 이벤트 메소드
        private void Btn_kspDir_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "KSP_x64.exe|KSP_x64.exe",
                Title = "KSP_x64.exe를 선택해주세요."
            };
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

        //KSP 디렉토리를 여는 버튼을 클릭한 경우의 이벤트 메소드
        private void Btn_OpenKspDir_Click(object sender, RoutedEventArgs e)
        {
            //KSP 디렉토리를 연다.
            Process.Start(KspDirectory);
        }

        //설정 시작 버튼을 클릭한 경우의 이벤트 메소드
        private void Btn_Setup_Click(object sender, RoutedEventArgs e)
        {
            int retval;

            //한글패치 적용을 시작한다.
            retval = HangulPatch();
            if (retval != 0)
            {
                WriteLine("한글패치의 전체 또는 일부를 실패했습니다.");
            }

            //칸 띄우기
            WriteLine("");

            //CKAN 설치에 체크했으면 CKAN을 설치한다.
            if (chkbox_ckan.IsChecked == true)
            {
                CkanInstall();
            }
        }

        //KSP 버전 선택기의 드롭다운을 닫았을 때의 이벤트 메소드
        private void Ksp_version_selector_DropDownClosed(object sender, EventArgs e)
        {
            //1.7.0과 1.6.1은 Breaking Ground DLC 한글파일이 없으므로 체크하지 못하게 한다.
            if ((ksp_version_170.IsSelected == true) || (ksp_version_161.IsSelected == true))
            {
                chkbox_dlc2.IsChecked = false;
                chkbox_dlc2.IsEnabled = false;
            }
            else
            {
                chkbox_dlc2.IsChecked = true;
                chkbox_dlc2.IsEnabled = true;
            }

            //KSP 버전 선택에 따라 필드의 값을 변경한다.
            if (ksp_version_172.IsSelected == true)
            {
                Ksp_version = 3;
            }
            else if (ksp_version_171.IsSelected == true)
            {
                Ksp_version = 2;
            }
            else if (ksp_version_170.IsSelected == true)
            {
                Ksp_version = 1;
            }
            else if (ksp_version_161.IsSelected == true)
            {
                Ksp_version = 0;
            }
            else
            {
                WriteLine("잘못된 버전을 선택했습니다.");
            }
        }
    }
}