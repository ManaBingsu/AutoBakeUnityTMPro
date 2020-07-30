# AutoBake
Unity package for automated TextMeshPro baking
## 개요
AutoBake는 Bobbin refresh 후 갱신된 Csv파일을 기반으로 기존에 있던 TextMeshPro의 폰트에셋들을 새롭게 굽는 과정을 자동화한 툴입니다.

----------
## 개발 환경

-   Unity 2019.4.3f1

----------

## 패키지 구성

-   AutoBake.cs
-   AutoBake2.cs
-   TMPro_FontAssetCreatorWindow.cs

----------

## 사용 방법

AutoBake와 AutoBake2 중에 필요하신 것을 사용하시면 됩니다.

### AutoBake 특징

-   장점
    -   TMP에서 제공하는 api를 사용하여 안정적입니다.
-   단점
    -   api 기반이 런타임에 굽는 것을 전제로 하였기에 에디터 상의 Font Asset Creator 창에서 굽는 방식과 다른 방식이 적용되어 기존 아틀라스보다 저압축입니다.

### AutoBake2 특징

-   장점
    -   에디터 상에서 사용하는 Bake 방식을 사용하여 기존과 같은 고압축으로 사용할 수 있습니다.
-   단점
    -   TextMeshPro 패키지 내 코드 수정을 요구합니다.
    -   내부 코드 수정으로 인하여 원하지 않는 동작이 발생할 수 있습니다.

### 필수 패키지

사용하기 위해 필수적인 패키지를 Github에서 받아 프로젝트에 적용해야 합니다.

CsvReader의 경우 CsvReader.cs 만 설치하셔도 됩니다.

-   [Bobbin](https://github.com/radiatoryang/bobbin) (by radiatorayang)
-   [CsvReader](https://github.com/nreco/csv/) (by NReco)

### Version 2를 사용할 경우

프로젝트명\Library\PackageCache\com.unity.textmeshpro@2.1.1\Scripts\Editor 폴더에 존재하는 TMPro_FontAssetCreatorWindow.cs 파일을 AutoBake 패키지 내에 있는 동일한 이름의 파일로 덮어씌워야 합니다.

### 생성
![howtocreate](https://user-images.githubusercontent.com/43133819/88871472-ec30e500-d252-11ea-9559-07b5972dbba7.png)

Project 창에서 마우스 우클릭 → Create → Auto Bake / Auto Bake2

### 세팅

![howtoset](https://user-images.githubusercontent.com/43133819/88871551-2306fb00-d253-11ea-98c0-f1640110e526.png)

-   Path

Bake 대상이 되는 Csv 파일의 경로입니다.

-   Location setting
    -   Name
        -   대상 언어, 사용자 식별용
    -   Index (중요)
        -   Csv 파일 내 해당 언어의 인덱스
    -   Font Assets
        -   해당 언어로 굽고자 하는 폰트 에셋들

### 적용

모든 세팅이 완료되었으면 'Auto bake'를 클릭하시면 됩니다.
