# AutoBake
Unity package for automated TextMeshPro baking
## 개요
AutoBake는 Bobbin refresh 후 갱신된 Csv파일을 기반으로 기존에 있던 TextMeshPro의 폰트에셋들을 새롭게 굽는 과정을 자동화한 툴입니다.

## Development environment

-   Unity 2019.4.3f1

## Package composition

-   AutoBake.cs
-   AutoBake2.cs
-   TMPro_FontAssetCreatorWindow.cs


## How to use

AutoBake와 AutoBake2 중에 필요하신 것을 사용하시면 됩니다.

### About AutoBake (Recommended)

-   Pros
    -   It is stable because the api provided by TMPro is used.
-   Cons
    -   Since api-based baking is premised on baking at runtime, a different method from baking in the Font Asset Creator window on the editor is applied, so it is less compressed than the existing atlas.

### About AutoBake2 (Not recommended)

-   Pros
    -   By using the Bake method used in the editor, it can be used in the same high compression as the method in the editor.
-   Cons
    -   It requires a code modification in the TextMeshPro package.
    -   Unwanted behavior may occur due to internal code modification.

### Essential package for use AutoBake

In order to use it, you need to get the essential packages from link and apply them to your project.

CsvReader의 경우 CsvReader.cs 만 설치하셔도 됩니다.

-   [Bobbin](https://github.com/radiatoryang/bobbin) (by radiatorayang)
-   [CsvReader](https://github.com/nreco/csv/) (by NReco)

~~### When using AutoBake 2~~

~~- 프로젝트명\Library\PackageCache\com.unity.textmeshpro@2.1.1\Scripts\Editor 폴더에 존재하는 TMPro_FontAssetCreatorWindow.cs 파일을 AutoBake 패키지 내에 있는 동일한 이름의 파일로 덮어씌워야 합니다.~~
~~- 이후 유니티가 자동으로 TMPro 패키지를 롤백하는 것을 막기 위해 프로젝트명\Library\PackageCache\com.unity.textmeshpro@2.1.1 폴더를 프로젝트명\Packages로 옮겨줍니다.~~AutoBake2 is not recommended.

### 
![howtocreate](https://user-images.githubusercontent.com/43133819/88871472-ec30e500-d252-11ea-9559-07b5972dbba7.png)

Right-click in the Project window → Create → Auto Bake / Auto Bake2

### Setting

![howtoset](https://user-images.githubusercontent.com/43133819/88871551-2306fb00-d253-11ea-98c0-f1640110e526.png)

-   Path

Path of the Csv file to be Bake target.

-   Location setting
    -   Name
        -   Target language. For user identification
    -   Index (Important)
        -   The index of the language in the csv file
    -   Font Assets
        -   Font assets you want to bake in that language

### Apply

When all settings are complete, click 'Auto bake'.
