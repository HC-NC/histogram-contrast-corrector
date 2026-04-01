using System;
using System.Collections.Generic;
using System.Text;

namespace Histogram_Contrast_Corrector.DataClasses
{
    public enum CorrectionMethods { Linear, Negative, Log, Power, Exp }

    public interface ICorrectionMethod
    {
        float F(float x);
        void SetA(float a);
        float GetA();
    }

    public class LinearCorrection : ICorrectionMethod
    {
        public float F(float x) => x;
        public float GetA() => 0;
        public void SetA(float a) { }
    }

    public class NegativeCorrection : ICorrectionMethod
    {
        public float F(float x) => 1f - x;
        public float GetA() => 0;
        public void SetA(float a) { }
    }

    public class LogCorrection : ICorrectionMethod
    {
        private float _a = 2f;
        public float F(float x)
        {
            return MathF.Log(1f + (_a - 1f) * x) / MathF.Log(_a);
        }
        public float GetA() => _a;
        public void SetA(float a)
        {
            if (a == 1f)
                return;

            _a = a;
        }
    }

    public class ExpCorrection : ICorrectionMethod
    {
        private float _a = 1f;
        public float F(float x) => MathF.Exp(_a * x) - 1f;
        public float GetA() => _a;
        public void SetA(float a) => _a = Math.Clamp(a, 0.01f, 10f);
    }

    public class PowerCorrection : ICorrectionMethod
    {
        private float _a = 1f;
        public float F(float x) => MathF.Pow(x, _a);
        public float GetA() => _a;
        public void SetA(float a) => _a = a;
    }
}
