# StateCache
This library supports fast and customisable caching of the public properties of an object. The caching object is built dynamically to only include the properties from the object that are required to be cached. This allows the store/restore to only be maginally slower than manually written caching code.

## Motivating Example
There is form of system design in which state is the primary way in which activities are configured. This is often done for performance reasons - you may not want to set all the properties of the system for every activity, so the state from other activities is "reused" and only changes to the specific state values necessary for configuring the activity at hand are changed.

For example a graphics system might draw everything in blue following the setting of the foreground colour to blue. Any call to Draw something will result in a blue entity being drawn until the foreground colour is switched to a different one.

Often a state change is required locally within a drawing object. In order to ensure this state change doesn't impact any objects later in the program flow, the initial value is stored before the local changes are made and restored after the action has been performed.

    void Draw(StatefulGraphicsSystem system)
    {
        // Draw a blue line
        var oldValue = system.Color;
        system.Color = Color.Blue;
        
        system.DrawLine(....);
        
        system.Color = oldValue;
    }
    
Sometimes a number of state changes are made which all require restoring...

    void DoSomething(StatefulSystem system)
    {
        var oldValue1 = system.Value1;
        var oldValue2 = system.Value2;
        var oldValue3 = system.Value3;
        
        system.Value1 = ...;
        system.Value2 = ...;
        system.Value3 = ...;
        
        system.DoSomething();
        
        system.Value1 = oldValue1;
        system.Value2 = oldValue2;
        system.Value3 = oldValue3;
    }
    
Typically the state properties that need caching are some subset of the overall set of possible state properties. 

## Solution
This library changes the code in the motivating examples above to something like the following...

    public class DrawableEntity
    {
        readonly StateCache<StatefulDrawingSystem> stateCache;
        
        public DrawableEntity()
        {
            stateCache =
                new StateCacheBuilder<StatefulDrawingSystem>()
                    .Property("Value1")
                    .Property("Value2")
                    .Property("Value3")
                    .Build();
        }
        
        public void Draw(StatefulGraphicsSystem system)
        {
            using (stateCache.AutoRestoreToPreviousState(system))
            {
                system.Value1 = ...;
                system.Value2 = ...;
                system.Value3 = ...;
                
                system.DrawSomething();
            }
        }
    }
    
Alternatively you can use the library without the "using".

    public class DrawableEntity
    {
        readonly StateCache<StatefulDrawingSystem> stateCache;
        
        public DrawableEntity()
        {
            stateCache =
                new StateCacheBuilder<StatefulDrawingSystem>()
                    .Property("Value1")
                    .Property("Value2")
                    .Property("Value3")
                    .Build();
        }
        
        public void Draw(StatefulGraphicsSystem system)
        {
            stateCache.Store(system);
            
            system.Value1 = ...;
            system.Value2 = ...;
            system.Value3 = ...;
                
            system.DrawSomething();
            
            stateCache.Restore(system);
        }
    }
    
## Performance
The library comes with BenchmarkDotNet tests that show this only adds a small overhead (from an already small base) to the hand-rolled version. The non-using version runs marginally quicker than the using version.

# TODO
- [ ] Write tests to illuminate weaknesses in Proof of Concept code
    - [ ] Error handling
        - [ ] Non-existent properties
        - [ ] Read-only properties
        - [ ] Write-only properties
    - [ ] Abstract types etc.?
- [ ] Change design to allow caches to be created more quickly
    - [ ] Profiling of the cache creation code
    - [ ] Determine how to make creation of multiple caches from a single type fast
