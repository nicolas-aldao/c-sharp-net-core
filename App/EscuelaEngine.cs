using System;
using System.Collections.Generic;
using System.Linq;
using CoreEscuela.Entidades;
using CoreEscuela.Util;

namespace CoreEscuela.App
{
    public sealed class EscuelaEngine // Clase "sellada", ninguna clase puede heredar
    {                                 // de EscuelaEngine pero si instanciar
        public Escuela Escuela { get; set; }

        public EscuelaEngine()
        {

        }

        public void Inicializar()
        {
            Escuela = new Escuela("Platzi Academay", 2012, TiposEscuela.Primaria,
            ciudad: "Bogotá", pais: "Colombia"
            );

            CargarCursos();
            CargarAsignaturas();
            CargarEvaluaciones();

        }

        public Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> GetDiccionarioObjetos()
        {
            var diccionario = new Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>>();

            diccionario.Add(LlaveDiccionario.Escuela, new[] { Escuela }); // arreglo con IEnumerable
            diccionario.Add(LlaveDiccionario.Cursos, Escuela.Cursos.Cast<ObjetoEscuelaBase>());

            var listaTempAsig = new List<Asignatura>();
            var listaTempAlum = new List<Alumno>();
            var listaTempEv = new List<Evaluación>();
            foreach (var curso in Escuela.Cursos)
            {
                listaTempAsig.AddRange(curso.Asignaturas);
                listaTempAlum.AddRange(curso.Alumnos);
                foreach (var alum in curso.Alumnos)
                {
                    listaTempEv.AddRange(alum.Evaluaciones);
                }
            }
            diccionario.Add(LlaveDiccionario.Asignaturas, listaTempAsig.Cast<ObjetoEscuelaBase>());
            diccionario.Add(LlaveDiccionario.Alumnos, listaTempAlum.Cast<ObjetoEscuelaBase>());
            diccionario.Add(LlaveDiccionario.Evaluaciones, listaTempEv.Cast<ObjetoEscuelaBase>());
            return diccionario;
        }

        public void ImprimirDiccionario(Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> dic,
                bool mostrarEv = false

                )
        {
            foreach (var obj in dic)
            {
                Printer.WriteTitle(obj.Key.ToString());
                foreach (var elemento in obj.Value)
                {
                    switch (obj.Key)
                    {
                        case LlaveDiccionario.Evaluaciones:
                            if (mostrarEv)
                                Console.WriteLine(elemento);
                            break;
                        case LlaveDiccionario.Escuela:
                            Console.WriteLine("Escuela: " + elemento);
                            break;
                        case LlaveDiccionario.Alumnos:
                            Console.WriteLine("Alumno: " + elemento.Nombre);
                            break;
                        case LlaveDiccionario.Cursos:
                            var curtmp = elemento as Curso;
                            if (curtmp != null)
                            {
                                int count = ((Curso)elemento).Alumnos.Count;
                                Console.WriteLine("Curso: " + elemento.Nombre + " Cantidad Alumnos: " + count);
                            }
                            break;
                        default:
                            Console.WriteLine(elemento);
                            break;
                    }
                }
            }
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetoEscuela(
            bool traeCursos = true,
            bool traeAsignaturas = true,
            bool traeAlumnos = true,
            bool traeEvaluaciones = true
        )
        {
            return GetObjetoEscuela(out int dummy, out dummy, out dummy, out dummy);
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetoEscuela(
                            out int CantiCursos,
                            bool traeCursos = true,
                            bool traeAsignaturas = true,
                            bool traeAlumnos = true,
                            bool traeEvaluaciones = true
                        )
        {
            return GetObjetoEscuela(out CantiCursos, out int dummy, out dummy, out dummy);
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetoEscuela(
                            out int CantiCursos,
                            out int CantiAsignaturas,
                            bool traeCursos = true,
                            bool traeAsignaturas = true,
                            bool traeAlumnos = true,
                            bool traeEvaluaciones = true
                        )
        {
            return GetObjetoEscuela(out CantiCursos, out CantiAsignaturas, out int dummy, out dummy);
        }


        public IReadOnlyList<ObjetoEscuelaBase> GetObjetoEscuela(
                            out int CantiCursos,
                            out int CantiAsignaturas,
                            out int CantiAlumnos,
                            bool traeCursos = true,
                            bool traeAsignaturas = true,
                            bool traeAlumnos = true,
                            bool traeEvaluaciones = true
                        )
        {
            return GetObjetoEscuela(out CantiCursos, out CantiAsignaturas, out CantiAlumnos, out int dummy);
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetoEscuela(
            out int CantiCursos,
            out int CantiAsignaturas,
            out int CantiAlumnos,
            out int CantiEvaluaciones,
            bool traeCursos = true,
            bool traeAsignaturas = true,
            bool traeAlumnos = true,
            bool traeEvaluaciones = true
        )
        {
            List<ObjetoEscuelaBase> listaObjetosEscuela = new List<ObjetoEscuelaBase>();
            CantiCursos = CantiAsignaturas = CantiAlumnos = CantiEvaluaciones = 0;

            listaObjetosEscuela.Add(Escuela);

            CantiCursos = Escuela.Cursos.Count;
            if (traeCursos)
                listaObjetosEscuela.AddRange(Escuela.Cursos);

            foreach (Curso curso in Escuela.Cursos)
            {
                CantiAsignaturas += curso.Asignaturas.Count;
                CantiAlumnos += curso.Alumnos.Count;
                if (traeAsignaturas)
                    listaObjetosEscuela.AddRange(curso.Asignaturas);
                if (traeAlumnos)
                    listaObjetosEscuela.AddRange(curso.Alumnos);

                foreach (Alumno alumno in curso.Alumnos)
                {
                    CantiEvaluaciones += alumno.Evaluaciones.Count;
                    listaObjetosEscuela.AddRange(alumno.Evaluaciones);
                }
            }

            return listaObjetosEscuela.AsReadOnly();
        }

        private List<Alumno> GenerarAlumnosAlAzar(int cantidad)
        {
            string[] nombre1 = { "Alba", "Felipa", "Eusebio", "Farid", "Donald", "Alvaro", "Nicolás" };
            string[] apellido1 = { "Ruiz", "Sarmiento", "Uribe", "Maduro", "Trump", "Toledo", "Herrera" };
            string[] nombre2 = { "Freddy", "Anabel", "Rick", "Murty", "Silvana", "Diomedes", "Nicomedes", "Teodoro" };

            var listaAlumnos = from n1 in nombre1
                               from n2 in nombre2
                               from a1 in apellido1
                               select new Alumno { Nombre = $"{n1} {n2} {a1}" };

            return listaAlumnos.OrderBy((al) => al.UniqueId).Take(cantidad).ToList();
        }

        #region Métodos Carga
        private void CargarEvaluaciones()
        {
            var rnd = new Random();
            foreach (var curso in Escuela.Cursos)
            {
                foreach (var asignatura in curso.Asignaturas)
                {
                    foreach (var alumno in curso.Alumnos)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            var ev = new Evaluación
                            {
                                Asignatura = asignatura,
                                Nombre = $"{asignatura.Nombre} Ev#{i + 1}",
                                Nota = (float)Math.Round(5 * rnd.NextDouble(), 2),
                                Alumno = alumno
                            };
                            alumno.Evaluaciones.Add(ev);
                        }
                    }
                }
            }

        }

        private void CargarAsignaturas()
        {
            foreach (var curso in Escuela.Cursos)
            {
                var listaAsignaturas = new List<Asignatura>(){
                            new Asignatura{Nombre="Matemáticas"} ,
                            new Asignatura{Nombre="Educación Física"},
                            new Asignatura{Nombre="Castellano"},
                            new Asignatura{Nombre="Ciencias Naturales"}
                };
                curso.Asignaturas = listaAsignaturas;
            }
        }

        private void CargarCursos()
        {
            Escuela.Cursos = new List<Curso>(){
                        new Curso(){ Nombre = "101", Jornada = TiposJornada.Mañana },
                        new Curso() {Nombre = "201", Jornada = TiposJornada.Mañana},
                        new Curso{Nombre = "301", Jornada = TiposJornada.Mañana},
                        new Curso(){ Nombre = "401", Jornada = TiposJornada.Tarde },
                        new Curso() {Nombre = "501", Jornada = TiposJornada.Tarde},
            };

            Random rnd = new Random();
            foreach (var c in Escuela.Cursos)
            {
                int cantRandom = rnd.Next(5, 20);
                c.Alumnos = GenerarAlumnosAlAzar(cantRandom);
            }
        }
    }
    #endregion

}