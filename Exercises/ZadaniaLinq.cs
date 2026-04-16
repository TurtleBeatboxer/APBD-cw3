using LinqConsoleLab.PL.Data;
using LinqConsoleLab.PL.Models;

namespace LinqConsoleLab.PL.Exercises;

public sealed class ZadaniaLinq
{
    /// <summary>
    /// Zadanie:
    /// Wyszukaj wszystkich studentów mieszkających w Warsaw.
    /// Zwróć numer indeksu, pełne imię i nazwisko oraz miasto.
    ///
    /// SQL:
    /// SELECT NumerIndeksu, Imie, Nazwisko, Miasto
    /// FROM Studenci
    /// WHERE Miasto = 'Warsaw';
    /// </summary>
    public IEnumerable<string> Zadanie01_StudenciZWarszawy()
    {
        /*var query = from s in DaneUczelni.Studenci where s.Miasto.Equals("Wasaw") 
            select new {s.NumerIndeksu, s.Imie, s.Nazwisko, s.Miasto};
        var query2 = from s in DaneUczelni.Studenci where s.Miasto.Equals("Wasaw") 
            select $"{s.NumerIndeksu}, {s.Imie}, {s.Nazwisko}, {s.Miasto}";*/
        
        var method = DaneUczelni.Studenci
            .Where(s => s.Miasto.Equals("Warsaw"))
            .Select(s =>
            $"{s.NumerIndeksu}, {s.Imie}, {s.Nazwisko}, {s.Miasto}");
        return method;
    }

    /// <summary>
    /// Zadanie:
    /// Przygotuj listę adresów e-mail wszystkich studentów.
    /// Użyj projekcji, tak aby w wyniku nie zwracać całych obiektów.
    ///
    /// SQL:
    /// SELECT Email
    /// FROM Studenci;
    /// </summary>
    public IEnumerable<string> Zadanie02_AdresyEmailStudentow()
    {
        var method = DaneUczelni.Studenci
            .Select(s =>
            $"{s.Email}");
        return method;
    }

    /// <summary>
    /// Zadanie:
    /// Posortuj studentów alfabetycznie po nazwisku, a następnie po imieniu.
    /// Zwróć numer indeksu i pełne imię i nazwisko.
    ///
    /// SQL:
    /// SELECT NumerIndeksu, Imie, Nazwisko
    /// FROM Studenci
    /// ORDER BY Nazwisko, Imie;
    /// </summary>
    public IEnumerable<string> Zadanie03_StudenciPosortowani()
    {
        var method = DaneUczelni.Studenci.OrderBy(s => s.Nazwisko)
            .ThenBy(s => s.Imie)
            .Select(s =>
            $"{s.NumerIndeksu}, {s.Imie}, {s.Nazwisko}");
        return method;
    }

    /// <summary>
    /// Zadanie:
    /// Znajdź pierwszy przedmiot z kategorii Analytics.
    /// Jeżeli taki przedmiot nie istnieje, zwróć komunikat tekstowy.
    ///
    /// SQL:
    /// SELECT TOP 1 Nazwa, DataStartu
    /// FROM Przedmioty
    /// WHERE Kategoria = 'Analytics';
    /// </summary>
    public IEnumerable<string> Zadanie04_PierwszyPrzedmiotAnalityczny()
    {
        var result = DaneUczelni.Przedmioty
            .FirstOrDefault(p => p.Kategoria == "Analytics"); 

        return result is not null
            ? [$"{result.Nazwa}, {result.DataStartu}"]
            : ["komunikat tekstowy"];
    }

    /// <summary>
    /// Zadanie:
    /// Sprawdź, czy w danych istnieje przynajmniej jeden nieaktywny zapis.
    /// Zwróć jedno zdanie z odpowiedzią True/False albo Tak/Nie.
    ///
    /// SQL:
    /// SELECT CASE WHEN EXISTS (
    ///     SELECT 1
    ///     FROM Zapisy
    ///     WHERE CzyAktywny = 0
    /// ) THEN 1 ELSE 0 END;
    /// </summary>
    public IEnumerable<string> Zadanie05_CzyIstniejeNieaktywneZapisanie()
    {
        var result = DaneUczelni.Zapisy.Any(s => !s.CzyAktywny);

        return result ? ["True"] : ["False"];
    }

    /// <summary>
    /// Zadanie:
    /// Sprawdź, czy każdy prowadzący ma uzupełnioną nazwę katedry.
    /// Warto użyć metody, która weryfikuje warunek dla całej kolekcji.
    ///
    /// SQL:
    /// SELECT CASE WHEN COUNT(*) = COUNT(Katedra)
    /// THEN 1 ELSE 0 END
    /// FROM Prowadzacy;
    /// </summary>
    public IEnumerable<string> Zadanie06_CzyWszyscyProwadzacyMajaKatedre()
    {
        var result = DaneUczelni.Prowadzacy.All(p => p.Katedra != null);
        return result ? ["Kazdy ma"] : ["Nie kazdy ma"];
    }

    /// <summary>
    /// Zadanie:
    /// Policz, ile aktywnych zapisów znajduje się w systemie.
    ///
    /// SQL:
    /// SELECT COUNT(*)
    /// FROM Zapisy
    /// WHERE CzyAktywny = 1;
    /// </summary>
    public IEnumerable<string> Zadanie07_LiczbaAktywnychZapisow()
    {
        var result = DaneUczelni.Zapisy.Count(zapisy => zapisy.CzyAktywny);
        return [$"{result}"];
    }

    /// <summary>
    /// Zadanie:
    /// Pobierz listę unikalnych miast studentów i posortuj ją rosnąco.
    ///
    /// SQL:
    /// SELECT DISTINCT Miasto
    /// FROM Studenci
    /// ORDER BY Miasto;
    /// </summary>
    //@TODO
    public IEnumerable<string> Zadanie08_UnikalneMiastaStudentow()
    {
        var result = DaneUczelni.Studenci.DistinctBy(s => s.Miasto).OrderBy(s => s.Miasto)
            .Select(s => $"{s.Miasto}");
        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Zwróć trzy najnowsze zapisy na przedmioty.
    /// W wyniku pokaż datę zapisu, identyfikator studenta i identyfikator przedmiotu.
    ///
    /// SQL:
    /// SELECT TOP 3 DataZapisu, StudentId, PrzedmiotId
    /// FROM Zapisy
    /// ORDER BY DataZapisu DESC;
    /// </summary>
    public IEnumerable<string> Zadanie09_TrzyNajnowszeZapisy()
    {
        var result = DaneUczelni.Zapisy.OrderByDescending(zapisy => zapisy.DataZapisu).
            Select(zapisy => $"{zapisy.DataZapisu}, {zapisy.StudentId}, {zapisy.PrzedmiotId}").Take(3);
        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Zaimplementuj prostą paginację dla listy przedmiotów.
    /// Załóż stronę o rozmiarze 2 i zwróć drugą stronę danych.
    ///
    /// SQL:
    /// SELECT Nazwa, Kategoria
    /// FROM Przedmioty
    /// ORDER BY Nazwa
    /// OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY;
    /// </summary>
    public IEnumerable<string> Zadanie10_DrugaStronaPrzedmiotow()
    {
        var result = DaneUczelni.Przedmioty
            .OrderBy(przedmiot => przedmiot.Nazwa)
            .Skip(2)
            .Take(2)
            .Select(przedmiot => $"{przedmiot.Nazwa}, {przedmiot.Kategoria}"); 

        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Połącz studentów z zapisami po StudentId.
    /// Zwróć pełne imię i nazwisko studenta oraz datę zapisu.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, z.DataZapisu
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId;
    /// </summary>
    public IEnumerable<string> Zadanie11_PolaczStudentowIZapisy()
    {
        var result = DaneUczelni.Studenci.Join(DaneUczelni.Zapisy, student => student.Id,
                zapisy => zapisy.StudentId, (student, zapis) => new { student, zapis })
            .Select(s => $"{s.student.Imie} {s.student.Nazwisko} {s.zapis.DataZapisu}");
        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Przygotuj wszystkie pary student-przedmiot na podstawie zapisów.
    /// Użyj podejścia, które pozwoli spłaszczyć dane do jednej sekwencji wyników.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, p.Nazwa
    /// FROM Zapisy z
    /// JOIN Studenci s ON s.Id = z.StudentId
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId;
    /// </summary>
    public IEnumerable<string> Zadanie12_ParyStudentPrzedmiot()
    {
        return DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy, 
                student => student.Id,
                zapis => zapis.StudentId, 
                (student, zapis) => new { student, zapis }
            )
            .Join(
                DaneUczelni.Przedmioty,
                joined => joined.zapis.PrzedmiotId,
                przedmiot => przedmiot.Id,
                (joined, przedmiot) => $"{joined.student.Imie} {joined.student.Nazwisko} - {przedmiot.Nazwa}"
            );
    }

    /// <summary>
    /// Zadanie:
    /// Pogrupuj zapisy według przedmiotu i zwróć nazwę przedmiotu oraz liczbę zapisów.
    ///
    /// SQL:
    /// SELECT p.Nazwa, COUNT(*)
    /// FROM Zapisy z
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    /// GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie13_GrupowanieZapisowWedlugPrzedmiotu()
    {
        var result = DaneUczelni.Zapisy.Join(DaneUczelni.Przedmioty,
            zapis => zapis.PrzedmiotId,
            przedmiot => przedmiot.Id,
            (zapis, przedmiot) => new { zapis, przedmiot }
        ).GroupBy(x => x.przedmiot.Nazwa,
            (nazwaPrzedmiotu, group) => $"{nazwaPrzedmiotu}: {group.Count()}");
        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Oblicz średnią ocenę końcową dla każdego przedmiotu.
    /// Pomiń rekordy, w których ocena końcowa ma wartość null.
    ///
    /// SQL:
    /// SELECT p.Nazwa, AVG(z.OcenaKoncowa)
    /// FROM Zapisy z
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie14_SredniaOcenaNaPrzedmiot()
    {
        var result =  DaneUczelni.Zapisy.Join
            (DaneUczelni.Przedmioty,
                zapis => zapis.PrzedmiotId,
                przedmiot => przedmiot.Id,
                (zapis, przedmiot) => new  { zapis, przedmiot })
            .Where(x => x.zapis.OcenaKoncowa != null)
            .GroupBy(x => x.przedmiot.Nazwa,
                (nazwaPrzedmiotu, group) =>  
                    $"{nazwaPrzedmiotu}: {group.Average(g => g.zapis.OcenaKoncowa)}");
        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Dla każdego prowadzącego policz liczbę przypisanych przedmiotów.
    /// W wyniku zwróć pełne imię i nazwisko oraz liczbę przedmiotów.
    ///
    /// SQL:
    /// SELECT pr.Imie, pr.Nazwisko, COUNT(p.Id)
    /// FROM Prowadzacy pr
    /// LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    /// GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie15_ProwadzacyILiczbaPrzedmiotow()
    {
        var result = DaneUczelni.Prowadzacy.Select(prow => new
            {
                prow.Imie,
                prow.Nazwisko,
                LiczbaPrzedmiotow = DaneUczelni.Przedmioty.Count(przed => przed.ProwadzacyId == prow.Id)
            })
            .Select(x => $"{x.Imie} {x.Nazwisko} - {x.LiczbaPrzedmiotow}");
        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Dla każdego studenta znajdź jego najwyższą ocenę końcową.
    /// Pomiń studentów, którzy nie mają jeszcze żadnej oceny.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, MAX(z.OcenaKoncowa)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY s.Imie, s.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie16_NajwyzszaOcenaKazdegoStudenta()
    {
        var result = DaneUczelni.Zapisy.Where(x => x.OcenaKoncowa is not null)
            .Join(DaneUczelni.Studenci,
            zapis => zapis.StudentId,
            studenc => studenc.Id,
            (zapis, studenc) => new { zapis, studenc }
        ).GroupBy(x => new { x.studenc.Imie, x.studenc.Nazwisko }, (imieNazwisko, group) =>
            $"{imieNazwisko.Imie} {imieNazwisko.Nazwisko}:  {group.Max(g => g.zapis.OcenaKoncowa)}");
        return result;
    }

    /// <summary>
    /// Wyzwanie:
    /// Znajdź studentów, którzy mają więcej niż jeden aktywny zapis.
    /// Zwróć pełne imię i nazwisko oraz liczbę aktywnych przedmiotów.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, COUNT(*)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.CzyAktywny = 1
    /// GROUP BY s.Imie, s.Nazwisko
    /// HAVING COUNT(*) > 1;
    /// </summary>
    public IEnumerable<string> Wyzwanie01_StudenciZWiecejNizJednymAktywnymPrzedmiotem()
    {
        var result = DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy.Where(z => z.CzyAktywny), 
                student => student.Id,
                zapis => zapis.StudentId,
                (student, zapis) => new { student.Imie, student.Nazwisko }
            )
            .GroupBy(x => new { x.Imie, x.Nazwisko })
            .Where(grupa => grupa.Count() > 1)
            .Select(grupa => $"{grupa.Key.Imie} {grupa.Key.Nazwisko} - {grupa.Count()}");
        return result;
    }

    /// <summary>
    /// Wyzwanie:
    /// Wypisz przedmioty startujące w kwietniu 2026, dla których żaden zapis nie ma jeszcze oceny końcowej.
    ///
    /// SQL:
    /// SELECT p.Nazwa
    /// FROM Przedmioty p
    /// JOIN Zapisy z ON p.Id = z.PrzedmiotId
    /// WHERE MONTH(p.DataStartu) = 4 AND YEAR(p.DataStartu) = 2026
    /// GROUP BY p.Nazwa
    /// HAVING SUM(CASE WHEN z.OcenaKoncowa IS NOT NULL THEN 1 ELSE 0 END) = 0;
    /// </summary>
    public IEnumerable<string> Wyzwanie02_PrzedmiotyStartujaceWKwietniuBezOcenKoncowych()
    {
        var result = DaneUczelni.Przedmioty
            .Where(p => p.DataStartu.Year == 2026 && p.DataStartu.Month == 4)
            .Join(
                DaneUczelni.Zapisy,
                p => p.Id,
                z => z.PrzedmiotId,
                (p, z) => new { p.Nazwa, z.OcenaKoncowa }
            )
            .GroupBy(x => x.Nazwa)
            .Where(g => g.All(x => x.OcenaKoncowa == null))
            .Select(g => g.Key);

        return result;
    }

    /// <summary>
    /// Wyzwanie:
    /// Oblicz średnią ocen końcowych dla każdego prowadzącego na podstawie wszystkich jego przedmiotów.
    /// Pomiń brakujące oceny, ale pozostaw samych prowadzących w wyniku.
    ///
    /// SQL:
    /// SELECT pr.Imie, pr.Nazwisko, AVG(z.OcenaKoncowa)
    /// FROM Prowadzacy pr
    /// LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    /// LEFT JOIN Zapisy z ON z.PrzedmiotId = p.Id
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Wyzwanie03_ProwadzacyISredniaOcenNaIchPrzedmiotach()
    {
        var result = DaneUczelni.Prowadzacy
            .GroupJoin(
                DaneUczelni.Przedmioty.Join(
                    DaneUczelni.Zapisy.Where(z => z.OcenaKoncowa != null),
                    przedmiot => przedmiot.Id,
                    zapis => zapis.PrzedmiotId,
                    (przedmiot, zapis) => new { przedmiot.ProwadzacyId, zapis.OcenaKoncowa }
                ),
                prowadzacy => prowadzacy.Id,
                ocena => ocena.ProwadzacyId,
                (prowadzacy, grupaOcen) => new { prowadzacy, grupaOcen }
            )
            .Select(x => x.grupaOcen.Any() 
                ? $"{x.prowadzacy.Imie} {x.prowadzacy.Nazwisko} {x.grupaOcen.Average(g => g.OcenaKoncowa)}" 
                : $"{x.prowadzacy.Imie} {x.prowadzacy.Nazwisko} Brak ocen"
            );
            return result;
    }

    /// <summary>
    /// Wyzwanie:
    /// Pokaż miasta studentów oraz liczbę aktywnych zapisów wykonanych przez studentów z danego miasta.
    /// Posortuj wynik malejąco po liczbie aktywnych zapisów.
    ///
    /// SQL:
    /// SELECT s.Miasto, COUNT(*)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.CzyAktywny = 1
    /// GROUP BY s.Miasto
    /// ORDER BY COUNT(*) DESC;
    /// </summary>
    public IEnumerable<string> Wyzwanie04_MiastaILiczbaAktywnychZapisow()
    {
        var result = DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy.Where(z => z.CzyAktywny),
                student => student.Id,
                zapis => zapis.StudentId,
                (student, zapis) => student
            )
            .GroupBy(student => student.Miasto)
            .OrderByDescending(grupa => grupa.Count())
            .Select(grupa => $"{grupa.Key} - {grupa.Count()}");

        return result;
    }

    private static NotImplementedException Niezaimplementowano(string nazwaMetody)
    {
        return new NotImplementedException(
            $"Uzupełnij metodę {nazwaMetody} w pliku Exercises/ZadaniaLinq.cs i uruchom polecenie ponownie.");
    }
}
