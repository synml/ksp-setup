using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;

namespace KSP_Setup
{
    internal class Setup
    {
        public MainWindow MainWindow { get; }       //메인 창의 객체를 저장한다.
        public string DownloadDir { get; set; }     //다운로드한 파일이 저장될 디렉토리를 저장한다.
        public string KspDirectory { get; set; }    //KSP가 설치된 디렉토리를 저장한다.
        public int KspLanguage { get; set; }        //사용자가 설정한 ksp 언어를 저장한다.
        public int KspVersion { get; set; }         //사용자가 설정한 ksp 버전을 저장한다.
        public bool IsInstallCkan { get; set; }     //CKAN을 설치할 것인지를 저장한다.

        /* 언어 파일의 다운로드 URL을 저장하는 3차원 배열을 선언한다. (면: 언어, 행: 버전, 열: 항목)
           0면: 한국어, 1면: 영어
           0행: 1.7.1, 1행: 1.7.2, 2행: 1.7.3
           0열: 바닐라, 1열: Making History DLC, 2열: Breaking Ground DLC */
        internal readonly string[,,] downloadURL = new string[2, 4, 3];


        //생성자
        public Setup(MainWindow main)
        {
            MainWindow = main;
        }

        //언어 파일을 적용하는 메소드 (모드 0번: 바닐라, 1번: Making DLC, 2번: Breaking DLC)
        private void ApplyLanguageFile(int applyMode)
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
                        MainWindow.WriteLine("잘못된 적용 모드입니다.");
                        return;
                }

                //파일이 존재하지 않으면 중단한다.
                if (!File.Exists(sourceFileName))
                {
                    MainWindow.WriteLine(name + "의 현지화 파일이 존재하지 않습니다.");
                    return;
                }
                if (!File.Exists(destFileName))
                {
                    MainWindow.WriteLine(name + "가 존재하지 않습니다.");
                    return;
                }

                //파일을 복사하여 덮어쓴다.
                File.Copy(sourceFileName, destFileName, true);

                //언어 파일의 적용을 완료했다고 알린다.
                if (KspLanguage == 0)
                {
                    MainWindow.WriteLine(name + " 한글패치를 완료했습니다.");
                }
                else if (KspLanguage == 1)
                {
                    MainWindow.WriteLine(name + " 영문패치를 완료했습니다.");
                }
            }
            catch (Exception e)
            {
                MainWindow.WriteLine("오류: " + e);
            }
        }

        //언어 파일을 다운로드하는 메소드 (모드 0번: 바닐라, 1번: Making DLC, 2번: Breaking DLC)
        private void DownloadLanguageFile(int downloadMode)
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
                        MainWindow.WriteLine("잘못된 다운로드 모드입니다.");
                        return;
                }

                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(address, fileName);
                }
            }
            catch (Exception e)
            {
                MainWindow.WriteLine("오류: " + e);
            }
        }

        //CKAN을 설치하는 메소드
        internal void InstallCkan()
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
                        MainWindow.Dispatcher.Invoke(new Action(() =>
                        {
                            url = MainWindow.web.Source.ToString();
                        }));
                        string latestVersion = url.Substring(url.LastIndexOf('/') + 1);
                        string ckanUrl = "https://github.com/KSP-CKAN/CKAN/releases/download/" + latestVersion + "/ckan.exe";

                        //다운로드 디렉토리를 만든다.
                        Directory.CreateDirectory(DownloadDir);

                        //파일을 다운로드한다.
                        MainWindow.WriteLine("CKAN 다운로드 중. . .");
                        using (WebClient webClient = new WebClient())
                        {
                            webClient.DownloadFile(ckanUrl, Path.Combine(DownloadDir, "ckan.exe"));
                        }
                        MainWindow.WriteLine("CKAN 다운로드 완료.");

                        //파일을 KSP 디렉토리로 복사한다. (이미 파일이 존재하면 덮어씌운다.)
                        File.Copy(Path.Combine(DownloadDir, "ckan.exe"), Path.Combine(KspDirectory, "ckan.exe"), true);

                        //CKAN 설치를 완료했다고 알린다.
                        MainWindow.WriteLine("CKAN 설치를 완료했습니다.");
                        MainWindow.WriteLine("");

                        //다운로드 디렉토리를 삭제한다.
                        Directory.Delete(DownloadDir, true);
                    }
                    catch (Exception exception)
                    {
                        MainWindow.WriteLine("오류: " + exception);
                    }
                    finally
                    {
                        MainWindow.web.Navigated -= p;
                        isFinished = true;
                    }
                };
                worker.RunWorkerAsync();
            }
            MainWindow.web.Navigated += p;

            //CKAN의 최신버전이 릴리즈된 곳으로 이동한다.
            if (MainWindow.Dispatcher.CheckAccess())
            {
                MainWindow.web.Navigate("https://github.com/KSP-CKAN/CKAN/releases/latest");
            }
            else
            {
                MainWindow.Dispatcher.Invoke(new Action(() =>
                {
                    MainWindow.web.Navigate("https://github.com/KSP-CKAN/CKAN/releases/latest");
                }));
            }

            //CKAN 설치가 완료될 때까지 기다린다.
            while (isFinished == false)
            {
                Thread.Sleep(100);
            }
        }

        //현지화를 하는 메소드
        internal void Localize()
        {
            try
            {
                //다운로드 디렉토리를 만든다.
                Directory.CreateDirectory(DownloadDir);

                for (int i = 0; i <= 2; i++)
                {
                    //언어 파일을 다운로드한다.
                    DownloadLanguageFile(i);

                    //언어 파일을 적용한다.
                    ApplyLanguageFile(i);
                }

                //다운로드 디렉토리를 삭제한다.
                Directory.Delete(DownloadDir, true);
            }
            catch (Exception e)
            {
                MainWindow.WriteLine("오류: " + e);
            }
        }
    }
}