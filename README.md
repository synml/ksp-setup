# KSP-Setup

> ksp의 초기 설정을 빠르게 해주는 프로그램입니다.

우리가 KSP를 설치하고 바로 게임을 하기에는 부족한 점이 많습니다. 한글패치를 해야하고, 모드를 사용하려면 ckan도 설치해야 합니다. 그러나 이것을 수동으로 하기에는 너무 번거롭고 시간도 오래 걸립니다.

따라서 이런 작업들을 자동으로 해주는 프로그램을 만들었습니다. 이것은 ksp를 처음 설치했을 때, 수행하는 여러 작업을 빠르고 간편하게 처리합니다.

이 프로그램이 유저 분들의 귀차니즘을 조금이나마 줄여준다면 좋겠습니다. 😎

## 프로젝트 소개

- 동기
  - ksp를 처음 설치했을 때, 손으로 설정하는 것이 번거로워서 만들었습니다.
- 목적
  - 최대한 쉽고 빠르고 범용적으로 ksp의 설정을 진행한다. (Easy, Fast, Wide use)
- 주요 기능
  1. KSP 버전에 맞는 한글파일을 자동으로 다운로드하여 적용한다.
  2. 옵션 설정 시, CKAN을 자동으로 다운로드하여 설치한다.

## Build Status

[![GitHub release](https://img.shields.io/github/release/clovadev/KSP-Setup.svg?style=popout-square)](https://github.com/clovadev/KSP-Setup/releases/latest)  ![GitHub last commit](https://img.shields.io/github/last-commit/clovadev/KSP-Setup.svg?style=popout-square) ![GitHub All Releases](https://img.shields.io/github/downloads/clovadev/KSP-Setup/total.svg?style=popout-square) ![GitHub](https://img.shields.io/github/license/clovadev/KSP-Setup.svg?style=popout-square) 

## 설치 방법

1. 위의 Release 뱃지를 클릭하거나 [Release 링크](https://github.com/clovadev/KSP-Setup/releases/latest)를 클릭한다.
2. 가장 최신 버전을 다운로드한다. (Latest Release)

## 사용 방법, 예제

![Instruction](https://github.com/clovadev/KSP-Setup/blob/master/img/instruction.gif)

1. '폴더 탐색' 버튼을 클릭하고 KSP_x64.exe를 선택한다.
2. 지역화할 KSP 버전과 언어를 선택한다.
3. '설정 시작' 버튼을 클릭한다.
4. KSP 폴더를 열거나 프로그램을 종료한다.

## 기능

1. 1초 안에 끝나는 한글/영문패치
2. KSP 버전별 패치
3. 한글패치, 영문패치를 지원
4. DLC 유무를 자동 감지
5. 한글파일을 자동으로 다운로드하여 적용
6. 옵션 설정 시, CKAN을 자동으로 다운로드하여 설치
7. 툴팁으로 부족한 설명 보충

## API, 프레임워크

- .NET Framework 4.7.2

## 개발 환경

- S/W 개발 환경 
  - Visual Studio 2019 Community (16.1.3)
  - .NET Framework 4.7.2
  - C# Language
- 개발 환경 설정 
  1. 리포지토리를 클론, 포크하거나 압축파일로 코드를 다운로드하세요.
  2. .NET Framework 4.7.2개발 도구가 설치되어 있는지 확인하세요. 없으면 설치.
  3. Visual Studio 2019로 솔루션 파일(.sln)을 여세요.
  4. 코딩 시작~!

## 기여 방법

1. 이 리포지토리를 포크합니다.
2. GitHub Desktop에서 새 브랜치를 만들거나 master 브랜치를 그대로 사용합니다.
3. 수정사항을 commit하세요.
4. 선택한 브랜치에 push하세요.
5. Pull request를 보내주세요.

## 라이센스

MIT License

`LICENSE`에서 자세한 정보를 확인할 수 있습니다.
