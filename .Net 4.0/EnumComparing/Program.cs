using System;

namespace EnumComparing
{
    [Flags]
    enum A
    {
        V1 = 0x01,
        V2 = 0x02,
        V3 = 0x04,
        V4 = 0x08,
    }

    enum B
    {
        V1,
        V2,
        V3,
    }

    enum C
    {
        V1 = 1,
        V2 = 2,
        V3 = 3,
    }

    enum D
    {
        V1 = 1,
        V2 = 2,
        V3 = 3,
    }

    enum E : byte
    {
        V1 = 1,
        V2 = 2,
        V3 = 3,
    }

    class Program
    {
        static void Main(string[] args)
        {
            object a = A.V1;
            object b = B.V1;
            object c = C.V1;
            object d = D.V1;
            object e = D.V1;

            var t1 = c == d;
            var v1 = (int) A.V1;
            var a1 = A.V1 | A.V2 | A.V3;
        }
    }
}
