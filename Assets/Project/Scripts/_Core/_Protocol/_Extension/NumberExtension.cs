// ReSharper disable UnusedMember.Global

public static class NumberExtension
{
    public static string ToKmbt(this int number)
    {
        return number switch
        {
            >= 100000000 => (number / 1000000D).ToString("0.#M"),
            >= 10000000  => (number / 1000000D).ToString("0.##M"),
            >= 1000000   => (number / 1000000D).ToString("0.###M"),
            >= 100000    => (number / 1000D).ToString("0.#K"),
            >= 10000     => (number / 1000D).ToString("0.##K"),
            _            => number.ToString("#,0")
        };
    }
        
    public static string ToKmbt(this long number)
    {
        return number switch
        {
            >= 100000000000000000 => (number / 1000000000000000D).ToString("0.#Q"),
            >= 10000000000000000  => (number / 1000000000000000D).ToString("0.##Q"),
            >= 1000000000000000   => (number / 1000000000000000D).ToString("0.###Q"),
            >= 100000000000000    => (number / 1000000000000D).ToString("0.#T"),
            >= 10000000000000     => (number / 1000000000000D).ToString("0.##T"),
            >= 1000000000000      => (number / 1000000000000D).ToString("0.###T"),
            >= 100000000000       => (number / 1000000000D).ToString("0.#B"),
            >= 10000000000        => (number / 1000000000D).ToString("0.##B"),
            >= 1000000000         => (number / 1000000000D).ToString("0.###B"),
            >= 100000000          => (number / 1000000D).ToString("0.#M"),
            >= 10000000           => (number / 1000000D).ToString("0.##M"),
            >= 1000000            => (number / 1000000D).ToString("0.###M"),
            >= 100000             => (number / 1000D).ToString("0.#K"),
            >= 10000              => (number / 1000D).ToString("0.##K"),
            _                     => number.ToString("#,0")
        };
    }
    
    public static int GetNumberOfDigits(this int original, int digit)
    {
        if (digit < 1)
        {
            UnityEngine.Debug.LogError($"The digit argument is not valid for the given number. input {original}");
            return 0;
        }
        for (var i = 1; i < digit; i++)
        {
            original /= 10;
        }
        return original % 10;
    }
    
    public static int GetNumberOfFromToDestDigits(this int original, int startDigit, int endDigit)
    {
        if (startDigit <= 0 || endDigit <= 0)
        {
            UnityEngine.Debug.LogError($"startDigit and endDigit must be greater than 0. InputStart:{startDigit}, InputEnd:{endDigit}. return 0");
            return 0;
        }

        if (startDigit > endDigit)
        {
            UnityEngine.Debug.LogError($"startDigit must be less than or equal to endDigit. InputStart:{startDigit}, InputEnd:{endDigit}. return 0");
            return 0;
        }
        
        // Calculate the divisor to isolate the start digit.
        var startDivisor = (int)UnityEngine.Mathf.Pow(10, startDigit - 1);
        var endDivisor = (int)UnityEngine.Mathf.Pow(10, endDigit - (startDigit - 1));
    
        // Remove the numbers to the right of the start digit.
        var result = original / startDivisor;
    
        // Keep only the numbers up to the end digit.
        result = result % endDivisor;

        return result;
    }
}