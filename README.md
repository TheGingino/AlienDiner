# AlienDiner
Aliens in een Diner

In deze repository vind je de informatie over het examen project.

Omschrijf de examenopdracht evt de klant en wat het doel voor de klant is.
Omschrijf ook beknopt wat het idee van je game is. 
Een complete en uitgebreide beschrijving komt in het functioneel ontwerp (onderdeel van de [wiki](https://github.com/erwinhenraat/VoorbeeldExamenRepo/wiki))

# Geproduceerde Game Onderdelen

Geef per teammember aan welke game onderdelen je hebt geproduceerd. Doe dit met behulp van omschrijvingen visual sheets en screenshots.
Maak ook een overzicht van alle onderdelen met een link naar de map waarin deze terug te vinden zijn.

Bijv..

Gino Schaap:
  * [Customer Types](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Customer/CustomerSO.cs)
  * [Customer Spawning](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Customer/CustomerSpawner.cs)
  * [DriveThrough Customer Spawning](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Customer/DriveThroughCustomer.cs)
  * [Customer Behavior](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Customer/Customer.cs)
  * [LevelTimer V2](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Customer/Customer.cs)
  * [WinLoseScreen](https://github.com/TheGingino/AlienDiner/blob/Develop/AlienDinerDash/Assets/Scripts/Customer/Customer.cs)

Julie Jaasma:
  * [Some beautifull script](https://github.com/erwinhenraat/VoorbeeldExamenRepo/tree/master/src/beautifull)
  * Some other Game object

Nikki van Wijngaarden:
 * Audio
 * 
Kiana Hiemstra:
  * Water Shader
  * [Some textured and rigged model](https://github.com/erwinhenraat/VoorbeeldExamenRepo/tree/master/assets/monsters)

Bo Bakker:
 * Character Model
 * Customer Models

Robin van Wandelen:
 * UI Shenan
 * 

 Gui
 * IDK

Min van der Veen:
 * Keuken Instrumenten

## Customers and Customer SO

Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of "de Finibus Bonorum et Malorum" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, "Lorem ipsum dolor sit amet..", comes from a line.

![Animation](https://user-images.githubusercontent.com/1262745/217570184-90dc4701-d60d-4816-80d0-5007fdd3f6be.gif)

### flowchart voor CustomerSO:
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

### Flowchart Customer
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

### class diagram voor game entities:

```mermaid
classDiagram
    class WaypointToLeave {
        +GameObject[] waypointToLeave
        +GameObject[] insideWaypointToLeave
        +GameObject[] driveThroughWaypointToLeave
    }

```

### Class Diagram voor Reward System
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


## Some other Mechanic X by Student X

Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of "de Finibus Bonorum et Malorum" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, "Lorem ipsum dolor sit amet..", comes from a line in section 1.10.32.

![example](https://user-images.githubusercontent.com/1262745/189135129-34d15823-0311-46b5-a041-f0bbfede9e78.png)

## Some other Mechanic Y by Student X

Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of "de Finibus Bonorum et Malorum" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, "Lorem ipsum dolor sit amet..", comes from a line in section 1.10.32.

![example](https://user-images.githubusercontent.com/1262745/189135129-34d15823-0311-46b5-a041-f0bbfede9e78.png)

## Water Shader by Student Y

Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of "de Finibus Bonorum et Malorum" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, "Lorem ipsum dolor sit amet..", comes from a line in section 1.10.32.

![example](https://user-images.githubusercontent.com/1262745/189135129-34d15823-0311-46b5-a041-f0bbfede9e78.png)

## Some textured and rigged model by Student Y

Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of "de Finibus Bonorum et Malorum" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, "Lorem ipsum dolor sit amet..", comes from a line in section 1.10.32.

![example](https://user-images.githubusercontent.com/1262745/189135129-34d15823-0311-46b5-a041-f0bbfede9e78.png)

## Some beautifull script by Student Z

Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of "de Finibus Bonorum et Malorum" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, "Lorem ipsum dolor sit amet..", comes from a line in section 1.10.32.

![example](https://user-images.githubusercontent.com/1262745/189135129-34d15823-0311-46b5-a041-f0bbfede9e78.png)

## Some other Game object by Student Z

Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of "de Finibus Bonorum et Malorum" (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, "Lorem ipsum dolor sit amet..", comes from a line in section 1.10.32.

![example](https://user-images.githubusercontent.com/1262745/189135129-34d15823-0311-46b5-a041-f0bbfede9e78.png)
