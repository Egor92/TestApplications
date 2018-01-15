using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ReSharperSettingsTest
{
    public class Class1
    {
        #region Prop1

        private bool _Prop1;

        public bool Prop1
        {
            get { return _Prop1; }
            set { this.RaiseAndSetIfChanged(x => x.Prop1, value); }
        }

        #endregion

        public int IsSelected
        {
            get
            {
                return null != AllEntries && AllEntries.Any()
                    ? AllEntries.First(evt => evt.IsSelected).ID
                    : 0;
            }
        }

        private object _field;
        public int Property { get; private set; }

        public Class1()
        {
            _field = new object();
        }

        public void MainMethod()
        {
            var list = new List<string>();
            var ints = list.Select(x => x.Length + x.Length + x.Length + x.Length)
                           .Select(x =>
                           {
                               return x + 1;
                           }).Cast<int>()
                           .ToList();
            var values = list.Select(x => x.Length).ToList();

            ManyArgsMethod(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                           string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            EmptyMethod();

            int a = 7;

            MethodWithFunc(x =>
            {
                return new object();
            });

            var displayName = GetType().Name;

            new A
            {
                P1 = nps.ID,
            };

            new A
            {
                P1 = nps.ID,
                P2 = nps.Name,
            };

            new A
            {
                P1 = nps.ID,
                P2 = nps.Name,
                P3 = Length.From(nps.Coordinate, LengthUnit.Meter)
                           .As(LengthUnit.Kilometer)
                           .As(LengthUnit.Kilometer)
                           .As(LengthUnit.Kilometer)
                           .As(LengthUnit.Kilometer)
                           .As(LengthUnit.Kilometer)
                           .As(LengthUnit.Kilometer)
                           .As(LengthUnit.Kilometer)
                           .As(LengthUnit.Kilometer)
            };

            var npss = dssHelper.NPSs.Select(nps => new NPSDescriptionModel
            {
                ID = nps.ID,
                Name = nps.Name,
                Coordinate = Length.From(nps.Coordinate, LengthUnit.Meter)
                                   .As(LengthUnit.Kilometer)
                                   .As(LengthUnit.Kilometer)
                                   .As(LengthUnit.Kilometer)
                                   .As(LengthUnit.Kilometer)
                                   .As(LengthUnit.Kilometer)
                                   .As(LengthUnit.Kilometer)
                                   .As(LengthUnit.Kilometer)
                                   .As(LengthUnit.Kilometer)
                                   .As(LengthUnit.Kilometer)
            }).ToList();

            if (a && b || c && d)
            {
                Debug.WriteLine("");
            }
        }

        public void ManyArgsMethod(string agr01,
                                   string agr02,
                                   string agr03,
                                   string agr04,
                                   string agr05,
                                   string agr06,
                                   string agr07,
                                   string agr08,
                                   string agr09,
                                   string agr10,
                                   string agr11,
                                   string agr12,
                                   string agr13,
                                   string agr14,
                                   string agr15,
                                   string agr16,
                                   string agr17,
                                   string agr18)
        {
        }

        public void MethodWithFunc(Func<object, object> func)
        {
        }

        public void EmptyMethod()
        {
        }

        public void D()
        {
            return;
            int i = 4;
        }
    }
}