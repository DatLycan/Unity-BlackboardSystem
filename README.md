
<h1 align="left">Unity C# Blackboard System</h1>

<div align="left">

[![Status](https://img.shields.io/badge/status-active-success.svg)]()
[![GitHub Issues](https://img.shields.io/github/issues/datlycan/Unity-BlackboardSystem.svg)](https://github.com/DatLycan/Unity-BlackboardSystem/issues)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](/LICENSE)

</div>

---

<p align="left"> Blackboard System framework for shared data integration for diverse specialized modules.
    <br> 
</p>

## 📝 Table of Contents

- [Getting Started](#getting_started)
- [Documentation](#documentation)
- [Usage](#usage)
- [Acknowledgments](#acknowledgement)

## 🏁 Getting Started <a name = "getting_started"></a>

### Installing

1. Install the [Git Dependency Resolver](https://github.com/mob-sakai/GitDependencyResolverForUnity).
2. Import it in Unity with the Unity Package Manager using this URL:<br>
``https://github.com/DatLycan/Unity-BlackboardSystem.git``

## 📦 Documentation <a name = "documentation"></a>
| Name                 | Inheritance         | Description                                                                                              |
|----------------------|---------------------|----------------------------------------------------------------------------------------------------------|
| Blackboard           |                     | Data storage that holds values and keys.                                                                 |
| BlackboardObject     | ScriptableObject    | Stores starting values and keys for a Blackboard.                                                        |
| IScholar             |                     | Interface that represents a scholar that can be added to a Chancellor.                                    |
| Chancellor           |                     | Holds a list of IScholars, evaluates their importance, and calls their Execute() method every frame.      |
| BlackboardController | MonoBehaviour       | Manages a BlackboardObject and Chancellor, providing methods for registering and unregistering IScholars. |

## 🎈 Usage <a name="usage"></a>

### Using a Blackboard
   ```C#
    public class MyClassA : MonoBehaviour {
        private readonly Blackboard blackboard = new();
        private BlackboardKey myKey;
    
        private void Awake() {
            myKey = blackboard.GetOrRegisterKey("MyKey");
            blackboard.SetValue(myKey, 10);
        } 
    
        private void Start() {
            blackboard.TryGetValue(myKey, out float value);
            Debug.Log(value);
        }
    }
   ```

### Using a BlackboardObject
   ```C#
    public class MyClassA : MonoBehaviour {
        [SerializeField] private BlackboardObject blackboardObject;
        private readonly Blackboard blackboard = new();
        private BlackboardKey myKey;
    
        private void Awake() {
            blackboardObject.SetValuesOnBlackboard(blackboard);
            myKey = blackboard.GetOrRegisterKey("MyKey");
        }
    
        // Decreases the value associated by "MyKey" by 10 on "A" keypress
        private void Update() {
            if (Input.GetKeyDown(KeyCode.A)) {
                if (blackboard.TryGetValue(myKey, out float value)) {
                    blackboard.SetValue(myKey, value - 10);
                    blackboard.Log(); 
                };
            }
        }
    }
   ```

### Using a BlackboardController
   ```C#
    public class MyClassA : MonoBehaviour, IScholar {
        [SerializeField] private BlackboardController blackboardController;
        private readonly int importance = 100;
        private readonly BlackboardKey myKey = new("MyKey");
        
        private void OnEnable() => blackboardController.RegisterScholar(this);
        private void OnDisable() => blackboardController.UnregisterScholar(this);
        
        public int GetImportance(Blackboard blackboard) => importance;

        public void Execute(Blackboard blackboard) {
            blackboard.AddAction(() => {
                if (blackboard.TryGetValue(myKey, out float value)) {
                    blackboard.SetValue(myKey, value + 10);
                };
            });
        }
    }
   ```
   ```C#
    public class MyClassB : MonoBehaviour, IScholar {
        [SerializeField] private BlackboardController blackboardController;
        private readonly int importance = 50;
        private readonly BlackboardKey myKey = new("MyKey");
        
        private void OnEnable() => blackboardController.RegisterScholar(this);
        private void OnDisable() => blackboardController.UnregisterScholar(this);
        
        public int GetImportance(Blackboard blackboard) => importance;

        // Does NOT get executed because MyClassA's importance is higher!
        public void Execute(Blackboard blackboard) {
            blackboard.AddAction(() => {
                if (blackboard.TryGetValue(myKey, out float value)) {
                    blackboard.SetValue(myKey, value - 10);
                };
            });
        }
    }
   ```
---



## 🎉 Acknowledgements <a name = "acknowledgement"></a>

- *Snippets taken from [adammyhre's Blackboard Architecture video](https://www.youtube.com/watch?v=HNGJ8KOqdYQ).*
- *Using [mob-sekai's Git Dependency Resolver For Unity](https://github.com/mob-sakai/GitDependencyResolverForUnity)*

