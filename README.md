# 🍽️ [평생 영업해주세요!] : 3D 식당 운영 시뮬레이션 게임

<img width="300" height="169" alt="PlzRestau_demo" src="https://github.com/user-attachments/assets/b9590f9f-c81e-4fb7-8532-e3d63df28558" />
<img width="300" height="169" alt="PlzRestau_demo (1)" src="https://github.com/user-attachments/assets/dfebfce9-46a4-4f26-a039-cfb07ac57072" />

> 손님의 입장부터 주문, 식사, 결제 및 퇴장까지의 전체 프로세스를 관리하는 타이쿤 장르의 식당 운영 게임입니다.
> 객체 간의 결합도를 낮추는 아키텍처 설계와 원활한 데이터 전달 프로세스, 그리고 원활한 팀 협업 환경 구축에 집중했습니다.

<br>

## Tech Stack
* **Engine:** Unity 3D
* **Language:** C#
* **Version Control:** Git / GitHub

<br>

## Implemented Features & Contributions (주요 구현 기능)

### 1. 생산자-소비자 모델 기반 주문 파이프라인 구축
* **손님 → 주문 정보 → 음식 DB → 주문 확인 UI → 셰프**로 이어지는 선형적 주문 파이프라인 아키텍처를 설계하고 전반부 로직을 구현했습니다.
* 비동기 작업 큐(FCFS)와 생산자-소비자 모델을 활용하여 주문 트래픽이 몰리더라도 객체 간 결합도 없이 안전하고 확장성 있게(Scalable) 처리되도록 설계했습니다.

### 2. 테이블 관리 시스템 및 손님 그룹 제어
* `TableManager'와 'Table'을 통해 맵 위치에 따른 테이블 위치를 매핑하고 손님 그룹의 빈자리 탐색부터 착석, 식사, 퇴장까지의 동선을 통합 제어하는 시스템을 구현했습니다.

### 3. 오브젝트 풀링(Object Pooling)을 통한 자원 최적화
* `VisitorPool`과 `VisitorSpawner`를 구축하여 빈번하게 생성되고 파괴되는 손님 객체의 가비지 컬렉션 부하를 최소화하고 런타임 성능을 확보했습니다.

### 4. Git 협업 환경 세팅 및 병합 충돌(Conflict) 해결 주도
* 초기 Unity-GitHub 연동을 주도하고, 게임 개발이 처음인 팀원을 위해 아키텍처 의도를 설명하는 오프라인 온보딩을 진행했습니다.
* 패키지 매니저 협업 중 발생한 `manifest.json` 및 `package-lock.json` 병합 충돌 문제를 `.gitattributes`의 `merge=union` 드라이버 설정과 유니티 패키지 의존성 트리 초기화를 통해 로컬 환경 종속성을 안전하게 동기화하며 해결했습니다.

<br>

## Retrospective & Refactoring Plans (회고 및 개선 계획)

단순 구현에 멈추지 않고, 완성된 코드를 리뷰하며 구조적 한계를 분석하고 최적화된 아키텍처 설계안을 도출했습니다. 

* **설계 개선 1 - 손님 상태 제어의 한계 분석과 Queue 기반 FSM 설계**
  * **한계:** 초기 구현 시 손님의 행동 제어가 분산되어 있어, 외부 요인(종업원, 요리 등)에 의한 상태 변화 대응 및 확장이 어려웠습니다.
  * **개선 계획:** 비동기적인 행동 예약이 가능하도록 **명령 큐(Command Queue)와 FSM(유한 상태 머신)을 결합**하여, 간섭 없이 안전하게 상태를 전이하는 상태 구조를 새롭게 설계했습니다. 

* **설계 개선 2 - 단일 책임 원칙(SRP) 위배 분석과 객체 책임 분리**
  * **한계:** `VisitorOrder` 객체가 DB 접근과 데이터 전달 역할을 동시에 수행하여, DB 스키마 변경 시 주문 로직까지 수정해야 하는 강한 결합(Tight Coupling) 문제가 있었습니다.
  * **개선 계획:** 코드 재사용성과 유지보수성을 높이기 위해 데이터 접근자(DAO) 클래스를 별도로 분리하여 권한을 격리하는 아키텍처 개선안을 도출했습니다.
  
 상세한 리팩터링 의사코드(Pseudocode)는 포트폴리오에 정리해 두었습니다. 

<br>

## 🔗 Links
* [상세 포트폴리오 (Notion)]([포트폴리오 링크 삽입](https://www.notion.so/35ba6c1ebfc780a29f4edd94e2cbcf60?source=copy_link))
