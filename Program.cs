using System.Text.RegularExpressions;

namespace Classes_1
{
    public class Angle : IEquatable<Angle>
    {
        private bool _isPositive;
        private ulong _totalMinutesAbs;

        public bool Sign { get => _isPositive; }
        public uint Degrees { get => (uint)(_totalMinutesAbs / 60); }
        public ushort Minutes { get => (ushort)(_totalMinutesAbs % 60); }

        public Angle(bool ispositive, uint degrees, ushort minutes)
        {
            _isPositive = ispositive;
            ulong total = (ulong)(degrees * 60 + minutes);
            _totalMinutesAbs = total;
        }

        public Angle(bool isPositive, ulong totalMinutesAbs)
        {
            _isPositive = isPositive;
            _totalMinutesAbs = totalMinutesAbs;
            if (_totalMinutesAbs == 0)
                _isPositive = true;
        }

        public Angle(double angle)
        {
            var degrees = (int)angle;
            var minutesAbs = (ushort)Math.Abs(Math.Round((angle % 1) * 60));
            _isPositive = degrees >= 0;
            _totalMinutesAbs = (ulong)((Math.Abs(degrees) * 60) + minutesAbs);
            if (_totalMinutesAbs == 0)
                _isPositive = true;
        }

        public double ConvertToRadians() => From0To360() * Math.PI / 180;
        public double Sin() => Math.Sin(ConvertToRadians());
        public double From0To360()
        {
            double degrees = (_isPositive ? 1 : -1) * (_totalMinutesAbs / 60.0);
            return ((degrees % 360) + 360) % 360;
        }

        public void AddDegrees(uint degrees) => Add(degrees, 0);
        public void AddMinutes(uint minutes) => Add(0, minutes);
        public void Add(uint degrees, uint minutes)
        {
            ulong add = (ulong)degrees * 60 + minutes;
            if (_isPositive)
                _totalMinutesAbs += add;
            else if (_totalMinutesAbs >= add)
                _totalMinutesAbs -= add;
            else
            {
                _totalMinutesAbs = add - _totalMinutesAbs;
                _isPositive = true;
            }

        }
        public static Angle operator +(Angle a, Angle b)
        {
            if (a._isPositive == b._isPositive)
                return new Angle(a._isPositive, a._totalMinutesAbs + b._totalMinutesAbs);

            return (a._totalMinutesAbs >= b._totalMinutesAbs)
                ? new Angle(a._isPositive, a._totalMinutesAbs - b._totalMinutesAbs)
                : new Angle(b._isPositive, b._totalMinutesAbs - a._totalMinutesAbs);
        }

        public void SubDegrees(uint degrees) => Sub(degrees, 0);
        public void SubMinutes(uint minutes) => Sub(0, minutes);
        public void Sub(uint degrees, uint minutes)
        {
            ulong sub = (ulong)degrees * 60 + minutes;
            if (!_isPositive)
                _totalMinutesAbs += sub;
            else if (_totalMinutesAbs >= sub)
                _totalMinutesAbs -= sub;
            else
            {
                _totalMinutesAbs = sub - _totalMinutesAbs;
                _isPositive = false;
            }
        }
        public static Angle operator -(Angle a, Angle b) => a + new Angle(!b._isPositive, b._totalMinutesAbs);

        public static bool operator >(Angle a, Angle b)
        {
            if (a._isPositive != b._isPositive)
                return a._isPositive;

            if (a._isPositive)
                return a._totalMinutesAbs > b._totalMinutesAbs;
            return a._totalMinutesAbs < b._totalMinutesAbs;
        }
        public static bool operator >=(Angle a, Angle b) => (a > b) || (a == b);

        public static bool operator <(Angle a, Angle b) => !(a > b) && (a != b);
        public static bool operator <=(Angle a, Angle b) => (a < b) || (a == b);

        public static bool operator ==(Angle a, Angle b) => (a._isPositive == b._isPositive) && (a._totalMinutesAbs == b._totalMinutesAbs);
        public static bool operator !=(Angle a, Angle b) => !(a == b);

        public override string ToString() => $"{(_isPositive || _totalMinutesAbs == 0 ? "" : "-")}{Degrees}°{Minutes}′";

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;

            return (obj is Angle angle) && Equals(angle);
        }

        public bool Equals(Angle? obj)
        {
            if (obj is null)
                return false;
            return this == obj;
        }

        public override int GetHashCode() => HashCode.Combine(Sign, Degrees, Minutes);
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========== Программа работы с 2 углами ===============");

            Console.WriteLine("введите первый угол в формате: (знак)(градусы)o(минуты)` ");
            Angle a = GetAngleFromCMD();
            Console.WriteLine("введите второй угол в формате: (знак)(градусы)o(минуты)` ");
            Angle b = GetAngleFromCMD();
            while (true)
            {
                Console.WriteLine("Что необходимо сделать: \n" +
                    "1.  привести к 0-360 1 угл \n" +
                    "2.  привести к 0-360 2 угл \n" +
                    "3.  перевести в радианы 1 угл \n" +
                    "4.  перевести в радианы 2 угл \n" +
                    "5.  найти синус 1 угла\n" +
                    "6.  найти синус 2 угла\n" +
                    "7.  сложить 2 угла\n" +
                    "8.  вычесть 1 из 2\n" +
                    "9.  вычесть 2 из 1\n" +
                    "10. вывести 1 угл\n" +
                    "11. вывести 2 угл\n" +
                    "12. 1 > 2\n" +
                    "13. 1 >= 2\n" +
                    "14. 1 < 2\n" +
                    "15. 1 <= 2\n" +
                    "16. 1 == 2\n" +
                    "17. 1 != 2\n" +
                    "18. добавить минуты к 1 углу\n" +
                    "19. добавить минуты к 2 углу\n" +
                    "20. добавить градусы к 1 углу\n" +
                    "21. добавить градусы к 2 углу\n" +
                    "22. добавить угол к 1 углу\n" +
                    "23. добавить угол к 2 углу\n" +
                    "Для завершения работы нажмите любую кнопку кроме представленных.....");
                string? choice = Console.ReadLine();
                if (!int.TryParse(choice, out _))
                    break;
                switch (choice)
                {
                    case "1":
                        Console.WriteLine(a.From0To360());
                        break;
                    case "2":
                        Console.WriteLine(b.From0To360());
                        break;
                    case "3":
                        Console.WriteLine(a.ConvertToRadians());
                        break;
                    case "4":
                        Console.WriteLine(b.ConvertToRadians());
                        break;
                    case "5":
                        Console.WriteLine(a.Sin());
                        break;
                    case "6":
                        Console.WriteLine(b.Sin());
                        break;
                    case "7":
                        Console.WriteLine(a + b);
                        break;
                    case "8":
                        Console.WriteLine(b - a);
                        break;
                    case "9":
                        Console.WriteLine(a - b);
                        break;
                    case "10":
                        Console.WriteLine(a);
                        break;
                    case "11":
                        Console.WriteLine(b);
                        break;
                    case "12":
                        Console.WriteLine(a > b);
                        break;
                    case "13":
                        Console.WriteLine(a >= b);
                        break;
                    case "14":
                        Console.WriteLine(a < b);
                        break;
                    case "15":
                        Console.WriteLine(a <= b);
                        break;
                    case "16":
                        Console.WriteLine(a == b);
                        break;
                    case "17":
                        Console.WriteLine(a != b);
                        break;
                    case "18":
                        Console.WriteLine("Введите количество минут которое ходите добавить:");
                        uint minutsa = 0;
                        bool ok = false;
                        while (!ok)
                        {
                            ok = uint.TryParse(Console.ReadLine(), out minutsa);
                            if (!ok)
                                Console.WriteLine("Неправильный ввод, попробуйте снова:");
                        }
                        a.AddMinutes(minutsa);
                        break;
                    case "19":
                        Console.WriteLine("Введите количество минут которое ходите добавить:");
                        uint minutsb = 0;
                        ok = false;
                        while (!ok)
                        {
                            ok = uint.TryParse(Console.ReadLine(), out minutsb);
                            if (!ok)
                                Console.WriteLine("Неправильный ввод, попробуйте снова:");
                        }
                        b.AddMinutes(minutsb);
                        break;
                    case "20":
                        Console.WriteLine("Введите количество градусов которое ходите добавить:");
                        uint degrees = 0;
                        ok = false;
                        while (!ok)
                        {
                            ok = uint.TryParse(Console.ReadLine(), out degrees);
                            if (!ok)
                                Console.WriteLine("Неправильный ввод, попробуйте снова:");
                        }
                        a.AddDegrees(degrees);
                        break;
                    case "21":
                        Console.WriteLine("Введите количество градусов которое ходите добавить:");
                        degrees = 0;
                        ok = false;
                        while (!ok)
                        {
                            ok = uint.TryParse(Console.ReadLine(), out degrees);
                            if (!ok)
                                Console.WriteLine("Неправильный ввод, попробуйте снова:");
                        }
                        b.AddDegrees(degrees);
                        break;
                    case "22":
                        Console.WriteLine("введите угол в формате: (знак)(градусы)o(минуты)` ");
                        Angle add = GetAngleFromCMD();
                        a += add;
                        break;
                    case "23":
                        Console.WriteLine("введите угол в формате: (знак)(градусы)o(минуты)` ");
                        add = GetAngleFromCMD();
                        b += add;
                        break;
                }
            }
        }

        private static Angle GetAngleFromCMD()
        {
            var pattern = @"^([+-]?)(\d{1,3})o(\d{1,2})`$";

            bool isNegative = false;
            uint deg = 0;
            ushort min = 0;

            while (true)
            {
                var match = Regex.Match(Console.ReadLine(), pattern);
                if (match.Success 
                    && uint.TryParse(match.Groups[2].Value, out deg) 
                    && ushort.TryParse(match.Groups[3].Value, out min) 
                    && deg <= 360 && min <= 59)
                {
                    isNegative = (match.Groups[1].Value == "-");
                    if (deg == 0)
                        isNegative = false;
                    break;
                }
                else 
                    Console.WriteLine("Ошибка ввода, введите пожалуйста угол в формате: (знак)(градусы)o(минуты)`");
            }
            return new Angle(!isNegative, deg, min);
        }
    }
}
