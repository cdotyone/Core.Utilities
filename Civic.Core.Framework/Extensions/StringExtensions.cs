using System;
using System.Diagnostics;

namespace Civic.Core.Framework.Extensions
{
    public static class StringExtensions
    {
        [DebuggerStepThrough]
        public static string Truncate(this string instr, int maxLength)
        {
            if (maxLength > instr.Length) maxLength = instr.Length;
            return instr.Substring(0, maxLength);
        }

        [DebuggerStepThrough]
        public static bool IsNullOrEmpty(this string instr)
        {
            return string.IsNullOrEmpty(instr);
        }


        [DebuggerStepThrough]
        public static int? ToInteger(this string instr)
        {
            if (string.IsNullOrEmpty(instr))
            {
                return null;
            }
            int val;
            if (int.TryParse(instr, out val)) return val;
            return null;
        }


        [DebuggerStepThrough]
        public static double? ToDouble(this string instr)
        {
            if (string.IsNullOrEmpty(instr))
            {
                return null;
            }
            double val;
            if (double.TryParse(instr, out val)) return val;
            return null;
        }


        [DebuggerStepThrough]
        public static string UseDefault(this string instr, string def)
        {
            if (instr.IsNullOrEmpty()) return def;
            return instr;
        }

        [DebuggerStepThrough]
        public static DateTime? ToDateTime(this string instr)
        {
            if (instr.IsNullOrEmpty()) return null;

            DateTime val;
            if (DateTime.TryParse(instr, out val)) return val;
            return null;
        }

        [DebuggerStepThrough]
        public static bool ToBool(this string instr)
        {
            if (instr.IsNullOrEmpty()) return false;

            instr = instr.ToLowerInvariant();

            if (instr == "y" || instr == "t" || instr == "true" || instr == "yes") return true;
            return false;
        }

    }


    /*
     * 
Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions

Public Module Extensions
    <DebuggerStepThrough()>
    <Extension()>
    Public Function Split(ByVal value As String, Count As Integer) As String()
        If String.IsNullOrEmpty(value) Then
            Return {""}
        ElseIf value.Length < Count Then
            Return {value}
        Else
            Return Civic.Common.Split(value, Count).ToArray
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToDouble(value As String) As Double?
        Return Civic.Common.ToDouble(value)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToDate(value As String) As Date?
        If value.IsNullOrWhiteSpace Then
            value = ""
        End If

        Dim item As Date

        If Date.TryParse(value.Trim(), item) Then
            Return item
        Else
            Return Nothing
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToDateFromFixedString(value As String) As Date?
        If value.IsNullOrWhiteSpace() Then
            Return Nothing
        End If

        If value.Length = 8 Then
            value = "{0}/{1}/{2}".QuickFormat(value.Substring(0, 2), value.Substring(2, 2), value.Substring(4))
        End If

        Return value.ToDate()
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToShortDate(value As String) As Date?
        If value.IsNullOrWhiteSpace Then
            value = ""
        End If

        Dim item As Date

        If Date.TryParse(value.Trim(), item) Then
            Return Convert.ToDateTime("{0:MM/dd/yyyy}".QuickFormat(item))
        Else
            Return Nothing
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToChar(value As String) As Char?
        If value.IsNullOrWhiteSpace() Then
            value = ""
        End If
        If value.Length = 1 Then
            Return value(0)
        Else
            Return Nothing
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Ascii(value As Char) As Integer
        Return Asc(value)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Ascii(value As Char?) As Integer?
        If value.HasValue Then
            Return value.Value.Ascii
        Else
            Return Nothing
        End If
    End Function


    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToBoolean(value As String) As Boolean?
        Dim bol As Boolean
        Dim int As Integer

        If value.IsEqual("yes") Then
            value = "true"
        ElseIf value.IsEqual("no") Then
            value = "false"
        End If

        If Boolean.TryParse(value, bol) Then
            Return bol
        ElseIf Integer.TryParse(value, int) Then
            Return Not int = 0
        Else
            Return Nothing
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToBase64(value As String) As String
        Return Civic.Common.StringToBase64(value)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToProperCase(value As String) As String
        Return Civic.Common.ToProperCase(value)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function FromCamel(value As String) As String
        Dim spacables As New List(Of String)("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToArray)
        Dim ret As New StringBuilder()

        For Each c As Char In value.ToCharArray()
            If ret.Length = 0 Then
                ret.Append(c.ToString().ToUpper())
                Continue For
            ElseIf spacables.Contains(c.ToString()) Then
                ret.Append(" ")
            End If

            ret.Append(c)
        Next

        Return ret.ToString().Trim()
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToArray(value As String) As String()
        Dim ret As New List(Of String)

        For Each c As Char In value.ToCharArray()
            ret.Add(c.ToString())
        Next

        Return ret.ToArray()
    End Function


    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToSentenceCase(value As String) As String
        Return Civic.Common.ToSentenceCase(value)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToSentence(Value As String) As String
        If Value.IsNullOrEmpty() Then
            Return ""
        End If
        Dim Search As String = "abcdefghijklmnopqrstuvwxyz, "
        Dim RetValue As New System.Text.StringBuilder

        Value = Value.Replace("_", " ")
        Value = Value.Replace(",", ", ")


        For Each c As Char In Value.ToCharArray
            If Search.IndexOf(c) = -1 Then
                RetValue.Append(" ")
            End If
            RetValue.Append(c)
        Next

        RetValue.Replace("  ", " ")
        RetValue.Replace(".", ".  ")
        RetValue.Replace("?", "?  ")
        RetValue.Replace("!", "!  ")

        Return RetValue.ToString().Trim()
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToStream(value As String) As System.IO.MemoryStream
        Return Civic.Common.StringToStream(value)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToStream(value As StringBuilder) As System.IO.MemoryStream
        Return Civic.Common.StringToStream(value.ToString())
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function IsNumeric(value As String) As Boolean
        Return Double.TryParse(value, Nothing)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function IsDate(value As String) As Boolean
        Return Date.TryParse(value, Nothing)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function IsBoolean(value As String) As Boolean
        Return Boolean.TryParse(value, Nothing)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function IsGuid(value As String) As Boolean
        Return Civic.Common.IsGuid(value)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Equals(value As String, Compare As String) As Boolean
        Return String.Equals(value, Compare, StringComparison.CurrentCultureIgnoreCase)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function IsEqual(value As String, ByVal ParamArray args As String()) As Boolean
        For Each compare As String In args
            If String.Equals(value, compare, StringComparison.CurrentCultureIgnoreCase) Then
                Return True
            End If
        Next
        Return False
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function IsEqual(value As Char, ByVal ParamArray args As Char()) As Boolean
        For Each compare As Char In args
            If String.Equals(value, compare, StringComparison.CurrentCultureIgnoreCase) Then
                Return True
            End If
        Next
        Return False
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function IsEqual(value As Char?, ByVal ParamArray args As Char()) As Boolean
        If Not value.HasValue Then
            Return False
        Else
            Return value.Value.IsEqual(args)
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function [Like](value As String, ParamArray Search As String()) As Boolean
        For Each item As String In Search
            If Not item.StartsWith("*") Then
                item = "*" & item
            End If
            If Not item.EndsWith("*") Then
                item &= "*"
            End If
            If value.ToLower() Like item.ToLower() Then
                Return True
            End If
        Next
        Return False
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Contains(value As System.Text.StringBuilder, Search As String) As Boolean
        Return value.ToString().ToLower().Contains(Search.ToLower())
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Remove(value As System.Text.StringBuilder, Search As String) As System.Text.StringBuilder
        If value.Contains(Search) Then
            Dim val As String = value.ToString().ToLower

            Search = Search.ToLower()

            value.Remove(val.IndexOf(Search), val.Length() - val.IndexOf(Search))
            Return value
        Else
            Return value
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Equals(value As String, Compare As String, IgnoreCase As Boolean) As Boolean
        If IgnoreCase Then
            Return Equals(value, Compare)
        Else
            Return String.Equals(value, Compare)
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function IsNullOrEmpty(value As String) As Boolean
        Return String.IsNullOrEmpty(value)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function IsNullOrWhiteSpace(value As String) As Boolean
        Return String.IsNullOrWhiteSpace(value)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function SafeString(value As String) As String
        Return Common.SafeString(value)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function JavaScriptSafe(value As String) As String
        Return Common.SafeJavaScriptString(value)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function RemoveJavaScriptSafe(value As String) As String
        Return Common.RemoveJavaScriptString(value)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function TrimAll(value As String) As String
        If value.IsNullOrWhiteSpace() Then
            Return ""
        End If

        While value.EndsWith(System.Environment.NewLine)
            value = value.Remove(value.Length - System.Environment.NewLine.Length, System.Environment.NewLine.Length)
        End While

        Return value.Trim()
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function QuickReplace(value As String, NewValue As String, ByVal ParamArray args As String()) As String
        Dim str As New StringBuilder(value)

        For Each item As String In args
            str.Replace(item, NewValue)
        Next

        Return str.ToString()
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function QuickFormat(value As String, ByVal ParamArray args As Object()) As String
        Return String.Format(value, args)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function QuickConcat(value As String, ByVal ParamArray args As String()) As String
        Dim re As New System.Text.StringBuilder()

        re.Append(value)

        For Each item As String In args
            re.Append(item)
        Next

        Return re.ToString()
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function GetAge(value As Date) As Integer
        Return Civic.Common.GetAge(value)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function StartOfFiscalYear(value As Date, StartMonth As Integer) As Date
        If StartMonth < 0 Then
            StartMonth = 1
        ElseIf StartMonth > 12 Then
            StartMonth = 12
        End If

        Dim Year As Integer = value.Year

        If StartMonth > 1 And value.Month < 13 Then
            Year -= 1
        End If

        Return Date.Parse("{0}/01/{1}".QuickFormat(StartMonth, Year))
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function EndOfFiscalYear(value As Date, StartMonth As Integer) As Date
        Dim fs As Date = StartOfFiscalYear(value, StartMonth)
        fs = fs.AddMonths(11).LastOfMonth

        Return fs
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function FirstOfMonth(Value As Date) As Date
        Dim Month As Integer = Value.Month
        Dim Year As Integer = Value.Year

        Return Date.Parse("{0}/01/{1}".QuickFormat(Month, Year))
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function PreviousSunday(item As Date) As Date
        Return item.AddDays(-1 * (CType(item.DayOfWeek, Integer)))
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function UpcomingSaturday(item As Date) As Date
        Return item.AddDays(6 - CType(item.DayOfWeek, Integer))
    End Function

    <DebuggerStepThrough()>
<Extension()>
    Public Function ThisFriday(item As Date) As Date
        Return item.AddDays(-1 * (CType(item.DayOfWeek, Integer)) - 2 + 7)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function NextFriday(item As Date) As Date
        Return item.AddDays(5 - CType(item.DayOfWeek, Integer) + 7)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function LastOfMonth(value As Date) As Date
        Dim Month As Integer = value.Month + 1
        Dim Year As Integer = value.Year

        If Month > 12 Then
            Month = 1
            Year += 1
        End If

        Return Date.Parse("{0}/01/{1}".QuickFormat(Month, Year)).AddDays(-1)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function StartOfDay(value As Date) As Date
        Return "{0:MM/dd/yyyy} 12:00 AM".QuickFormat(value).ToDate().Value
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function StartOfDay(value As Date?) As Date?
        If Not value.HasValue Then
            Return Nothing
        Else
            Return value.Value.StartOfDay()
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function EndOfDay(value As Date) As Date
        Return "{0:MM/dd/yyyy}".QuickFormat(value).ToDate().Value.AddDays(1).AddMilliseconds(-1)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function EndOfDay(value As Date?) As Date?
        If Not value.HasValue Then
            Return Nothing
        Else
            Return value.Value.EndOfDay()
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function FirstOfPreviousMonth(Value As Date) As Date
        Dim Month As Integer = Value.Month - 1
        Dim Year As Integer = Value.Year

        If Month < 1 Then
            Month = 12
            Year -= 1
        End If

        Return Date.Parse("{0}/01/{1}".QuickFormat(Month, Year))
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function LastOfPreviousMonth(value As Date) As Date
        Return value.FirstOfMonth.AddDays(-1)
    End Function


    <DebuggerStepThrough()>
    <Extension()>
    Public Function AddBusinessDays(DateValue As Date, Value As Integer) As Date
        Dim Subtract As Boolean = Value < 0

        If Not Subtract Then
            DateValue = DateValue.NextBusinessDay()
        Else
            DateValue = DateValue.PreviousBusinessDay()
        End If

        If Value = 0 Then
            Return DateValue
        End If

        Value = Math.Abs(Value)

        For i As Integer = 1 To Value
            If Subtract Then
                DateValue = DateValue.AddDays(-1).PreviousBusinessDay()
            Else
                DateValue = DateValue.AddDays(1).NextBusinessDay()
            End If
        Next

        Return DateValue
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function NextBusinessDay(DateValue As Date) As Date
        If DateValue.DayOfWeek = DayOfWeek.Saturday Then
            DateValue = DateValue.AddDays(2)
        ElseIf DateValue.DayOfWeek = DayOfWeek.Sunday Then
            DateValue = DateValue.AddDays(1)
        End If

        Return DateValue
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function PreviousBusinessDay(DateValue As Date) As Date
        If DateValue.DayOfWeek = DayOfWeek.Saturday Then
            DateValue = DateValue.AddDays(-1)
        ElseIf DateValue.DayOfWeek = DayOfWeek.Sunday Then
            DateValue = DateValue.AddDays(-2)
        End If

        Return DateValue
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function BusinessDayDifference(DateValue As Date, CutOff As Date) As Integer
        Dim i As Integer
        Dim StartDate As Date
        Dim EndDate As Date
        Dim IsNegative As Boolean

        If DateValue.Date = CutOff.Date Then
            Return 0
        ElseIf DateValue.Date < CutOff Then
            IsNegative = False
            StartDate = DateValue.Date
            EndDate = CutOff.Date
        Else
            IsNegative = True
            StartDate = CutOff.Date
            EndDate = DateValue.Date
        End If

        Do Until StartDate >= EndDate
            i += 1
            StartDate = StartDate.AddBusinessDays(1).Date
        Loop

        If IsNegative Then
            i *= -1
        End If

        Return i

    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Display(value As System.Enum) As String
        Return value.ToString.Replace("_", " ")
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Display(value As Date, Format As DateFormat) As String
        Select Case Format
            Case DateFormat.Date
                Return Constants.FormatDate.QuickFormat(value)
            Case DateFormat.Time
                Return Constants.FormatTime2.QuickFormat(value)
            Case DateFormat.Time2
                Return Constants.FormatTime3.QuickFormat(value)
            Case DateFormat.DateAndTime
                Return Constants.FormatDateTime.QuickFormat(value)
            Case DateFormat.FullDateAndTime
                Return Constants.FormatFullDateTime.QuickFormat(value)
            Case DateFormat.LongDate
                Return Constants.FormatLongDate.QuickFormat(value)
        End Select

        Return ""
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Display(value As Double) As String
        Return "{0:#,##0.00}".QuickFormat(value)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Eval(value As Double?) As Double
        If value.HasValue Then
            Return value.Value
        Else
            Return 0
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Display(value As Date?, Format As DateFormat) As String
        If Not value.HasValue Then
            Return ""
        Else
            Return Display(value.Value, Format)
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Display(value As Double, Format As NumberFormat) As String
        Select Case Format
            Case NumberFormat.Double
                Return Constants.FormatNumber.QuickFormat(value)
            Case NumberFormat.Integer
                Return Constants.FormatWholeNumber.QuickFormat(value)
            Case NumberFormat.Currency
                Return Constants.FormatCurrency.QuickFormat(value)
            Case NumberFormat.Coordinate
                Return Constants.FormatNumberCoord.QuickFormat(value)
        End Select

        Return ""
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Display(value As Double?, Format As NumberFormat) As String
        If Not value.HasValue Then
            Return ""
        Else
            Return Display(value.Value, Format)
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToValue(value As Double?) As Double
        If value.HasValue Then
            Return value.Value
        Else
            Return 0.0
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToValue(value As Boolean?) As Boolean
        If value.HasValue Then
            Return value.Value
        Else
            Return False
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToValue(value As Date?, DefaultValue As Date) As Date
        If value.HasValue Then
            Return value.Value
        Else
            Return DefaultValue
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Display(value As Boolean?, TrueValue As String, FalseValue As String) As String
        Return Display(value, TrueValue, FalseValue, "")
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Display(value As Boolean?, TrueValue As String, FalseValue As String, NoValue As String) As String
        If Not value.HasValue Then
            Return NoValue
        ElseIf value.Value Then
            Return TrueValue
        Else
            Return FalseValue
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Display(value As Integer) As String
        Return "{0:#,##0}".QuickFormat(value)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToValue(value As Integer?) As Integer
        If value.HasValue Then
            Return value.Value
        Else
            Return 0
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Display(value As Integer?) As String
        If value.HasValue Then
            Return Display(value.Value)
        Else
            Return ""
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Display(value As Integer, Format As String) As String
        If Format.IsNullOrWhiteSpace() Then
            Return value.ToString
        Else
            Return Format.QuickFormat(value)
        End If

    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Display(value As Integer?, Format As String) As String
        If value.HasValue Then
            Return Display(value.Value, Format)
        Else
            Return ""
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Sub Remove(rows As DataRowCollection, items As DataRow())
        If items IsNot Nothing AndAlso items.Count > 0 Then
            For Each item As DataRow In items
                rows.Remove(item)
            Next
        End If
    End Sub

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToJson(Data As DataSet) As String
        Return Common.ToJson(Data)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToJson(Data As Dictionary(Of String, Object)) As String
        Return Common.ToJson(Data)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToJson(Data As String()) As String
        Return Common.ToJson(Data)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToJson(Data As Dictionary(Of String, String)) As String
        Return Common.ToJson(Data)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToJson(Data As DataTable) As String
        Return Common.ToJson(Data)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function BeginsWith(source As String, value As String) As Boolean
        Return source.StartsWith(value, StringComparison.CurrentCultureIgnoreCase)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function StringValue(value As Boolean, TrueValue As String, FalseValue As String) As String
        If value Then
            Return TrueValue
        Else
            Return FalseValue
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function StringValue(value As Boolean?, TrueValue As String, FalseValue As String) As String
        If Not value.HasValue Then
            Return ""
        Else
            Return value.Value.StringValue(TrueValue, FalseValue)
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function CharValue(value As Boolean, TrueValue As Char, FalseValue As Char) As Char
        If value Then
            Return TrueValue
        Else
            Return FalseValue
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToBoolean(value As Char, TrueValue As Char) As Boolean
        Return value.ToString().IsEqual(TrueValue.ToString())
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToBoolean(value As String, TrueValue As String) As Boolean
        Return value.IsEqual(TrueValue)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Coalesce(value As String, ParamArray items As String()) As String
        If value.IsNullOrWhiteSpace() Then
            value = ""
        End If

        value = value.Trim()

        If Not value.IsNullOrWhiteSpace() Then
            Return value
        End If

        For Each item As String In items
            If Not item.IsNullOrWhiteSpace() Then
                Return item
            End If
        Next

        Return ""
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Coalesce(value As Double?, ParamArray items As Double?()) As Double?
        If value.HasValue Then
            Return value
        End If

        For Each item As Double? In items
            If item.HasValue Then
                Return item.Value
            End If
        Next

        Return Nothing
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Coalesce(value As Integer?, ParamArray items As Integer?()) As Integer?
        If value.HasValue Then
            Return value
        End If

        For Each item As Integer? In items
            If item.HasValue Then
                Return item.Value
            End If
        Next

        Return Nothing
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Coalesce(value As Date?, ParamArray items As Date?()) As Date?
        If value.HasValue Then
            Return value
        End If

        For Each item As Date? In items
            If item.HasValue Then
                Return item.Value
            End If
        Next

        Return Nothing
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Truncate(value As String, length As Integer) As String
        If value.IsNullOrWhiteSpace() Then
            Return ""
        ElseIf value.Length <= length Then
            Return value
        Else
            Return value.Substring(0, length - 1)
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function FirstChar(value As String) As Char
        If value.IsNullOrWhiteSpace() Then
            Return Nothing
        End If

        Return value.ToCharArray()(0)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function IsEqual(item As Char, value As Char) As Boolean
        Dim a As String = Convert.ToString(item)
        Dim b As String = Convert.ToString(value)

        Return a.IsEqual(b)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function IsNullOrEmpty(value As Char) As Boolean
        If value = Nothing Then
            Return True
        Else
            Return False
        End If

    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function IsNullOrWhiteSpace(value As Char) As Boolean
        If value = Nothing Then
            Return True
        Else
            Return value = " "c
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Coalesce(value As Char, ParamArray items As Char()) As Char
        If Not value.IsNullOrWhiteSpace() Then
            Return value
        End If

        For Each item As Char In items
            If Not item.IsNullOrWhiteSpace() Then
                Return item
            End If
        Next

        Return Nothing
    End Function

    ''' <summary>
    ''' Returns the value that is greater
    ''' </summary>
    ''' <param name="val"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DebuggerStepThrough()>
    <Extension()>
    Public Function Greatest(val As Integer, value As Integer) As Integer
        If val >= value Then
            Return val
        Else
            Return value
        End If
    End Function

    ''' <summary>
    ''' Returns the value that is least
    ''' </summary>
    ''' <param name="val"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DebuggerStepThrough()>
    <Extension()>
    Public Function Least(val As Integer, value As Integer) As Integer
        If val <= value Then
            Return val
        Else
            Return value
        End If
    End Function

    ''' <summary>
    ''' Returns the value that is greater
    ''' </summary>
    ''' <param name="val"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DebuggerStepThrough()>
    <Extension()>
    Public Function Greatest(val As Double, value As Double) As Double
        If val >= value Then
            Return val
        Else
            Return value
        End If
    End Function

    ''' <summary>
    ''' Returns the value that is least
    ''' </summary>
    ''' <param name="val"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DebuggerStepThrough()>
    <Extension()>
    Public Function Least(val As Double, value As Double) As Double
        If val <= value Then
            Return val
        Else
            Return value
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function Change(item As String, oldValue As String, newValue As String) As String
        If item.IsNullOrEmpty() Then
            Return ""
        End If


        Dim oldValueStart As Integer = item.IndexOf(oldValue, StringComparison.CurrentCultureIgnoreCase)
        Dim oldValueLength As Integer = oldValue.Length

        Do Until oldValueStart < 0
            item = item.Remove(oldValueStart, oldValueLength)
            item = item.Insert(oldValueStart, newValue)

            oldValueStart = item.IndexOf(oldValue, StringComparison.CurrentCultureIgnoreCase)
        Loop

        Return item
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function JustTime(item As Date) As Date
        Return "01/01/0001 {0:hh:mm tt}".QuickFormat(item).ToDate().Value
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function JustTime(item As Date?) As Date?
        If item.HasValue Then
            Return item.Value.JustTime()
        Else
            Return Nothing
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function JustTimeSQL(item As Date) As Date
        Return "01/01/1900 {0:hh:mm tt}".QuickFormat(item).ToDate().Value
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function JustTimeSQL(item As Date?) As Date?
        If item.HasValue Then
            Return item.Value.JustTimeSQL()
        Else
            Return Nothing
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToUrlBase64(item As String) As String
        Return Civic.Common.UrlStringToBase64(item)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function FromUrlBase64(item As String) As String
        Return Civic.Common.UrlBase64ToString(item)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function EnsureEnd(item As String, value As String) As String
        If Not item.EndsWith(value) Then
            item &= value
        End If

        Return item
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function EnsureBegins(item As String, value As String) As String
        If Not item.StartsWith(value) Then
            item = value & item
        End If

        Return item
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function SameDay(item As Date, value As Date) As Boolean
        item = item.Display(DateFormat.Date).ToDate().Value
        value = item.Display(DateFormat.Date).ToDate().Value

        Return item = value
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function SameDay(item As Date?, value As Date?) As Boolean
        If Not item.HasValue OrElse Not value.HasValue Then
            Return False
        End If

        item = item.Display(DateFormat.Date).ToDate().Value
        value = item.Display(DateFormat.Date).ToDate().Value

        Return item.Value = value.Value
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function JustDay(item As Date) As Date
        Return item.Display(DateFormat.Date).ToDate().Value
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function JustDay(item As Date?) As Date?
        If Not item.HasValue Then
            Return Nothing
        End If

        Return item.Value.JustDay()
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function UseDefault(item As Boolean?, value As Boolean) As Boolean
        If item.HasValue Then
            Return item.Value
        Else
            Return value
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function UseDefault(item As Double?, value As Double) As Double
        If item.HasValue Then
            Return item.Value
        Else
            Return value
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function UseDefault(item As Integer?, value As Integer) As Integer
        If item.HasValue Then
            Return item.Value
        Else
            Return value
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function UseDefault(item As String, value As String) As String
        If Not item.IsNullOrWhiteSpace() Then
            Return item
        Else
            Return value
        End If
    End Function    Public Function UseDefault(item As String, value As String) As String
        If Not item.IsNullOrWhiteSpace() Then
            Return item
        Else
            Return value
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function IsValid(item As String, ParamArray values As String()) As Boolean
        For Each value As String In values
            If value.IsEqual(value) Then
                Return True
            End If
        Next
        Return False
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function IsValid(item As Char, ParamArray values As Char()) As Boolean
        Return item.ToString().IsValid(values)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function IsValid(item As Char?, ParamArray values As Char()) As Boolean
        If Not item.HasValue Then
            Return True
        Else
            Return IsValid(item.Value, values)
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToInteger(value As String) As Integer?
        If value.IsNullOrWhiteSpace() Then
            value = ""
        End If
        Return Civic.Common.ToInteger(value.Trim())
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToInteger(value As String, UseDefault As Integer) As Integer
        Return value.ToInteger().UseDefault(UseDefault)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToInteger(item As Double?) As Integer?
        If Not item.HasValue Then
            Return Nothing
        Else
            Return item.Value.ToInteger()
        End If
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToInteger(item As Double) As Integer
        Return CType(Math.Round(item, 0), Integer)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ToNumeric(item As String) As String
        If item.IsNullOrWhiteSpace() Then
            Return ""
        End If

        Dim value As New StringBuilder

        For Each c As Char In item.ToCharArray()
            If c.ToString().IsNumeric() Then
                value.Append(c)
            End If
        Next

        Return value.ToString()
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ContainsLike(values As List(Of String), search As String) As Boolean
        If values Is Nothing OrElse values.Count = 0 OrElse search.IsNullOrEmpty() Then
            Return False
        End If

        search = search.ToLower()
        For Each item As String In values
            If search.Like(item) Then
                Return True
            End If
        Next

        Return False
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function IsEmail(value As String) As Boolean
        Dim pattern As String = "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?"

        Return value.ToLower().IsMatch(pattern)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function IsMatch(value As String, RexPattern As String) As Boolean
        Return Regex.IsMatch(value, RexPattern)
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function IsUrl(value As String) As Boolean
        Return value.Like("http", "..", "~")
    End Function

    <Extension(),
    DebuggerStepThrough()>
    Public Function LongSplit(value As String, Delimiter As String) As String()
        If value.IsNullOrWhiteSpace() Then
            Return {}
        ElseIf Delimiter.IsNullOrEmpty() Then
            Return {value}
        End If

        Dim Delimiters As String = "|,;:./?\]}}[{{=+-_)(*&^%$#@!~` {0}1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ".QuickFormat(vbTab)

        For Each item As Char In Delimiters.ToCharArray()
            If Not value.Contains(item.ToString()) Then
                If Not Delimiter.IsNullOrEmpty() Then
                    value = value.Replace(Delimiter, item)
                    Return value.Split(item)
                End If
            End If
        Next

        Throw New System.Exception("The current string cannot be split due all splitting options exist in the string")
    End Function

End Module
* 
     */
}
