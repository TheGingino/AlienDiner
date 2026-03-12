# AlienDiner

### Beschrijving
De opracht is dat we een Mobile game moeten maken waar 3 keuzes voor gegeven werden. We moeten de gekozen game maken met onze eigen style met bepaalde kriteria en we moeten onze eigen twist brengen aan het spel.

Wij kozen de game Diner Dash waar we als een serveerster spelen om klanten te serveren en dan geld te verdienen en blije klanten te hebben. Dus wij kozen ervoor om de ruimte te gebruiken als locatie en thema. Onze twist is dat je een Drive Through hebt die je in de gaten moet houden omdat daar een nieuw soort klant spawnt die ook eten wilt. De kriteria is dat wij 3 klanten soorten hebben, audio voor het spel, visuele feedback en een manier om eten te bereiden.

# Geproduceerde Game Onderdelen

Gino Schaap:
  * [Customer Types](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Customer/CustomerSO.cs)
  * [Customer Spawning](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Customer/CustomerSpawner.cs)
  * [DriveThrough Customer Spawning](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Customer/DriveThroughCustomer.cs)
  * [Customer Behavior](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Customer/Customer.cs)
  * [LevelTimer V2](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Timer/LevelTimer.cs)
  * [WinLoseScreen](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/UI/WinLoseScreen.cs)
  * [Failure Consequence](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Timer/Timer.cs)


Julie Jaasma:
  * [Cooking stations](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Gerechten/InteractionManagers/InteractableObject.cs)
  * [Player Interactions](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Gerechten/InteractionManagers/PlayerInteraction.cs)
  * [Station Clikcer](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Gerechten/InteractionManagers/StationClickHandler.cs)
  * [Customer Order](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Gerechten/OrderingFood.cs)
     

Nikki van Wijngaarden:
 * [PlayerMovement](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Character/PlayerMovement.cs)
 * [TapMarker](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Character/TapMarker.cs)
 * [tapMarkerPool](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Character/TapMarkerPool.cs)
 * [CustomerDragManager](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Customer/CustomerDragManager.cs)
 * [CustomerSeating](https://github.com/TheGingino/AlienDiner/edit/Develop/README.md)
 * [CustomerVisualFeedback](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Customer/CustomerVisualFeedback.cs)
 * [Table](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Customer/Table.cs)
 * [SeatHoverManager](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Customer/SeatHoverManager.cs)
 * [SeatHighLight](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Customer/SeatHighLight.cs)

Kiana Hiemstra:
  * Audio
  * [Timer V1](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Timer/LevelTimer.cs)

Bo Bakker:
 * Character Models
 * Character animations
 * Drive through models
 *  all particle systems
 

Robin van Wandelen:
 * UI onderdelen
 * Booth models
 * Ramen/deur model
 

 Gui
 * Alle textures
 * Fries/milkshake model

Min van der Veen:
 * Blockout
 * Keuken Instrumenten
 * Outside enviroment
 * Props
   


## Customers and Customer SO by Gino Schaap

de CustomerSO is waar de soorten klanten worden in staan en er staat in hoeveel wachttijd hij heeft en hoeveel geld hij geeft.
In de Customer script staat zijn gedrag in. Wanneer hij kiest om weg te lopen of dat hij eten wilt en uiteindelijk weg gaat en geld achterlaat.

![Animation]("gif van de klanten")

### Flowchart voor CustomerSO:
```mermaid
classDiagram
    class CustomerSO {
        +CustomerType customerType
        +GameObject customerPrefab
        +float customerTimer
        +float customerFoodTimer
        +int customerMoney
        -OnEnable()
    }

    class CustomerType {
        <<enumeration>>
        AVERAGE
        ANNOYING
        PATIENT
        DRIVETHROUGH
    }
    CustomerSO --> CustomerType : uses


```

### Flowchart Customer by Gino Schaap
```mermaid
flowchart TD
    A([Customer Spawned]) --> B[Start]
    B --> C[Find WaypointToLeave\nFind Slider\nGet Animator\nRegister with Timer]
    C --> D{Update Loop}

    D -->|State == HUNGRY\n& coroutine not started| E[StartCoroutine CustomerState\nSet _hasCoroutineStarted = true]
    D -->|Otherwise| D

    E --> F{Check customerType}
    F -->|ANNOYING| G[AnnoyingCustomer]
    F -->|AVERAGE| H[NormalCustomer]
    F -->|PATIENT| I[PatientCustomer]

    G & H & I --> J[CustomerBehavior Loop\nwaitTime > 0]

    J --> K[DecreaseSliderValue]
    K --> L{hasBeenServed?}

    L -->|Yes| M[Animator: Sit = true\nState = SERVED\nBreak loop]
    L -->|No| N[waitTime -= deltaTime]
    N --> J

    J -->|waitTime <= 0| O{!hasBeenServed\n& slider <= 0\n& ANNOYING type?}
    O -->|Yes| P[State = LEAVING\nSet angry waypoints\nInvoke hasLeftAngry]
    O -->|No| Q([End - Customer stays idle])

    P --> R[LeaveRestaurant\nStartCoroutine MoveToExit]

    M --> S[ServeFood called externally]
    S --> T[hasBeenServed = true\nState = SERVED\nHide orderImage\nStartCoroutine EatThenLeave]

    T --> U[Wait 5 seconds]
    U --> V[State = LEAVING\nSet normal waypoints\nDropMoney\nLeaveRestaurant]
    V --> R

    R --> W[Animator: Walk = true]
    W --> X{Reached all\nwaypoints?}
    X -->|No| Y[MoveTowards next waypoint\nRotate towards waypoint]
    Y --> X
    X -->|Yes| Z[Animator: Walk = false]
    Z --> AA([Destroy gameObject])

    style A fill:#4CAF50,color:#fff
    style AA fill:#f44336,color:#fff
    style Q fill:#9E9E9E,color:#fff
    style M fill:#2196F3,color:#fff
    style P fill:#FF9800,color:#fff
    style V fill:#9C27B0,color:#fff
```
## Waypoints by Gino Schaap
De waypoints zijn er voor de klanten om het gebouw te kunnen verlaten. de eerste is voor een van de klanten soorten om het gebouw vervroegd te verlaten en de inside is voor de normale klant als ze klaar zijn met eten en de drive through voor de drivethrough klant om weg te gaan

### Class Diagram voor de Waypoints:

```mermaid
classDiagram
    class WaypointToLeave {
        +GameObject[] waypointToLeave
        +GameObject[] insideWaypointToLeave
        +GameObject[] driveThroughWaypointToLeave
    }

```
## Reward System by Gino Schaap
Dit is de manier hoe het hoeveelheid geld dat je hebt gemaakt en de hoeveelheid customers op je scherm staat die zich aanpast als er wat bij komt

## Failure Consequence
Als speler gaat er tijd van je klok af als een klant boos weg loopt als een negatief effect in het spel

![DecreaseTimerGif](https://github.com/TheGingino/AlienDiner/blob/Develop/GitVisuals/Gino/DecreaseTimer.gif)

## Timer V2 by Gino Schaap
De timer is er als een tijds limiet voor de speler zodat ze lichtelijk gehaast de klankten moeten serven en geld moeten verzamelen voordat de tijd voorbij is

### Class Diagram voor Timer V2 by Gino Schaap
```mermaid
classDiagram
    class RewardSystem {
        -int money
        -int customerServed
        -TextMeshProUGUI moneyText
        -TextMeshProUGUI customerServedText
        +AddMoney(int amount)
        +IncrementCustomerServed()
        -UpdateUI()
    }

    class Timer {
        -Image clockImage
        -TextMeshProUGUI timerText
        -float startTime
        -float currentTime
        -bool isRunning
        -static Timer _instance
        +static RegisterCustomer(Customer)
        +ApplyPenalty()
        -UpdateDisplay()
        -OnTimerComplete()
    }

    class Customer {
        +CustomerSO CustomerSO
        +UnityEvent HasLeftAngry
        +ServeFood()
        +SetDesiredDish(DishType)
        +IsWaitingFor(DishType) bool
    }

    Timer --> Customer : listens to HasLeftAngry
    RewardSystem ..> Customer : tracks served
```

![Timer](https://github.com/TheGingino/AlienDiner/blob/Develop/GitVisuals/Gino/TimerUI.gif)

## Win Lose Screen by Gino Schaap
Dit scherm komt naar boven als de timer om is en hij laat het verdiende geld zien, of je genoeg klanten hebt geserveerd en er staat of je hebt gewonnen of hebt verloren.

![WinLoseScreen](https://github.com/TheGingino/AlienDiner/blob/Develop/GitVisuals/Gino/LevelEnded.gif)

###
```mermaid
---
config:
  layout: elk
---
flowchart TD
    A([CheckWinLoseCondition called]) --> B[Find RewardSystem]
    B --> C{rewardSystem != null?}
    C -->|No| D([Do nothing])
    C -->|Yes| E{customerServed >=\ncustomersNeededToWin?}

    E -->|Yes| F[ShowWinScreen]
    E -->|No| G[ShowLoseScreen]

    F --> H[UpdateWinScreenStats\nTime.timeScale = 0]
    G --> I[UpdateWinScreenStats\nTime.timeScale = 0]

    H & I --> J[Find RewardSystem again]
    J --> K{rewardSystem != null?}
    K -->|No| L[Skip stats update]
    K -->|Yes| M{customerServed >=\ncustomersNeededToWin?}

    M -->|Yes| N[StatusText = YOU WIN!]
    M -->|No| O[StatusText = YOU LOSE!]

    N & O --> P[Update MoneyText\nUpdate CustomerText]
    L & P --> Q[winLoseScreen.SetActive true]

    R([MainMenu called]) --> S[Time.timeScale = 1\nLoad StartScreen]
    T([ReloadLevel called]) --> U[Time.timeScale = 1\nLoad Main]

    style A fill:#2196F3,color:#fff
    style D fill:#9E9E9E,color:#fff
    style N fill:#4CAF50,color:#fff
    style O fill:#f44336,color:#fff
    style R fill:#9C27B0,color:#fff
    style T fill:#9C27B0,color:#fff
    style S fill:#9C27B0,color:#fff
    style U fill:#9C27B0,color:#fff
```


## PlayerMovement by Nikki van Wijngaarden

Een NavMesh based movementsysteem dat spelers overal op de grond laat klicken om daarnatoe te lopen, en ook kort laat zien waar je hebt geklickt.

#gifje?

## Flowchart — PlayerMovement

```mermaid
flowchart TD
    A([Speler tikt / klikt]) --> B[ScreenPointToRay]
    B --> C{Raycast raakt\nInteractableObject?}
    C -- Ja --> Z([Negeer invoer])
    C -- Nee --> D{Raycast raakt\nGround layer?}
    D -- Nee --> Z
    D -- Ja --> E{Geldig NavMesh\npunt gevonden?}
    E -- Nee --> Z
    E -- Ja --> F[agent.SetDestination]
    F --> G[TapMarker spawnen\nop bestemming]
    G --> H[TapMarker animeert\nen keert terug naar pool]
    F --> I[Agent loopt naar bestemming]
    I --> J{Beweging\nvia script?}
    J -- Nee --> K{velocity\ngroter dan 0.01?}
    K -- Ja --> L[Animator Walk = true]
    K -- Nee --> M[Animator Walk = false]
    J -- Ja --> N[Wacht op aankomst]
    N --> O[Warp naar exacte positie\nisStopped = true]
    O --> P{Aanroeper}
    P -- MoveToInteraction --> Q[StartInteraction]
    P -- MoveToTable --> R[TryServeCustomersAtTable]
```

---

## classDiagram - PlayerMovement

```mermaid
classDiagram
    class PlayerMovement {
        -NavMeshAgent _agent
        -Camera _camera
        -LayerMask groundLayer
        -TapMarkerPool _tapMarkerPool
        -Animator _animator
        -MoveToPosision(screenPosition)
        -MoveToTarget(target, OnArrive)
        -CheckArrival(target, OnArrive)
        +LockPlayerMovement(enabled)
        +MoveToInteraction(station)
        +MoveToTable(servepoint, tableTransform)
    }

    class TapMarkerPool {
        -GameObject tapMarkerPrefab
        -int poolSize
        -Queue~GameObject~ _pool
        +GetMarker() GameObject
    }

    class TapMarker {
        -float lifetime
        -AnimationCurve scaleCurve
        -float timer
    }

    class InteractableObject {
        +Transform InteractionWaypoint
    }

    class PlayerInteraction {
        +StartInteraction(station)
        +TryServeCustomersAtTable(tableTransform)
    }

    PlayerMovement --> TapMarkerPool : gebruikt
    PlayerMovement --> InteractableObject : raycasts en negeert
    PlayerMovement --> PlayerInteraction : triggert via callback
    TapMarkerPool --> TapMarker : beheert pool van
```

## CustomerSeating by Nikki van Wijngaarden

Een drag-and-drop systeem waarmee customers kunt op pakken en neer zetten bij een tavel, waarna ze gaan zitten op de stoel dichts bij waar je ze neer hebt gezet. Met visual feedback op de customer bij het oppakken en bij het hoveren boven een stoel.

#gifje?

## Flowchart - CustomerSeating

```mermaid
flowchart TD
    A([Speler klikt / tikt]) --> B[Raycast op CustomerLayer]
    B --> C{Customer geraakt?}
    C -- Nee --> Z([Negeer invoer])
    C -- Ja --> D{CanBeDragged?}
    D -- Nee --> Z
    D -- Ja --> E[_draggedCustomer instellen\nOnDragStart schaalanimatie]

    E --> F[Speler beweegt vinger / muis]
    F --> G[Raycast op GroundLayer]
    G --> H[SetDraggedPosition\nmet hoogte offset]
    H --> I[SeatHoverManager.UpdateHover]
    I --> J{Vrije stoel geraakt?}
    J -- Ja --> K[SeatHighLight.Show]
    J -- Nee --> L[SeatHighLight.Hide]
    H --> F

    E --> M([Speler laat los])
    M --> N[Raycast op TableLayer]
    N --> O{Tafel geraakt?}
    O -- Nee --> P[ReturnToOrigin]
    O -- Ja --> Q{HasFreeSeat?}
    Q -- Nee --> P
    Q -- Ja --> R[TrySeatCustomer\ndichtstbijzijnde vrije stoel]
    R --> S{Stoel gevonden?}
    S -- Nee --> P
    S -- Ja --> T[occupied = true\nSnapToSeat]
    T --> U[Draai naar tafel\ncanBeDragged = false\nhasBeenSeated.Invoke]

    P --> V[OnDragEnd animatie\nClearHighLight\n_draggedCustomer = null]
    U --> V
```

---

## Class Diagram

```mermaid
classDiagram
    class CustomerDragManager {
        -SeatHoverManager _seatHoverManager
        -LayerMask customerLayer
        -LayerMask tableLayer
        -LayerMask groundLayer
        -Camera _camera
        -CustomerSeating _draggedCustomer
        -StartDrag(screenpos)
        -Drag(screenpos)
        -EndDrag(screenpos)
    }

    class CustomerSeating {
        -Transform _currentSeat
        -Table _currentTable
        -Vector3 _originPosition
        -bool _canBeDragged
        +bool IsSeated
        +UnityEvent hasBeenSeated
        +CanBeDragged() bool
        +SetDraggedPosition(pos)
        +SnapToSeat(seat, table)
        +ReturnToOrigin()
        +LeaveSeat()
    }

    class CustomerVisualFeedback {
        -float dragScaleMultiplier
        -float scaleSpeed
        -Vector3 _OrginalScale
        +OnDragStart()
        +OnDragEnd()
    }

    class Table {
        -List~Transform~ seats
        -bool[] _occupied
        +HasFreeSeat() bool
        +TrySeatCustomer(customer, dropPosition) bool
        +IsSeatOccupied(seat) bool
        +FreeSeat(seat)
    }

    class SeatHoverManager {
        -LayerMask seatLayer
        -SeatHighLight _currentHighLight
        +UpdateHover(screenPosition)
        +ClearHighLight()
    }

    class SeatHighLight {
        -GameObject highLightVisual
        -Table _table
        -Transform _seat
        +CanHighLight() bool
        +Show()
        +Hide()
    }

    CustomerDragManager --> CustomerSeating : sleept
    CustomerDragManager --> SeatHoverManager : gebruikt
    CustomerDragManager --> Table : raycasts
    CustomerSeating --> Table : referentie
    CustomerSeating --> CustomerVisualFeedback : GetComponent
    SeatHoverManager --> SeatHighLight : beheert
    SeatHighLight --> Table : IsSeatOccupied
    Table --> CustomerSeating : SnapToSeat
```


## Water Shader by Student Y

Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of "de Finibus Bonorum et Malorum" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, "Lorem ipsum dolor sit amet..", comes from a line in section 1.10.32.

![](https://user-images.githubusercontent.com/1262745/189135129-34d15823-0311-46b5-a041-f0bbfede9e78.png)

## Blockout by Min

Robin had een paar schetsen gemaakt van layouts van een diner. De gekozen layout daarvan heb ik snel een blockout van gemaakt en goed opgelet op scale. Ik heb me goed aan de schets gehouden.

<img width="650" height="324" alt="Blockout" src="https://github.com/user-attachments/assets/7b919c64-7b48-4e4a-9d79-c0b22a22d2ec" />

## Keuken Instrumenten by Min
In het begin had ik Robin de keuze gegeven tussen de diner en de keuken kant. Het maakte mij niet uit en Robin koos voor de diner kant dus ik dee de keuken kant. Dat houd in: Koelkast, fornuis, frituur, milkshame machine, werkbank 1 en 2, drive through packing station, bar vloer, bar, barstoel. We moeten alles aan een 1950 retero thema houden.

### Koelkast
Ik begon met het koelkast model. Mid priority voor de keuken, heeft geen interactie met de speler of customer. 

<img width="650" height="324" alt="Koelkast" src="https://github.com/user-attachments/assets/a32ebbcf-867d-4e64-9666-9b920c9bd6ee" />

### Fornuis
Na de koelkast ben ik begonnen met de high priority models. Je kan er mee interacten als speler want daar kook je de burger. 

<img width="650" height="324" alt="Fornuis" src="https://github.com/user-attachments/assets/19471a64-cf1f-4fab-b93f-9dc185753bd5" />

### Frituur
Met dit model kan de speler interacten want daar maak je de frietjes.

<img width="650" height="324" alt="Frituur" src="https://github.com/user-attachments/assets/2068b2b6-f321-4ad9-8de5-83f38921e49e" />

### Milkshake Machine
Met dit model kan de speler interacten want daar maak je de milkshake.

<img width="650" height="324" alt="Milkshake machine" src="https://github.com/user-attachments/assets/23408433-2b78-4a9b-9b2b-0c7d3eb085a1" />

### Werkbank 1 en 2
Naast de interactable modellen heb ik meer surfaces nodig. Op werkbank 1 staat de milkshake machine en op werkbank 2 staat de drive through packing station.

<img width="650" height="324" alt="Werkbank 1" src="https://github.com/user-attachments/assets/656d4a15-57fa-41c7-a168-e313674f3d75" />
<img width="650" height="324" alt="Werkbank 2" src="https://github.com/user-attachments/assets/3907cb52-f4f5-42ce-93ec-08cecfcf50ca" />

### Drive through packing station
Wij hadden als twist een drive through. Dus er moest ook een packing station waar de speler het eten moet inpakken. Met dit model kan de speler interacten want daar pak je het eten in. 

<img width="650" height="324" alt="Packing station" src="https://github.com/user-attachments/assets/bfee8eee-a798-4d98-be76-a58e31c5dfe2" />

### Bar vloer, Bar en de Barstoel
Je hebt een klein opstapje in de vloer om de area van de keuken en diner area uit elkaar te houden. Daarnaast moest de bar en de barstoelen. De customer kan interacten om daar te zitten en eten.

<img width="650" height="324" alt="Bar, vloer en barstoel" src="https://github.com/user-attachments/assets/3791b87d-987c-4145-b8e0-e390cea446ac" />

## Props by Min
Daarnaas had ik nog 1,5 weken over nadat ik de blockout en keuken allemaal af had had ik ook een paar props gedaan voor de diner kant. 

### Burger
We hebben 3 gerechten. Ik heb de burger gekozen. Het is een hersen burger met een plakje kaas.

<img width="650" height="324" alt="Burger" src="https://github.com/user-attachments/assets/46abb109-ac86-4104-ae0d-eae162677469" />

### Dirty dishes + geld
Als de customer klaar is met eten dan lopen ze weg. Ze laten dan wel een vies bord met geld achter.

<img width="750" height="324" alt="Bord met geld" src="https://github.com/user-attachments/assets/35999666-76d1-49a5-bd18-10021f2faee1" />

### Prullenbak 
Als je eten heb gemaakt wat verkeerd is/ niet het goede gerecht dan kan je het hier weg gooien. Met dit model kan de speler interacten want je kan gerechten weggooien.

<img width="650" height="324" alt="Prullenbak" src="https://github.com/user-attachments/assets/2001b8d9-1c23-40f5-88b9-bb03f9544f36" />

### Juke box
Decoratief model waar de background music uit komt.

<img width="650" height="324" alt="Juke box" src="https://github.com/user-attachments/assets/0ba67e87-cb3b-45e7-a2a3-7bb291b9a94e" />

### Deur bell
Decoratief model wat ringt als er een customer binnen komt.

<img width="650" height="324" alt="Deur bel" src="https://github.com/user-attachments/assets/a9870bc4-b6da-48d1-9de3-4ebd15c48388" />

## Outside Environmnet by Min
Ik was snel klaar met de props dus op het laatst wist ik even niet meer wat ik zou doen. Niet voor lang want Bo had me verteld dat er ook nog een outside environment worden zouden gemodeld. Dus ik heb zelf designs gemaakt wat een beetje Alien environment is.

### Boom
Decoratief model. Ik had een paar bloem en planten modellen gemaakt. Ik heb dan een boomstam gemaakt en alles een beetje geprobeerd welke compositie het best past. 

<img width="650" height="324" alt="Boom" src="https://github.com/user-attachments/assets/8c8ac493-212b-41ca-9c03-3f928fcaf398" />

### Struik
Decoratief model. Ik had een paar bloem en planten modellen gemaakt. Ik heb dan een boomstam gemaakt en alles een beetje geprobeerd welke compositie het best past.   

<img width="650" height="324" alt="Struik" src="https://github.com/user-attachments/assets/d2c1d13f-1239-450b-b5a0-4d9987916213" />

### Stenen
Decoratief model. Ik heb beide ronde en vierkante stenen geprobeerd. Ik heb gekozen voor de vierkante stenen.

<img width="850" height="324" alt="Stenen" src="https://github.com/user-attachments/assets/726ca362-c4db-47fd-8bd5-d4b756ef96a7" />

### Eiland
Heel erg simpel model als de grond voor de scene.

<img width="650" height="324" alt="Eiland" src="https://github.com/user-attachments/assets/5a5b98b5-55e6-4548-b9ec-1d2949f25a2d" />


## Character models + temporary textures by Bo
Voor het spel zijn er minstens 3 klanten nodig en 1 main character. Daarvan heb ik de models en tijdelijke textures gemaakt.

### Main character 
Dit is de main character. Met deze character speelt de speler en kan de speler klanten bedienen. 

<img width="200" height="350" alt="Screenshot 2026-03-12 224757" src="https://github.com/user-attachments/assets/0087f1bd-05d7-4dcf-b8de-607371bfbcfa" />

### Neutral klant + Temporary texture
Deze klant is de basis-type klant. Hij ziet er neutraal uit en gedraagt zich ook zo.

<img width="325" height="355" alt="Screenshot 2026-03-12 224735" src="https://github.com/user-attachments/assets/ecc646d5-b6bc-4126-bb23-56ff8bd861fc" /> <img width="375" height="378" alt="Screenshot 2026-03-12 231345" src="https://github.com/user-attachments/assets/a3c52963-460d-4ef0-b92d-777f7e88970b" />



### Geduldige klant + Temporary texture
Dit is een klant die langer wacht op hun bestellingen en minder snel boos wordt. Ik heb ronde vormen gebruikt om een vriendelijkere uitstraling te geven.

<img width="325" height="350" alt="Screenshot 2026-03-12 224840" src="https://github.com/user-attachments/assets/28e456ba-149a-469a-bd1a-b37272aa77e2" /> <img width="410" height="406" alt="Screenshot 2026-03-12 231320" src="https://github.com/user-attachments/assets/0b88fcbc-1773-4257-bb9a-93ce7e5e03e9" />


### Ongeduldige klant + Temporary texture
Deze klant zal erg snel boos worden en sneller weglopen uit het restaurant. Ik heb hem een driehoekige bouw gegeven en een boos gezicht zodat hij ongeduldigheid uitstraalt.

<img width="325" height="350" alt="Screenshot 2026-03-12 224855" src="https://github.com/user-attachments/assets/9372b681-d83c-4396-a2f9-1dc3cf373c47" /> <img width="448" height="415" alt="Screenshot 2026-03-12 231315" src="https://github.com/user-attachments/assets/09973352-0850-4626-9178-ce1458c00bfd" />


## Character animations by Bo 

### Walking
Dit is de alien en de main character die lopen. Het enige verschil tussen de twee is dat de main character een dienblad vast heeft.

![walk_1](https://github.com/user-attachments/assets/f5b2eb2d-1aee-4749-a108-a3e3b2b6808b) ![walk_2](https://github.com/user-attachments/assets/59547995-c824-43b7-9030-453a18ec79d7)

### Sitting down
Dit is de animatie van het gaan zitten. 

![sit_1](https://github.com/user-attachments/assets/0d2d34a1-caa1-416b-aa21-45fd67c70d11)

### Eating
Dit is de animatie van de aliens die eten. 

![Eat_1](https://github.com/user-attachments/assets/8051998a-4a3b-44c9-b766-2f1b1dc79b4e)

## Drive through models by Bo

### Drive through window
Dit is het raam waardoor aliens in een UFO eten kunnen bestellen. 

<img width="300" height="250" alt="Screenshot 2026-03-12 224603" src="https://github.com/user-attachments/assets/4fcf85c0-486f-4259-b46a-18210a889e43" />

### UFO 
Dit is het model van de UFO. In dit model zit de alien die bij de drive through besteld. 

<img width="330" height="255" alt="Screenshot 2026-03-12 224134" src="https://github.com/user-attachments/assets/e208f3da-406d-4dd1-8ff3-5b0db649e0ad" /> <img width="330" height="255" alt="Screenshot 2026-03-12 224127" src="https://github.com/user-attachments/assets/b704bb82-00b7-4ca0-942e-92960d7aa292" />



## Particle systems by Bo

### Fryer particle system
Deze particle system laat zien dat je de fryer aan het gebruiken bent.

### Stove particle system
Deze particle system laat zien dat je de stove aan het gebruiken bent.

### Milkshake machine particle system
Deze particle system laat zien dat je de milkshake machine aan het gebruiken bent.

### Packing station particle system
Deze particle system laat zien dat je de packing station aan het gebruiken bent.

### Jukebox particle system
Deze particle system laat zien dat de jukebox aan staat en muziek maakt.


