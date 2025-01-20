## 🚀 Feature Overview

<h1>
    Spawners
</h1>

> Spawners simplify the Instantiation of Components. They also use <i>object pooling</i> for efficiency.

<img src="Documentation/Images/Spawner_Script.png">
<img src="Documentation/Images/Spawner_Inspector.png">

**Spawner**
- Easiest to implement

**SpawnableSpawner**
- Most efficient, but requires the spawning object to inherit **ISpawnable** or **Spawnable**

**MultiSpawner**
- Can spawn objects of different types. Prefab needs to be provided in the **Spawn()** method


<h1>
    SaveSystem
</h1>

> Allows you to save anything as JSON or XML file in the Documents folder of the user

> Quickly save and load a single value (bool, int, float).

<img src="Documentation/Images/SaveSystem_Settings.png">

> Save and load a **Serialized Class**

<img src="Documentation/Images/SaveSystem_Data.png">


Useful [Extensions](Runtime/Extensions.cs)

<h1>
    And more!
</h1>

