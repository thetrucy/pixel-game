# AI Interaction Log

This log tracks significant interactions with AI models.

## Phase 1: Conceptualization & Architectural Design

**Prompt** : My team will create a 2D game project with 4 members int 10 weeks. We are beginner and want to use Unity and github for and collaboration. Give us some advice of what to do

**Response**:Of course. Here's some advice for your team.

Your biggest challenge in 10 weeks won't be programming a specific feature; it will be managing **scope** and **collaboration**. With beginners, simple tasks take longer than you think. Your goal is to **finish a small, polished game**, not to start a large, unfinished one.

---

### Week 1: Your Foundation 📝

This is your most important week. Do not start coding features yet.

1.  **Aggressively Cut Your Scope:** Take your initial game idea and cut it in half. Now cut it in half again. That's your 10-week project. A simple, single-level arcade game (like a top-down shooter or a basic platformer) is perfect. Avoid RPGs, complex inventory systems, or online multiplayer.
2.  **Create a "GDD-Lite":** Make a simple Game Design Document. This can be a 2-3 page Google Doc that answers:
    * **What's the core loop?** (e.g., "Player moves, shoots asteroids, avoids getting hit, picks up power-ups.")
    * **What are the controls?** (e.g., "WASD to move, Mouse to aim, Left-Click to shoot.")
    * **What are the win/loss conditions?** (e.g., "Win by surviving for 2 minutes. Lose if health reaches zero.")
3.  **Define Roles:** Even in a small team, know who is responsible for what. It's okay to wear multiple hats, but have primary owners.
    * **Project Lead/Producer (1 person):** Keeps track of tasks, runs meetings, and ensures the team is sticking to the GDD.
    * **Programmer(s) (1-2 people):** Write the C# scripts.
    * **Artist/Designer (1-2 people):** Creates sprites, UI elements, and assembles levels.
4.  **Set Up Your Tools CORRECTLY:**
    * **Unity:** Everyone on the team **must install the exact same version of Unity** (e.g., 2022.3.15f1). Mismatched versions will cause chaos.
    * **GitHub:** Create one repository on GitHub.
    * **Crucial GitHub Setup:** Before anyone commits, you must set up your repository to work with Unity.
        * Go to [gitignore.io](https://www.toptal.com/developers/gitignore) and generate a `.gitignore` file for Unity.
        * Add this `.gitignore` file to your repository *first*. This tells Git to ignore Unity's large, user-specific library files, preventing major conflicts.

---

### Weeks 2-9: The Development Workflow 👨‍💻

How you work together is key to avoiding frustration. The biggest technical hurdle for teams in Unity is **conflicts**, especially in scene files.

#### **The Golden Rule: Avoid Working in the Same Scene**

If two people change the same scene file (`.unity`) at the same time, merging those changes is a nightmare.

* **Work with Prefabs:** This is the most important habit. Instead of putting a fully-configured player directly into your main game scene, turn it into a **Prefab**.
    * Programmer A works on the "Player" prefab in an empty test scene.
    * Artist B works on the "Enemy" prefab in their own test scene.
    * When the prefabs are ready, one person is responsible for pulling them into the main game scene.
* **Communicate Clearly:** Before you start work, announce it to your team. "I'm going to be editing the Player prefab for the next hour." A simple message on Discord saves hours of fixing conflicts.

#### **A Simple GitHub Flow**

For every new task (e.g., "add player shooting"), follow these steps:

1.  **Pull:** Before you do anything, run `git pull` to get the latest changes from the remote `main` branch.
2.  **Branch:** Create a new branch for your task: `git checkout -b feature/player-shooting`. Never work directly on the `main` branch.
3.  **Work:** Do your work. Create your script, modify your prefab, etc.
4.  **Commit:** Commit your changes with a clear message: `git commit -m "feat: implement basic player shooting mechanic"`.
5.  **Push:** Push your branch to GitHub: `git push -u origin feature/player-shooting`.
6.  **Pull Request (PR):** Go to GitHub and open a Pull Request. Assign at least one other team member to review it. They should check the code and test the feature.
7.  **Merge:** Once the PR is approved, merge it into the `main` branch. The person who made the changes is responsible for resolving any merge conflicts.

---

### Week 10: The Finish Line 🏁

1.  **Feature Freeze:** Absolutely no new features this week. Zero. This week is for **bug fixing, polishing, and building the game.**
2.  **Build Your Game:** Create a standalone build (`.exe` for Windows, etc.). Test it on a computer that doesn't have Unity installed. You'll be surprised what bugs show up.
3.  **Celebrate!** You finished a game. That's a massive achievement. Host it on itch.io, share it with friends, and be proud of what you accomplished together. Good luck!

**Analysis**

----
**Prompt** : We now have ideas for our game: a 2D-sideview like DeadCells or Hollow Knight, but simpler version. What classes should be include, briefly explain how 4 OOP principles would be included in those classes

**Response**: For a simpler 2D side-scroller, you should start with a few core classes. Your main goal is to create reusable and manageable code.

Here are the essential classes you'll likely need:

* Character: A base class that contains shared logic for any living entity.

* Player: Inherits from Character. It will handle player-specific logic like input and unique abilities.

* Enemy: Also inherits from Character. It will manage AI behavior, like patrolling or attacking the player.

* GameManager: A singleton class that manages the overall game state, like score, player lives, and win/loss conditions.

* Weapon: A class to define how a weapon behaves, including its damage, attack speed, and visual effects.

1.  Inheritance

Inheritance allows a class to receive properties and methods from another class. This avoids code duplication.

Example: Both your Player and Enemy classes will need variables for health and moveSpeed, and methods like TakeDamage().

2.  Encapsulation

Encapsulation means bundling data (variables) and the methods that operate on that data into a single unit (a class), and hiding the data from outside access.

3. Polymorphism
   
Polymorphism allows objects of different classes to be treated as objects of a common superclass. It's often used with inheritance or interfaces to perform a single action in different ways.

Example: Your base Character class could have a virtual method called Attack().

The Player class would override Attack() to swing its equipped Weapon.

The Enemy class would override Attack() to perform a simple lunge.

4. Abstraction

Abstraction means hiding complex implementation details and only showing the necessary features of an object. It focuses on what an object does, not how it does it.


**Analysis**
