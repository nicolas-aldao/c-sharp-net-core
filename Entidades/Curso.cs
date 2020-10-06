using System;
using System.Collections.Generic;
using static System.Console;
using CoreEscuela.Util;

namespace CoreEscuela.Entidades
{
    public class Curso:ObjetoEscuelaBase, ILugar
    {
        public TiposJornada Jornada { get; set; }
        public List<Asignatura> Asignaturas{ get; set; }
        public List<Alumno> Alumnos{ get; set; }
        public string Dirección { get; set; }

        public void LimpiarLugar()
        {
            Printer.DrawLine();
            WriteLine("Limpiando lugar en curso...");
            WriteLine($"Curso {Nombre} limpiado.");
        }
    }
}