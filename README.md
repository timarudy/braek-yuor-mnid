# braek-yuor-mnid
 Game is mainly created just for practising coding and building a strong architecture skills.

## Structure
The game has really easy adjustable levels system, where each level has a certain monitor to enter the code
to go to the next level, in its turn, the code is being given in a notepad which the player has permanently.

### Game life-cycle
The game has a general state machine (bootstrap state, menu state, load level state, game loop state, reward state).

### Zenject
Zenject is used as a powerful DI Container technology with a bunch of other functionalities. However, I think it is a little overloaded
so I use it simply as a DI Container itself.

### Daily rewards
By creating daily rewards I wanted to show that I am also familiar with 2D game development. There you can collect coins and read a fun dog fact
that is being requested from an open api with random dog facts.

## UI

### Windows
I created a really convenient and easily to add new windows system.

### World input
The base for the world input from a player was created by me as well. This way 
this mechanism is also really adjustable, can be broaden easily and implemented to different structures depended on a task.

### Tooltips
Tooltips for the objects that the user can interact with were also created. They are also
not strong hard-coded and can be changed easily for any need of the developer.

## Services

### Sounds
Sound service was created for mainly all the sounds. It is being binded in the bootstrap scene and then can be used 
in literally any class you need.

### Progress data
Some of the level and user data is saved into Player prefs as there is no need in a dedicated database, so
using this service the data can be accessible, but to save and load it there is a special service **save load data service**.

### Scene loader
This service is used obviously for loading scenes.

### Factories
There are several factories: UIFactory, GameFactory, EnemiesFactory in a project.

## Other

### Interaction
I did an interaction system with various objects marked by _Interactable_ tag. This method is
used in lots of places of code.

### Attack
This one is an example of my _interaction-based_ system that is created to attack objects marked by
tag _Attackable_. These can be enemies as well as some other objects such as trees, buildings etc.
One thing also to be mentioned is that there was created a base class fore different attack objects, which also can be added and changed simply.

### Enemies
Enemies are also a really universal thing that can be expanded easily.

# Summary

Thus, I tried my best to make as flexible and maintainable code as I could. 