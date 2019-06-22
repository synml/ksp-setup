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
        public string KoreanDownloadDir { get; set; }   //한글패치 파일이 저장된 디렉토리를 저장한다.
        public string KspDirectory { get; set; }    //KSP가 설치된 디렉토리를 저장한다.


        //메인 메소드
        public MainWindow()
        {
            CkanDownloadDir = "./CKAN/";
            KoreanDownloadDir = "./한글패치/";
            KspDirectory = null;
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
                //다운로드 디렉토리를 만든다.
                Directory.CreateDirectory(CkanDownloadDir);

                //CKAN의 최신버전을 다운로드할 수 있는 URL을 만든다.
                string url = web.Url.ToString();
                string latestVersion = url.Substring(url.LastIndexOf('/') + 1);
                string ckanUrl = "https://github.com/KSP-CKAN/CKAN/releases/download/" + latestVersion + "/ckan.exe";
                txtbox_log.AppendText("CKAN 다운로드 URL: " + ckanUrl);

                //파일을 다운로드한다.
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(ckanUrl, CkanDownloadDir + "ckan.exe");
                }
                txtbox_log.AppendText("CKAN 다운로드 완료.");

                //파일을 KSP 디렉토리로 이동한다. (이미 파일이 존재하면 덮어씌운다.)
                if (File.Exists(KspDirectory + "/ckan.exe"))
                    File.Delete(KspDirectory + "/ckan.exe");
                File.Move(CkanDownloadDir + "ckan.exe", KspDirectory + "/ckan.exe");

                //CKAN 설치를 완료했다고 알린다.
                txtbox_log.AppendText("CKAN 설치 완료.");

                //다운로드 디렉토리를 삭제한다.
                Directory.Delete(CkanDownloadDir, true);
            };

            //CKAN의 최신버전이 릴리즈된 곳으로 이동한다.
            web.Navigate("https://github.com/KSP-CKAN/CKAN/releases/latest");
        }

        //한글패치 파일을 다운로드하는 메소드
        private void KoreanFileDownload(int downloadMode, string fileName)
        {
            using (WebClient webClient = new WebClient())
            {
                switch (downloadMode)
                {
                    case 0:
                        webClient.DownloadFile("http://cfile239.uf.daum.net/attach/998A94355D028BBA0109E9", KoreanDownloadDir + fileName);
                        break;
                    case 1:
                        webClient.DownloadFile("http://cfile231.uf.daum.net/attach/998AF0355D028BC1011CCB", KoreanDownloadDir + fileName);
                        break;
                    case 2:
                        webClient.DownloadFile("http://cfile224.uf.daum.net/attach/998AF4355D028BC6012A69", KoreanDownloadDir + fileName);
                        break;
                    default:
                        txtbox_log.AppendText("잘못된 다운로드 모드 설정입니다.");
                        break;
                }
            }
        }

        //한글패치를 적용하는 메소드
        private void KoreanPatch()
        {
            //다운로드 디렉토리를 만든다.
            Directory.CreateDirectory(KoreanDownloadDir);

            //체크박스 체크 유무에 따라 설치를 진행한다.
            if (chkbox_vanilla.IsChecked == true)
            {
                //한글패치 파일을 다운로드한다.
                KoreanFileDownload(0, "바닐라.cfg");

                //파일을 이동한다.
                string sourceFileName = KoreanDownloadDir + "바닐라.cfg";
                string destFileName = KspDirectory + "/GameData/Squad/Localization/dictionary.cfg";
                File.Delete(destFileName);
                File.Move(sourceFileName, destFileName);

                //한글패치 적용을 완료했다고 알린다.
                txtbox_log.AppendText("바닐라 한글패치 완료.");
            }
            if (chkbox_dlc1.IsChecked == true)
            {
                //한글패치 파일을 다운로드한다.
                KoreanFileDownload(1, "Making_History_DLC.cfg");

                //파일을 이동한다.
                string sourceFileName = KoreanDownloadDir + "Making_History_DLC.cfg";
                string destFileName = KspDirectory + "/GameData/SquadExpansion/MakingHistory/Localization/dictionary.cfg";
                File.Delete(destFileName);
                File.Move(sourceFileName, destFileName);

                //한글패치 적용을 완료했다고 알린다.
                txtbox_log.AppendText("Making History DLC 한글패치 완료.");
            }
            if (chkbox_dlc2.IsChecked == true)
            {
                //한글패치 파일을 다운로드한다.
                KoreanFileDownload(2, "Breaking_Ground_DLC.cfg");

                //파일을 이동한다.
                string sourceFileName = KoreanDownloadDir + "Breaking_Ground_DLC.cfg";
                string destFileName = KspDirectory + "/GameData/SquadExpansion/Serenity/Localization/dictionary.cfg";
                File.Delete(destFileName);
                File.Move(sourceFileName, destFileName);

                //한글패치 적용을 완료했다고 알린다.
                txtbox_log.AppendText("Breaking Ground DLC 한글패치 완료.");
            }

            //다운로드 디렉토리를 삭제한다.
            Directory.Delete(KoreanDownloadDir, true);
        }

        //KSP가 설치된 디렉터리를 탐색하는 버튼을 클릭한 경우의 이벤트 메소드
        private void Btn_kspDir_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "모든 파일(*.*)|*.*",
                Title = "KSP_x64.exe를 선택해주세요."
            };
            DialogResult result = openFileDlg.ShowDialog();
            if (result.ToString() == "OK")
            {
                string filePath = openFileDlg.FileName;
                string directory = filePath.Substring(0, filePath.LastIndexOf("\\"));
                KspDirectory = directory;
                txtbox_kspDir.Text = directory;
            }
        }

        //설정 시작 버튼을 클릭한 경우의 이벤트 메소드
        private void Btn_Setup_Click(object sender, RoutedEventArgs e)
        {
            //오류 제어
            if (KspDirectory == null)
            {
                System.Windows.MessageBox.Show("먼저 KSP가 설치된 디렉토리를 입력하세요.", "오류");
                return;
            }

            //한글패치 적용을 시작한다.
            KoreanPatch();

            //CKAN 설치에 체크했으면 CKAN을 설치한다.
            if (chkbox_ckan.IsChecked == true)
                CkanInstall();
        }

        //종료 버튼을 클릭한 경우의 이벤트 메소드
        private void Btn_Exit_Click(object sender, RoutedEventArgs e) => Close();
    }
}