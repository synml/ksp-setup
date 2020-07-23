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
           0행: 1.7.3, 1행: 1.8.0(1.8.1), 2행: 1.9.0
           0열: 바닐라, 1열: Making History DLC, 2열: Breaking Ground DLC */
        internal readonly string[,,] downloadURL = new string[2, 4, 3];


        //생성자
        public Setup(MainWindow main)
        {
            MainWindow = main;

            //한글파일 다운로드 링크를 초기화한다.
            downloadURL[0, 3, 0] = "https://blog.kakaocdn.net/dn/bto6Dk/btqFyQJ6Rbt/g4CktLV4RERiptq2Rl9Eyk/dictionary.cfg?attach=1&knm=tfile.cfg";
            downloadURL[0, 3, 1] = "";
            downloadURL[0, 3, 2] = "https://blog.kakaocdn.net/dn/Y3kz6/btqFyqZrQhH/wBq38nkVkGKpeyEupKxtXk/dictionary.cfg?attach=1&knm=tfile.cfg";

            downloadURL[0, 2, 0] = "https://k.kakaocdn.net/dn/c5LcD5/btqB1GK0uae/YCX5EZI3RptwMMSYHgk3uk/dictionary.cfg?attach=1&knm=tfile.cfg";
            downloadURL[0, 2, 1] = "https://k.kakaocdn.net/dn/cIgAPG/btqBZMrTKu2/sOuIKbyUtzlGUDzxH8r6b0/dictionary.cfg?attach=1&knm=tfile.cfg";
            downloadURL[0, 2, 2] = "https://k.kakaocdn.net/dn/biAwcf/btqB1dvM4yf/N7k2bFcOgvYwBU2YvOPXG0/dictionary.cfg?attach=1&knm=tfile.cfg";

            downloadURL[0, 1, 0] = "https://k.kakaocdn.net/dn/l4DWp/btqziF9SnQ3/ZpGyluTiGCvldIQvdPfTHK/dictionary.cfg?attach=1&knm=tfile.cfg";
            downloadURL[0, 1, 1] = "https://k.kakaocdn.net/dn/bESaSu/btqzi3JpPZe/a4pCiosirM533YvkPNsLzK/dictionary.cfg?attach=1&knm=tfile.cfg";
            downloadURL[0, 1, 2] = "https://k.kakaocdn.net/dn/kTLyS/btqzi3byopv/6PIPIlsDGEBetMaAHS0HY0/dictionary.cfg?attach=1&knm=tfile.cfg";

            downloadURL[0, 0, 0] = "https://k.kakaocdn.net/dn/b1IqZR/btqxBQkdeoT/k7iLsYs27mAEohjQT4Ivu1/dictionary.cfg?attach=1&knm=tfile.cfg";
            downloadURL[0, 0, 1] = "https://k.kakaocdn.net/dn/lAC54/btqxwD1qwcQ/dUVIYn3hmKk7MasrsnwkXK/dictionary.cfg?attach=1&knm=tfile.cfg";
            downloadURL[0, 0, 2] = "https://k.kakaocdn.net/dn/bt6okj/btqxygRJi7l/zDtOTl5k4pN98ee6dBkask/dictionary.cfg?attach=1&knm=tfile.cfg";

            //영문파일 다운로드 링크를 초기화한다.
            downloadURL[1, 3, 0] = "https://blog.kakaocdn.net/dn/cRQNcs/btqFv2rEScQ/M6pPAkBGaJKckkKVz2ZIB0/dictionary.cfg?attach=1&knm=tfile.cfg";
            downloadURL[1, 3, 1] = "";
            downloadURL[1, 3, 2] = "https://blog.kakaocdn.net/dn/KYKc0/btqFzb8kvP8/Lz7MemWwmNUF5LyeBYCAoK/dictionary.cfg?attach=1&knm=tfile.cfg";

            downloadURL[1, 2, 0] = "https://k.kakaocdn.net/dn/nhVnG/btqBYfhwdlk/X4MEwg6cMnJRzVQQH0AkVk/dictionary.cfg?attach=1&knm=tfile.cfg";
            downloadURL[1, 2, 1] = "https://k.kakaocdn.net/dn/De20I/btqB1ZXSv98/NM7UZzw5wUE7TLjKkDOkdK/dictionary.cfg?attach=1&knm=tfile.cfg";
            downloadURL[1, 2, 2] = "https://k.kakaocdn.net/dn/ckozS0/btqB1HiQVqa/pKtx35iQGV7Y9BH3Axs1Rk/dictionary.cfg?attach=1&knm=tfile.cfg";

            downloadURL[1, 1, 0] = "https://k.kakaocdn.net/dn/M10EJ/btqzhg4eWJj/vMG2BmZNWycLU1ShONXTuk/dictionary.cfg?attach=1&knm=tfile.cfg";
            downloadURL[1, 1, 1] = "https://k.kakaocdn.net/dn/bk6JaF/btqzhJrmQEb/Ws5pgaUk2EgVtXP1fUuwd1/dictionary.cfg?attach=1&knm=tfile.cfg";
            downloadURL[1, 1, 2] = "https://k.kakaocdn.net/dn/bPIQUL/btqzibVut1Z/hD9JKO7q0EPJ7Aw6GVlVok/dictionary.cfg?attach=1&knm=tfile.cfg";

            downloadURL[1, 0, 0] = "https://k.kakaocdn.net/dn/bTFvoQ/btqxx1ArNTU/fsDXKYgKUkDTVHgDkAEcV0/dictionary.cfg?attach=1&knm=tfile.cfg";
            downloadURL[1, 0, 1] = "https://k.kakaocdn.net/dn/xhA70/btqxx1mYepI/UnrCt2nEqwOaOqOkJO8iT0/dictionary.cfg?attach=1&knm=tfile.cfg";
            downloadURL[1, 0, 2] = "https://k.kakaocdn.net/dn/P7Y7N/btqxB7zgw34/ZYyd7M5u3DPjx4voKVtIF1/dictionary.cfg?attach=1&knm=tfile.cfg";
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
                    if (KspVersion == 3 && i == 1)
                    {
                        MainWindow.WriteLine("*** 1.10부터 Making History DLC의 언어파일이 바닐라 언어파일과 통합되었습니다.");
                        continue;
                    }

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