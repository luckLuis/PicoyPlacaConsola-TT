using System;

class PicoPlacaService
{
    public bool PuedeCircular(string placa, DateTime fecha, TimeSpan hora)
    {
        if (string.IsNullOrEmpty(placa) || placa.Length < 1)
        {
            throw new ArgumentException("La placa no puede ser nula o vacía.");
        }

        int ultimoDigito;
        if (!int.TryParse(placa.Substring(placa.Length - 1), out ultimoDigito))
        {
            throw new ArgumentException("El último carácter de la placa no es un dígito válido.");
        }

        DayOfWeek diaSemana = fecha.DayOfWeek;

        var restricciones = new (DayOfWeek, int[])[]
        {
            (DayOfWeek.Monday, new[] {1, 2}),
            (DayOfWeek.Tuesday, new[] {3, 4}),
            (DayOfWeek.Wednesday, new[] {5, 6}),
            (DayOfWeek.Thursday, new[] {7, 8}),
            (DayOfWeek.Friday, new[] {9, 0}),
        };

        TimeSpan inicioMañana = new TimeSpan(6, 0, 0);
        TimeSpan finMañana = new TimeSpan(9, 30, 0);
        TimeSpan inicioTarde = new TimeSpan(16, 0, 0);
        TimeSpan finTarde = new TimeSpan(20, 00, 0);

        foreach (var restriccion in restricciones)
        {
            if (diaSemana == restriccion.Item1 && Array.Exists(restriccion.Item2, digito => digito == ultimoDigito))
            {
                if ((hora >= inicioMañana && hora <= finMañana) || (hora >= inicioTarde && hora <= finTarde))
                {
                    return false;
                }
            }
        }

        return true;
    }
}
