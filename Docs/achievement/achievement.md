# Achievement

## 종류가 다양한 업적 조건과 보상을 모듈화
* 게임 특성상 업적의 조건과 보상이 서로 비슷하면서 조금씩 다 다르다.
* 업적의 조건은 예를 들어 무기별,스킬별,코스튬별 몬스터 킬체크, 보스 킬체크, 코스튬 수집...이렇게 다양하므로 진행상황을 알 수 있는 공통된 변수는 부모클래스로 빼고, 자식 클래스에서는 각 조건을 체크하는 API를 오버라이드하여 구현
* 보상도 업적의 조건과 마찬가지로 보상의 타입별로 보상을 주는 API를 오버라이드하여 구현
* 업적을 관리하는 매니저에서 해당 업적과 보상을 직렬화된 Dictionary로 관리
* [업적 구조](./AchievementUml.png)

## 추가 개선 사항
* Unity의 ScriptableObject를 이용한 모듈화로 타 직군으로부터 받은 데이터를 이용하여 ScriptableObject로 관리
* 프로그래밍을 잘 모르는 타 직군들도 쉽게 Asset을 직접 생성하여 시간단축에 용이함


## 참고 소스
[AchievementManager.cs](../../Assets/Scripts/AchievementManager/AchievementManager.cs)
[Achievement.cs](../../Assets/Scripts/AchievementManager/Achievement/Achievement.cs)
[Reward.cd](../../Assets/Scripts/AchievementManager/Reward/Reward.cs)