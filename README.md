# FG21GPFT_ComputerTech_Assignment
Performance-aware space shooter using Unity DOTS

## Assignment brief
Create a small space shooter game with the following features:
- Simple movement.
- Shooting.
- Waves of enemies.

The game needs to be programmed in a data-oriented way with the hardware in mind following this criteria:

- Are efficient in their heap allocations and memory usage.
- Use algorithms and loops that are smartly done and minimize negative impact in the user experience when employed at scale.
- Reuse software effectively and contain original code that is easy to understand and used by others.

## Component & Data layout
This is an short overview on how i have structured my project. Now in hindsight i think one of the main issues i had is to
get away from an OOP mindset.

![Component Systems](https://user-images.githubusercontent.com/85444462/208070702-b8b0d048-b12c-4fb8-968a-a2687a06b095.png)

## Data Authoring
I used a mix of custom authoring scripts & using the [[GenerateAuthoringComponent]](https://docs.unity3d.com/Packages/com.unity.entities@0.51/manual/gp_overview.html) attribute for my components.   
The custom authoring was used on components that i felt would needed more controll, especially if this was a collab project with other people it is a nice touch
to make them able to set data in the editor.

Using [GenerateAuthoringComponent] attribute:
- PrefabPlayer.cs
- PrefabEnemy.cs
- PrefabProjectile.cs

Using custom authoring:
- GameSettings using GameSettingsAuthoring.cs
- PlayerInput using PlayerInputAuthoring.cs
- Velocity using VelocityAuthoring.cs
- Weapon using WeaponAuthoring.cs
- Lifetime using LifetimeAuthoring.cs
