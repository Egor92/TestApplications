namespace NestedEnumInXaml
{
    public class MyClass1
    {
        public class MyClass2
        {
            public class MyClass3
            {
                public class MyClass4
                {
                    public class MyClass5
                    {
                        public enum MyEnum
                        {
                            A,
                            B,
                            C,
                            D,
                        }
                    }
                }
            }
        }
    }

    public enum SimpleEnum
    {
        On,
        Off,
        IsWorking,
    }
}
