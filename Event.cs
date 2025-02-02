namespace MoodyLib.SimpleEvents {
    
    /// <summary>
    /// A simple base event from which all other events should inherit.
    /// </summary>
    public abstract class SimpleEvent { }

    /// <summary>
    /// A base event that contains a single integer value, which can be used for simple value updates. 
    /// </summary>
    public abstract class IntValueEvent : SimpleEvent {
        public int value;
    }

    /// <summary>
    /// A base event that contains a single float value, which can be used for simple value updates.
    /// </summary>
    public abstract class FloatValueEvent : SimpleEvent {
        public float value;
    }
}
