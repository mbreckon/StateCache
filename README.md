# StateCache
This library supports fast and customisable caching of the public properties of an object. The caching object is built dynamically to only include the properties from the object that are required to be cache. This allows the store/restore to only be maginally slower than manually written caching code.

## Motivating Example
There is form of system design in which state is the primary way in which activities are configured. This is often done for performance reasons - you may not want to set all the properties of the system for every activity, so the state from other activities is "reused" and only changes to the specific state values necessary for configuring the activity at hand are changed.

For example a graphics system might draw everything in blue following the setting of the foreground colour to blue. Any call to Draw something will result in a blue entity being drawn until the foreground colour is switched to a different one.
