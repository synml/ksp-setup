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
        public string KspDirectory { get; set; }
        public string CkanUrl { get; set; }

        //CKAN을 다운로드하는 메소드
        private void CkanDownload()
        {
            using (WebBrowser web = new WebBrowser())
            {
                web.Navigated += delegate
                {
                    string url = web.Url.ToString();
                    string latestVersion = url.Substring(url.LastIndexOf('/') + 1);
                    CkanUrl = "https://github.com/KSP-CKAN/CKAN/releases/download/" + latestVersion + "/ckan.exe";
                };
                web.Navigate("https://github.com/KSP-CKAN/CKAN/releases/latest");
            }

            WebClient ckanDownload = new WebClient();
            ckanDownload.DownloadFileAsync(new Uri(CkanUrl), "./CKAN/ckan.exe");
        }

        //한글패치를 적용하는 메소드
        private void KoreanPatch(string directory)
        {
            Directory.CreateDirectory(directory);
        }

        //CKAN을 설치하는 메소드
        private void CkanInstall()
        {
            Directory.CreateDirectory("./CKAN");
            CkanDownload();
        }

        public MainWindow()
        {
            InitializeComponent();
        }

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

        private void Btn_Setup_Click(object sender, RoutedEventArgs e)
        {
            if (chkbox_vanilla.IsChecked == true)
            {
                KoreanPatch("./한글패치/바닐라");
            }
            if (chkbox_dlc1.IsChecked == true)
            {
                KoreanPatch("./한글패치/Making History DLC");
            }
            if (chkbox_dlc2.IsChecked == true)
            {
                KoreanPatch("./한글패치/Breaking Ground DLC");
            }
            if (chkbox_ckan.IsChecked == true)
            {
                CkanInstall();
            }
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e) => Close();
    }
}
