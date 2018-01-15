namespace GenericTypeTestApp
{
    public interface IData<T>
    {
        T Value { get; set; }
    }

    public abstract class A<T> : IData<T>
    {
        public T Value { get; set; }
    }

    public class IntData : A<int>
    {
    }

    public class StringData : A<string>
    {
    }

    public class BoolData : IData<bool>
    {
        public bool Value { get; set; }
    }
}
