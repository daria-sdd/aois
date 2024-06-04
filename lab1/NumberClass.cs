public class Number
{
    public int value;

    public Number(int value)
    {
        this.value = value;
    }

    public void Input(int newValue)
    {
        this.value = newValue;
    }

    public int Output()
    {
        return this.value;
    }

    public string ToBinary()
    {
        int number = Math.Abs(this.value);
        string binary = string.Empty;
        while (number > 0)
        {
            binary = (number % 2).ToString() + binary;
            number = number / 2;
        }
        return binary.PadLeft(7, '0');
    }

    public string ToDirectCode()
    {
        string sign = this.value < 0 ? "1" : "0";
        return sign + ToBinary();
    }

    public string ToReverseCode()
    {
        if (this.value >= 0)
        {
            return ToDirectCode();
        }
        else
        {
            char[] binary = ToBinary().ToCharArray();
            for (int i = 0; i < binary.Length; i++)
            {
                binary[i] = binary[i] == '0' ? '1' : '0';
            }
            return "1" + new string(binary);
        }
    }

    public string ToAdditionalCode()
    {
        if (this.value >= 0)
        {
            return ToDirectCode();
        }
        else
        {
            string reverseCode = ToReverseCode().Substring(1);
            int carry = 1;
            char[] additionalCode = new char[reverseCode.Length];
            for (int i = reverseCode.Length - 1; i >= 0; i--)
            {
                int bit = (reverseCode[i] - '0') + carry;
                additionalCode[i] = (bit % 2).ToString()[0];
                carry = bit / 2;
            }
            return "1" + new string(additionalCode);
        }
    }

    public static Number Add(Number a, Number b)
    {
        string aAdditional = a.ToAdditionalCode();
        string bAdditional = b.ToAdditionalCode();
        char[] sum = new char[aAdditional.Length];
        int carry = 0;
        for (int i = aAdditional.Length - 1; i >= 0; i--)
        {
            int bitSum = (aAdditional[i] - '0') + (bAdditional[i] - '0') + carry;
            sum[i] = (bitSum % 2).ToString()[0];
            carry = bitSum / 2;
        }

        if (sum[0] == '1')
        {
            for (int i = 0; i < sum.Length; i++)
            {
                sum[i] = sum[i] == '0' ? '1' : '0';
            }
            int decimalSum = -(Convert.ToInt32(new string(sum), 2) + 1);
            return new Number(decimalSum);
        }
        else
        {
            int decimalSum = Convert.ToInt32(new string(sum), 2);
            return new Number(decimalSum);
        }
    }

    public static Number Subtract(Number a, Number b)
    {
        Number bCopy = new Number(b.Output());
        bCopy.Input(-bCopy.Output());
        return Add(a, bCopy);
    }

    public static Number Multiply(Number a, Number b)
    {
        string aBinary = a.ToBinary();
        string bBinary = b.ToBinary();
        string result = "0";

        for (int i = 0; i < bBinary.Length; i++)
        {
            if (bBinary[bBinary.Length - i - 1] == '1')
            {
                string temp = aBinary + new string('0', i);
                result = Add(new Number(BinaryToDecimal(result)), new Number(BinaryToDecimal(temp))).ToBinary();
            }
        }

        string sign = a.ToDirectCode()[0] == b.ToDirectCode()[0] ? "0" : "1";
        if (sign == "1")
        {
            int final = -BinaryToDecimal(result);
            return new Number(final);
        }
        else
        {
            int final = BinaryToDecimal(result);
            return new Number(final);
        }
    }


    public static Number Divide(Number a, Number b)
    {
        string aBinary = a.ToBinary();
        string bBinary = b.ToBinary();

        if (b.Output() == 0) // Проверка деления на ноль
        {
            throw new DivideByZeroException();
        }

        string quotient = "";
        string remainder = "";

        for (int i = 0; i < aBinary.Length; i++)
        {
            remainder += aBinary[i];
            if (BinaryToDecimal(remainder) >= BinaryToDecimal(bBinary))
            {
                remainder = Subtract(new Number(BinaryToDecimal(remainder)), b).ToBinary();
                quotient += "1";
            }
            else
            {
                quotient += "0";
            }
        }

        string sign = a.ToDirectCode()[0] == b.ToDirectCode()[0] ? "0" : "1";
        if (sign == "1")
        {
            int final = -BinaryToDecimal(quotient);
            return new Number(final);
        }
        else
        {
            int final = BinaryToDecimal(quotient);
            return new Number(final);
        }
    }



    public static int BinaryToDecimal(string binary)
    {
        int decimalNumber = 0;
        for (int i = 0; i < binary.Length; i++)
        {
            if (binary[binary.Length - i - 1] == '1')
            {
                decimalNumber += (int)Math.Pow(2, i);
            }
        }
        return decimalNumber;
    }

    public void PrintCodes()
    {
        Console.WriteLine("Число: " + Output());
        Console.WriteLine("Прямой код: " + ToDirectCode());
        Console.WriteLine("Обратный код: " + ToReverseCode());
        Console.WriteLine("Дополнительный код: " + ToAdditionalCode());
        Console.WriteLine("------------------------------------------");
    }
}


