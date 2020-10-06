using System;
using System.Collections.Generic;
using System.Linq;
using CoreEscuela.Entidades;
using CoreEscuela.App;
using CoreEscuela.Util;
using static System.Console;

namespace CoreEscuela
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new EscuelaEngine();
            engine.Inicializar();
            Printer.WriteTitle("BIENVENIDOS A LA ESCUELA");
            
            var reporteador = new Reporteador(engine.GetDiccionarioObjetos());
            IEnumerable<string> lasAsignaturas = reporteador.GetListaAsignaturas();

            Printer.WriteTitle("Prueba método GetListEvalXAsig");
            Dictionary<string, IEnumerable<Evaluación>> diccEvXAsig = reporteador.GetListaEvXAsig();

            var PromedioAlumnos = reporteador.ExtraerPromedioPorNotaAlumno();

            var Mejores3Promedios = reporteador.MejoresXPromediosDeXAsignatura(3);

        }

        private static void ImpimirCursosEscuela(Escuela escuela)
        {
            
            Printer.WriteTitle("Cursos de la Escuela");
            
            
            if (escuela?.Cursos != null)
            {
                foreach (var curso in escuela.Cursos)
                {
                    WriteLine($"Nombre {curso.Nombre  }, Id  {curso.UniqueId}");
                }
            }
        }
    }
}
