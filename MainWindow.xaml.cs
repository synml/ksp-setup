using System;
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
            //다운로드한 파일을 저장할 디렉토리를 만든다.
            Directory.CreateDirectory(CkanDownloadDir);

            using (WebBrowser web = new WebBrowser())
            {
                web.Navigated += delegate
                {
                    //CKAN의 최신버전을 다운로드할 수 있는 URL을 만든다.
                    string url = web.Url.ToString();
                    string latestVersion = url.Substring(url.LastIndexOf('/') + 1);
                    string ckanUrl = "https://github.com/KSP-CKAN/CKAN/releases/download/" + latestVersion + "/ckan.exe";

                    //만든 URL을 사용하여 파일을 다운로드한다.
                    WebClient ckanDownload = new WebClient();
                    ckanDownload.DownloadFile(ckanUrl, CkanDownloadDir + "ckan.exe");

                    //다운로드한 파일을 KSP 디렉토리로 이동한다.
                    File.Move(CkanDownloadDir + "ckan.exe", KspDirectory + "ckan.exe");

                    //CKAN을 다운로드했던 디렉토리를 삭제한다.
                    Directory.Delete(CkanDownloadDir, true);
                };

                //CKAN의 최신버전이 릴리즈된 곳으로 이동한다.
                web.Navigate("https://github.com/KSP-CKAN/CKAN/releases/latest");
            }
        }

        //한글패치 파일을 다운로드하는 메소드
        private void KoreanFileDownload(string download_dir)
        {
            Directory.CreateDirectory(download_dir);
        }

        //한글패치를 적용하는 메소드
        private void KoreanPatch()
        {
            //파일 다운로드
            KoreanFileDownload("바닐라");
            KoreanFileDownload("Making_History_DLC");
            KoreanFileDownload("Breaking_Ground_DLC");

        }

        //KSP가 설치된 디렉터리를 탐색하는 버튼을 클릭한 경우의 이벤트 메소드
        private void Btn_kspDir_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "모든 파일(*.*)|*.*",
                Title = "KSP.exe를 선택해주세요."
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
