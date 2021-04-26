using System;

namespace FunkySheep.Variables
{
    [Serializable]
    public class FloatReference
    {
        public bool UseConstant = true;
        public int ConstantValue;
        public IntVariable Variable;

        public FloatReference()
        { }

        public FloatReference(int value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public int Value
        {
            get { return UseConstant ? ConstantValue : Variable.Value; }
        }

        public static implicit operator int(FloatReference reference)
        {
            return reference.Value;
        }
    }
}