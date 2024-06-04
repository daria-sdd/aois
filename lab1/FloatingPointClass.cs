using System;


public class FloatingPointNumber
{
    private float value;

    public FloatingPointNumber(float value)
    {
        this.value = value;
    }

    public void Input(float newValue)
    {
        this.value = newValue;
    }

    public float Output()
    {
        return this.value;
    }

    public string ToBinary()
    {
        int bits = BitConverter.SingleToInt32Bits(this.value);
        return Convert.ToString(bits, 2).PadLeft(32, '0');
    }

    public void PrintBinary()
    {
        Console.WriteLine("Число: " + Output());
        Console.WriteLine("Бинарное представление: " + ToBinary());
        Console.WriteLine("------------------------------------------");
    }

    public static FloatingPointNumber Add(FloatingPointNumber a, FloatingPointNumber b)
    {
        if (a.Output() < 0 || b.Output() < 0)
        {
            throw new ArgumentException("Оба числа должны быть положительными.");
        }

        try
        {
            double result = a.Output() + b.Output();

            return new FloatingPointNumber((float)result);
        }
        catch (OverflowException)
        {
            throw new OverflowException("Результат достиг максимального значения.");
        }
    }
}

