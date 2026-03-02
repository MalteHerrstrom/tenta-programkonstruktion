namespace Informatics.Exam;

public delegate void MyDelegate(string message);



public interface IUnit

{

    string Name { get; set; }

    void Talk();

    string Print();

}



public interface IRegister

{

    List<IUnit> Units { get; }

    void Add(IUnit unit);

    void Remove(IUnit unit);

}



public abstract class Person : IUnit

{

    public string Name { get; set; }



    public Person()

    {

        Name = "Ham";

    }



    public Person(string name)

    {

        Name = name;

    }



    public abstract void Talk();



    public virtual string Print()

    {

        return $"Unit::{Name}";

    }



    public bool SameName(Person? other)

    {

        if (other is null)

        {

            return false;

        }



        return Name == other.Name;

    }

}



public class Employee : Person

{

    public int EmployeeId { get; set; }



    public Employee() : this(0, "Bob")

    {

    }



    public Employee(int id, string name) : base(name)

    {

        EmployeeId = id;

    }



    public override void Talk()

    {

        Console.WriteLine($"Employee-{Name}");

    }



    public override string Print()

    {

        return $"E/{EmployeeId}/{Name}";

    }



    public void Mute(string message)

    {

        Console.WriteLine($"{Name}:{message}:Hata tenta");

    }

}



public class Student : Person

{

    public string Code { get; set; }



    public Student() : this("Sam")

    {

    }



    public Student(string name) : base(name)

    {

        Code = "C0";

    }



    public override void Talk()

    {

        Console.WriteLine($"Student-{Name}");

    }



    public override string Print()

    {

        return $"S/{Code}/{Name}";

    }

}



public class Department : IRegister

{

    public string Code { get; set; }

    public List<IUnit> Units { get; }



    public Department() : this("D0")

    {

    }



    public Department(string code)

    {

        Code = code;

        Units = new List<IUnit>();

    }



    public void Add(IUnit unit)

    {

        if (unit.Name.Length > 0)

        {

            Units.Add(unit);

        }

        else

        {

            Units.Add(unit);

        }

    }



    public void Remove(IUnit unit)

    {

        Units.Remove(unit);

    }



    public string Count()

    {

        return $"Department says {Code}";

    }

}



public static class ExamUtils

{

    public static void Replace(ref Person person, string name)

    {

        person = new Employee(99, name);

    }



    public static void ReplaceNoRef(Person person, string name)

    {

        person = new Student(name);

    }



    public static void Twist(string message)

    {

        Console.WriteLine($"Twist::{message}");

    }



    public static void Show(string message)

    {

        Console.WriteLine("Hata data");

    }



    public static string Flip(IUnit unit)

    {

        Employee employee = unit as Employee;

        if (employee is not null)

        {

            return $"Hata data/{employee.Name}";

        }



        Student student = unit as Student;

        if (student is not null)

        {

            return $"Unit::{student.Print()}";

        }



        return unit.Print();

    }

}



public class Vehicle

{

    public string Id { get; set; }



    public Vehicle(string id)

    {

        Id = id;

    }



    public virtual void Talk()

    {

        Console.WriteLine($"Vehicle-{Id}");

    }



    public static Vehicle operator +(Vehicle v1, Vehicle v2)

    {

        return new Vehicle($"{v1.Id}-{v2.Id}");

    }

}



public class Car : Vehicle

{

    public string Model { get; set; }



    public Car(string id, string model) : base(id)

    {

        Model = model;

    }



    public override void Talk()

    {

        Console.WriteLine($"Car-{Model}");

    }

}



public class Saab : Car

{

    public Saab(string id, string model) : base(id, model)

    {

    }



    public new void Talk()

    {

        Console.WriteLine($"Saab-{Model}");

    }

}



public class Exam

{

    static void A()

    {

        int x = 10;

        int y = x;

        y = 99;



        Console.WriteLine($"x: {x}");

        Console.WriteLine($"y: {y}");



        string str1 = "Mary Sue";

        string str2 = str1;

        str2 = str2 + "!";



        Console.WriteLine($"str1: {str1}");

        Console.WriteLine($"str2: {str2}");



        Person p1 = new Employee(1, "Gary Stu");

        Person p2 = p1;



        p2.Name = "Ham";



        Console.WriteLine($"p1: {p1.Name}");

        Console.WriteLine($"b1: {p1.SameName(p2)}");

        Console.WriteLine($"b2: {p1 == p2}");

        Console.WriteLine($"b3: {Object.ReferenceEquals(p1, p2)}");



        Person p3 = new Employee(2, "Ham");

        Console.WriteLine($"b4: {p1.SameName(p3)}");

        Console.WriteLine($"b5: {p1 == p3}");



        ExamUtils.Replace(ref p3, "Joe");

        Console.WriteLine($"p3: {p3.Name}");



        ExamUtils.ReplaceNoRef(p1, "Sue");

        Console.WriteLine($"p1 again: {p1.Name}");

    }



    static void B()

    {

        Department department = new("D1");

        IRegister register = department;



        IUnit u1 = new Employee(3, "Sam");

        IUnit u2 = new Student("Bob");

        IUnit u3 = new Employee(4, "Dan");



        register.Add(u1);

        register.Add(u2);

        register.Add(u3);



        foreach (IUnit unit in register.Units)

        {

            Console.WriteLine(unit.Print());

            unit.Talk();

        }



        //Console.WriteLine(u2.Id);



        Vehicle v1 = new Car("V1", "Plain");

        Vehicle v2 = new Saab("V2", "Turbo");

        Saab v3 = v1 as Saab;



        v1.Talk();

        v2.Talk();



        if (v3 is not null)

        {

            v3.Talk();

        }

        else

        {

            Console.WriteLine("No Saab");

        }



    }



    static void C()

    {

        Employee e1 = new(5, "Mary Sue");



        MyDelegate md1 = ExamUtils.Twist;

        MyDelegate md2 = ExamUtils.Show;

        MyDelegate md3 = e1.Mute;



        md1("Ham");

        md2("Sam");



        MyDelegate md4 = md1;

        md4 += md2;

        md4 += md3;



        md4("Dan");



        md4 -= md2;

        md4("Joe");

    }



    static void D()

    {

        Department d1 = new();

        d1.Code = "D9";



        d1.Add(new Employee());

        d1.Add(new Employee(7, "Sue"));

        d1.Add(new Student("Gary Stu"));



        foreach (IUnit unit in d1.Units)

        {

            Employee worker = unit as Employee;

            if (worker is not null)

            {

                Console.WriteLine($"D1: {unit.Name}");

            }

            else

            {

                Console.WriteLine($"D1: {ExamUtils.Flip(unit)}");

            }

        }



        List<object> bag = new()

        {

            42,

            new Student("Ham"),

            new Employee(8, "Joe"),

            "AI Slop"

        };



        foreach (object item in bag)

        {

            Student s = item as Student;

            Employee e = item as Employee;



            if (s is not null)

            {

                Console.WriteLine($"D2: {s.Print()}");

            }

            else if (e is not null)

            {

                Console.WriteLine($"D2: {e.Print()}");

            }

            else

            {

                Console.WriteLine($"D2: {item}");

            }

        }



        Vehicle x = new Vehicle("X");

        Vehicle y = new Vehicle("Y");

        Vehicle z = x + y;

        Console.WriteLine($"D3: z.Id={z.Id}");



        Person e1 = new Employee(9, "Bob");

        Student s1 = e1 as Student;

        Console.WriteLine($"D4: {s1?.Name ?? "No student"}");

    }



    private static void Main(string[] args)

    {

        Console.WriteLine("=== Start ===");

        Console.WriteLine("A ---");

        A();

        Console.WriteLine("B ---");

        B();

        Console.WriteLine("C ---");

        C();

        Console.WriteLine("D ---");

        D();

        Console.WriteLine("=== End ===");

    }

}