using TMPro;
using UnityEngine;

public class TextManager
{
    public void SetText(object message, TMP_Text textRow, bool formatIndexedNumber = false, bool formatAsRoman = false, string frontAddedMessage = "", string endAddedMessage = "", bool addToPrevious = false)
    {
        string formattedText = GetFormattedText(message, formatIndexedNumber, formatAsRoman);

        if (addToPrevious)
        {
            textRow.text += frontAddedMessage + formattedText + endAddedMessage;
        }
        else
        {
            textRow.text = frontAddedMessage + formattedText + endAddedMessage;
        }
    }

    public void SetTimerText(TMP_Text textRow, float seconds, bool showHoursAndMinutes = false, string frontAddedMessage = "", string endAddedMessage = "")
    {
        textRow.text = $"{frontAddedMessage}{FormatTime(seconds, showHoursAndMinutes)}{endAddedMessage}";
    }

    private string FormatTime(float seconds, bool showHoursAndMinutes)
    {
        if (showHoursAndMinutes)
        {
            int hours = Mathf.FloorToInt(seconds / 3600);
            int minutes = Mathf.FloorToInt((seconds % 3600) / 60);
            int secs = Mathf.FloorToInt(seconds % 60);

            return hours > 0
                ? $"{hours:D2}:{minutes:D2}:{secs:D2}"
                : $"{minutes:D2}:{secs:D2}";
        }

        int secsOnly = Mathf.FloorToInt(seconds);
        return $"{secsOnly}";
    }

    private string GetFormattedText(object message, bool formatIndexedNumber = false, bool formatAsRoman = false)
    {
        if (formatAsRoman && message is int romanNumber)
        {
            return ToRoman(romanNumber);
        }

        if (formatIndexedNumber && message is int kNumber)
        {
            return FormatIndexedNumber(kNumber);
        }

        return message.ToString();
    }

    private string FormatIndexedNumber(float number)
    {
        string[] suffixes = { "", "K", "M", "B", "T", "Q" }; // K=thousand, M=million, etc.
        int suffixIndex = 0;

        while (number >= 1000f && suffixIndex < suffixes.Length - 1)
        {
            number /= 1000f;
            suffixIndex++;
        }

        return number.ToString("0.#") + suffixes[suffixIndex];
    }

    private string ToRoman(int number)
    {
        if (number < 1 || number > 3999)
            return number.ToString(); 

        string[] thousands = { "", "M", "MM", "MMM" };
        string[] hundreds = { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" };
        string[] tens = { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
        string[] units = { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };

        return thousands[number / 1000] +
               hundreds[(number % 1000) / 100] +
               tens[(number % 100) / 10] +
               units[number % 10];
    }
}