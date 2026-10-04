# Tourmi Framework
Personal repo for general-use code in .NET projects

No guarantees given if you use this repo or these packages.

## Tourmi.Framework
Utility and general purpose code for any .NET project.

- Tourmi.Framework.Extensions
  - Contains useful extension methods for C# types
- Tourmi.Framework.GuardClauses
  - Contains Fluent guard clauses for C# types
- Tourmi.Framework.Reflection
  - Contains Reflection utilities
- Tourmi.Framework.ServiceProviders
  - Personal implementation of a lazy service provider

## [BETA] Tourmi.Coroutines
ValueTask-like type for use in games (or applications with an update loop), 
allowing for GC-free async code that can be contextualized per update loop 
(ie: Variable Update, Fixed Update, etc).

- Used through the CoroutineContext class.
  Coroutines can only be queued within a CoroutineContext.Enter() `using` scope, and will only update once the scope is exited.

### TODO
- Write tests
    - Integration (ensure resources are properly freed, even when a coroutine isn't awaited)
    - Benchmark (ensure there is no garbage collector pressure)
- Add a way to persist the result of a coroutine, allowing to await it multiple times (coroutine.Persist()?).

## Tourmi.Monogame
Contains useful extension methods and helper functions for Monogame/XNA
- Tourmi.Monogame.Tweens
  - Tweening for properties and fields via `TweenFactory`

## Tourmi.Aseprite
Models for Serialization/deserialization of Aseprite json data
- For Spritesheet exports in version 1.3 of Aseprite

## [BETA] Tourmi.EntityComponentSystem
Implementation of an ECS for personal projects and learning.

Refer to sample projects for example uses.

### Todo
- Implement OnAdd, OnModified, OnRemove events.
  - Allow subscribing to those events via queries
    - Query for matching, and query for additional values
  - OnSet should ideally have the before and after values.
- Implement Querying by Relation Type, and by Relation Target (ie: `Relation<ChildOf, *>` or `Relation<*,Target>`)
- Implement ChildOf relation type
  - Kill child if parent is killed
- Implement ordered structures
- Implement dependency tree structure
  - Make registered systems use dependency tree structure for ordering
- Implement automatic parallelization of systems

### Maybe
- Create Source Generator project which automatically creates ref struct types representing a Query result.
  - Would allow for less boilerplate in systems

### References and useful reading
- https://ajmmertens.medium.com/doing-a-lot-with-a-little-ecs-identifiers-25a72bd2647 (and related blogs)
