# TankCommander
This is a simple turn based strategy game. Back attacks do triple damage, so careful positioning is key!
This game was built with **Unity 2018.2.18f1**.

# How to Play
Left Click to move your tanks.
Left Click + A to fire your cannon.
Click 'End Turn' you end your turn.

# Assets Used
 - Tanks! Tutorial (https://assetstore.unity.com/packages/essentials/tutorial-projects/tanks-tutorial-46209)
	 - Models and effects
 - 2D Game Kit (https://assetstore.unity.com/packages/essentials/tutorial-projects/2d-game-kit-107098)
	 - Behavior Tree (BTAI)
 - NavMeshComponents (https://github.com/Unity-Technologies/NavMeshComponents)

# Brief Explanation of the Architecture
## TankController
The **TankController** is the component that actually drives the tank.
It directly manipulates the **NavMeshAgent** to move the tank, and controls the turret rotation and cannon fire.

## ExternalController (PlayerController, AIController)
**ExternalController** is the base class that acts as an interface to the **TankController**.
**PlayerController** changes keyboard and mouse input into commands the **TankController** can understand.
**AIController** contains the behavior tree, and the actions decided in behavior tree are relayed to the **TankController**. In the current implementation, AI is designed to exploit the weak back side of the player tanks whenever possible.

## BattleManager
**BattleManager** instantiates tanks, manages turns, and also checks victory conditions.

## ShellManager
If I were to instantiate a new shell every time a cannon is fired, it would lead to a waste of memory.
Since this is a turn based game, only single cannon shell should be active in the game at all times.
**ShellManager** controls and manages the single cannon shell instance.

# Why Behavior Tree?
Behavior tree is a quick and fast way to implement AI in video games. It is very easy to create complex behaviors by combining smaller behaviors. The behaviors may be changed and rearranged easily, which makes the behavior tree very scalable and quick to implement. Since I am working on this project for a limited time, I wanted a architecture where I can quickly iterate through various AI designs.
However, there are certain problems to behavior trees.

## Deterministic Behavior
Condition evaluation and behaviors flow from left-to-right (or top-to-bottom). This makes AI made with behavior trees very easy to predict. For example, in the current implementation of the tank AI, the AI will always try to move to your behind and attack you whenever possible. This makes behavior tree based agents feel very 'robotic'. In some games, where exploiting the weakness in the AI is part of the fun, this is actually not a problem. However, I think that strategy games require a more robust AI.

Of course, you can always add some 'randomness' to the conditions and behavior sequences. You can also write a complex tree of multiple conditions and branches, but at the end of the day, both methods are not very efficient.

Perhaps a utility based AI can give a more 'organic' feel, but defining the utilities and creating a plausible utility function can be a challenge.

## Lack of Strategy
By design, behavior trees are meant to control single agents. For this reason, using behavior trees may not have been a suitable choice for implementing a turn based strategy game.

However, I think that this weakness can be mitigated by implementing a **AI Director**, which oversees the total strategy, and choosing appropriate targets for each AI agent.

# To Do List
- Improved UI
	- Show move range and fire range
	- Alert players when action point is low
- Improved AI
	- **AI Director** that overlooks the whole battle field and assigns appropriate targets to each AI tank
- Complete Game Loop
	- Intro -> Main Game -> Restart -> Main Game ...
