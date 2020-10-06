using System;
using System.Collections.Generic;
using System.Linq;
using CoreEscuela.Entidades;

namespace CoreEscuela.App
{
    public class Reporteador
    {
        Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> _diccionario;

        public Reporteador(Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> dicObj)
        {
            if (dicObj == null)
                throw new ArgumentException(nameof(dicObj));
            _diccionario = dicObj;
        }

        public IEnumerable<Evaluación> GetListaEvaluaciones()
        {
            if (_diccionario.TryGetValue(LlaveDiccionario.Evaluaciones,
                                        out IEnumerable<ObjetoEscuelaBase> lista))
            {
                return lista.Cast<Evaluación>();
            }
            {
                return new List<Evaluación>();
            }
        }

        public IEnumerable<string> GetListaAsignaturas()
        {
            return GetListaAsignaturas(out var dummy); // SOBRECARGA DE LA FORMA DE ABAJO, ASÍ NO PONGO
                                                    // EL OTRO PARÁMETRO DE SALIDA
        }

        public IEnumerable<string> GetListaAsignaturas(out IEnumerable<Evaluación> listaEvaluaciones)
        {
            listaEvaluaciones = GetListaEvaluaciones();

            return (from ev in listaEvaluaciones
                    select ev.Asignatura.Nombre).Distinct();

        }
        public Dictionary<string, IEnumerable<Evaluación>> GetListaEvXAsig()
        {
            var dicResp = new Dictionary<string, IEnumerable<Evaluación>>();
            var listaAsig = GetListaAsignaturas(out var listaEval);

            foreach (var asig in listaAsig)
            {
                var evalAsig = from eval in listaEval
                                where eval.Asignatura.Nombre == asig
                                select eval;
                dicResp.Add(asig, evalAsig);
            }

            return dicResp;
        } 

        public Dictionary<string, IEnumerable<PromedioAlumno>> ExtraerPromedioPorNotaAlumno()
        {
            var rta = new Dictionary<string, IEnumerable<PromedioAlumno>>();
            var listaEv = GetListaEvXAsig();

            foreach (var asigConListEval in listaEv)
            {
                var EvalsDeAlumno = from evals in asigConListEval.Value
                                    group evals by new 
                                    {
                                        evals.Alumno.UniqueId,
                                        evals.Alumno.Nombre
                                    }
                                    into grupoEvalsAlumno
                                    select new PromedioAlumno
                                    {
                                        alumnoId = grupoEvalsAlumno.Key.UniqueId,
                                        alumnoNombre = grupoEvalsAlumno.Key.Nombre,
                                        nota = grupoEvalsAlumno.Average( e => e.Nota)
                                    };
                rta.Add(asigConListEval.Key, EvalsDeAlumno);
            }

            return rta; 
        }

        public Dictionary<string, IEnumerable<PromedioAlumno>> MejoresXPromediosDeXAsignatura(int Top)
        {
            Dictionary<string, IEnumerable<PromedioAlumno>> rta = ExtraerPromedioPorNotaAlumno();
            Dictionary<string, IEnumerable<PromedioAlumno>> rta2 = new Dictionary<string, IEnumerable<PromedioAlumno>>();
            
            foreach (var asig in rta)
            {
                var mejoresPromDeMateriaX = from proms in asig.Value.OrderByDescending(pr => pr.nota).Take(Top)
                                            select proms;
                rta2.Add(asig.Key, mejoresPromDeMateriaX);
            }

            return rta2;
        }

    }
}