﻿namespace VisualMutator.Views.Converters
{
    #region

    using System;
    using System.Windows.Data;

    #endregion

    [ValueConversion(typeof(int), typeof(bool))]
    public class ZeroToTrueConverter : Converter<ZeroToTrueConverter, double, bool>
    {
        public override bool Convert(double value)
        {
            return Math.Abs(value - 0) < 0.001d;
        }

        public override double ConvertBack(bool value)
        {
            throw new NotSupportedException();
        }
    }
}